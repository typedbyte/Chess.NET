//-----------------------------------------------------------------------
// <copyright file="EnPassantRule.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System;

    /// <summary>
    /// Represents the en passant rule of a standard chess game.
    /// </summary>
    public class EnPassantRule : IChessPieceVisitor<bool>, ICommandVisitor<Func<Position, Pawn, IMaybe<ICommand>>>
    {
        /// <summary>
        /// Get the possible en passant command for a specific pawn.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position of the analyzed pawn.</param>
        /// <param name="pawn">The analyzed pawn.</param>
        /// <returns>An en passant command, if the game state allows it, or else <see cref="Nothing{T}"/>.</returns>
        public IMaybe<ICommand> GetCommand(ChessGame game, Position position, Pawn pawn)
        {
            return game.LastUpdate.Bind(e => e.Command.Accept(this)(position, pawn));
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

        /// <summary>
        /// Checks if a command contains a pawn movement that qualifies for an en passant command.
        /// </summary>
        /// <param name="_">The command to be checked for a pawn movement, which is always ignored.</param>
        /// <returns>Given a position and a pawn, returns always <see cref="Nothing{T}"/>.</returns>
        public Func<Position, Pawn, IMaybe<ICommand>> Visit(EndTurnCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }

        /// <summary>
        /// Checks if a command contains a pawn movement that qualifies for an en passant command.
        /// </summary>
        /// <param name="command">The command to be checked for a pawn movement.</param>
        /// <returns>Given a position and a pawn, returns its corresponding en passant command, if possible.</returns>
        public Func<Position, Pawn, IMaybe<ICommand>> Visit(MoveCommand command)
        {
            return (position, pawn) =>
            {
                var enemyColor = command.Piece.Color;
                var enPassantRow = enemyColor == Color.White ? 3 : 4;
                var source = command.Source;
                var target = command.Target;

                // In order to perform en passant ...
                if
                (
                    // ... our pawn must be in the correct row
                    position.Row == enPassantRow &&
                    // ... the previous move of the enemy must include a pawn
                    command.Piece.Accept(this) &&
                    // ... the enemy's pawn must have ended its move next to us
                    position.Row == target.Row &&
                    Math.Abs(position.Column - target.Column) == 1 &&
                    // ... the enemy's pawn must have moved two fields
                    Math.Abs(source.Row - target.Row) == 2
                )
                {
                    var removeCommand = new RemoveCommand(target, command.Piece);
                    var newPosition = new Position((source.Row + target.Row) / 2, target.Column);
                    var moveCommand = new MoveCommand(position, newPosition, pawn);

                    return new Just<ICommand>
                    (
                        new SequenceCommand(removeCommand, moveCommand)
                    );
                }

                return Nothing<ICommand>.Instance;
            };
        }

        /// <summary>
        /// Checks if a command contains a pawn movement that qualifies for an en passant command.
        /// </summary>
        /// <param name="_">The command to be checked for a pawn movement, which is always ignored.</param>
        /// <returns>Given a position and a pawn, returns always <see cref="Nothing{T}"/>.</returns>
        public Func<Position, Pawn, IMaybe<ICommand>> Visit(RemoveCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }

        /// <summary>
        /// Checks if a command contains a pawn movement that qualifies for an en passant command.
        /// </summary>
        /// <param name="command">The command to be checked for a pawn movement.</param>
        /// <returns>Given a position and a pawn, returns its corresponding en passant command, if possible.</returns>
        public Func<Position, Pawn, IMaybe<ICommand>> Visit(SequenceCommand command)
        {
            return (position, pawn) =>
            {
                var firstResult = command.FirstCommand.Accept(this)(position, pawn);

                if (firstResult.HasValue)
                {
                    return firstResult;
                }

                return command.SecondCommand.Accept(this)(position, pawn);
            };
        }

        /// <summary>
        /// Checks if a command contains a pawn movement that qualifies for an en passant command.
        /// </summary>
        /// <param name="_">The command to be checked for a pawn movement, which is always ignored.</param>
        /// <returns>Given a position and a pawn, returns always <see cref="Nothing{T}"/>.</returns>
        public Func<Position, Pawn, IMaybe<ICommand>> Visit(SetLastUpdateCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }

        /// <summary>
        /// Checks if a command contains a pawn movement that qualifies for an en passant command.
        /// </summary>
        /// <param name="_">The command to be checked for a pawn movement, which is always ignored.</param>
        /// <returns>Given a position and a pawn, returns always <see cref="Nothing{T}"/>.</returns>
        public Func<Position, Pawn, IMaybe<ICommand>> Visit(SpawnCommand _)
        {
            return (_, _) => Nothing<ICommand>.Instance;
        }
    }
}