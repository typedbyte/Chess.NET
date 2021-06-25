//-----------------------------------------------------------------------
// <copyright file="PositionComparer.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a method that allows the comparison of two position objects.
    /// </summary>
    public class PositionComparer : IComparer<Position>
    {
        /// <summary>
        /// Represents the default instance of the stateless class <see cref="PositionComparer"/>.
        /// </summary>
        public static readonly IComparer<Position> DefaultComparer = new PositionComparer();

        /// <summary>
        /// Compares two positions and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first position to compare.</param>
        /// <param name="y">The second position to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y.
        /// If the value is less than zero, then x is less than y.
        /// If the value is zero, then x equals y.
        /// If the value is greater than zero, then x is greater than y.
        /// </returns>
        public int Compare(Position x, Position y)
        {
            var rowComparison = x.Row.CompareTo(y.Row);

            return
                rowComparison == 0
                    ? x.Column.CompareTo(y.Column)
                    : rowComparison;
        }
    }
}