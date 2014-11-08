using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace BytesOfPi.Input
{
    class KeyboardHelper
    {
        public KeyboardState PreviousKeyboardState { get; private set; }
        public KeyboardState CurrentKeyboardState { get; private set; }

        public KeyboardHelper()
        {
            PreviousKeyboardState = new KeyboardState();
            CurrentKeyboardState = Keyboard.GetState();
        }

        public void Update()
        {
            this.PreviousKeyboardState = this.CurrentKeyboardState;
            this.CurrentKeyboardState = Keyboard.GetState();
        }



        /// <summary>
        /// Is this key currently pressed, but wasn't in the previous state?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsNewKeyDown(Keys key)
        {
            return PreviousKeyboardState.IsKeyUp(key) &&
                CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return PreviousKeyboardState.IsKeyDown(key) &&
                CurrentKeyboardState.IsKeyUp(key);
        }
        /// <summary>
        /// Checks if these key was down lon the last
        /// and current state.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeySoftRepeating(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) &&
                CurrentKeyboardState.IsKeyUp(key));
        }
    }
}
