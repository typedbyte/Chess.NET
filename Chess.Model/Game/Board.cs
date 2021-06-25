//-----------------------------------------------------------------------
// <copyright file="Board.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using Chess.Model.Data;
    using Chess.Model.Piece;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    /// <summary>
    /// Represents a chess board.
    /// </summary>
    public class Board : IEnumerable<PlacedPiece>
    {
        /// <summary>
        /// Represents the chess pieces on the chess board.
        /// </summary>
        private readonly IImmutableDictionary<Position, ChessPiece> pieces;

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="pieces">The chess pieces on the chess board.</param>
        public Board(IImmutableDictionary<Position, ChessPiece> pieces)
        {
            Validation.NotNull(pieces, nameof(pieces));
            this.pieces = pieces;
        }

        /// <summary>
        /// Introduces a new chess piece to the chess board at a specific position.
        /// </summary>
        /// <param name="position">The position of the newly introduced chess piece.</param>
        /// <param name="piece">The newly introduced chess piece.</param>
        /// <returns>The new chess board, if the specified position has not been occupied.</returns>
        public IMaybe<Board> Add(Position position, ChessPiece piece)
        {
            try
            {
                return new Just<Board>(new Board(this.pieces.Add(position, piece)));
            }
            catch
            {
                return Nothing<Board>.Instance;
            }
        }

        /// <summary>
        /// Removes a chess piece from the chess board at a given position.
        /// </summary>
        /// <param name="position">The position of the chess piece to be removed.</param>
        /// <returns>The new chess board, if the specified position was indeed occupied by a chess piece.</returns>
        public IMaybe<Board> Remove(Position position)
        {
            var newPieces = this.pieces.Remove(position);

            return
                this.pieces == newPieces
                    ? Nothing<Board>.Instance
                    : new Just<Board>(new Board(newPieces));
        }

        /// <summary>
        /// Checks if a specified position of the chess board is occupied by a chess piece.
        /// </summary>
        /// <param name="position">The position to be checked for occupation.</param>
        /// <returns>True if the specified position is occupied, or else false.</returns>
        public bool IsOccupied(Position position)
        {
            return this.GetPiece(position).HasValue;
        }

        /// <summary>
        /// Checks if a specified position of the chess board is occupied by a chess piece of a given color.
        /// </summary>
        /// <param name="position">The position to be checked for occupation.</param>
        /// <param name="color">The color of the possibly occupying chess piece.</param>
        /// <returns>True if the specified position is occupied by a chess piece of the given color, or else false.</returns>
        public bool IsOccupied(Position position, Color color)
        {
            return this.GetPiece(position, color).HasValue;
        }

        /// <summary>
        /// Gets a chess piece at a specific position of the chess board.
        /// </summary>
        /// <param name="position">The position to look for a chess piece.</param>
        /// <returns>The chess piece, if the specified position was indeed occupied by it.</returns>
        public IMaybe<PlacedPiece> GetPiece(Position position)
        {
            if (this.pieces.TryGetValue(position, out var piece))
            {
                return new Just<PlacedPiece>(new PlacedPiece(position, piece));
            }

            return Nothing<PlacedPiece>.Instance;
        }

        /// <summary>
        /// Gets a chess piece of a given color at a specific position of the chess board.
        /// </summary>
        /// <param name="position">The position to look for a chess piece.</param>
        /// <param name="color">The color of the chess piece that is possibly at the specified position.</param>
        /// <returns>The chess piece of the given color, if the specified position was indeed occupied by it.</returns>
        public IMaybe<PlacedPiece> GetPiece(Position position, Color color)
        {
            return this.GetPiece(position).Guard(p => p.Color == color);
        }

        /// <summary>
        /// Gets a chess piece at a specific position of the chess board.
        /// </summary>
        /// <param name="row">The row of the position to look for a chess piece.</param>
        /// <param name="column">The column of the position to look for a chess piece.</param>
        /// <returns>The chess piece, if the specified position was indeed occupied by it.</returns>
        public IMaybe<PlacedPiece> GetPiece(int row, int column)
        {
            return this.GetPiece(new Position(row, column));
        }

        /// <summary>
        /// Gets a chess piece of a given color at a specific position of the chess board.
        /// </summary>
        /// <param name="row">The row of the position to look for a chess piece.</param>
        /// <param name="column">The column of the position to look for a chess piece.</param>
        /// <param name="color">The color of the chess piece that is possibly at the specified position.</param>
        /// <returns>The chess piece of the given color, if the specified position was indeed occupied by it.</returns>
        public IMaybe<PlacedPiece> GetPiece(int row, int column, Color color)
        {
            return this.GetPiece(new Position(row, column), color);
        }

        /// <summary>
        /// Gets all chess pieces of a given color that are currently on the chess board.
        /// </summary>
        /// <param name="color">The color of the chess pieces.</param>
        /// <returns>The chess pieces of the specified color.</returns>
        public IEnumerable<PlacedPiece> GetPieces(Color color)
        {
            foreach (var pair in this.pieces)
            {
                if (pair.Value.Color == color)
                {
                    yield return new PlacedPiece(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the chess pieces of the chess board.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the chess pieces of the chess board.</returns>
        public IEnumerator<PlacedPiece> GetEnumerator()
        {
            foreach (var pair in this.pieces)
            {
                yield return new PlacedPiece(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the chess pieces of the chess board.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the chess pieces of the chess board.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}