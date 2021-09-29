﻿using System;
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

        public paddle(int apadx, int apady, int aframex, int aframey, string acolor)
        {
            padx = apadx;
            pady = apady;
            color = acolor;
            framex = aframex;
            framey = aframey;

            paddlestart(color);
            paddlemovement(color);
        }

        Vector2 paddlestart(string color)
        {
            startpady = framey / 2 - pady / 2;
            startpadx = framex - padx;
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

        void paddlemovement(string color)
        {
            int frameypady = framey - pady;
            int playerSpeed = 5;
            var up = Keys.None;
            var down = Keys.None;

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