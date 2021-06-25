//-----------------------------------------------------------------------
// <copyright file="Nothing.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Data
{
    using System;

    /// <summary>
    /// Represents a type which does not contain a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the non-contained value.</typeparam>
    public class Nothing<T> : IMaybe<T>
    {
        /// <summary>
        /// Represents the default instance of the stateless class <see cref="Nothing{T}"/>.
        /// </summary>
        public static readonly Nothing<T> Instance = new();

        /// <summary>
        /// Gets a value indicating whether the object contains a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <value>Returns always false.</value>
        public bool HasValue => false;

        /// <summary>
        /// Applies a function to the contained value, which is not possible.
        /// </summary>
        /// <typeparam name="V">The result type of the passed function.</typeparam>
        /// <param name="_">The transformation function for the value, which is never executed.</param>
        /// <returns>Returns always <see cref="Nothing{V}"/>.</returns>
        public IMaybe<V> Bind<V>(Func<T, IMaybe<V>> _)
        {
            return Nothing<V>.Instance;
        }

        /// <summary>
        /// Applies an action to the contained value, which is not possible.
        /// </summary>
        /// <param name="_">The action that makes use of the contained value, which is never executed.</param>
        public void Do(Action<T> _)
        {
            return;
        }

        /// <summary>
        /// Applies an action to the contained value, which is not possible.
        /// It always executes an alternative action.
        /// </summary>
        /// <param name="_">The action that makes use of the contained value, which is never executed.</param>
        /// <param name="alternative">The alternative action, which is always used.</param>
        public void DoOrElse(Action<T> _, Action alternative)
        {
            alternative();
        }

        /// <summary>
        /// Gets the contained value, which is not possible.
        /// It always returns an alternative value.
        /// </summary>
        /// <param name="alternative">The alternative value, which is always used.</param>
        /// <returns>The alternative value.</returns>
        public T GetOrElse(T alternative)
        {
            return alternative;
        }

        /// <summary>
        /// Gets the contained value, which is not possible.
        /// It always returns an alternative value.
        /// </summary>
        /// <param name="alternative">The function which produces the alternative value, which is always used.</param>
        /// <returns>The alternative value.</returns>
        public T GetOrElse(Func<T> alternative)
        {
            return alternative();
        }

        /// <summary>
        /// Gets the contained value and applies a function to it, which is not possible.
        /// </summary>
        /// <typeparam name="V">The result type of the applied function.</typeparam>
        /// <param name="_">The function that makes use of the contained value, which is never executed.</param>
        /// <param name="alternative">The alternative value, which is always used.</param>
        /// <returns>The alternative value.</returns>
        public V GetOrElse<V>(Func<T, V> _, V alternative)
        {
            return alternative;
        }

        /// <summary>
        /// Gets the contained value and applies a function to it, which is not possible.
        /// </summary>
        /// <typeparam name="V">The result type of the applied function.</typeparam>
        /// <param name="_">The function that makes use of the contained value, which is never executed.</param>
        /// <param name="alternative"> The function which produces an alternative value, which is always used.</param>
        /// <returns>The alternative value.</returns>
        public V GetOrElse<V>(Func<T, V> _, Func<V> alternative)
        {
            return alternative();
        }

        /// <summary>
        /// Applies a function to the contained value, which is not possible.
        /// </summary>
        /// <typeparam name="V">The result type of the passed function.</typeparam>
        /// <param name="_">The transformation function for the value, which is never executed.</param>
        /// <returns>Returns always <see cref="Nothing{V}"/>.</returns>
        public IMaybe<V> Map<V>(Func<T, V> _)
        {
            return Nothing<V>.Instance;
        }
    }
}