//-----------------------------------------------------------------------
// <copyright file="IChessPieceVisitor.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// The chess piece visitor is able to process a chess piece based on the type of the piece.
    /// </summary>
    public interface IChessPieceVisitor
    {
        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Bishop"/>.
        /// </summary>
        /// <param name="bishop">The <see cref="Bishop"/> that should be processed.</param>
        void Visit(Bishop bishop);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="King"/>.
        /// </summary>
        /// <param name="king">The <see cref="King"/> that should be processed.</param>
        void Visit(King king);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Knight"/>.
        /// </summary>
        /// <param name="knight">The <see cref="Knight"/> that should be processed.</param>
        void Visit(Knight knight);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Pawn"/>.
        /// </summary>
        /// <param name="pawn">The <see cref="Pawn"/> that should be processed.</param>
        void Visit(Pawn pawn);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Queen"/>.
        /// </summary>
        /// <param name="queen">The <see cref="Queen"/> that should be processed.</param>
        void Visit(Queen queen);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Rook"/>.
        /// </summary>
        /// <param name="rook">The <see cref="Rook"/> that should be processed.</param>
        void Visit(Rook rook);
    }

    /// <summary>
    /// The chess piece visitor is able to process a chess piece based on the type of the piece,
    /// returning a value of type <typeparamref name="T"/> when doing so.
    /// </summary>
    /// <typeparam name="T">The result type of processing a chess piece.</typeparam>
    public interface IChessPieceVisitor<T>
    {
        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Bishop"/>.
        /// </summary>
        /// <param name="bishop">The <see cref="Bishop"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="Bishop"/>.</returns>
        T Visit(Bishop bishop);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="King"/>.
        /// </summary>
        /// <param name="king">The <see cref="King"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="King"/>.</returns>
        T Visit(King king);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Knight"/>.
        /// </summary>
        /// <param name="knight">The <see cref="Knight"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="Knight"/>.</returns>
        T Visit(Knight knight);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Pawn"/>.
        /// </summary>
        /// <param name="pawn">The <see cref="Pawn"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="Pawn"/>.</returns>
        T Visit(Pawn pawn);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Queen"/>.
        /// </summary>
        /// <param name="queen">The <see cref="Queen"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="Queen"/>.</returns>
        T Visit(Queen queen);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="Rook"/>.
        /// </summary>
        /// <param name="rook">The <see cref="Rook"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="Rook"/>.</returns>
        T Visit(Rook rook);
    }
}