using System;
using System.Diagnostics;
using BytesOfPi.Input;
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

        private readonly TetrisModel _tetrisModel;
        /// <summary>
        /// The amount of time remaining until movement is allowed.
        /// </summary>
        private TimeSpan _movementTimer = TimeSpan.Zero;
        /// <summary>
        /// The amount of time remaining until rotation is allowed.
        /// </summary>
        private TimeSpan _rotationTimer = TimeSpan.Zero;

        BytesOfPi.Input.KeyboardHelper _keyboardHelper = new BytesOfPi.Input.KeyboardHelper();


        static TetrisController()
        {
            MovementCooldown = new TimeSpan(0, 0, 0, 0, 50);
            RotationCooldown = new TimeSpan(0, 0, 0, 0, 50);
        }

        public TetrisController(TetrisModel tetrisModel)
        {
            Debug.Assert(tetrisModel != null);

            _tetrisModel = tetrisModel;

            // We need to know if the following keys ever do a keyboard repeat.
            KeyboardHelper.TrackKeyForHardRepeats(MoveLeftKey);
            KeyboardHelper.TrackKeyForHardRepeats(MoveRightKey);
            KeyboardHelper.TrackKeyForHardRepeats(RotateLeftKey);
            KeyboardHelper.TrackKeyForHardRepeats(RotateRightKey);
        }

        private KeyboardHelper KeyboardHelper
        {
            get { return _keyboardHelper; }
            set { _keyboardHelper = value; }
        }

        /// <summary>
        ///     Attempts to move left if movement is not on cooldown.
        /// </summary>
        private void MoveLeft()
        {
            bool allowMovement = false;
            if (_movementTimer <= TimeSpan.Zero &&
                (KeyboardHelper.IsKeyJustPressed(MoveLeftKey) ||
                 KeyboardHelper.IsKeyHardRepeating(MoveLeftKey)))
            {
                if (_tetrisModel.MoveLeft())
                    _movementTimer = MovementCooldown;
            }
        }

        /// <summary>
        ///     Attempts to move right if movement is not on cooldown.
        /// </summary>
        private void MoveRight()
        {
            bool allowMovement = false;
            if (_movementTimer <= TimeSpan.Zero &&
                (KeyboardHelper.IsKeyJustPressed(MoveRightKey) ||
                 KeyboardHelper.IsKeyHardRepeating(MoveRightKey)))
            {
                if (_tetrisModel.MoveRight())
                    _movementTimer = MovementCooldown;
            }
        }

        /// <summary>
        ///     Attempts to rotate left if rotation is not on cooldown.
        /// </summary>
        private void RotateLeft()
        {
            bool allowRotation = false;
            if (_rotationTimer <= TimeSpan.Zero &&
                (KeyboardHelper.IsKeyJustPressed(RotateLeftKey) ||
                 KeyboardHelper.IsKeyHardRepeating(RotateLeftKey)))
            {
                if (_tetrisModel.RotateLeft())
                    _rotationTimer = RotationCooldown;
            }
        }

        /// <summary>
        ///     Attempts to rotate right if rotation is not on cooldown.
        /// </summary>
        private void RotateRight()
        {
            bool allowRotation = false;
            if (_rotationTimer <= TimeSpan.Zero &&
                (KeyboardHelper.IsKeyJustPressed(RotateRightKey) ||
                 KeyboardHelper.IsKeyHardRepeating(RotateRightKey)))
            {
                if (_tetrisModel.RotateRight())
                    _rotationTimer = RotationCooldown;
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardHelper.Update(gameTime);
            UpdateMovementTimer(gameTime);
            UpdateRotationTimer(gameTime);
            // Process the player controls. 
            ProcessKeyStates(gameTime);

            if (KeyboardHelper.IsKeyJustPressed(Keys.Space))
                _tetrisModel.HardDrop();
            else if (KeyboardHelper.IsKeyJustPressed(Keys.LeftShift))
                _tetrisModel.SoftDrop();
        }

        /// <summary>
        ///     Process the player's input (movement/rotation).
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessKeyStates(GameTime gameTime)
        {
            bool moveLeftKeyIsDown =
                KeyboardHelper.IsKeyDown(MoveLeftKey);
            bool moveRightKeyIsDown =
                _keyboardHelper.IsKeyDown(MoveRightKey);

            bool rotateLeftKeyIsDown =
                KeyboardHelper.IsKeyDown(RotateLeftKey);
            bool rotateRightKeyIsDown =
                KeyboardHelper.IsKeyDown(RotateRightKey);

            // If a rotation key was just pressed, we want to immedietly get rid of
            // the rotation cooldown.
            if (KeyboardHelper.IsKeyJustPressed(RotateLeftKey) ||
                KeyboardHelper.IsKeyJustPressed(RotateRightKey))
                _rotationTimer = TimeSpan.Zero;

            // If a movement key was just pressed, we want to immedietly get rid of
            // the movement cooldown.
            if (KeyboardHelper.IsKeyJustPressed(MoveLeftKey) ||
                KeyboardHelper.IsKeyJustPressed(MoveRightKey))
                _movementTimer = TimeSpan.Zero;

            if (moveLeftKeyIsDown && !moveRightKeyIsDown)
                MoveLeft();
            else if (moveRightKeyIsDown && !moveLeftKeyIsDown)
                MoveRight();

            if (rotateLeftKeyIsDown && !rotateRightKeyIsDown)
                RotateLeft();
            else if (rotateRightKeyIsDown && !rotateLeftKeyIsDown)
                RotateRight();
        }

        private void UpdateMovementTimer(GameTime gametTime)
        {
            if (_movementTimer > TimeSpan.Zero)
                _movementTimer -= gametTime.ElapsedGameTime;

            // Keep the timer from going below zero.
            if (_movementTimer < TimeSpan.Zero)
                _movementTimer = TimeSpan.Zero;
        }

        private void UpdateRotationTimer(GameTime gameTime)
        {
            if (_rotationTimer > TimeSpan.Zero)
                _rotationTimer -= gameTime.ElapsedGameTime;

            if (_rotationTimer < TimeSpan.Zero)
                _rotationTimer = TimeSpan.Zero;
        }
    }
}
