//-----------------------------------------------------------------------
// <copyright file="PromotionSelector.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.View.Selector
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System.Collections.Generic;

    /// <summary>
    /// Provides the functionality to extract promotions from a sequence of updates.
    /// </summary>
    public class PromotionSelector : ICommandVisitor<IMaybe<ChessPiece>>
    {
        /// <summary>
        /// Extracts promotions (i.e., turning a pawn into another chess piece) from a sequence of updates.
        /// </summary>
        /// <param name="events">The sequence of events to be analyzed.</param>
        /// <returns>A dictionary mapping the chess piece a pawn transforms into to their corresponding update.</returns>
        public Dictionary<ChessPiece, Update> Find(IEnumerable<Update> events)
        {
            var result = new Dictionary<ChessPiece, Update>();

            foreach (var e in events)
            {
                e.Command.Accept(this).Do
                (
                    p => result.Add(p, e)
                );
            }

            return result;
        }

        /// <summary>
        /// Extracts the chess piece a pawn transforms into from a <see cref="EndTurnCommand"/>.
        /// </summary>
        /// <param name="command">The command to be analyzed.</param>
        /// <returns>Returns always <see cref="Nothing{ChessPiece}"/>.</returns>
        public IMaybe<ChessPiece> Visit(EndTurnCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        /// <summary>
        /// Extracts the chess piece a pawn transforms into from a <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="command">The command to be analyzed.</param>
        /// <returns>Returns always <see cref="Nothing{ChessPiece}"/>.</returns>
        public IMaybe<ChessPiece> Visit(MoveCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        /// <summary>
        /// Extracts the chess piece a pawn transforms into from a <see cref="RemoveCommand"/>.
        /// </summary>
        /// <param name="command">The command to be analyzed.</param>
        /// <returns>Returns always <see cref="Nothing{ChessPiece}"/>.</returns>
        public IMaybe<ChessPiece> Visit(RemoveCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        /// <summary>
        /// Extracts the chess piece a pawn transforms into from a <see cref="SequenceCommand"/>.
        /// </summary>
        /// <param name="command">The command to be analyzed.</param>
        /// <returns>
        /// The chess piece a pawn transforms into, if this command contains a <see cref="SpawnCommand"/>,
        /// or else <see cref="Nothing{ChessPiece}"/>.
        /// </returns>
        public IMaybe<ChessPiece> Visit(SequenceCommand command)
        {
            var firstResult = command.FirstCommand.Accept(this);

            if (firstResult.HasValue)
            {
                return firstResult;
            }

            return command.SecondCommand.Accept(this);
        }

        /// <summary>
        /// Extracts the chess piece a pawn transforms into from a <see cref="SetLastUpdateCommand"/>.
        /// </summary>
        /// <param name="command">The command to be analyzed.</param>
        /// <returns>Returns always <see cref="Nothing{ChessPiece}"/>.</returns>
        public IMaybe<ChessPiece> Visit(SetLastUpdateCommand command)
        {
            return Nothing<ChessPiece>.Instance;
        }

        /// <summary>
        /// Extracts the chess piece a pawn transforms into from a <see cref="SpawnCommand"/>.
        /// </summary>
        /// <param name="command">The command to be analyzed.</param>
        /// <returns>Returns always <see cref="Just{ChessPiece}"/> containing the spawned piece.</returns>
        public IMaybe<ChessPiece> Visit(SpawnCommand command)
        {
            return new Just<ChessPiece>(command.Piece);
        }
    }
}