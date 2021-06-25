//-----------------------------------------------------------------------
// <copyright file="PlacedPiece.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using System;

    /// <summary>
    /// Represents a chess piece that is positioned on a chess board.
    /// </summary>
    public class PlacedPiece : ChessPiece
    {
        /// <summary>
        /// Represents the position of the chess piece on the chess board.
        /// </summary>
        public readonly Position Position;

        /// <summary>
        /// Represents the positioned chess piece.
        /// </summary>
        public readonly ChessPiece Piece;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlacedPiece"/> class.
        /// </summary>
        /// <param name="position">The position of the chess piece on the chess board.</param>
        /// <param name="piece">The positioned chess piece.</param>
        public PlacedPiece(Position position, ChessPiece piece) : base(piece.Color)
        {
            Validation.NotNull(position, nameof(position));
            Validation.NotNull(piece, nameof(piece));

            this.Position = position;
            this.Piece = piece;
        }

        /// <summary>
        /// Moves the chess piece in a specific direction.
        /// </summary>
        /// <param name="direction">The direction of the move.</param>
        /// <returns>
        /// The newly placed chess piece, if the new position is within the bounds of the chess board.
        /// </returns>
        public IMaybe<PlacedPiece> Move(Direction direction)
        {
            return this.Position.Offset(direction).Map
            (
                p => new PlacedPiece(p, this.Piece)
            );
        }

        /// <summary>
        /// Moves the chess piece to a new position.
        /// </summary>
        /// <param name="newPosition">The new position of the chess piece.</param>
        /// <returns>The newly placed chess piece.</returns>
        public PlacedPiece MoveTo(Position newPosition)
        {
            return new PlacedPiece(newPosition, this.Piece);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call it back based on the type of the piece.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called back by the piece.</param>
        public override void Accept(IChessPieceVisitor visitor)
        {
            this.Piece.Accept(visitor);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call it back based on the type of the piece.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the chess piece.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called back by the piece.</param>
        /// <returns>The result of the visitor when processing the chess piece.</returns>
        public override T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return this.Piece.Accept(visitor);
        }

        /// <summary>
        /// Indicates whether the current chess piece is equal to another chess piece.
        /// </summary>
        /// <param name="obj">The chess piece to compare with this chess piece.</param>
        /// <returns>True if the current chess piece is equal to the other one, or else false.</returns>
        public override bool Equals(ChessPiece obj)
        {
            return
                obj is PlacedPiece other &&
                this.Position.Equals(other.Position) &&
                this.Piece.Equals(other.Piece);
        }

        /// <summary>
        /// Indicates whether the current chess piece is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with this chess piece.</param>
        /// <returns>True if the current chess piece is equal to the other object, or else false.</returns>
        public override bool Equals(object obj)
        {
            return
                obj is PlacedPiece other &&
                this.Position.Equals(other.Position) &&
                this.Piece.Equals(other.Piece);
        }

        /// <summary>
        /// Calculates a hash code which represents the chess piece.
        /// </summary>
        /// <returns>A hash code for the chess piece.</returns>
        public override int GetHashCode()
        {
            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(this.Position);
            hashCodeBuilder.Add(this.Piece);
            return hashCodeBuilder.ToHashCode();
        }
    }
}