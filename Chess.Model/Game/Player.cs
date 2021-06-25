//-----------------------------------------------------------------------
// <copyright file="Player.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using Chess.Model.Piece;

    /// <summary>
    /// Represents a player who participates in a chess game.
    /// Currently, a player is only identified by the color of his/her controlled chess pieces.
    /// In the future, a player could have more attributes, like name, ELO rating, etc.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Represents the color of the chess pieces that are controlled by the player.
        /// </summary>
        public readonly Color Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="color">The color of the chess pieces that are controlled by the player.</param>
        public Player(Color color)
        {
            this.Color = color;
        }
    }
}