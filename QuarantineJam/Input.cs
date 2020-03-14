using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace QuarantineJam
{
    public static class Input
    {
        public static int direction; // -1 = left, 1 = right
        public static Keys Jump = Keys.Up;
        public static Keys Left = Keys.Left;
        public static Keys Right = Keys.Right;
        //public static Keys  = Keys.;
        public static void Update(KeyboardState ks)
        {
            if (ks.IsKeyDown(Left) && !ks.IsKeyDown(Right)) direction = -1;
            else if (!ks.IsKeyDown(Left) && ks.IsKeyDown(Right)) direction = 1;
            else direction = 0;
        }
    }
}
