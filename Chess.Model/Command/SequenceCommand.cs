//-----------------------------------------------------------------------
// <copyright file="SequenceCommand.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    /// <summary>
    /// A command which indicates that two commands are executed one after another.
    /// </summary>
    public class SequenceCommand : ICommand
    {
        /// <summary>
        /// Represents the first command to be executed.
        /// </summary>
        public readonly ICommand FirstCommand;

        /// <summary>
        /// Represents the second command to be executed.
        /// </summary>
        public readonly ICommand SecondCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceCommand"/> class.
        /// </summary>
        /// <param name="firstCommand">The first command to be executed.</param>
        /// <param name="secondCommand">The second command to be executed.</param>
        public SequenceCommand(ICommand firstCommand, ICommand secondCommand)
        {
            Validation.NotNull(firstCommand, nameof(firstCommand));
            Validation.NotNull(secondCommand, nameof(secondCommand));

            this.FirstCommand = firstCommand;
            this.SecondCommand = secondCommand;
        }

        /// <summary>
        /// Applies the command to a chess game state.
        /// </summary>
        /// <param name="game">The old chess game state.</param>
        /// <returns>The new chess game state, if the command succeeds.</returns>
        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return this.FirstCommand.Execute(game).Bind(g => this.SecondCommand.Execute(g));
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="SequenceCommand"/>.
        /// </summary>
        /// <param name="visitor">The command visitor to be called.</param>
        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="SequenceCommand"/>.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the command.</typeparam>
        /// <param name="visitor">The command visitor to be called.</param>
        /// <returns>The result of the visitor when processing the command.</returns>
        public T Accept<T>(ICommandVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}