using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deadlocked.Classes
{
    public class GameActionEventArgs : EventArgs
    {
        public Action action;
        public Level level;
        public GameActionEventArgs(Action action, Level level)
        {
            this.action = action;
            this.level = level;
        }
    }
}
