//-----------------------------------------------------------------------
// <copyright file="CommandNegator.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Visitor
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;

    /// <summary>
    /// Provides methods which allow to negate/invert commands.
    /// Negated/inverted commands have the inverted effect on a chess game than the original commands.
    /// </summary>
    public class CommandNegator : IChessPieceVisitor<bool>, ICommandVisitor<ICommand>
    {
        /// <summary>
        /// Negates/inverts a <see cref="SequenceCommand"/>.
        /// </summary>
        /// <param name="command">The command to be negated/inverted.</param>
        /// <returns>The command that has the inverted effect on the chess game.</returns>
        public ICommand Visit(SequenceCommand command)
        {
            return new SequenceCommand
            (
                command.SecondCommand.Accept(this),
                command.FirstCommand.Accept(this)
            );
        }

        /// <summary>
        /// Negates/inverts a <see cref="EndTurnCommand"/>.
        /// </summary>
        /// <param name="command">The command to be negated/inverted.</param>
        /// <returns>The command that has the inverted effect on the chess game.</returns>
        public ICommand Visit(EndTurnCommand command)
        {
            return command;
        }

        /// <summary>
        /// Negates/inverts a <see cref="MoveCommand"/>.
        /// </summary>
        /// <param name="command">The command to be negated/inverted.</param>
        /// <returns>The command that has the inverted effect on the chess game.</returns>
        public ICommand Visit(MoveCommand command)
        {
            // Promotion is negated differently than a normal move in order to get the animations right.
            if ((command.Target.Row == 0 || command.Target.Row == 7) && command.Piece.Accept(this))
            {
                return new SequenceCommand
                (
                    new RemoveCommand(command.Target, command.Piece),
                    new SpawnCommand(command.Source, command.Piece)
                );
            }

            return new MoveCommand(command.Target, command.Source, command.Piece);
        }

        /// <summary>
        /// Negates/inverts a <see cref="SetLastUpdateCommand"/>.
        /// </summary>
        /// <param name="command">The command to be negated/inverted.</param>
        /// <returns>The command that has the inverted effect on the chess game.</returns>
        public ICommand Visit(SetLastUpdateCommand command)
        {
            return new SetLastUpdateCommand(Nothing<Update>.Instance);
        }

        /// <summary>
        /// Negates/inverts a <see cref="RemoveCommand"/>.
        /// </summary>
        /// <param name="command">The command to be negated/inverted.</param>
        /// <returns>The command that has the inverted effect on the chess game.</returns>
        public ICommand Visit(RemoveCommand command)
        {
            return new SpawnCommand(command.Position, command.Piece);
        }

        /// <summary>
        /// Negates/inverts a <see cref="SpawnCommand"/>.
        /// </summary>
        /// <param name="command">The command to be negated/inverted.</param>
        /// <returns>The command that has the inverted effect on the chess game.</returns>
        public ICommand Visit(SpawnCommand command)
        {
            return new RemoveCommand(command.Position, command.Piece);
        }

        /// <summary>
        /// Checks if a bishop is a pawn.
        /// </summary>
        /// <param name="_">The analyzed bishop.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Bishop _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a king is a pawn.
        /// </summary>
        /// <param name="_">The analyzed king.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(King _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a knight is a pawn.
        /// </summary>
        /// <param name="_">The analyzed knight.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Knight _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a pawn is a pawn.
        /// </summary>
        /// <param name="_">The analyzed pawn.</param>
        /// <returns>Returns always true.</returns>
        public bool Visit(Pawn _)
        {
            return true;
        }

        /// <summary>
        /// Checks if a queen is a pawn.
        /// </summary>
        /// <param name="_">The analyzed queen.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Queen _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a rook is a pawn.
        /// </summary>
        /// <param name="_">The analyzed rook.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Rook _)
        {
            return false;
        }
    }
}