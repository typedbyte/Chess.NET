//-----------------------------------------------------------------------
// <copyright file="PieceWindow.xaml.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.View.Window
{
    using Chess.Model.Piece;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for the <see cref="PieceWindow"/> window.
    /// </summary>
    public partial class PieceWindow : Window
    {
        /// <summary>
        /// Represents the selected chess piece.
        /// </summary>
        private ChessPiece selectedPiece;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceWindow"/> class.
        /// </summary>
        public PieceWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the window and returns the selected chess piece.
        /// </summary>
        /// <param name="pieces">The possible chess pieces to choose from.</param>
        /// <returns>The chess piece selected by the user.</returns>
        public ChessPiece Show(IEnumerable<ChessPiece> pieces)
        {
            this.pieceControl.ItemsSource = pieces;
            this.ShowDialog();
            return this.selectedPiece;
        }

        /// <summary>
        /// Closes the window after the user has selected a chess piece.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Additional information about the event.</param>
        private void ChooseClick(object sender, RoutedEventArgs e)
        {
            // Wanted to write the following shorter, namely ...
            //
            // if (sender is FrameworkElement element && element.Tag is ChessPiece piece)
            //
            // ... but StyleCop refused to see the variable "piece" as a local variable.
            if (sender is FrameworkElement element)
            {
                var piece = element.Tag as ChessPiece;

                if (piece != null)
                {
                    this.selectedPiece = piece;
                    this.Close();
                }
            }
        }
    }
}