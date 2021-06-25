//-----------------------------------------------------------------------
// <copyright file="IMaybe.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a type which may or may not contain a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the possibly contained value.</typeparam>
    public interface IMaybe<T>
    {
        /// <summary>
        /// Gets a value indicating whether the object contains a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <value>True if the object contains a value, or else false.</value>
        bool HasValue { get; }

        /// <summary>
        /// Applies a function to the contained value, if available.
        /// </summary>
        /// <typeparam name="V">The result type of the passed function.</typeparam>
        /// <param name="func">The transformation function for the value.</param>
        /// <returns>
        /// An <see cref="IMaybe{V}"/> which holds the result,
        /// if there was a contained object of type <typeparamref name="T"/> to begin with,
        /// and if the transformation function succeeds.
        /// </returns>
        IMaybe<V> Bind<V>(Func<T, IMaybe<V>> func);

        /// <summary>
        /// Applies an action to the contained value, if available.
        /// </summary>
        /// <param name="func">The action that makes use of the contained value.</param>
        void Do(Action<T> func);

        /// <summary>
        /// Applies an action to the contained value, if available.
        /// If the value is not available, an alternative action is executed.
        /// </summary>
        /// <param name="func">The action that makes use of the contained value.</param>
        /// <param name="alternative">The alternative action, if the contained value is not available.</param>
        void DoOrElse(Action<T> func, Action alternative);

        /// <summary>
        /// Gets the contained value, if available.
        /// If the value is not available, an alternative value is returned.
        /// </summary>
        /// <param name="alternative">The alternative value, if the contained value is not available.</param>
        /// <returns>The contained value, if available, or else the alternative.</returns>
        T GetOrElse(T alternative);

        /// <summary>
        /// Gets the contained value, if available.
        /// If the value is not available, an alternative value is returned.
        /// </summary>
        /// <param name="alternative">
        /// The function which produces the alternative value,
        /// if the contained value is not available.
        /// </param>
        /// <returns>The contained value if available, or else the alternative.</returns>
        T GetOrElse(Func<T> alternative);

        /// <summary>
        /// Gets the contained value, if available, and applies a function to it.
        /// </summary>
        /// <typeparam name="V">The result type of the applied function.</typeparam>
        /// <param name="func">The function that makes use of the contained value.</param>
        /// <param name="alternative">The alternative value, if the contained value is not available.</param>
        /// <returns>The transformed value, if available, or else the alternative.</returns>
        V GetOrElse<V>(Func<T, V> func, V alternative);

        /// <summary>
        /// Gets the contained value, if available, and applies a function to it.
        /// </summary>
        /// <typeparam name="V">The result type of the applied function.</typeparam>
        /// <param name="func">The function that makes use of the contained value.</param>
        /// <param name="alternative">
        /// The function which produces an alternative value,
        /// if the contained value is not available.
        /// </param>
        /// <returns>The transformed value, if available, or else the alternative.</returns>
        V GetOrElse<V>(Func<T, V> func, Func<V> alternative);

        /// <summary>
        /// Applies a function to the contained value, if available.
        /// </summary>
        /// <typeparam name="V">The result type of the passed function.</typeparam>
        /// <param name="func">The transformation function for the value.</param>
        /// <returns>
        /// An <see cref="IMaybe{V}"/> which holds the result,
        /// if there was a contained object of type <typeparamref name="T"/> to begin with.
        /// </returns>
        IMaybe<V> Map<V>(Func<T, V> func);
    }

    /// <summary>
    /// Provides extension methods related to the <see cref="IMaybe{T}"/> interface.
    /// </summary>
    public static class MaybeExtension
    {
        /// <summary>
        /// Makes an assumption about the value contained in an <see cref="IMaybe{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the possibly contained value.</typeparam>
        /// <param name="maybe">The object which possibly contains a value of type <typeparamref name="T"/>.</param>
        /// <param name="predicate">The assumption about the contained value.</param>
        /// <returns>The original <see cref="IMaybe{T}"/>, if the assumption holds, or else <see cref="Nothing{T}"/>.</returns>
        public static IMaybe<T> Guard<T>(this IMaybe<T> maybe, Predicate<T> predicate)
        {
            return maybe.Bind(p => predicate(p) ? maybe : Nothing<T>.Instance);
        }

        /// <summary>
        /// Removes all <see cref="IMaybe{T}"/> instances from an <see cref="IEnumerable{IMaybe{T}}"/>
        /// which do not contain a value.
        /// </summary>
        /// <typeparam name="T">The type of the possibly contained value.</typeparam>
        /// <param name="items">The sequence to be filtered.</param>
        /// <returns>A sequence which yields only the contained values.</returns>
        public static IEnumerable<T> FilterMaybes<T>(this IEnumerable<IMaybe<T>> items)
        {
            return items.SelectMany(i => i.Yield());
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a condition,
        /// or <see cref="Nothing{T}"/> if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the values contained in the sequence.</typeparam>
        /// <param name="items">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <param name="predicate">The condition to test each element for.</param>
        /// <returns>A <see cref="Just{T}"/> containing the found value, or <see cref="Nothing{T}"/>.</returns>
        public static IMaybe<T> Find<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return new Just<T>(item);
                }
            }

            return Nothing<T>.Instance;
        }

        /// <summary>
        /// Converts a type which possibly contains some values into a sequence of those values.
        /// </summary>
        /// <typeparam name="T">The type of the values contained in the sequence.</typeparam>
        /// <param name="maybe">The type which possibly contains the values.</param>
        /// <returns>
        /// The contained sequence of values, or an empty <see cref="IEnumerable{T}"/>
        /// if the values do not exist.
        /// </returns>
        public static IEnumerable<T> ToEnumerable<T>(this IMaybe<IEnumerable<T>> maybe)
        {
            return maybe.Yield().SelectMany(p => p);
        }

        /// <summary>
        /// Converts an <see cref="IMaybe{T}"/> into an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the possibly contained value.</typeparam>
        /// <param name="maybe">The type which possibly contains a value.</param>
        /// <returns>
        /// A singleton <see cref="IEnumerable{T}"/> if the specified object
        /// contains a value, or else an empty sequence.
        /// </returns>
        public static IEnumerable<T> Yield<T>(this IMaybe<T> maybe)
        {
            return maybe.GetOrElse
            (
                v => v.Yield(),
                Enumerable.Empty<T>()
            );
        }
    }
}