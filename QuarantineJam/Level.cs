using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace QuarantineJam
{
    public static class Level
    {
        public static void InitLevel(int level, List<Rectangle> worldHitbox, List<PhysicalObject> stuff)
        {
            Rectangle r(int x, int y, int w, int h) => new Rectangle(x, y, w, h);
            worldHitbox.Clear();
            stuff.Clear();
            switch(level)
            {
                case 0:
                    //worldHitbox.Add();
                    break;
            }
        }
    }
}
