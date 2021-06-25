//-----------------------------------------------------------------------
// <copyright file="EndRule.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System.Linq;

    /// <summary>
    /// Represents the rule which determines if a standard chess game has ended.
    /// </summary>
    public class EndRule
    {
        /// <summary>
        /// Represents the check rule, which is a prerequisite for ending a standard chess game.
        /// </summary>
        private readonly CheckRule checkRule;

        /// <summary>
        /// Represents the movement rule for validating possible movements.
        /// </summary>
        private readonly MovementRule movementRule;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndRule"/> class.
        /// </summary>
        /// <param name="checkRule">The check rule, which is a prerequisite for ending a standard chess game.</param>
        /// <param name="movementRule">The movement rule for validating possible movements.</param>
        public EndRule(CheckRule checkRule, MovementRule movementRule)
        {
            Validation.NotNull(checkRule, nameof(checkRule));
            Validation.NotNull(movementRule, nameof(movementRule));

            this.checkRule = checkRule;
            this.movementRule = movementRule;
        }

        /// <summary>
        /// Gets the status of a chess game, according to the standard rulebook.
        /// </summary>
        /// <param name="game">The game to be analyzed.</param>
        /// <returns>The current status of the game.</returns>
        public Status GetStatus(ChessGame game)
        {
            var color = game.ActivePlayer.Color;
            var pieces = game.Board.GetPieces(color);
            var isChecked = this.checkRule.Check(game, game.ActivePlayer);
            var possibleMoves = pieces.SelectMany(p => this.movementRule.GetCommands(game, p));
            var futures = possibleMoves.Select(c => c.Execute(game)).FilterMaybes();
            var hasMoves = futures.Any(g => !this.checkRule.Check(g, game.ActivePlayer));

            if (isChecked && !hasMoves)
            {
                return
                    color == Color.White
                        ? Status.BlackWin
                        : Status.WhiteWin;
            }
            else if (!isChecked && !hasMoves)
            {
                return Status.Draw;
            }

            return
                color == Color.White
                    ? Status.WhiteTurn
                    : Status.BlackTurn;
        }
    }
}