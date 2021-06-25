//-----------------------------------------------------------------------
// <copyright file="PlacedPieceVM.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.ViewModel.Piece
{
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.ViewModel.Game;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents the view model of a chess piece placed on a chess board.
    /// </summary>
    public class PlacedPieceVM : INotifyPropertyChanged, IChessPieceVisitable
    {
        /// <summary>
        /// Indicates whether the placed chess piece is marked for removal.
        /// </summary>
        private bool removed;

        /// <summary>
        /// Represents the position of the placed chess piece.
        /// </summary>
        private PositionVM position;

        /// <summary>
        /// Represents the placed chess piece.
        /// </summary>
        private ChessPiece piece;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlacedPieceVM"/> class.
        /// </summary>
        /// <param name="piece">The placed chess piece, including its position.</param>
        public PlacedPieceVM(PlacedPiece piece) : this(piece.Position, piece.Piece)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlacedPieceVM"/> class.
        /// </summary>
        /// <param name="position">The position of the placed chess piece.</param>
        /// <param name="piece">The placed chess piece.</param>
        public PlacedPieceVM(Position position, ChessPiece piece)
        {
            this.Removed = false;
            this.Position = new PositionVM(position);
            this.Piece = piece;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the placed chess piece is marked for removal.
        /// </summary>
        /// <value>True if the placed chess piece is marked for removal, or else false.</value>
        public bool Removed
        {
            get
            {
                return this.removed;
            }

            set
            {
                if (this.removed != value)
                {
                    this.removed = value;
                    this.OnPropertyChanged(nameof(this.Removed));
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the placed chess piece.
        /// </summary>
        /// <value>The position of the placed chess piece.</value>
        public PositionVM Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if (this.position != value)
                {
                    this.position = value ?? throw new ArgumentNullException(nameof(this.Position));
                    this.OnPropertyChanged(nameof(this.Position));
                }
            }
        }

        /// <summary>
        /// Gets the placed chess piece.
        /// </summary>
        /// <value>The placed chess piece.</value>
        public ChessPiece Piece
        {
            get
            {
                return this.piece;
            }

            private set
            {
                if (this.piece != value)
                {
                    this.piece = value ?? throw new ArgumentNullException(nameof(this.Piece));
                    this.OnPropertyChanged(nameof(this.Piece));
                }
            }
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call it back based on the type of the piece.
        /// </summary>
        /// <param name="visitor">The chess piece visitor to be called back by the piece.</param>
        public void Accept(IChessPieceVisitor visitor)
        {
            this.Piece.Accept(visitor);
        }

        /// <summary>
        /// Accepts a chess piece visitor in order to call it back based on the type of the piece.
        /// </summary>
        /// <typeparam name="T">The result type of the visitor when processing the chess piece.</typeparam>
        /// <param name="visitor">The chess piece visitor to be called back by the piece.</param>
        /// <returns>The result of the visitor when processing the chess piece.</returns>
        public T Accept<T>(IChessPieceVisitor<T> visitor)
        {
            return this.Piece.Accept(visitor);
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