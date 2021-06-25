//-----------------------------------------------------------------------
// <copyright file="EndTurnCommand.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Command
{
    using Chess.Model.Data;
    using Chess.Model.Game;

    /// <summary>
    /// A command which indicates the end of a player's turn.
    /// </summary>
    public class EndTurnCommand : ICommand
    {
        /// <summary>
        /// Represents the default instance of the stateless class <see cref="EndTurnCommand"/>.
        /// </summary>
        public static readonly EndTurnCommand Instance = new();

        /// <summary>
        /// Applies the command to a chess game state.
        /// </summary>
        /// <param name="game">The old chess game state.</param>
        /// <returns>The new chess game state, if the command succeeds.</returns>
        public IMaybe<ChessGame> Execute(ChessGame game)
        {
            return new Just<ChessGame>(game.EndTurn());
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="EndTurnCommand"/>.
        /// </summary>
        /// <param name="visitor">The command visitor to be called.</param>
        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Accepts a command visitor in order to call its implementation for <see cref="EndTurnCommand"/>.
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