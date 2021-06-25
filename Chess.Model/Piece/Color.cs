//-----------------------------------------------------------------------
// <copyright file="Color.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Piece
{
    /// <summary>
    /// Specifies constants that define the colors involved in a chess game.
    /// </summary>
    public enum Color
    {
        /// <summary>
        /// The white color, mainly used for indicating white chess pieces.
        /// </summary>
        White,

        /// <summary>
        /// The black color, mainly used for indicating black chess pieces.
        /// </summary>
        Black
    }

    /// <summary>
    /// Provides extension methods related to the <see cref="Color"/> enumeration.
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Gets the opposite color of a specified color.
        /// </summary>
        /// <param name="color">The color to get the opposite of.</param>
        /// <returns><see cref="Color.White"/> if the argument is <see cref="Color.Black"/>, or vice versa.</returns>
        public static Color Toggle(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}