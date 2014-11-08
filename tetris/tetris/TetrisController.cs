using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    class TetrisController
    {
        public const Keys MoveLeftKey = Keys.A;
        public const Keys MoveRightKey = Keys.D;
        public const Keys RotateLeftKey = Keys.Left;
        public const Keys RotateRightKey = Keys.Right;

        public static readonly TimeSpan MovementCooldown;
        public static readonly TimeSpan RotationCooldown;

        private readonly TetrisModel tetrisModel;

        private TimeSpan movementTimer = TetrisController.MovementCooldown;
        private TimeSpan rotationTimer = TetrisController.RotationCooldown;

        BytesOfPi.Input.KeyboardHelper keyboardHelper = new BytesOfPi.Input.KeyboardHelper();


        static TetrisController()
        {
            TetrisController.MovementCooldown = new TimeSpan(0, 0, 0, 0, 100);
            TetrisController.RotationCooldown = new TimeSpan(0, 0, 1);
        }

        public TetrisController(TetrisModel tetrisModel)
        {
            Debug.Assert(tetrisModel != null);

            this.tetrisModel = tetrisModel;
            this.MovementAllowed = true;
            this.RotationAllowed = true;
        }

        private TetrisModel TetrisModel { get { return tetrisModel; } }

        public TimeSpan MovementTimer
        {
            get { return this.movementTimer; }
            set { this.movementTimer = value; }
        }

        public TimeSpan RotationTimer
        {
            get { return this.rotationTimer; }
            set { this.rotationTimer = value; }
        }

        public bool MovementAllowed
        {
            get { return this.MovementTimer <= TimeSpan.Zero; }
            private set
            {
                if (value == false)
                    this.MovementTimer = TetrisController.MovementCooldown;
                else
                    this.MovementTimer = TimeSpan.Zero;
            }
        }

        public bool RotationAllowed { get; private set; }


        private BytesOfPi.Input.KeyboardHelper KeyboardHelper
        {
            get { return this.keyboardHelper; }
            set { this.keyboardHelper = value; }
        }

        private void MoveLeft()
        {
            if (this.MovementAllowed)
            {
                this.TetrisModel.MoveLeft();
                this.MovementAllowed = false;
            }
        }
        /// <summary>
        /// Move right and then disallow movement.
        /// </summary>
        /// <seealso cref="TetrisController.MovementCooldown"/>
        public void MoveRight()
        {
            if (this.MovementAllowed)
            {
                this.TetrisModel.MoveRight();
                this.MovementAllowed = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            this.KeyboardHelper.Update();
            this.UpdateMovementTimer(gameTime);
            this.UpdateRotationTimer(gameTime);

            bool moveLeftKeyIsDown =
                this.KeyboardHelper.CurrentKeyboardState.IsKeyDown(TetrisController.MoveLeftKey);
            bool moveRightKeyIsDown =
                this.keyboardHelper.CurrentKeyboardState.IsKeyDown(TetrisController.MoveRightKey);

            if (MovementAllowed)
            {
                // Move left if only the left key is down.
                if (moveLeftKeyIsDown && !moveRightKeyIsDown)
                    this.MoveLeft();
                // Move right if only the right key is down.
                else if (moveRightKeyIsDown && !moveLeftKeyIsDown)
                    this.MoveRight();
            }
        }

        private void UpdateMovementTimer(GameTime gameTime)
        {
            // If the player is not allowed to move, then we
            // will count down until enough time has passed
            // to allow movement.
            if (!this.MovementAllowed)
            {
                this.MovementTimer -= gameTime.ElapsedGameTime;
                if (this.MovementTimer <= TimeSpan.Zero)
                    this.MovementAllowed = true;
            }
        }

        private void UpdateRotationTimer(GameTime gameTime)
        {
            // If the player is not allowed to rotate, we'll count down until
            // the player can rotate again.
            if (!this.RotationAllowed)
            {
                this.RotationTimer -= gameTime.ElapsedGameTime;
                if (this.RotationTimer <= TimeSpan.Zero)
                {
                    this.RotationAllowed = true;
                    this.RotationTimer = TetrisController.RotationCooldown;
                }
            }
        }


    }
}
