//-----------------------------------------------------------------------
// <copyright file="PositionVM.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Game
{
    using Chess.Model.Game;
    using System;

    /// <summary>
    /// Represents the view model of a chess board position.
    /// </summary>
    public class PositionVM : IEquatable<Position>
    {
        /// <summary>
        /// Represents the position on the chess board.
        /// </summary>
        public readonly Position Position;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionVM"/> class.
        /// </summary>
        /// <param name="position">The position on the chess board.</param>
        public PositionVM(Position position)
        {
            this.Position = position ?? throw new ArgumentNullException(nameof(position));
        }

        /// <summary>
        /// Gets the row of the position, where 0 represents the bottom row.
        /// </summary>
        /// <value>The row component of the position.</value>
        public int Row => this.Position.Row;

        /// <summary>
        /// Gets the column of the position, where 0 represents the leftmost column.
        /// </summary>
        /// <value>The column component of the position.</value>
        public int Column => this.Position.Column;

        /// <summary>
        /// Indicates whether the current position is equal to another position.
        /// </summary>
        /// <param name="other">The position to compare with this position.</param>
        /// <returns>True if the current position is equal to the other one, or else false.</returns>
        public bool Equals(Position other)
        {
            return this.Position.Equals(other);
        }
    }
}