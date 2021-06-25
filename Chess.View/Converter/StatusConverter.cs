//-----------------------------------------------------------------------
// <copyright file="StatusConverter.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.View.Converter
{
    using Chess.Model.Game;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Represents a converter from a chess game status to specific types.
    /// </summary>
    public class StatusConverter : IValueConverter
    {
        /// <summary>
        /// Represents the color indicating that the chess player using the white chess pieces is playing.
        /// </summary>
        private readonly Color whiteColor;

        /// <summary>
        /// Represents the color indicating that the chess player using the black chess pieces is playing.
        /// </summary>
        private readonly Color blackColor;

        /// <summary>
        /// Represents the color indicating that the game has ended.
        /// </summary>
        private readonly Color otherColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusConverter"/> class.
        /// </summary>
        public StatusConverter()
        {
            this.whiteColor = Colors.White;
            this.blackColor = Colors.Black;
            this.otherColor = Colors.Purple;
        }

        /// <summary>
        /// Converts a chess game status to a specific type.
        /// </summary>
        /// <param name="value">Expected to be a <see cref="Status"/> object.</param>
        /// <param name="targetType">The target type to convert the chess game status to.</param>
        /// <param name="parameter">Additional parameter, which is not used.</param>
        /// <param name="culture">Culture information, which is not used.</param>
        /// <returns>An object of the specified type, or null if such an object cannot be constructed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status)
            {
                if (typeof(string).IsAssignableFrom(targetType))
                {
                    switch (status)
                    {
                        case Status.WhiteTurn:
                            return "Status: White Playing";
                        case Status.WhiteWin:
                            return "Game End: 1-0";
                        case Status.BlackTurn:
                            return "Status: Black Playing";
                        case Status.BlackWin:
                            return "Game End: 0-1";
                        case Status.Draw:
                            return "Game End: \u00bd-\u00bd";
                        default:
                            return null;
                    }
                }

                if (typeof(Visibility).IsAssignableFrom(targetType))
                {
                    switch (status)
                    {
                        case Status.WhiteWin:
                        case Status.BlackWin:
                        case Status.Draw:
                            return Visibility.Visible;
                        default:
                            return Visibility.Collapsed;
                    }
                }

                if (typeof(Color?).IsAssignableFrom(targetType))
                {
                    switch (status)
                    {
                        case Status.WhiteTurn:
                            return this.whiteColor;
                        case Status.BlackTurn:
                            return this.blackColor;
                        default:
                            return this.otherColor;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converts am object back to a chess game status. This is not used.
        /// </summary>
        /// <param name="value">The object to be converted to a chess game status.</param>
        /// <param name="targetType">The type of object to be converted back.</param>
        /// <param name="parameter">Additional parameter, which is not used.</param>
        /// <param name="culture">Culture information, which is not used.</param>
        /// <returns>Returns always null.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}