//-----------------------------------------------------------------------
// <copyright file="Knight.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// Represents a knight chess piece.
    /// </summary>
    public class Knight : ChessPiece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Knight"/> class.
        /// </summary>
        /// <param name="color">The color of the knight.</param>
        public Knight(Color color) : base(color)
        {
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Knight"/>.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        public override void Accept(IChessPieceVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Knight"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the knight.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        /// <returns>The result of the visitor when processing the knight.</returns>
        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}