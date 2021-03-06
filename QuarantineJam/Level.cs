﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace Bikip
{
    public static class Level
    {
        static Random random;
        public static void InitLevel(int level, List<Rectangle> worldHitbox, List<PhysicalObject> stuff, ref Rectangle Bounds, ref Vector2 Spawn)
        {
            random = new Random(37);
            Rectangle r(int x, int y, int w, int h) => new Rectangle(x, y, w, h);
            Bounds = Rectangle.Empty;
            worldHitbox.Clear();
            stuff.Clear();
            //Vector2 Spawn = Vector2.Zero;
            switch (level)
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
                    Bounds = new Rectangle(-280, -860, (int)(1280 / 0.8f), (int)(720 / 0.8f));
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
                case 2: // 3 beeboxs
                    worldHitbox.AddRange(new List<Rectangle> {
                            new Rectangle(-250, 0, 1030, 460) ,
                            new Rectangle(600, -230, 790, 1440) ,
                            new Rectangle(-340, -390, 360, 70) ,
                            new Rectangle(-560, -1280, 1840, 440) ,
                            new Rectangle(930, -930, 450, 870) ,
                            new Rectangle(-580, -1110, 350, 1820) ,
                    });
                    stuff.Add(new Ruche(new Vector2(470, -10), 20));
                    stuff.Add(new Ruche(new Vector2(700, -250), 20));
                    stuff.Add(new Ruche(new Vector2(-100, -400), 20, -1));
                    Bounds = new Rectangle(-500, -800, 1342, 964);
                    Spawn = new Vector2(200, -1);
                    break;

                case 3: // niveau efflam
                    worldHitbox.AddRange(new List<Rectangle> {
                        r(-150, 100, 1630, 100),
                        r(-150, -1650, 55, 1755),
                        r(1425, -1650, 55, 1755),
                        r(-150, -1655, 1700, 55),

                        r(-105,-350,210,50),
                        r(100,-450,50,350),
                        r(1380,-200,50,305),
                        r(1100,-200,100,50),
                        r(600,-500,100,50),
                        r(-100,-750,100,50),
                        r(300,-1050,500,50),
                        r(0,-1300,100,50),
                        r(1300,-1150,130,50),
                        r(700,-1605,50,310),
                        r(700,-1300,400,50),
                        r(950,-1500,150,50),
                        r(1050,-1605,50,150)
                    });
                    stuff.AddRange(new List<PhysicalObject>
                    {
                        new Bee(new Vector2(0,-250)),
                        new Bee(new Vector2(0,-400)),
                        new Bee(new Vector2(650,-550)),
                        new Bee(new Vector2(50,-1350)),
                        new Bee(new Vector2(1350,-1200)),
                        new Bee(new Vector2(1000,-1550)),
                        new Ruche(new Vector2(1150,0),50,-1)
                    });
                    stuff.AddRange(BeesFilling(new Rectangle(250, -200, 700, 250)));
                    stuff.AddRange(BeesFilling(new Rectangle(1150, -1000, 1, 800)));
                    stuff.AddRange(BeesFilling(new Rectangle(-50, -1200, 1, 400)));
                    stuff.AddRange(BeesFilling(new Rectangle(300, -1100, 500, 1)));
                    stuff.AddRange(BeesFilling(new Rectangle(300, -1350, 400, 1)));
                    Bounds = new Rectangle(-150, -1650, 1, 1800);
                    break;
                case 4: // level bees à guider
                    worldHitbox.AddRange(new List<Rectangle> {
                            new Rectangle(460, -460, 160, 30) ,
                            new Rectangle(590, -450, 30, 110) ,
                            new Rectangle(690, -530, 30, 90) ,
                            new Rectangle(590, -370, 150, 270) ,
                            new Rectangle(820, -470, 30, 350) ,
                            new Rectangle(-530, -910, 340, 1400) ,
                            new Rectangle(1230, -1230, 850, 1200) ,
                            new Rectangle(-520, -1400, 1940, 530) ,
                            new Rectangle(350, -550, 370, 30) ,
                            new Rectangle(700, -470, 130, 30) ,
                            new Rectangle(350, -530, 30, 170) ,
                            new Rectangle(-220, -160, 2060, 500) ,
                    });
                    stuff.Add(new Ruche(new Vector2(840, -160), 3));
                    stuff.AddRange(BeesFilling(new Rectangle(-125, -838, 1297, 259), 100));
                    Bounds = new Rectangle(-263, -999, 1655, 877);
                    Spawn = new Vector2(500, -200);
                    break;
                case 5: // ventilos intros
                    worldHitbox.AddRange(new List<Rectangle> {
                                                       new Rectangle(-260, -960, 350, 1760) ,
                            //new Rectangle(1130, -310, 520, 530) ,
                            new Rectangle(1380, -580, 280, 370) ,
                            new Rectangle(1590, -960, 440, 1490) ,
                            new Rectangle(0, 0, 1670, 1200) ,
                            new Rectangle(-160, -1230, 1990, 390) ,
                            new Rectangle(1010, -150, 160, 260) ,
                    });
                    stuff.Add(new Ventilateur(new Vector2(1070, -150)));
                    stuff.Add(new Ventilateur(new Vector2(1530, -600)));
                    stuff.AddRange(BeesFilling(new Rectangle(1200, -485, 153, 112)));
                    stuff.AddRange(BeesFilling(new Rectangle(1262, -148, 309, 131)));
                    stuff.AddRange(BeesFilling(new Rectangle(1000, -700, 309, 300)));
                    Bounds = new Rectangle(21, -850, 647, 530);
                    Spawn = new Vector2(330, -1);
                    break;
                case 6: // cycle de ventilos
                    worldHitbox.AddRange(new List<Rectangle> {
                            new Rectangle(0, 0, 1670, 1200) ,
                            new Rectangle(-160, -1190, 1990, 390) ,
                            new Rectangle(1550, -960, 440, 1490) ,
                            new Rectangle(-260, -960, 350, 1760) ,
                            new Rectangle(20, -310, 130, 460) ,
                            new Rectangle(360, -630, 510, 60) ,
                            new Rectangle(280, -630, 150, 730) ,
                            new Rectangle(1070, -630, 530, 60) ,
                            new Rectangle(350, -190, 150, 250) ,
                            new Rectangle(1480, -610, 90, 230) ,
                            new Rectangle(690, -380, 580, 180) ,
                    });
                    stuff.Add(new Ventilateur(new Vector2(460, -410), 1));
                    stuff.Add(new Ventilateur(new Vector2(1490, 0)));
                    stuff.AddRange(BeesFilling(new Rectangle(559, -749, 956, 95)));
                    stuff.AddRange(BeesFilling(new Rectangle(616, -510, 800, 102)));
                    stuff.AddRange(BeesFilling(new Rectangle(640, -128, 835, 103)));
                    stuff.AddRange(BeesFilling(new Rectangle(500, -410, 200, 300)));
                    stuff.AddRange(BeesFilling(new Rectangle(1300, -410, 180, 300)));
                    Bounds = new Rectangle(21, -850, 647, 530);
                    Spawn = new Vector2(200, -1);
                    break;
                case 7: // ventilos et ruches
                    worldHitbox.AddRange(new List<Rectangle> {
                            new Rectangle(-260, -960, 350, 1760) ,
                            new Rectangle(-160, -1230, 2680, 390) ,
                            new Rectangle(60, -560, 240, 100) ,
                            new Rectangle(630, -220, 330, 350) ,
                            new Rectangle(740, -950, 90, 470) ,
                            new Rectangle(1150, -580, 220, 80) ,
                            new Rectangle(0, 0, 2360, 1200) ,
                            new Rectangle(1750, -430, 640, 660) ,
                            new Rectangle(2290, -960, 550, 1490) ,
                    });
                    stuff.Add(new Ventilateur(new Vector2(2230, -430)));
                    stuff.Add(new Ventilateur(new Vector2(800, -220)));
                    stuff.Add(new Ventilateur(new Vector2(1300, -580), 1));
                    stuff.Add(new Ruche(new Vector2(1950, -440), 20));
                    stuff.AddRange(BeesFilling(new Rectangle(915, -750, 470, 200), 60));
                    stuff.AddRange(BeesFilling(new Rectangle(110, -750, 580, 227), 60));
                    stuff.AddRange(BeesFilling(new Rectangle(1094, -254, 567, 129), 80));
                    Bounds = new Rectangle(-11, -891, 2385, 931);
                    Spawn = new Vector2(200, -1);
                    break;
                case 8: // the end
            {
                    worldHitbox.AddRange(new List<Rectangle> {
                            new Rectangle(-260, -960, 350, 1760) ,
                            new Rectangle(2750, -960, 440, 1490) ,
                            new Rectangle(0, 0, 2870, 1200) ,
                            new Rectangle(2520, -240, 280, 320) ,
                    });

                        stuff.AddRange(BeesFilling(new Rectangle(550, -648, 171, 42)));
                        stuff.AddRange(BeesFilling(new Rectangle(630, -613, 40, 180)));
                        stuff.AddRange(BeesFilling(new Rectangle(830, -638, 30, 220)));
                        stuff.AddRange(BeesFilling(new Rectangle(994, -634, 20, 230)));
                        stuff.AddRange(BeesFilling(new Rectangle(848, -524, 155, 17)));
                        stuff.AddRange(BeesFilling(new Rectangle(1114, -635, 21, 226)));
                        stuff.AddRange(BeesFilling(new Rectangle(1128, -629, 135, 24)));
                        stuff.AddRange(BeesFilling(new Rectangle(1125, -524, 103, 20)));
                        stuff.AddRange(BeesFilling(new Rectangle(1121, -434, 147, 22)));
                        stuff.AddRange(BeesFilling(new Rectangle(1530, -792, 17, 355)));
                        stuff.AddRange(BeesFilling(new Rectangle(1540, -791, 191, 19)));
                        stuff.AddRange(BeesFilling(new Rectangle(1529, -625, 185, 9)));

                        stuff.AddRange(BeesFilling(new Rectangle(1531, -452, 204, 9)));
                        stuff.AddRange(BeesFilling(new Rectangle(1125, -524, 103, 20)));
                        stuff.AddRange(BeesFilling(new Rectangle(1121, -434, 147, 22)));
                        stuff.AddRange(BeesFilling(new Rectangle(1530, -792, 17, 355)));
                        stuff.AddRange(BeesFilling(new Rectangle(1540, -791, 191, 19)));
                        stuff.AddRange(BeesFilling(new Rectangle(1529, -625, 185, 9)));

                        stuff.Add(new Ruche(new Vector2(2630, -240)));
                        Bounds = new Rectangle(-200, -850, 3865, 1829);
                        Spawn = new Vector2(200, -1);
                        break;
        }
            }

            if(Bounds.Width < (int)(1280 / 0.8f)) Bounds.Width = (int)(1280/0.8f);
            if (Bounds.Height < (int)(720 / 0.8f)) Bounds.Height = (int)(720 / 0.8f);

            //return Spawn;
            // if (Bounds == Rectangle.Empty) Bounds = new Rectangle(worldHitbox.Min(rec => rec.Left),
            //                                                          worldHitbox.Min(rec => rec.Top),
            //                                                          worldHitbox.Max(rec => rec.Right) - worldHitbox.Min(rec => rec.Left),
            //                                                          worldHitbox.Max(rec => rec.Bottom) - worldHitbox.Min(rec => rec.Top));
        }

        private static List<PhysicalObject> BeesFilling(Rectangle toFill, int spaceBetweenBees = 40)
        {
            List<PhysicalObject> BeesFilling = new List<PhysicalObject>() { };
            for (int i = toFill.Left; i < toFill.Right; i+= spaceBetweenBees)
            {
                for (int j = toFill.Top; j < toFill.Bottom; j += spaceBetweenBees)
                {
                    BeesFilling.Add(new Bee(new Vector2(i, j)));
                }
            }
            return BeesFilling;
        }
    }
}
