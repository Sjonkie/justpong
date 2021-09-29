using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace pong
{
    class paddle : Game
    {
        public int padx, pady;
        public int startpady, startpadx;
        public int framex, framey;
        public string color;
        public Vector2 startpad;
        private int playerSpeed;
        Keys up;
        Keys down;

        public paddle(int apadx, int apady, int aframex, int aframey, string acolor)
        {
            padx = apadx;
            pady = apady;
            color = acolor;
            framex = aframex;
            framey = aframey;
            playerSpeed = 5;

            if (color == "red")
            {
                up = Keys.W;
                down = Keys.S;
            }

            else if (color == "blue")
            {
                up = Keys.Up;
                down = Keys.Down;
            }




        }

        public Vector2 paddlestart()
        {
            if (color == "red")
            {
                startpad.X = 0;
            }
            else if (color == "blue")
            {
                startpad.X = framex - padx;
            }
            startpad.Y = framey / 2 - pady / 2;

            return startpad;
        }

        public void paddlemovement()
        {
            int frameypady = framey - pady;

            // up
            if (Keyboard.GetState().IsKeyDown(up))
            {
                startpad.Y -= playerSpeed;
                if (startpad.Y < 0)
                    startpad.Y = 0;
            }

            else if (Keyboard.GetState().IsKeyDown(down))
            {
                startpad.Y += playerSpeed;
                if (startpad.Y > frameypady)
                    startpad.Y = frameypady;
            }
        }



    }
}
