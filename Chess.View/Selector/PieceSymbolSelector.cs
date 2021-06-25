//-----------------------------------------------------------------------
// <copyright file="PieceSymbolSelector.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.View.Selector
{
    using Chess.Model.Piece;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a template selector which selects the right look for chess piece based on its type.
    /// </summary>
    public class PieceSymbolSelector : DataTemplateSelector, IChessPieceVisitor<string>
    {
        /// <summary>
        /// Select the right look for chess piece based on its type.
        /// </summary>
        /// <param name="item">The chess piece for which the data template is selected.</param>
        /// <param name="container">The container of the styled item.</param>
        /// <returns>The data template for the chess piece, as can be found in the application resources.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IChessPieceVisitable piece)
            {
                return Application.Current.FindResource(piece.Accept(this)) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        /// <summary>
        /// Gets the resource string for the bishop chess piece.
        /// </summary>
        /// <param name="bishop">The bishop chess piece.</param>
        /// <returns>The string identifying the resource in the application resources.</returns>
        public string Visit(Bishop bishop)
        {
            return ToResourceString(bishop, "Bishop");
        }

        /// <summary>
        /// Gets the resource string for the king chess piece.
        /// </summary>
        /// <param name="king">The king chess piece.</param>
        /// <returns>The string identifying the resource in the application resources.</returns>
        public string Visit(King king)
        {
            return ToResourceString(king, "King");
        }

        /// <summary>
        /// Gets the resource string for the knight chess piece.
        /// </summary>
        /// <param name="knight">The knight chess piece.</param>
        /// <returns>The string identifying the resource in the application resources.</returns>
        public string Visit(Knight knight)
        {
            return ToResourceString(knight, "Knight");
        }

        /// <summary>
        /// Gets the resource string for the pawn chess piece.
        /// </summary>
        /// <param name="pawn">The pawn chess piece.</param>
        /// <returns>The string identifying the resource in the application resources.</returns>
        public string Visit(Pawn pawn)
        {
            return ToResourceString(pawn, "Pawn");
        }

        /// <summary>
        /// Gets the resource string for the queen chess piece.
        /// </summary>
        /// <param name="queen">The queen chess piece.</param>
        /// <returns>The string identifying the resource in the application resources.</returns>
        public string Visit(Queen queen)
        {
            return ToResourceString(queen, "Queen");
        }

        /// <summary>
        /// Gets the resource string for the rook chess piece.
        /// </summary>
        /// <param name="rook">The rook chess piece.</param>
        /// <returns>The string identifying the resource in the application resources.</returns>
        public string Visit(Rook rook)
        {
            return ToResourceString(rook, "Rook");
        }

        /// <summary>
        /// Adds a "white" or "black" prefix in front of a chess piece resource string.
        /// </summary>
        /// <param name="piece">The chess piece whose color is analyzed.</param>
        /// <param name="form">The string that is appended to the prefix.</param>
        /// <returns>
        /// Returns "white" + <paramref name="form"/> if the chess piece is white.
        /// Returns "black" + <paramref name="form"/> if the chess piece is back.
        /// </returns>
        private static string ToResourceString(ChessPiece piece, string form)
        {
            return
                piece.Color == Color.White
                    ? "white" + form
                    : "black" + form;
        }
    }
}