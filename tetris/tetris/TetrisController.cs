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

        /// <summary>
        /// Minimum time that should pass between moves.
        /// </summary>
        public static readonly TimeSpan MovementCooldown;
        /// <summary>
        /// Minimum time that should pass between rotations.
        /// </summary>
        public static readonly TimeSpan RotationCooldown;

        private readonly TetrisModel tetrisModel;
        /// <summary>
        /// The amount of time remaining until movement is allowed.
        /// </summary>
        private TimeSpan movementTimer = TimeSpan.Zero;
        /// <summary>
        /// The amount of time remaining until rotation is allowed.
        /// </summary>
        private TimeSpan rotationTimer = TimeSpan.Zero;

        BytesOfPi.Input.KeyboardHelper keyboardHelper = new BytesOfPi.Input.KeyboardHelper();


        static TetrisController()
        {
            TetrisController.MovementCooldown = new TimeSpan(0, 0, 0, 0, 50);
            TetrisController.RotationCooldown = new TimeSpan(0, 0, 0, 0, 50);
        }

        public TetrisController(TetrisModel tetrisModel)
        {
            Debug.Assert(tetrisModel != null);

            this.tetrisModel = tetrisModel;

            // We need to know if the following keys ever do a keyboard repeat.
            this.KeyboardHelper.trackKeyForHardRepeats(TetrisController.MoveLeftKey);
            this.KeyboardHelper.trackKeyForHardRepeats(TetrisController.MoveRightKey);
            this.KeyboardHelper.trackKeyForHardRepeats(TetrisController.RotateLeftKey);
            this.KeyboardHelper.trackKeyForHardRepeats(TetrisController.RotateRightKey);
        }

        private BytesOfPi.Input.KeyboardHelper KeyboardHelper
        {
            get { return this.keyboardHelper; }
            set { this.keyboardHelper = value; }
        }

        /// <summary>
        /// Attempts to move left if movement is not on cooldown.
        /// </summary>
        private void MoveLeft()
        {
            bool allowMovement = false;
            if (movementTimer <= TimeSpan.Zero &&
                    (KeyboardHelper.WasKeyJustPressed(TetrisController.MoveLeftKey) ||
                    KeyboardHelper.IsKeyHardRepeating(TetrisController.MoveLeftKey)))
            {
                if (tetrisModel.MoveLeft())
                    movementTimer = TetrisController.MovementCooldown;
            }
        }

        /// <summary>
        /// Attempts to move right if movement is not on cooldown.
        /// </summary>
        private void MoveRight()
        {
            bool allowMovement = false;
            if (movementTimer <= TimeSpan.Zero &&
                    (KeyboardHelper.WasKeyJustPressed(TetrisController.MoveRightKey) ||
                    KeyboardHelper.IsKeyHardRepeating(TetrisController.MoveRightKey)))
            {
                if (tetrisModel.MoveRight())
                    movementTimer = TetrisController.MovementCooldown;
            }
        }

        /// <summary>
        /// Attempts to rotate left if rotation is not on cooldown.
        /// </summary>
        private void RotateLeft()
        {
            bool allowRotation = false;
            if (rotationTimer <= TimeSpan.Zero &&
                (KeyboardHelper.WasKeyJustPressed(TetrisController.RotateLeftKey) ||
                KeyboardHelper.IsKeyHardRepeating(TetrisController.RotateLeftKey)))
            {
                if (tetrisModel.RotateLeft())
                    rotationTimer = TetrisController.RotationCooldown;
            }
        }

        /// <summary>
        /// Attempts to rotate right if rotation is not on cooldown.
        /// </summary>
        private void RotateRight()
        {
            bool allowRotation = false;
            if (rotationTimer <= TimeSpan.Zero &&
                (KeyboardHelper.WasKeyJustPressed(TetrisController.RotateRightKey) ||
                KeyboardHelper.IsKeyHardRepeating(TetrisController.RotateRightKey)))
            {
                if (tetrisModel.RotateRight())
                    rotationTimer = TetrisController.RotationCooldown;
            }
        }

        public void Update(GameTime gameTime)
        {
            this.KeyboardHelper.Update(gameTime);
            this.UpdateMovementTimer(gameTime);
            this.UpdateRotationTimer(gameTime);
            // Process the player controls. 
            this.ProcessKeyStates(gameTime);
        }

        /// <summary>
        /// Process the player's input (movement/rotation).
        /// </summary>
        /// <param name="gameTime"></param>
        void ProcessKeyStates(GameTime gameTime)
        {
            bool moveLeftKeyIsDown =
                this.KeyboardHelper.IsKeyDown(TetrisController.MoveLeftKey);
            bool moveRightKeyIsDown =
                this.keyboardHelper.IsKeyDown(TetrisController.MoveRightKey);

            bool rotateLeftKeyIsDown =
                this.KeyboardHelper.IsKeyDown(TetrisController.RotateLeftKey);
            bool rotateRightKeyIsDown =
                this.KeyboardHelper.IsKeyDown(TetrisController.RotateRightKey);

            // If a rotation key was just pressed, we want to immedietly get rid of
            // the rotation cooldown.
            if (KeyboardHelper.WasKeyJustPressed(TetrisController.RotateLeftKey) ||
                KeyboardHelper.WasKeyJustPressed(TetrisController.RotateRightKey))
                this.rotationTimer = TimeSpan.Zero;

            // If a movement key was just pressed, we want to immedietly get rid of
            // the movement cooldown.
            if (KeyboardHelper.WasKeyJustPressed(TetrisController.MoveLeftKey) ||
                KeyboardHelper.WasKeyJustPressed(TetrisController.MoveRightKey))
                this.movementTimer = TimeSpan.Zero;

            if (moveLeftKeyIsDown && !moveRightKeyIsDown)
                this.MoveLeft();
            else if (moveRightKeyIsDown && !moveLeftKeyIsDown)
                this.MoveRight();

            if (rotateLeftKeyIsDown && !rotateRightKeyIsDown)
                this.RotateLeft();
            else if (rotateRightKeyIsDown && !rotateLeftKeyIsDown)
                this.RotateRight();
        }

        void UpdateMovementTimer(GameTime gametTime)
        {
            if (this.movementTimer > TimeSpan.Zero)
                this.movementTimer -= gametTime.ElapsedGameTime;

            // Keep the timer from going below zero.
            if (this.movementTimer < TimeSpan.Zero)
                this.movementTimer = TimeSpan.Zero;
        }

        void UpdateRotationTimer(GameTime gameTime)
        {
            if (this.rotationTimer > TimeSpan.Zero)
                this.rotationTimer -= gameTime.ElapsedGameTime;

            if (this.rotationTimer < TimeSpan.Zero)
                this.rotationTimer = TimeSpan.Zero;
        }
    }
}
