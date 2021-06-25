//-----------------------------------------------------------------------
// <copyright file="Rook.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// Represents a rook chess piece.
    /// </summary>
    public class Rook : ChessPiece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rook"/> class.
        /// </summary>
        /// <param name="color">The color of the rook.</param>
        public Rook(Color color) : base(color)
        {
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Rook"/>.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        public override void Accept(IChessPieceVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Rook"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the rook.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        /// <returns>The result of the visitor when processing the rook.</returns>
        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}