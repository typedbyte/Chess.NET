//-----------------------------------------------------------------------
// <copyright file="Validation.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Data
{
    using System;

    /// <summary>
    /// Provides methods for making assumptions about objects.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Assumes that <paramref name="obj"/> is not null,
        /// or else throws a <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="obj">The object to be assumed not null.</param>
        /// <param name="paramName">
        /// The parameter name to be reported in the <see cref="ArgumentNullException"/>.
        /// </param>
        public static void NotNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Assumes that <paramref name="obj"/> is between <paramref name="min"/> and <paramref name="max"/>,
        /// or else throws a <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object that is validated.</typeparam>
        /// <param name="obj">The object that is validated.</param>
        /// <param name="min">The inclusive lower bound of valid values.</param>
        /// <param name="max">The inclusive upper bound of valid values.</param>
        /// <param name="paramName">
        /// The parameter name to be reported in the <see cref="ArgumentOutOfRangeException"/>.
        /// </param>
        public static void InRange<T>(T obj, T min, T max, string paramName) where T : IComparable<T>
        {
            if (obj.CompareTo(min) < 0 || obj.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        /// <summary>
        /// Checks if <paramref name="obj"/> is between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object that is validated.</typeparam>
        /// <param name="obj">The object that is validated.</param>
        /// <param name="min">The inclusive lower bound of valid values.</param>
        /// <param name="max">The inclusive upper bound of valid values.</param>
        /// <returns>True if the object is within the valid range, or else false.</returns>
        public static bool IsInRange<T>(T obj, T min, T max) where T : IComparable<T>
        {
            return obj.CompareTo(min) >= 0 && obj.CompareTo(max) <= 0;
        }
    }
}