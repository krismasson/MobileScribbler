using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    class PathManager
    {
        public static List<SKPath> Paths { get; set; }

        static PathManager()
        {
            Paths = new List<SKPath>();
        }
    }
}
