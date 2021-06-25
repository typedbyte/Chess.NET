//-----------------------------------------------------------------------
// <copyright file="BoardVM.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Game
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.ViewModel.Piece;
    using Chess.ViewModel.Visitor;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Represents the view model of a chess board.
    /// </summary>
    public class BoardVM
    {
        /// <summary>
        /// Represents the visitor that can set potential targets for chess pieces on the board.
        /// </summary>
        private readonly TargetSetter targetSetter;

        /// <summary>
        /// Represents the currently presented fields of the chess board.
        /// </summary>
        private readonly FieldVM[,] fields;

        /// <summary>
        /// Represents the mapping from target fields to potential game updates.
        /// </summary>
        private readonly Dictionary<FieldVM, List<Update>> targets;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardVM"/> class.
        /// </summary>
        /// <param name="board">The current chess board state.</param>
        public BoardVM(Board board)
        {
            var pieces = board.Select(p => new PlacedPieceVM(p));
            var fieldArray = new FieldVM[8, 8];
            var fieldVMs =
               from row in Enumerable.Range(0, 8)
               from column in Enumerable.Range(0, 8)
               select new FieldVM(row, column);

            foreach (var field in fieldVMs)
            {
                fieldArray[field.Row, field.Column] = field;
            }

            this.fields = fieldArray;
            this.Pieces = new ObservableCollection<PlacedPieceVM>(pieces);
            this.targets = new Dictionary<FieldVM, List<Update>>();
            this.targetSetter = new TargetSetter();
        }

        /// <summary>
        /// Gets or sets the selected source field for which the <see cref="targets"/> were determined.
        /// </summary>
        /// <value>
        /// The field which is currently selected by a player before committing to a specific target field
        /// (i.e., before making the actual move of a chess piece).
        /// </value>
        public FieldVM Source { get; set; }

        /// <summary>
        /// Gets the current pieces on the chess board.
        /// </summary>
        /// <value>The current pieces on the chess board.</value>
        public ObservableCollection<PlacedPieceVM> Pieces { get; }

        /// <summary>
        /// Gets a sequence of the currently presented chess board fields.
        /// </summary>
        /// <value>A sequence of the chess board fields.</value>
        public IEnumerable<FieldVM> Fields
        {
            get
            {
                var rowCount = this.fields.GetLength(0);
                var columnCount = this.fields.GetLength(1);

                return
                    from row in Enumerable.Range(0, rowCount)
                    from column in Enumerable.Range(0, columnCount)
                    select this.fields[row, column];
            }
        }

        /// <summary>
        /// Removes all chess pieces from the board that are marked as removed.
        /// </summary>
        public void CleanUp()
        {
            foreach (var piece in this.Pieces.Where(p => p.Removed).ToList())
            {
                this.Pieces.Remove(piece);
            }
        }

        /// <summary>
        /// Gets the field of a specified position.
        /// </summary>
        /// <param name="position">The position of the field.</param>
        /// <returns>The field that corresponds to the specified position.</returns>
        public FieldVM GetField(Position position)
        {
            return this.fields[position.Row, position.Column];
        }

        /// <summary>
        /// Clears the selected source field and all of its corresponding targets.
        /// </summary>
        public void ClearUpdates()
        {
            if (this.Source != null)
            {
                this.Source.IsTarget = false;
                this.Source = null;
            }

            foreach (var target in this.targets.Keys)
            {
                target.IsTarget = false;
            }

            this.targets.Clear();
        }

        /// <summary>
        /// Sets the game state updates a chess player can choose from.
        /// </summary>
        /// <param name="updates">The game state updates a chess player can choose from.</param>
        public void SetTargets(IEnumerable<Update> updates)
        {
            foreach (var update in updates)
            {
                update.Command.Accept(this.targetSetter)(update, this);
            }
        }

        /// <summary>
        /// Gets the game state updates for a specified field of the chess board.
        /// </summary>
        /// <param name="field">The field (i.e., target) to get the updates for.</param>
        /// <returns>A list of updates the user can choose from.</returns>
        public IList<Update> GetUpdates(FieldVM field)
        {
            if (this.targets.TryGetValue(field, out var updates))
            {
                return updates;
            }

            return Array.Empty<Update>();
        }

        /// <summary>
        /// Sets the selected source field for which <see cref="targets"/> will be determined.
        /// </summary>
        /// <param name="position">The position of the selected source field.</param>
        public void SetSource(Position position)
        {
            var field = this.fields[position.Row, position.Column];
            field.IsTarget = true;
            this.Source = field;
        }

        /// <summary>
        /// Adds a selectable update to a specific field.
        /// </summary>
        /// <param name="position">The position of the field.</param>
        /// <param name="update">The update to be added to the field.</param>
        public void AddUpdate(Position position, Update update)
        {
            var field = this.fields[position.Row, position.Column];
            field.IsTarget = true;

            if (this.targets.TryGetValue(field, out var updates))
            {
                updates.Add(update);
            }
            else
            {
                this.targets.Add(field, new List<Update> { update });
            }
        }

        /// <summary>
        /// Executes a move command and repositions the chess piece.
        /// </summary>
        /// <param name="command">The move command to be executed.</param>
        public void Execute(MoveCommand command)
        {
            var piece = this.Pieces.FirstOrDefault
            (
                p => !p.Removed && p.Position.Equals(command.Source)
            );

            if (piece != null)
            {
                piece.Position = new PositionVM(command.Target);
            }
        }

        /// <summary>
        /// Executes a remove command and marks the corresponding chess piece as removed.
        /// </summary>
        /// <param name="command">The remove command to be executed.</param>
        public void Execute(RemoveCommand command)
        {
            var piece = this.Pieces.FirstOrDefault
            (
                p => !p.Removed && p.Position.Equals(command.Position)
            );

            if (piece != null)
            {
                piece.Removed = true;
            }
        }

        /// <summary>
        /// Executes a spawn command and adds a new chess piece to the board.
        /// </summary>
        /// <param name="command">The spawn command to be executed.</param>
        public void Execute(SpawnCommand command)
        {
            this.Pieces.Add(new PlacedPieceVM(command.Position, command.Piece));
        }
    }
}