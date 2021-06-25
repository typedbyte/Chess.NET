//-----------------------------------------------------------------------
// <copyright file="TargetSetter.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Visitor
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.ViewModel.Game;
    using System;

    /// <summary>
    /// Provides methods which allow to relate a chess game update with a field on the chess board.
    /// Such an update is then usually selected by the chess player (e.g., by clicking on the respective field).
    /// </summary>
    public class TargetSetter : ICommandVisitor<Func<Update, BoardVM, bool>>
    {
        /// <summary>
        /// Relates a chess game update with a target field found in a <see cref="SequenceCommand"/>.
        /// </summary>
        /// <param name="command">The command to be searched for a target field.</param>
        /// <returns>
        /// Given an update and the board state, returns true if the update
        /// can be related to a field on the board, or else false.
        /// </returns>
        public Func<Update, BoardVM, bool> Visit(SequenceCommand command)
        {
            return (update, board) =>
                command.FirstCommand.Accept(this)(update, board) ||
                command.SecondCommand.Accept(this)(update, board);
        }

        /// <summary>
        /// Relates a chess game update with a target field found in a <see cref="EndTurnCommand"/>,
        /// which does not have a target.
        /// </summary>
        /// <param name="_">The command to be searched for a target field.</param>
        /// <returns>
        /// Given an update and the board state, returns always false.
        /// </returns>
        public Func<Update, BoardVM, bool> Visit(EndTurnCommand _)
        {
            return (_, _) => false;
        }

        /// <summary>
        /// Relates a chess game update with a target field found in a <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="command">The command to be searched for a target field.</param>
        /// <returns>
        /// Given an update and the board state, related the target field of the move command with the
        /// given update and then returns true.
        /// </returns>
        public Func<Update, BoardVM, bool> Visit(MoveCommand command)
        {
            return (update, board) =>
            {
                board.AddUpdate(command.Target, update);
                return true;
            };
        }

        /// <summary>
        /// Relates a chess game update with a target field found in a <see cref="RemoveCommand"/>,
        /// which does not have a target.
        /// </summary>
        /// <param name="_">The command to be searched for a target field.</param>
        /// <returns>
        /// Given an update and the board state, returns always false.
        /// </returns>
        public Func<Update, BoardVM, bool> Visit(RemoveCommand _)
        {
            return (_, _) => false;
        }

        /// <summary>
        /// Relates a chess game update with a target field found in a <see cref="SetLastUpdateCommand"/>,
        /// which does not have a target.
        /// </summary>
        /// <param name="_">The command to be searched for a target field.</param>
        /// <returns>
        /// Given an update and the board state, returns always false.
        /// </returns>
        public Func<Update, BoardVM, bool> Visit(SetLastUpdateCommand _)
        {
            return (_, _) => false;
        }

        /// <summary>
        /// Relates a chess game update with a target field found in a <see cref="SpawnCommand"/>,
        /// which does not have a target.
        /// </summary>
        /// <param name="_">The command to be searched for a target field.</param>
        /// <returns>
        /// Given an update and the board state, returns always false.
        /// </returns>
        public Func<Update, BoardVM, bool> Visit(SpawnCommand _)
        {
            return (_, _) => false;
        }
    }
}