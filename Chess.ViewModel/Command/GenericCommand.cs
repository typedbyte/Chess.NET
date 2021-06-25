//-----------------------------------------------------------------------
// <copyright file="GenericCommand.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Command
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Represents a generic <see cref="ICommand"/> that can execute a specified <see cref="Action"/>
    /// without a precondition.
    /// </summary>
    public class GenericCommand : ICommand
    {
        /// <summary>
        /// Represents the function which determines if the command can execute in its current state.
        /// </summary>
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Represents the action to be executed when the <see cref="ICommand"/> is triggered.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCommand"/> class.
        /// </summary>
        /// <param name="canExecute">The function which determines if the command can execute in its current state.</param>
        /// <param name="action">The action to be executed when the <see cref="ICommand"/> is triggered.</param>
        public GenericCommand(Func<bool> canExecute, Action action)
        {
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="_">The data used by the command, which will never be used.</param>
        /// <returns>Returns always true.</returns>
        public bool CanExecute(object _)
        {
            return this.canExecute();
        }

        /// <summary>
        /// Represents the method to be called when the command is invoked.
        /// </summary>
        /// <param name="_">The data used by the command, which will never be used.</param>
        public void Execute(object _)
        {
            this.action();
        }

        /// <summary>
        /// Fires the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void FireCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}