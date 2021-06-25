//-----------------------------------------------------------------------
// <copyright file="MovementRule.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.Model.Visitor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the movement rule of a standard chess game.
    /// </summary>
    public class MovementRule : IChessPieceVisitor<Func<ChessGame, Position, IEnumerable<ICommand>>>
    {
        /// <summary>
        /// Represents the castling rule of a standard chess game.
        /// </summary>
        private readonly CastlingRule castlingRule;

        /// <summary>
        /// Represents the en passant rule of a standard chess game.
        /// </summary>
        private readonly EnPassantRule enPassantRule;

        /// <summary>
        /// Represents the promotion rule of a standard chess game.
        /// </summary>
        private readonly PromotionRule promotionRule;

        /// <summary>
        /// Represents the threat analyzer for validating hit moves.
        /// </summary>
        private readonly ThreatAnalyzer threatAnalyzer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementRule"/> class.
        /// </summary>
        /// <param name="castlingRule">The castling rule of a standard chess game.</param>
        /// <param name="enPassantRule">The en passant rule of a standard chess game.</param>
        /// <param name="promotionRule">The promotion rule of a standard chess game.</param>
        /// <param name="threatAnalyzer">The threat analyzer for validating hit moves.</param>
        public MovementRule
        (
            CastlingRule castlingRule,
            EnPassantRule enPassantRule,
            PromotionRule promotionRule,
            ThreatAnalyzer threatAnalyzer
        )
        {
            Validation.NotNull(castlingRule, nameof(castlingRule));
            Validation.NotNull(enPassantRule, nameof(enPassantRule));
            Validation.NotNull(promotionRule, nameof(promotionRule));
            Validation.NotNull(threatAnalyzer, nameof(threatAnalyzer));

            this.castlingRule = castlingRule;
            this.enPassantRule = enPassantRule;
            this.promotionRule = promotionRule;
            this.threatAnalyzer = threatAnalyzer;
        }

        /// <summary>
        /// Gets all possible movement commands for a specified chess piece.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="piece">The analyzed chess piece.</param>
        /// <returns>A sequence of all possible movement commands, according to the standard rulebook.</returns>
        public IEnumerable<ICommand> GetCommands(ChessGame game, PlacedPiece piece)
        {
            return this.GetCommands(game, piece.Position, piece.Piece);
        }

        /// <summary>
        /// Gets all possible movement commands for a specified chess piece.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position of the analyzed chess piece.</param>
        /// <param name="piece">The analyzed chess piece.</param>
        /// <returns>A sequence of all possible movement commands, according to the standard rulebook.</returns>
        public IEnumerable<ICommand> GetCommands(ChessGame game, Position position, ChessPiece piece)
        {
            return piece.Accept(this)(game, position);
        }

        /// <summary>
        /// Gets all possible movement commands for a chess piece on a specified position.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position of the analyzed chess piece.</param>
        /// <returns>A sequence of all possible movement commands, according to the standard rulebook.</returns>
        public IEnumerable<ICommand> GetCommands(ChessGame game, Position position)
        {
            return game.Board.GetPiece(position).GetOrElse
            (
                p => p.Accept(this)(game, position),
                Enumerable.Empty<ICommand>()
            );
        }

        /// <summary>
        /// Gets all possible movement commands for a specified bishop.
        /// </summary>
        /// <param name="bishop">The analyzed bishop.</param>
        /// <returns>
        /// Given a game state and a position for the bishop,
        /// returns all possible movement commands for the bishop.
        /// </returns>
        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Bishop bishop)
        {
            return this.GetThreatCommands(bishop);
        }

        /// <summary>
        /// Gets all possible movement commands for a specified king.
        /// </summary>
        /// <param name="king">The analyzed king.</param>
        /// <returns>
        /// Given a game state and a position for the king,
        /// returns all possible movement commands for the king.
        /// </returns>
        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(King king)
        {
            return (game, position) =>
            {
                var threatMoves = this.GetThreatCommands(game.Board, position, king);
                var castlingMoves = this.castlingRule.GetCommands(game, position, king);

                return threatMoves.Union(castlingMoves);
            };
        }

        /// <summary>
        /// Gets all possible movement commands for a specified knight.
        /// </summary>
        /// <param name="knight">The analyzed knight.</param>
        /// <returns>
        /// Given a game state and a position for the knight,
        /// returns all possible movement commands for the knight.
        /// </returns>
        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Knight knight)
        {
            return this.GetThreatCommands(knight);
        }

        /// <summary>
        /// Gets all possible movement commands for a specified pawn.
        /// </summary>
        /// <param name="pawn">The analyzed pawn.</param>
        /// <returns>
        /// Given a game state and a position for the pawn,
        /// returns all possible movement commands for the pawn.
        /// </returns>
        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Pawn pawn)
        {
            return (game, position) =>
            {
                // Check hits.
                var enemyColor = pawn.Color.Toggle();
                var threats = this.threatAnalyzer.Visit(pawn)(game.Board, position);
                var hits = threats.Select(p => game.Board.GetPiece(p, enemyColor)).FilterMaybes();

                // Check one and two fields forward.
                var rowBase = pawn.Color == Color.White ? 1 : 6;
                var rowDelta = pawn.Color == Color.White ? 1 : -1;

                var oneForward = position.Offset(rowDelta, 0);
                var oneFreeForward = oneForward.Guard(p => !game.Board.IsOccupied(p));

                var twoForwardValid = oneFreeForward.Guard(p => p.Row == rowBase + rowDelta);
                var twoForward = twoForwardValid.Bind(p => p.Offset(rowDelta, 0));
                var twoFreeForward = twoForward.Guard(p => !game.Board.IsOccupied(p));

                var forwardPositions = oneFreeForward.Yield().Union(twoFreeForward.Yield());

                // Check en passant.
                var enPassantCommand = this.enPassantRule.GetCommand(game, position, pawn);

                // Create commands and put it all together.
                var hitCommands = hits.SelectMany
                (
                    enemy =>
                    {
                        var removeCommand = new RemoveCommand(enemy);
                        var moveCommand = new MoveCommand(position, enemy.Position, pawn);
                        var sequenceCommand = new SequenceCommand(removeCommand, moveCommand);
                        var promotionCommands = this.promotionRule.GetCommands(enemy.Position, pawn);

                        if (promotionCommands.Any())
                        {
                            return promotionCommands.Select
                            (
                                c => new SequenceCommand(sequenceCommand, c)
                            );
                        }

                        return sequenceCommand.Yield();
                    }
                );

                var moveCommands = forwardPositions.SelectMany<Position, ICommand>
                (
                    target =>
                    {
                        var moveCommand = new MoveCommand(position, target, pawn);
                        var promotionCommands = this.promotionRule.GetCommands(target, pawn);

                        if (promotionCommands.Any())
                        {
                            return promotionCommands.Select
                            (
                                c => new SequenceCommand(moveCommand, c)
                            );
                        }

                        return moveCommand.Yield();
                    }
                );

                return hitCommands.Union(moveCommands).Union(enPassantCommand.Yield());
            };
        }

        /// <summary>
        /// Gets all possible movement commands for a specified queen.
        /// </summary>
        /// <param name="queen">The analyzed queen.</param>
        /// <returns>
        /// Given a game state and a position for the queen,
        /// returns all possible movement commands for the queen.
        /// </returns>
        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Queen queen)
        {
            return this.GetThreatCommands(queen);
        }

        /// <summary>
        /// Gets all possible movement commands for a specified rook.
        /// </summary>
        /// <param name="rook">The analyzed rook.</param>
        /// <returns>
        /// Given a game state and a position for the rook,
        /// returns all possible movement commands for the rook.
        /// </returns>
        public Func<ChessGame, Position, IEnumerable<ICommand>> Visit(Rook rook)
        {
            return this.GetThreatCommands(rook);
        }

        /// <summary>
        /// Gets all possible movement commands for a chess piece based on its threatened
        /// chess board fields and opponent pieces.
        /// </summary>
        /// <param name="piece">The analyzed chess piece.</param>
        /// <returns>
        /// Given a game state and a position for the chess piece,
        /// returns all possible movement commands for the chess piece.
        /// </returns>
        private Func<ChessGame, Position, IEnumerable<ICommand>> GetThreatCommands(ChessPiece piece)
        {
            return (game, position) => this.GetThreatCommands(game.Board, position, piece);
        }

        /// <summary>
        /// Gets all possible movement commands for a chess piece based on its threatened
        /// chess board fields and opponent pieces.
        /// </summary>
        /// <param name="board">The current chess board state.</param>
        /// <param name="position">The position of the analyzed chess piece.</param>
        /// <param name="piece">The analyzed chess piece.</param>
        /// <returns>Returns all possible movement commands for the chess piece.</returns>
        private IEnumerable<ICommand> GetThreatCommands(Board board, Position position, ChessPiece piece)
        {
            var targets = piece.Accept(this.threatAnalyzer)(board, position);

            foreach (var target in targets)
            {
                var moveCommand = new MoveCommand(position, target, piece);
                var enemy = board.GetPiece(target, piece.Color.Toggle());

                yield return enemy.GetOrElse<ICommand>(
                    e => new SequenceCommand(new RemoveCommand(e), moveCommand),
                    moveCommand
                );
            }
        }
    }
}