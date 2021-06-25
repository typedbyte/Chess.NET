//-----------------------------------------------------------------------
// <copyright file="PromotionRule.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the promotion rule of a standard chess game.
    /// </summary>
    public class PromotionRule
    {
        // We ignore the warning regarding the method GetCommands not being static
        // in order to be consistent with the other rule classes.
#pragma warning disable CA1822
        /// <summary>
        /// Gets all possible promotion commands for a specific pawn.
        /// </summary>
        /// <param name="position">The position of the analyzed pawn.</param>
        /// <param name="pawn">The analyzed pawn.</param>
        /// <returns>Returns all possible promotion commands for the specified pawn.</returns>
        public IEnumerable<ICommand> GetCommands(Position position, Pawn pawn)
        {
            switch (position.Row)
            {
                case 0:
                case 7:
                    foreach (var form in PromotionRule.GetPieces(pawn.Color))
                    {
                        yield return new SequenceCommand
                        (
                            new RemoveCommand(position, pawn),
                            new SpawnCommand(position, form)
                        );
                    }
                    break;
                default:
                    yield break;
            }
        }
#pragma warning restore CA1822

        /// <summary>
        /// Creates a sequence of possible promotion chess pieces.
        /// </summary>
        /// <param name="color">The color of the generated chess pieces.</param>
        /// <returns>A sequence of pieces that can be obtained through the promotion rule.</returns>
        private static IEnumerable<ChessPiece> GetPieces(Color color)
        {
            yield return new Queen(color);
            yield return new Bishop(color);
            yield return new Knight(color);
            yield return new Rook(color);
        }
    }
}