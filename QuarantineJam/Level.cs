using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace QuarantineJam
{
    public static class Level
    {
        static Random random;
        public static void InitLevel(int level, List<Rectangle> worldHitbox, List<PhysicalObject> stuff, ref Rectangle Bounds, Vector2 Spawn)
        {
            random = new Random(37);
            Rectangle r(int x, int y, int w, int h) => new Rectangle(x, y, w, h);
            Bounds = Rectangle.Empty;
            worldHitbox.Clear();
            stuff.Clear();
            switch(level)
            {
                case 0:

                    List<Rectangle> LevelList3 = new List<Rectangle>()
                    {
                            new Rectangle(-210, 0, 1290, 640) ,
                            new Rectangle(400, -140, 110, 260) ,
                            new Rectangle(-620, -880, 450, 1500) ,
                            new Rectangle(160, -530, 470, 90) ,
                            new Rectangle(-540, -1230, 1970, 420) ,
                            new Rectangle(1250, -920, 480, 1490) ,
                            new Rectangle(1020, -260, 260, 950) ,
                    };
                    worldHitbox.AddRange(LevelList3);
                    stuff.AddRange(BeesFilling(new Rectangle(-40, -710, 100, 330)));
                    stuff.AddRange(BeesFilling(new Rectangle(270, -670, 740, 10)));
                    stuff.AddRange(BeesFilling(new Rectangle(270, -370, 400, 180)));
                    stuff.AddRange(BeesFilling(new Rectangle(1000, -750, 40, 400)));
                    foreach (PhysicalObject o in stuff)
                    {
                        o.FeetPosition += new Vector2(random.Next(-2, 2), random.Next(-2, 2));
                    }
                    Bounds = new Rectangle(-280, -860,(int) (1280 / 0.8f), (int)(720/0.8f));
                    Spawn = new Vector2(0, 0);
                    break;
                case 1:
                    worldHitbox.Add(r(0, 500, 1000, 300));

                    List<Rectangle> LevelList2 = new List<Rectangle>()
                    {
                           new Rectangle(-810, -1660, 500, 2680) ,
                            new Rectangle(-380, 0, 3330, 700) ,
                            new Rectangle(-340, -250, 220, 410) ,
                            new Rectangle(-330, -640, 210, 80) ,
                            new Rectangle(2430, -1690, 1380, 2660) ,
                            new Rectangle(1320, -610, 610, 80) ,
                            new Rectangle(1400, -350, 820, 100) ,
                            new Rectangle(1400, -260, 150, 320) ,
                            new Rectangle(530, -890, 260, 80) ,
                    };
                    worldHitbox.AddRange(LevelList2);
                    stuff.AddRange(BeesFilling(new Rectangle(360, -360, 580, 40))); // line at spawn in the air
                    stuff.AddRange(BeesFilling(new Rectangle(1455, -480, 420, 80)));
                    stuff.AddRange(BeesFilling(new Rectangle(1600, -200, 620, 160)));
                    stuff.AddRange(BeesFilling(new Rectangle(-90, -900, 80, 770)));
                    stuff.AddRange(BeesFilling(new Rectangle(150, -950, 1040, 70)));
                    foreach (PhysicalObject o in stuff)
                    {
                        o.FeetPosition += new Vector2(random.Next(-2, 2), random.Next(-2, 2));
                        //Console.WriteLine(o.FeetPosition);
                    }
                    Bounds = new Rectangle(-400, -1100, 2950, 1240);
                    Spawn = new Vector2(0, 0);
                    break;
                case 2:
                    worldHitbox.AddRange(new List<Rectangle> {
                        r(0, 720-100, 1600, 100),
                        r(0, 720-2000, 50, 2000)
                    });
                    Bounds = new Rectangle(0, 720 - 2000, 1600, 2000);
                    break;
                case 999:
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
                    stuff.Add(new Bee(new Vector2(1300, 400)));
                    stuff.Add(new Bee(new Vector2(1300, 400)));
                    stuff.Add(new Bee(new Vector2(1300, 400)));
                    stuff.Add(new Bee(new Vector2(1300, 400)));
                    stuff.Add(new Ruche(new Vector2(500, 0), 25));
                    foreach (PhysicalObject o in stuff)
                    {
                        o.FeetPosition += new Vector2(random.Next(-2, 2), random.Next(-2, 2));
                        //Console.WriteLine(o.FeetPosition);
                    }
                    Bounds = new Rectangle(-400, -1100, 2950, 1240);

                    break;
            }
           // if (Bounds == Rectangle.Empty) Bounds = new Rectangle(worldHitbox.Min(rec => rec.Left),
           //                                                          worldHitbox.Min(rec => rec.Top),
           //                                                          worldHitbox.Max(rec => rec.Right) - worldHitbox.Min(rec => rec.Left),
           //                                                          worldHitbox.Max(rec => rec.Bottom) - worldHitbox.Min(rec => rec.Top));
        }

        private static List<PhysicalObject> BeesFilling(Rectangle toFill)
        {
            List<PhysicalObject> BeesFilling = new List<PhysicalObject>() { };
            for (int i = toFill.Left; i < toFill.Right; i+= 40)
            {
                for (int j = toFill.Top; j < toFill.Bottom; j += 40)
                {
                    BeesFilling.Add(new Bee(new Vector2(i, j)));
                }
            }
            return BeesFilling;
        }
    }
}
