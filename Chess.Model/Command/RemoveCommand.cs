//-----------------------------------------------------------------------
// <copyright file="RemoveCommand.cs">
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
    /// A command which indicates a chess piece removal.
    /// </summary>
    public class RemoveCommand : ICommand
    {
        /// <summary>
        /// Represents the position of the chess piece to be removed.
        /// </summary>
        public readonly Position Position;

        /// <summary>
        /// Represents the chess piece to be removed.
        /// </summary>
        public readonly ChessPiece Piece;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommand"/> class.
        /// </summary>
        /// <param name="piece">The placed chess piece to be removed.</param>
        public RemoveCommand(PlacedPiece piece) : this(piece.Position, piece.Piece)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommand"/> class.
        /// </summary>
        /// <param name="position">The position of the chess piece to be removed.</param>
        /// <param name="piece">The chess piece to be removed.</param>
        public RemoveCommand(Position position, ChessPiece piece)
        {
            Validation.NotNull(position, nameof(position));
            Validation.NotNull(piece, nameof(piece));

            this.Position = position;
            this.Piece = piece;
        }

        /// <summary>
        /// Applies the command to a chess game state.
        /// </summary>
        /// <param name="game">The old chess game state.</param>
        /// <returns>The new chess game state, if the command succeeds.</returns>
        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return game.Board.Remove(this.Position).Map
            (
                newBoard => game.SetBoard(newBoard)
            );
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="RemoveCommand"/>.
        /// </summary>
        /// <param name="visitor">The command visitor to be called.</param>
        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="RemoveCommand"/>.
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