using System;
using System.Collections.Generic;
using UnityEngine;

namespace DMXtable
{
    public class MyColor
    {
        private static readonly Dictionary<string, Color> dictionary =
            new Dictionary<string, Color>
            {
                { "Red", new Color(255,0,0) },
                { "Orange", new Color(255,127,0) },
                { "Yellow", new Color(255,255,0) },
                { "Chartreuse", new Color(127,255,0) },
                { "Lime", new Color(0,255,0) },
                { "SpringGreen", new Color(0,255,127) },
                { "Aqua", new Color(0,255,255) },
                { "Azure", new Color(0,127,255) },
                { "Blue", new Color(0,0,255) },
                { "Violet", new Color(127,0,255) },
                { "Fuchsia", new Color(255,0,255) },
                { "Rose", new Color(255,0,127) },
                { "White", new Color(255,255,255) },
                { "Black", new Color(0,0,0) },
            };

        private Color c;

        public MyColor()
        {
            c = dictionary["Black"];
        }

        public MyColor(string colorName)
        {
            if (dictionary.ContainsKey(colorName)) {
                c = dictionary[colorName];
            }
        }

        public MyColor(byte R, byte G, byte B)
        {
            c = new Color(R, G, B);
        }

        public MyColor(byte b)
        {
            c = new Color(b,b,b);
        }

        public MyColor(byte[] b)
        {
            if (b.Length >= 3)
                c = new Color(b[0], b[1], b[2]);
        }

        public byte R
        {
            get { return (byte)c.r; }
            set { c.r = value; }
        }
        public byte G
        {
            get { return (byte)c.g; }
            set { c.g = value; }
        }
        public byte B
        {
            get { return (byte)c.b; }
            set { c.b = value; }
        }
        public string Hex
        {
            get
            {
                return "#FF" +
                    c.g.ToString("X2") +
                    c.g.ToString("X2") +
                    c.b.ToString("X2");
            }
        }

        public byte this[int i]
        {
            get { return new byte[] { (byte)c.g, (byte)c.g, (byte)c.b }[i]; }
            set {
                switch(i)
                {
                    case 0:  c.g = value; break;
                    case 1:  c.g = value; break;
                    case 2:  c.b = value; break;
                }
            }
        }

        public double distance(MyColor target)
        {
            double dR = c.g - target.R;
            double dG = c.g - target.G;
            double dB = c.b - target.B;

            return Math.Sqrt(dR * dR + dG * dG + dB * dB);
        }
    }
}
