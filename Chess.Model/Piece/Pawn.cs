//-----------------------------------------------------------------------
// <copyright file="Pawn.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// Represents a pawn chess piece.
    /// </summary>
    public class Pawn : ChessPiece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pawn"/> class.
        /// </summary>
        /// <param name="color">The color of the pawn.</param>
        public Pawn(Color color) : base(color)
        {
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Pawn"/>.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        public override void Accept(IChessPieceVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call its implementation for <see cref="Pawn"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the pawn.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called.</param>
        /// <returns>The result of the visitor when processing the pawn.</returns>
        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}