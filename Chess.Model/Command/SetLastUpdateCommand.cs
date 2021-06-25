//-----------------------------------------------------------------------
// <copyright file="SetLastUpdateCommand.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    /// <summary>
    /// A command which indicates an update of the last recorded command of a chess game.
    /// </summary>
    public class SetLastUpdateCommand : ICommand
    {
        /// <summary>
        /// Represents the game update to be recorded in a chess game.
        /// </summary>
        public readonly IMaybe<Update> Update;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetLastUpdateCommand"/> class.
        /// </summary>
        /// <param name="update">The game update to be recorded in a chess game.</param>
        public SetLastUpdateCommand(Update update) : this(new Just<Update>(update))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetLastUpdateCommand"/> class.
        /// </summary>
        /// <param name="update">The game update to be recorded in a chess game.</param>
        public SetLastUpdateCommand(IMaybe<Update> update)
        {
            Validation.NotNull(update, nameof(update));
            this.Update = update;
        }

        /// <summary>
        /// Applies the command to a chess game state.
        /// </summary>
        /// <param name="game">The old chess game state.</param>
        /// <returns>The new chess game state, if the command succeeds.</returns>
        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return new Just<ChessGame>(game.SetLastUpdate(this.Update));
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="SetLastUpdateCommand"/>.
        /// </summary>
        /// <param name="visitor">The command visitor to be called.</param>
        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="SetLastUpdateCommand"/>.
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