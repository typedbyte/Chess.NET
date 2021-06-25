//-----------------------------------------------------------------------
// <copyright file="ThreatAnalyzer.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Visitor
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods which allow to detect the threatened empty chess board fields and opponent
    /// pieces for a specified chess piece.
    /// </summary>
    public class ThreatAnalyzer : IChessPieceVisitor<Func<Board, Position, IEnumerable<Position>>>
    {
        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified chess piece.
        /// </summary>
        /// <param name="board">The current chess board state.</param>
        /// <param name="piece">The analyzed chess piece (i.e., the attacker).</param>
        /// <returns>A sequence of positions that are threatened by the specified chess piece.</returns>
        public IEnumerable<Position> GetThreats(Board board, PlacedPiece piece)
        {
            return piece.Accept(this)(board, piece.Position);
        }

        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified bishop.
        /// </summary>
        /// <param name="bishop">The analyzed bishop.</param>
        /// <returns>
        /// Given a board state and a position for the bishop,
        /// returns all positions that are threatened by the specified bishop.
        /// </returns>
        public Func<Board, Position, IEnumerable<Position>> Visit(Bishop bishop)
        {
            return (board, position) =>
            {
                return Iterate(board, position, bishop.Color, Direction.Diagonals);
            };
        }

        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified king.
        /// </summary>
        /// <param name="king">The analyzed king.</param>
        /// <returns>
        /// Given a board state and a position for the king,
        /// returns all positions that are threatened by the specified king.
        /// </returns>
        public Func<Board, Position, IEnumerable<Position>> Visit(King king)
        {
            return (board, position) =>
            {
                var directions = Direction.Orthogonals.Union(Direction.Diagonals);
                return Iterate(board, position, king.Color, directions, 1);
            };
        }

        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified knight.
        /// </summary>
        /// <param name="knight">The analyzed knight.</param>
        /// <returns>
        /// Given a board state and a position for the knight,
        /// returns all positions that are threatened by the specified knight.
        /// </returns>
        public Func<Board, Position, IEnumerable<Position>> Visit(Knight knight)
        {
            return (board, position) =>
            {
                var deltas = new int[] { -1, 1, 2, -2 };
                var directions = from row in deltas
                                 from column in deltas
                                 where row != column && row != -column
                                 select new Direction(row, column);
                return Iterate(board, position, knight.Color, directions, 1);
            };
        }

        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified pawn.
        /// </summary>
        /// <param name="pawn">The analyzed pawn.</param>
        /// <returns>
        /// Given a board state and a position for the pawn,
        /// returns all positions that are threatened by the specified pawn.
        /// </returns>
        public Func<Board, Position, IEnumerable<Position>> Visit(Pawn pawn)
        {
            return (board, position) =>
            {
                var rowDelta = pawn.Color == Color.White ? 1 : -1;
                var directions = Direction.Diagonals.Where(p => p.RowDelta == rowDelta);
                return Iterate(board, position, pawn.Color, directions, 1);
            };
        }

        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified queen.
        /// </summary>
        /// <param name="queen">The analyzed queen.</param>
        /// <returns>
        /// Given a board state and a position for the queen,
        /// returns all positions that are threatened by the specified queen.
        /// </returns>
        public Func<Board, Position, IEnumerable<Position>> Visit(Queen queen)
        {
            return (board, position) =>
            {
                var directions = Direction.Orthogonals.Union(Direction.Diagonals);
                return Iterate(board, position, queen.Color, directions);
            };
        }

        /// <summary>
        /// Gets the positions of all threatened empty chess board fields and opponent pieces
        /// for a specified rook.
        /// </summary>
        /// <param name="rook">The analyzed rook.</param>
        /// <returns>
        /// Given a board state and a position for the rook,
        /// returns all positions that are threatened by the specified rook.
        /// </returns>
        public Func<Board, Position, IEnumerable<Position>> Visit(Rook rook)
        {
            return (board, position) =>
            {
                return Iterate(board, position, rook.Color, Direction.Orthogonals);
            };
        }

        /// <summary>
        /// Starting at a specified position, provides a sequence of all the positions that are reachable
        /// by a chess piece in the given directions.
        /// </summary>
        /// <param name="board">The current chess board state.</param>
        /// <param name="position">The starting position for the sequence.</param>
        /// <param name="color">The color of the analyzed chess piece.</param>
        /// <param name="directions">The directions to iterate, beginning by the starting position.</param>
        /// <returns>A sequence of all the reachable positions.</returns>
        private static IEnumerable<Position> Iterate(Board board, Position position, Color color, IEnumerable<Direction> directions)
        {
            return Iterate(board, position, color, directions, int.MaxValue);
        }

        /// <summary>
        /// Starting at a specified position, provides a sequence of all the positions that are reachable
        /// by a chess piece in the given directions.
        /// </summary>
        /// <param name="board">The current chess board state.</param>
        /// <param name="position">The starting position for the sequence.</param>
        /// <param name="color">The color of the analyzed chess piece.</param>
        /// <param name="directions">The directions to iterate, beginning by the starting position.</param>
        /// <param name="maxSteps">The maximum count of steps to take in a specific direction.</param>
        /// <returns>A sequence of all the reachable positions.</returns>
        private static IEnumerable<Position> Iterate(Board board, Position position, Color color, IEnumerable<Direction> directions, int maxSteps)
        {
            var friends = board.GetPieces(color);
            var enemies = board.GetPieces(color.Toggle());

            bool isFriend(Position p) => friends.Any(f => f.Position.Equals(p));
            bool isEnemy(Position p) => enemies.Any(f => f.Position.Equals(p));

            foreach (var dir in directions)
            {
                var start = position.Offset(dir);
                var positionStream = start.Repeat(p => p.Bind(m => m.Offset(dir)));
                var inBoundsPositions = positionStream.TakeWhile(p => p.HasValue);
                var validPositions = inBoundsPositions.FilterMaybes().Take(maxSteps);

                foreach (var pos in validPositions)
                {
                    if (isFriend(pos))
                    {
                        break;
                    }

                    yield return pos;

                    if (isEnemy(pos))
                    {
                        break;
                    }
                }
            }
        }
    }
}