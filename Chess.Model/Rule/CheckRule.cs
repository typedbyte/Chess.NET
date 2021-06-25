//-----------------------------------------------------------------------
// <copyright file="CheckRule.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.Model.Visitor;
    using System.Linq;

    /// <summary>
    /// Represents the check rule of a standard chess game.
    /// </summary>
    public class CheckRule : IChessPieceVisitor<bool>
    {
        /// <summary>
        /// Represents the threat analyzer for validating checks.
        /// </summary>
        private readonly ThreatAnalyzer threatAnalyzer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRule"/> class.
        /// </summary>
        /// <param name="threatAnalyzer">The threat analyzer for validating checks.</param>
        public CheckRule(ThreatAnalyzer threatAnalyzer)
        {
            Validation.NotNull(threatAnalyzer, nameof(threatAnalyzer));
            this.threatAnalyzer = threatAnalyzer;
        }

        /// <summary>
        /// Checks if a specific player is currently checked.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="player">The player who is analyzed for being checked.</param>
        /// <returns>True if the specified player is checked, or else false.</returns>
        public bool Check(ChessGame game, Player player)
        {
            var color = player.Color;
            var enemyColor = color.Toggle();
            var enemies = game.Board.GetPieces(enemyColor);
            var threats = enemies.SelectMany(p => this.threatAnalyzer.GetThreats(game.Board, p));
            var king = game.Board.GetPieces(color).Find(p => p.Accept(this));
            var isChecked = king.Map(p => threats.Any(t => t.Equals(p.Position)));

            return isChecked.GetOrElse(c => c, false);
        }

        /// <summary>
        /// Checks if a bishop is a king.
        /// </summary>
        /// <param name="_">The analyzed bishop.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Bishop _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a king is a king.
        /// </summary>
        /// <param name="_">The analyzed king.</param>
        /// <returns>Returns always true.</returns>
        public bool Visit(King _)
        {
            return true;
        }

        /// <summary>
        /// Checks if a knight is a king.
        /// </summary>
        /// <param name="_">The analyzed knight.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Knight _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a pawn is a king.
        /// </summary>
        /// <param name="_">The analyzed pawn.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Pawn _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a queen is a king.
        /// </summary>
        /// <param name="_">The analyzed queen.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Queen _)
        {
            return false;
        }

        /// <summary>
        /// Checks if a rook is a king.
        /// </summary>
        /// <param name="_">The analyzed rook.</param>
        /// <returns>Returns always false.</returns>
        public bool Visit(Rook _)
        {
            return false;
        }
    }
}