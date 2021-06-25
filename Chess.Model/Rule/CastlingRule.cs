//-----------------------------------------------------------------------
// <copyright file="CastlingRule.cs">
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
    /// Represents the castling rule of a standard chess game.
    /// </summary>
    public class CastlingRule : IChessPieceVisitor<bool>, ICommandVisitor<Func<Position, bool>>
    {
        /// <summary>
        /// Represents the threat analyzer for validating castling moves.
        /// </summary>
        private readonly ThreatAnalyzer threatAnalyzer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastlingRule"/> class.
        /// </summary>
        /// <param name="threatAnalyzer">The threat analyzer for validating castling moves.</param>
        public CastlingRule(ThreatAnalyzer threatAnalyzer)
        {
            Validation.NotNull(threatAnalyzer, nameof(threatAnalyzer));
            this.threatAnalyzer = threatAnalyzer;
        }

        /// <summary>
        /// Gets all possible castling commands for a specific king.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position of the analyzed king.</param>
        /// <param name="king">The analyzed king.</param>
        /// <returns>A sequence of possible castling commands.</returns>
        public IEnumerable<ICommand> GetCommands(ChessGame game, Position position, King king)
        {
            // Check if the king is in the right place.
            var expectedPosition =
                    king.Color == Color.White
                        ? new Position(0, 4)
                        : new Position(7, 4);

            if (!position.Equals(expectedPosition))
            {
                return Enumerable.Empty<ICommand>();
            }

            // Get the threatened fields of the enemy, but lazily because we might not need them.
            var threats = new Lazy<List<Position>>
            (
                () =>
                {
                    var enemies = game.Board.GetPieces(king.Color.Toggle());
                    var stream = enemies.SelectMany(p => this.threatAnalyzer.GetThreats(game.Board, p));
                    return stream.ToList();
                }
            );

            return this.GetCommands(game, position, king, threats);
        }

        /// <summary>
        /// Checks if a bishop is a rook.
        /// </summary>
        /// <param name="_">The analyzed bishop.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Bishop _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a king is a rook.
        /// </summary>
        /// <param name="_">The analyzed king.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(King _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a knight is a rook.
        /// </summary>
        /// <param name="_">The analyzed knight.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Knight _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a pawn is a rook.
        /// </summary>
        /// <param name="_">The analyzed pawn.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Pawn _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a queen is a rook.
        /// </summary>
        /// <param name="_">The analyzed queen.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Queen _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a rook is a rook.
        /// </summary>
        /// <param name="_">The analyzed rook.</param>
        /// <returns>Returns always true.</returns>
        public bool Visit(Rook _)
        {
            return true;
        }

        /// <summary>
        /// Checks if a chess piece has moved away from a specific position.
        /// </summary>
        /// <param name="_">The command to be checked for a move, which is always ignored.</param>
        /// <returns>Given a position, returns always false.</returns>
        public Func<Position, bool> Visit(EndTurnCommand _)
        {
            return _ => false;
        }

        /// <summary>
        /// Checks if a chess piece has moved away from a specific position.
        /// </summary>
        /// <param name="command">The command to be checked for a move.</param>
        /// <returns>Given a position, returns if a chess piece has moved away from it.</returns>
        public Func<Position, bool> Visit(MoveCommand command)
        {
            return position => command.Source.Equals(position);
        }

        /// <summary>
        /// Checks if a chess piece has moved away from a specific position.
        /// </summary>
        /// <param name="_">The command to be checked for a move, which is always ignored.</param>
        /// <returns>Given a position, returns always false.</returns>
        public Func<Position, bool> Visit(RemoveCommand _)
        {
            return _ => false;
        }

        /// <summary>
        /// Checks if a chess piece has moved away from a specific position.
        /// </summary>
        /// <param name="command">The command to be checked for a move.</param>
        /// <returns>Given a position, returns if a chess piece has moved away from it.</returns>
        public Func<Position, bool> Visit(SequenceCommand command)
        {
            return position =>
                command.FirstCommand.Accept(this)(position) ||
                command.SecondCommand.Accept(this)(position);
        }

        /// <summary>
        /// Checks if a chess piece has moved away from a specific position.
        /// </summary>
        /// <param name="_">The command to be checked for a move, which is always ignored.</param>
        /// <returns>Given a position, returns always false.</returns>
        public Func<Position, bool> Visit(SetLastUpdateCommand _)
        {
            return _ => false;
        }

        /// <summary>
        /// Checks if a chess piece has moved away from a specific position.
        /// </summary>
        /// <param name="_">The command to be checked for a move, which is always ignored.</param>
        /// <returns>Given a position, returns always false.</returns>
        public Func<Position, bool> Visit(SpawnCommand _)
        {
            return _ => false;
        }

        /// <summary>
        /// Gets all castling commands for a specific king.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position of the analyzed king.</param>
        /// <param name="king">The analyzed king.</param>
        /// <param name="threats">The positions that are threatened by chess pieces of the opponent.</param>
        /// <returns>A sequence of possible castling commands.</returns>
        private IEnumerable<ICommand> GetCommands(ChessGame game, Position position, King king, Lazy<List<Position>> threats)
        {
            var maybeLeftRook = game.Board.GetPiece(position.Row, 0, king.Color);
            var maybeRightRook = game.Board.GetPiece(position.Row, 7, king.Color);
            var leftRook = maybeLeftRook.Guard(r => r.Accept(this));
            var rightRook = maybeRightRook.Guard(r => r.Accept(this));

            foreach (var rook in leftRook.Yield().Union(rightRook.Yield()))
            {
                var direction = position.Column > rook.Position.Column ? -1 : 1;
                var oneNext = new Position(position.Row, position.Column + direction);
                var twoNext = new Position(position.Row, oneNext.Column + direction);

                // In order to perform a castling ...
                if
                (
                    // ... the fields between the king and the took must be empty
                    !game.Board.IsOccupied(oneNext) &&
                    !game.Board.IsOccupied(twoNext) &&
                    // ... the fields between the king and the rook must not be threatened
                    !threats.Value.Contains(oneNext) &&
                    !threats.Value.Contains(twoNext) &&
                    // ... the king must not be threatened
                    !threats.Value.Contains(position) &&
                    // ... the king must not have moved during the game
                    // ... the rook must not have moved during the game
                    !game.History.Any(u =>
                    {
                        var eval = u.Command.Accept(this);
                        return eval(position) || eval(rook.Position);
                    })
                )
                {
                    yield return new SequenceCommand
                    (
                        new MoveCommand(position, twoNext, king),
                        new MoveCommand(rook, oneNext)
                    );
                }
            }
        }
    }
}