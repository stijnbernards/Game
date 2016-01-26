using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GenHelpers
{
    public class Rectangle
    {
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        private int width;
        private int height;

        public Rectangle(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}

