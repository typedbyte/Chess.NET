//-----------------------------------------------------------------------
// <copyright file="FieldBrushConverter.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.View.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Represents a converter between a field position and a brush indicating white or black.
    /// </summary>
    public class FieldBrushConverter : IMultiValueConverter
    {
        /// <summary>
        /// Represents the brush that is used for white fields.
        /// </summary>
        private readonly Brush whiteBrush;

        /// <summary>
        /// Represents the brush that is used for black fields.
        /// </summary>
        private readonly Brush blackBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldBrushConverter"/> class.
        /// </summary>
        public FieldBrushConverter()
        {
            this.whiteBrush = new SolidColorBrush(Colors.NavajoWhite);
            this.blackBrush = new SolidColorBrush(Colors.Peru);
        }

        /// <summary>
        /// Converts a field position to a brush.
        /// </summary>
        /// <param name="values">Expected to be an <see cref="int"/>[2] array holding row and column indices of the position.</param>
        /// <param name="targetType">Expected to be the <see cref="Brush"/> type.</param>
        /// <param name="parameter">Additional parameter, which is not used.</param>
        /// <param name="culture">Culture information, which is not used.</param>
        /// <returns>A brush indicating a white or black field, or null if the inputs are wrong.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !typeof(Brush).IsAssignableFrom(targetType))
            {
                return null;
            }

            var rowObj = values[0];
            var columnObj = values[1];

            if (rowObj is int row && columnObj is int column)
            {
                return
                    (row + column) % 2 == 0
                        ? this.blackBrush
                        : this.whiteBrush;
            }

            return null;
        }

        /// <summary>
        /// Converts a brush back to a position. This is not used.
        /// </summary>
        /// <param name="value">Expected to be a <see cref="Brush"/> object.</param>
        /// <param name="targetTypes">Expected to be the <see cref="int"/>[2] array type.</param>
        /// <param name="parameter">Additional parameter, which is not used.</param>
        /// <param name="culture">Culture information, which is not used.</param>
        /// <returns>Returns always null.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}