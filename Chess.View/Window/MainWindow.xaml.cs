//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.View.Window
{
    using Chess.Model.Game;
    using Chess.View.Selector;
    using Chess.ViewModel.Game;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for the <see cref="MainWindow"/> window.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Represents the view model of the window.
        /// </summary>
        private readonly ChessGameVM game;

        /// <summary>
        /// Provides the functionality to extract promotions from a sequence of updates.
        /// </summary>
        private readonly PromotionSelector promotionSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.game = new ChessGameVM(this.Choose);
            this.promotionSelector = new PromotionSelector();
            this.DataContext = this.game;
        }

        /// <summary>
        /// Translates a click on the chess board to a corresponding command to the view model.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Additional information about the mouse click.</param>
        private void BoardMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(sender as Canvas);
            var row = 7 - (int)point.Y;
            var column = (int)point.X;
            var validRow = Math.Max(0, Math.Min(7, row));
            var validColumn = Math.Max(0, Math.Min(7, column));

            this.game.Select(validRow, validColumn);
        }

        /// <summary>
        /// Event handler that closes the window.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Additional information about the event.</param>
        private void ExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Fires when a chess piece was visually removed from the chess board.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Additional information about the event.</param>
        private void RemoveCompleted(object sender, EventArgs e)
        {
            this.game.Board.CleanUp();
        }

        /// <summary>
        /// Chooses a game state update from a list of possible game state updates.
        /// </summary>
        /// <param name="updates">The game state update to choose from.</param>
        /// <returns>The chosen game state update.</returns>
        private Update Choose(IList<Update> updates)
        {
            if (updates.Count == 0)
            {
                return null;
            }
            
            if (updates.Count == 1)
            {
                return updates[0];
            }

            // If there are multiple choices, there must be a promotion.
            var promotions = this.promotionSelector.Find(updates);
            var pieceWindow = new PieceWindow() { Owner = this };
            var selectedPiece = pieceWindow.Show(promotions.Keys);

            return
                selectedPiece != null
                    ? promotions[selectedPiece]
                    : null;
        }
    }
}