//-----------------------------------------------------------------------
// <copyright file="SpawnCommand.cs">
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
    /// A command which indicates the introduction of a new chess piece.
    /// </summary>
    public class SpawnCommand : ICommand
    {
        /// <summary>
        /// Represents the position of the newly introduced chess piece.
        /// </summary>
        public readonly Position Position;

        /// <summary>
        /// Represents the newly introduced chess piece.
        /// </summary>
        public readonly ChessPiece Piece;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnCommand"/> class.
        /// </summary>
        /// <param name="position">The position of the newly introduced chess piece.</param>
        /// <param name="piece">The newly introduced chess piece.</param>
        public SpawnCommand(Position position, ChessPiece piece)
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
            return game.Board.Add(this.Position, this.Piece).Map
            (
                newBoard => game.SetBoard(newBoard)
            );
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="SpawnCommand"/>.
        /// </summary>
        /// <param name="visitor">The command visitor to be called.</param>
        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="SpawnCommand"/>.
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