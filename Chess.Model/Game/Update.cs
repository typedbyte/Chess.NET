//-----------------------------------------------------------------------
// <copyright file="Update.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using Chess.Model.Command;
    using Chess.Model.Data;

    /// <summary>
    /// Represents an update of a chess game state.
    /// </summary>
    public class Update
    {
        /// <summary>
        /// Represents the chess game state before or after executing the corresponding <see cref="Command"/>,
        /// depending on the context.
        /// </summary>
        public readonly ChessGame Game;

        /// <summary>
        /// Represents the command that is involved in the game state update.
        /// </summary>
        public readonly ICommand Command;

        /// <summary>
        /// Initializes a new instance of the <see cref="Update"/> class.
        /// </summary>
        /// <param name="game">
        /// The chess game state before or after executing the corresponding command,
        /// depending on the context.
        /// </param>
        /// <param name="command">The command that is involved in the game state update.</param>
        public Update(ChessGame game, ICommand command)
        {
            Validation.NotNull(game, nameof(game));
            Validation.NotNull(command, nameof(command));

            this.Game = game;
            this.Command = command;
        }
    }
}