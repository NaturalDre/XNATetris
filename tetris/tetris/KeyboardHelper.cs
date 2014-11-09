using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BytesOfPi.Input
{
    class KeyboardHelper
    {
        public static readonly TimeSpan HardRepeatTime;
        private KeyboardState PreviousKeyboardState { get; set; }
        private KeyboardState CurrentKeyboardState { get; set; }




        private List<Keys> trackedKeys = new List<Keys>();
        private Dictionary<Keys, TimeSpan> trackedKeysRepeatTime = new Dictionary<Keys, TimeSpan>();

        static KeyboardHelper()
        {
            KeyboardHelper.HardRepeatTime = new TimeSpan(0, 0, 0, 0, 500);
        }
        public KeyboardHelper()
        {
            PreviousKeyboardState = new KeyboardState();
            CurrentKeyboardState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            this.PreviousKeyboardState = this.CurrentKeyboardState;
            this.CurrentKeyboardState = Keyboard.GetState();

            foreach (Keys key in trackedKeys)
            {
                // TODO: Add IsNewKeyUp function and update the below statement
                if (IsKeyUp(key))
                    this.trackedKeysRepeatTime[key] = TimeSpan.Zero;
                else if (this.IsKeySoftRepeating(key))
                {
                    this.trackedKeysRepeatTime[key] += gameTime.ElapsedGameTime;
                }
            }
        }


        /// <summary>
        /// Was the key up on the previous state, but is now down?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyJustPressed(Keys key)
        {
            return PreviousKeyboardState.IsKeyUp(key) &&
                CurrentKeyboardState.IsKeyDown(key);
        }
        /// <summary>
        /// Was the key down on the previous state, but is now up?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyJustReleased(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) &&
                CurrentKeyboardState.IsKeyUp(key);
        }
        /// <summary>
        /// Was the key down on the previous state and is still down
        /// on the current state?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeySoftRepeating(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) &&
                CurrentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        ///  Has the key been held down consistently for more 
        ///  than KeyboardHelper.HardRepeatTime?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyHardRepeating(Keys key)
        {
            if (this.trackedKeysRepeatTime.ContainsKey(key) &&
                this.trackedKeysRepeatTime[key] >= KeyboardHelper.HardRepeatTime)
                return true;

            return false;
        }

        public bool IsKeyDown(Keys key) { return this.CurrentKeyboardState.IsKeyDown(key); }
        public bool WasKeyDown(Keys key) { return this.PreviousKeyboardState.IsKeyDown(key); }

        public bool IsKeyUp(Keys key) { return this.CurrentKeyboardState.IsKeyUp(key); }
        public bool WasKeyUp(Keys key) { return this.PreviousKeyboardState.IsKeyUp(key); }

        /// <summary>
        /// Track a key for hard repeats.
        /// </summary>
        /// <param name="key"></param>
        /// <remarks> This exists because I'd have to track every key otherwise,
        /// and in most cases only a few keys need to be tracked for keyboard repeats.
        /// </remarks>
        public void TrackKeyForHardRepeats(Keys key)
        {
            if (!trackedKeys.Contains(key))
            {
                this.trackedKeys.Add(key);
                this.trackedKeysRepeatTime[key] = TimeSpan.Zero;
            }
        }
    }
}
