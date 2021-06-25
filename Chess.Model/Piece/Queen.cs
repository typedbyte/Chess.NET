//-----------------------------------------------------------------------
// <copyright file="Queen.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// Represents a queen chess piece.
    /// </summary>
    public class Queen : ChessPiece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Queen"/> class.
        /// </summary>
        /// <param name="color">The color of the queen.</param>
        public Queen(Color color) : base(color)
        {
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Queen"/>.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        public override void Accept(IChessPieceVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Queen"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the queen.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        /// <returns>The result of the visitor when processing the queen.</returns>
        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}