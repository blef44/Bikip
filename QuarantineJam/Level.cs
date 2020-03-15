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
        public static void InitLevel(int level, List<Rectangle> worldHitbox, List<PhysicalObject> stuff, ref Rectangle Bounds)
        {
            random = new Random(37);
            Rectangle r(int x, int y, int w, int h) => new Rectangle(x, y, w, h);
            Bounds = Rectangle.Empty;
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
                    stuff.AddRange(BeesFilling(new Rectangle(150, -1050, 1040, 140)));
                    foreach (PhysicalObject o in stuff)
                    {
                        o.FeetPosition += new Vector2(random.Next(-2, 2), random.Next(-2, 2));
                        //Console.WriteLine(o.FeetPosition);
                    }
                    Bounds = new Rectangle(-400, -1100, 2950, 1240);

                    break;
                case 2:
                    worldHitbox.AddRange(new List<Rectangle>
                    {
                        r(0, 720-100, 1280, 100)
                    });
                    Bounds = new Rectangle(0, 720 - 2000, 1280, 2000);
                    break;
            }
            if (Bounds == Rectangle.Empty) Bounds = new Rectangle(worldHitbox.Min(rec => rec.Left),
                                                                     worldHitbox.Min(rec => rec.Top),
                                                                     worldHitbox.Max(rec => rec.Right),
                                                                     worldHitbox.Max(rec => rec.Bottom));
        }

        public static List<PhysicalObject> BeesFilling(Rectangle toFill)
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
