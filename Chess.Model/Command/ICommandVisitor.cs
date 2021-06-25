//-----------------------------------------------------------------------
// <copyright file="ICommandVisitor.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    /// <summary>
    /// The command visitor is able to process a chess command based on the type of the command.
    /// </summary>
    public interface ICommandVisitor
    {
        /// <summary>
        /// Instruct the visitor to process a specific <see cref="EndTurnCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="EndTurnCommand"/> that should be processed.</param>
        void Visit(EndTurnCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="MoveCommand"/> that should be processed.</param>
        void Visit(MoveCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="RemoveCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="RemoveCommand"/> that should be processed.</param>
        void Visit(RemoveCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="SequenceCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="SequenceCommand"/> that should be processed.</param>
        void Visit(SequenceCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="SetLastUpdateCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="SetLastUpdateCommand"/> that should be processed.</param>
        void Visit(SetLastUpdateCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="SpawnCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="SpawnCommand"/> that should be processed.</param>
        void Visit(SpawnCommand command);
    }

    /// <summary>
    /// The command visitor is able to process a chess command based on the type of the command,
    /// returning a value of type <typeparamref name="T"/> when doing so.
    /// </summary>
    /// <typeparam name="T">The result type of processing a command.</typeparam>
    public interface ICommandVisitor<T>
    {
        /// <summary>
        /// Instruct the visitor to process a specific <see cref="EndTurnCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="EndTurnCommand"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="EndTurnCommand"/>.</returns>
        T Visit(EndTurnCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="MoveCommand"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="MoveCommand"/>.</returns>
        T Visit(MoveCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="RemoveCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="RemoveCommand"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="RemoveCommand"/>.</returns>
        T Visit(RemoveCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="SequenceCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="SequenceCommand"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="SequenceCommand"/>.</returns>
        T Visit(SequenceCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="SetLastUpdateCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="SetLastUpdateCommand"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="SetLastUpdateCommand"/>.</returns>
        T Visit(SetLastUpdateCommand command);

        /// <summary>
        /// Instruct the visitor to process a specific <see cref="SpawnCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="SpawnCommand"/> that should be processed.</param>
        /// <returns><typeparamref name="T"/>, the result of processing the <see cref="SpawnCommand"/>.</returns>
        T Visit(SpawnCommand command);
    }
}