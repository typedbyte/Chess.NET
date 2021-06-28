//-----------------------------------------------------------------------
// <copyright file="IRulebook.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rulebook for a chess game.
    /// </summary>
    public interface IRulebook
    {
        /// <summary>
        /// Creates a new chess game, according to the rulebook.
        /// </summary>
        /// <returns>The newly created chess game.</returns>
        ChessGame CreateGame();

        /// <summary>
        /// Gets the status of a chess game, according to the rulebook.
        /// </summary>
        /// <param name="game">The game state to be analyzed.</param>
        /// <returns>The current status of the game.</returns>
        Status GetStatus(ChessGame game);

        /// <summary>
        /// Gets all possible updates (i.e., future game states) for a chess piece on a specified position,
        /// according to the rulebook.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position to be analyzed.</param>
        /// <returns>A sequence of all possible updates for a chess piece on the specified position.</returns>
        IEnumerable<Update> GetUpdates(ChessGame game, Position position);
    }
}