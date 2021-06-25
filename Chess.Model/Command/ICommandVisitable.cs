//-----------------------------------------------------------------------
// <copyright file="ICommandVisitable.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    /// <summary>
    /// Represents the ability to accept an <see cref="ICommandVisitor"/>
    /// or an <see cref="ICommandVisitor{T}"/>.
    /// </summary>
    public interface ICommandVisitable
    {
        /// <summary>
        /// Accepts a command visitor in order to call it back based on the type of the command.
        /// </summary>
        /// <param name="visitor">The command visitor to be called back by the command.</param>
        void Accept(ICommandVisitor visitor);

        /// <summary>
        /// Accepts a command visitor in order to call it back based on the type of the command.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the command.</typeparam>
        /// <param name="visitor">The command visitor to be called back by the command.</param>
        /// <returns>The result of the visitor when processing the command.</returns>
        T Accept<T>(ICommandVisitor<T> visitor);
    }
}