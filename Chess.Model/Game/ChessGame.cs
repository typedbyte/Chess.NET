//-----------------------------------------------------------------------
// <copyright file="ChessGame.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using Chess.Model.Data;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a chess game.
    /// </summary>
    public class ChessGame
    {
        /// <summary>
        /// Represents the current state of the chess board.
        /// </summary>
        public readonly Board Board;

        /// <summary>
        /// Represents the player who has currently the right to move.
        /// </summary>
        public readonly Player ActivePlayer;

        /// <summary>
        /// Represents the player who is currently waiting for the opponent's move.
        /// </summary>
        public readonly Player PassivePlayer;

        /// <summary>
        /// Represents the last update that has led to this game state.
        /// </summary>
        public readonly IMaybe<Update> LastUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessGame"/> class.
        /// </summary>
        /// <param name="board">The current state of the chess board.</param>
        /// <param name="activePlayer">The player who has currently the right to move.</param>
        /// <param name="passivePlayer">The player who is currently waiting for the opponent's move.</param>
        public ChessGame(Board board, Player activePlayer, Player passivePlayer)
            : this(board, activePlayer, passivePlayer, Nothing<Update>.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessGame"/> class.
        /// </summary>
        /// <param name="board">The current state of the chess board.</param>
        /// <param name="activePlayer">The player who has currently the right to move.</param>
        /// <param name="passivePlayer">The player who is currently waiting for the opponent's move.</param>
        /// <param name="lastUpdate">The update that has led to the newly created game state.</param>
        public ChessGame(Board board, Player activePlayer, Player passivePlayer, IMaybe<Update> lastUpdate)
        {
            Validation.NotNull(board, nameof(board));
            Validation.NotNull(activePlayer, nameof(activePlayer));
            Validation.NotNull(passivePlayer, nameof(passivePlayer));
            Validation.NotNull(lastUpdate, nameof(lastUpdate));

            this.Board = board;
            this.ActivePlayer = activePlayer;
            this.PassivePlayer = passivePlayer;
            this.LastUpdate = lastUpdate;
        }

        /// <summary>
        /// Gets the history of updates that led to this game state.
        /// </summary>
        /// <value>An enumerable which contains the history of updates, starting with the newest update.</value>
        public IEnumerable<Update> History
        {
            get
            {
                return this.LastUpdate.GetOrElse
                (
                    u => Enumerable.Prepend(u.Game.History, u),
                    Enumerable.Empty<Update>()
                );
            }
        }

        /// <summary>
        /// Sets the last update that has led to this game state.
        /// </summary>
        /// <param name="update">The last update that has led to this game state.</param>
        /// <returns>The new chess game state, including the specified <see cref="LastUpdate"/>.</returns>
        public ChessGame SetLastUpdate(IMaybe<Update> update)
        {
            Validation.NotNull(update, nameof(update));

            return new ChessGame
            (
                this.Board,
                this.ActivePlayer,
                this.PassivePlayer,
                update
            );
        }

        /// <summary>
        /// Sets a new state of the chess board.
        /// </summary>
        /// <param name="board">The new state of the chess board.</param>
        /// <returns>The new chess game state with the updated chess board.</returns>
        public ChessGame SetBoard(Board board)
        {
            Validation.NotNull(board, nameof(board));

            return new ChessGame
            (
                board,
                this.ActivePlayer,
                this.PassivePlayer,
                this.LastUpdate
            );
        }

        /// <summary>
        /// Finishes the turn of the active player.
        /// This essentially swaps <see cref="ActivePlayer"/> and <see cref="PassivePlayer"/>.
        /// </summary>
        /// <returns>The new chess game state, including the swapped player status.</returns>
        public ChessGame EndTurn()
        {
            return new ChessGame
            (
                this.Board,
                this.PassivePlayer,
                this.ActivePlayer,
                this.LastUpdate
            );
        }
    }
}