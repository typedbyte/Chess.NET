//-----------------------------------------------------------------------
// <copyright file="Just.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Data
{
    using System;

    /// <summary>
    /// Represents a type which contains a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the contained value.</typeparam>
    public class Just<T> : IMaybe<T>
    {
        /// <summary>
        /// Represents the contained value.
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Just{T}"/> class.
        /// </summary>
        /// <param name="value">The contained value.</param>
        public Just(T value)
        {
            Validation.NotNull(value, nameof(value));
            this.Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the object contains a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <value>Returns always true.</value>
        public bool HasValue => true;

        /// <summary>
        /// Applies a function to the contained value.
        /// </summary>
        /// <typeparam name="V">The result type of the passed function.</typeparam>
        /// <param name="func">The transformation function for the value.</param>
        /// <returns>
        /// An <see cref="IMaybe{V}"/> which holds the result, if the transformation function succeeds.
        /// </returns>
        public IMaybe<V> Bind<V>(Func<T, IMaybe<V>> func)
        {
            return func(this.Value);
        }

        /// <summary>
        /// Applies an action to the contained value.
        /// </summary>
        /// <param name="func">The action that makes use of the contained value.</param>
        public void Do(Action<T> func)
        {
            func(this.Value);
        }

        /// <summary>
        /// Applies an action to the contained value.
        /// </summary>
        /// <param name="func">The action that makes use of the contained value.</param>
        /// <param name="_">The alternative action, which is never executed.</param>
        public void DoOrElse(Action<T> func, Action _)
        {
            func(this.Value);
        }

        /// <summary>
        /// Gets the contained value.
        /// </summary>
        /// <param name="_">The alternative value, which is never used.</param>
        /// <returns>The contained value.</returns>
        public T GetOrElse(T _)
        {
            return this.Value;
        }

        /// <summary>
        /// Gets the contained value.
        /// </summary>
        /// <param name="_">The function which produces the alternative value, which is never used.</param>
        /// <returns>The contained value.</returns>
        public T GetOrElse(Func<T> _)
        {
            return this.Value;
        }

        /// <summary>
        /// Gets the contained value and applies a function to it.
        /// </summary>
        /// <typeparam name="V">The result type of the applied function.</typeparam>
        /// <param name="func">The function that makes use of the contained value.</param>
        /// <param name="_">The alternative value, which is never used.</param>
        /// <returns>The transformed value.</returns>
        public V GetOrElse<V>(Func<T, V> func, V _)
        {
            return func(this.Value);
        }

        /// <summary>
        /// Gets the contained value and applies a function to it.
        /// </summary>
        /// <typeparam name="V">The result type of the applied function.</typeparam>
        /// <param name="func">The function that makes use of the contained value.</param>
        /// <param name="_">The function which produces an alternative value, which is never executed.</param>
        /// <returns>The transformed value.</returns>
        public V GetOrElse<V>(Func<T, V> func, Func<V> _)
        {
            return func(this.Value);
        }

        /// <summary>
        /// Applies a function to the contained value.
        /// </summary>
        /// <typeparam name="V">The result type of the passed function.</typeparam>
        /// <param name="func">The transformation function for the value.</param>
        /// <returns>An <see cref="IMaybe{V}"/> which holds the result.</returns>
        public IMaybe<V> Map<V>(Func<T, V> func)
        {
            return new Just<V>(func(this.Value));
        }
    }
}