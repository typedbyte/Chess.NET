//-----------------------------------------------------------------------
// <copyright file="ChessGameVM.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Game
{
    using Chess.Model.Command;
    using Chess.Model.Game;
    using Chess.Model.Rule;
    using Chess.ViewModel.Command;
    using Chess.ViewModel.Visitor;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Represents the view model of a chess game.
    /// </summary>
    public class ChessGameVM : ICommandVisitor, INotifyPropertyChanged
    {
        /// <summary>
        /// Represents the rulebook for the game.
        /// </summary>
        private readonly IRulebook rulebook;

        /// <summary>
        /// Represents the disambiguation mechanism if multiple updates are available for a target field.
        /// </summary>
        private readonly Func<IList<Update>, Update> updateSelector;

        /// <summary>
        /// Represents an object who can negate/invert a given command.
        /// </summary>
        private readonly CommandNegator negator;

        /// <summary>
        /// Represents the undo command, which reverts to a previous game state.
        /// </summary>
        private readonly GenericCommand undoCommand;

        /// <summary>
        /// Represents the current game state.
        /// </summary>
        private ChessGame game;

        /// <summary>
        /// Represents the currently presented chess board.
        /// </summary>
        private BoardVM board;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessGameVM"/> class.
        /// </summary>
        /// <param name="updateSelector">The disambiguation mechanism if multiple updates are available for a target field.</param>
        public ChessGameVM(Func<IList<Update>, Update> updateSelector)
        {
            this.rulebook = new StandardRulebook();
            this.Game = this.rulebook.CreateGame();
            this.board = new BoardVM(this.Game.Board);
            this.updateSelector = updateSelector;
            this.negator = new CommandNegator();

            this.undoCommand = new GenericCommand
            (
                () => this.Game.LastUpdate.HasValue,
                () => this.Game.LastUpdate.Do
                (
                    e =>
                    {
                        this.Game = e.Game;
                        this.Board.ClearUpdates();
                        e.Command.Accept(this.negator).Accept(this);
                    }
                )
            );
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the currently presented state of the chess board.
        /// </summary>
        /// <value>The current state of the chess board.</value>
        public BoardVM Board
        {
            get
            {
                return this.board;
            }

            private set
            {
                if (this.board != value)
                {
                    this.board = value ?? throw new ArgumentNullException(nameof(this.Board));
                    this.OnPropertyChanged(nameof(this.Board));
                }
            }
        }

        /// <summary>
        /// Gets the current status of the chess game.
        /// </summary>
        /// <value>The current status of the presented chess game.</value>
        public Status Status => this.rulebook.GetStatus(this.Game);

        /// <summary>
        /// Gets the command that starts a new chess game.
        /// </summary>
        /// <value>The command that starts a new chess game.</value>
        public GenericCommand NewCommand
        {
            get
            {
                return new GenericCommand
                (
                    () => true,
                    () =>
                    {
                        this.Game = this.rulebook.CreateGame();
                        this.Board = new BoardVM(this.Game.Board);
                        this.OnPropertyChanged(nameof(this.Status));
                    }
                );
            }
        }

        /// <summary>
        /// Gets the command that reverts the last action of the presented chess game.
        /// </summary>
        /// <value>The command that reverts the last action of the presented chess game.</value>
        public GenericCommand UndoCommand => this.undoCommand;

        /// <summary>
        /// Gets or sets the current chess game state.
        /// </summary>
        private ChessGame Game
        {
            get
            {
                return this.game;
            }

            set
            {
                if (this.game != value)
                {
                    this.game = value ?? throw new ArgumentNullException(nameof(this.Game));
                    this.UndoCommand?.FireCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Selects a specific field of the chess board.
        /// </summary>
        /// <param name="row">The row of the field.</param>
        /// <param name="column">The column of the field.</param>
        public void Select(int row, int column)
        {
            var position = new Position(row, column);
            var field = this.Board.GetField(position);

            if (this.Board.Source == field)
            {
                this.Board.ClearUpdates();
                return;
            }

            var updates = this.Board.GetUpdates(field);
            var selectedUpdate = this.updateSelector(updates);
            this.Board.ClearUpdates();

            if (selectedUpdate != null)
            {
                this.Game = selectedUpdate.Game;
                selectedUpdate.Command.Accept(this);
            }
            else if (this.game.Board.IsOccupied(position, this.game.ActivePlayer.Color))
            {
                var newUpdates = this.rulebook.GetUpdates(this.Game, position);
                this.Board.SetSource(position);
                this.Board.SetTargets(newUpdates);
            }
        }

        /// <summary>
        /// Executes a <see cref="SequenceCommand"/> in order to change the presented game state.
        /// </summary>
        /// <param name="command">The <see cref="SequenceCommand"/> to be executed.</param>
        public void Visit(SequenceCommand command)
        {
            command.FirstCommand.Accept(this);
            command.SecondCommand.Accept(this);
        }

        /// <summary>
        /// Executes a <see cref="EndTurnCommand"/> in order to change the presented game state.
        /// </summary>
        /// <param name="command">The <see cref="EndTurnCommand"/> to be executed.</param>
        public void Visit(EndTurnCommand command)
        {
            this.OnPropertyChanged(nameof(this.Status));
        }

        /// <summary>
        /// Executes a <see cref="MoveCommand"/> in order to change the presented game state.
        /// </summary>
        /// <param name="command">The <see cref="MoveCommand"/> to be executed.</param>
        public void Visit(MoveCommand command)
        {
            this.Board.Execute(command);
        }

        /// <summary>
        /// Executes a <see cref="RemoveCommand"/> in order to change the presented game state.
        /// </summary>
        /// <param name="command">The <see cref="RemoveCommand"/> to be executed.</param>
        public void Visit(RemoveCommand command)
        {
            this.Board.Execute(command);
        }

        /// <summary>
        /// Executes a <see cref="SetLastUpdateCommand"/> in order to change the presented game state.
        /// </summary>
        /// <param name="command">The <see cref="SetLastUpdateCommand"/> to be executed.</param>
        public void Visit(SetLastUpdateCommand command)
        {
            // Not used at the moment, can be used to dispay the game history in the GUI.
        }

        /// <summary>
        /// Executes a <see cref="SpawnCommand"/> in order to change the presented game state.
        /// </summary>
        /// <param name="command">The <see cref="SpawnCommand"/> to be executed.</param>
        public void Visit(SpawnCommand command)
        {
            this.Board.Execute(command);
        }

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has been changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}