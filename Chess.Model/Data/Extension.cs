//-----------------------------------------------------------------------
// <copyright file="Extension.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods related to the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Produces an endless stream of values.
        /// </summary>
        /// <typeparam name="T">The type of the produced values.</typeparam>
        /// <param name="start">The first value of the stream.</param>
        /// <param name="next">The function that calculates the next value of the stream based on the current value.</param>
        /// <returns>A sequence that allows to iterate the endless stream of values.</returns>
        public static IEnumerable<T> Repeat<T>(this T start, Func<T, T> next)
        {
            while (true)
            {
                yield return start;
                start = next(start);
            }
        }

        /// <summary>
        /// Wraps a single value of type <typeparamref name="T"/> into a singleton <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the wrapped value.</typeparam>
        /// <param name="item">The wrapped value.</param>
        /// <returns>A sequence that contains the specified item.</returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}