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
                    worldHitbox.Add(r(0,500,1000,300));

                    List<Rectangle> LevelList = new List<Rectangle>()
                    {
                            new Rectangle(-920, -250, 570, 1060) ,
                            new Rectangle(-920, -1170, 330, 1020) ,
                            new Rectangle(370, -310, 260, 80) ,
                            new Rectangle(-390, 0, 1040, 190) ,
                            new Rectangle(-410, 590, 1780, 360) ,
                            new Rectangle(1030, 280, 740, 670) ,
                            new Rectangle(1440, -370, 330, 680) ,
                            new Rectangle(1270, -50, 190, 360) ,
                            new Rectangle(1560, -1030, 260, 690) ,
                            new Rectangle(-900, -1380, 2070, 520) ,
                            new Rectangle(1110, -1170, 610, 260) ,
                            new Rectangle(920, -220, 110, 150) ,
                    };
                    List<Rectangle> newList = new List<Rectangle>() {};
                    foreach (Rectangle re in LevelList)
                    {
                        re.Offset(new Vector2(200, 200));
                        newList.Add(re);
                    }
                    worldHitbox.AddRange(newList);
                    stuff.Add(new Bee(new Vector2(1300, 400)));
                    break;
            }
        }
    }
}
