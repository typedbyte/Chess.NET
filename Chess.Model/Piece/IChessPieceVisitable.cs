//-----------------------------------------------------------------------
// <copyright file="IChessPieceVisitable.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// Represents the ability to accept an <see cref="IChessPieceVisitor"/>
    /// or an <see cref="IChessPieceVisitor{T}"/>.
    /// </summary>
    public interface IChessPieceVisitable
    {
        /// <summary>
        /// Accepts a chess piece visitor in order to call it back based on the type of the piece.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called back by the piece.</param>
        void Accept(IChessPieceVisitor visitor);

        /// <summary>
        /// Accepts a chess piece visitor in order to call it back based on the type of the piece.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the chess piece.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called back by the piece.</param>
        /// <returns>The result of the visitor when processing the chess piece.</returns>
        T Accept<T>(IChessPieceVisitor<T> visitor);
    }
}