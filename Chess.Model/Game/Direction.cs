//-----------------------------------------------------------------------
// <copyright file="Direction.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using System.Collections.Immutable;

    /// <summary>
    /// Represents a two-dimensional direction.
    /// </summary>
    public class Direction
    {
        /// <summary>
        /// Represents the four integral vectors in the directions up, right, down and left.
        /// </summary>
        public static readonly ImmutableArray<Direction> Orthogonals = ImmutableArray.Create
        (
            new Direction(1, 0),
            new Direction(0, 1),
            new Direction(-1, 0),
            new Direction(0, -1)
        );

        /// <summary>
        /// Represents the four integral vectors in the directions left up, right up, right down and left down.
        /// </summary>
        public static readonly ImmutableArray<Direction> Diagonals = ImmutableArray.Create
        (
            new Direction(1, -1),
            new Direction(1, 1),
            new Direction(-1, 1),
            new Direction(-1, -1)
        ); 
        
        /// <summary>
        /// Represents the row component of the direction.
        /// </summary>
        public readonly int RowDelta;

        /// <summary>
        /// Represents the column component of the direction.
        /// </summary>
        public readonly int ColumnDelta;

        /// <summary>
        /// Initializes a new instance of the <see cref="Direction"/> class.
        /// </summary>
        /// <param name="rowDelta">The row component of the direction.</param>
        /// <param name="columnDelta">The column component of the direction.</param>
        public Direction(int rowDelta, int columnDelta)
        {
            this.RowDelta = rowDelta;
            this.ColumnDelta = columnDelta;
        }
    }
}