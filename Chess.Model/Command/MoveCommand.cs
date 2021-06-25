//-----------------------------------------------------------------------
// <copyright file="MoveCommand.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;

    /// <summary>
    /// A command which indicates a chess piece move.
    /// </summary>
    public class MoveCommand : ICommand
    {
        /// <summary>
        /// Represents the source position of the chess piece.
        /// </summary>
        public readonly Position Source;

        /// <summary>
        /// Represents the target position of the chess piece.
        /// </summary>
        public readonly Position Target;

        /// <summary>
        /// Represents the moved chess piece.
        /// </summary>
        public readonly ChessPiece Piece;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCommand"/> class.
        /// </summary>
        /// <param name="piece">The placed chess piece to be moved.</param>
        /// <param name="target">The target position of the chess piece.</param>
        public MoveCommand(PlacedPiece piece, Position target) : this(piece.Position, target, piece.Piece)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCommand"/> class.
        /// </summary>
        /// <param name="source">The source position of the chess piece.</param>
        /// <param name="target">The target position of the chess piece.</param>
        /// <param name="piece">The moved chess piece.</param>
        public MoveCommand(Position source, Position target, ChessPiece piece)
        {
            Validation.NotNull(source, nameof(source));
            Validation.NotNull(target, nameof(target));
            Validation.NotNull(piece, nameof(piece));

            this.Source = source;
            this.Target = target;
            this.Piece = piece;
        }

        /// <summary>
        /// Applies the command to a chess game state.
        /// </summary>
        /// <param name="game">The old chess game state.</param>
        /// <returns>The new chess game state, if the command succeeds.</returns>
        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return game.Board.Remove(this.Source).Bind(b => b.Add(this.Target, this.Piece)).Map
            (
                newBoard => game.SetBoard(newBoard)
            );
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="visitor">The command visitor to be called.</param>
        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="MoveCommand"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the command.</typeparam>
        /// <param name="visitor">The command visitor to be called.</param>
        /// <returns>The result of the visitor when processing the command.</returns>
        public T Accept<T>(ICommandVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}