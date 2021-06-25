//-----------------------------------------------------------------------
// <copyright file="ICommand.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    /// <summary>
    /// A command can be applied to a chess game state in order to obtain a new game state.
    /// </summary>
    public interface ICommand : ICommandVisitable
    {
        /// <summary>
        /// Applies the command to a chess game state.
        /// </summary>
        /// <param name="game">The old chess game state.</param>
        /// <returns>The new chess game state, if the command succeeds.</returns>
        IMaybe<ChessGame> Execute(ChessGame game);
    }
}