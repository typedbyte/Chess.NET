//-----------------------------------------------------------------------
// <copyright file="Status.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    /// <summary>
    /// Represents the possible states of a chess game.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Indicates that the player controlling the white chess pieces is currently playing.
        /// </summary>
        WhiteTurn,

        /// <summary>
        /// Indicates that the player controlling the white chess pieces has won.
        /// </summary>
        WhiteWin,

        /// <summary>
        /// Indicates that the player controlling the black chess pieces is currently playing.
        /// </summary>
        BlackTurn,

        /// <summary>
        /// Indicates that the player controlling the black chess pieces has won.
        /// </summary>
        BlackWin,

        /// <summary>
        /// Indicates that the chess game has ended in a draw.
        /// </summary>
        Draw
    }
}