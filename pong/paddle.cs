using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace pong
{
    class paddle : Game
    {
        // declare paddle variables
        public int padWidth, padHeight;
        public int frameWidth, frameHeight;
        public string color;
        public Vector2 paddlePos;
        private int playerSpeed;
        Keys up;
        Keys down;

        
        public paddle(int apadWidth, int apadHeight, int aframeWidth, int aframeHeight, string acolor)
        {
            padWidth = apadWidth;
            padHeight = apadHeight;
            color = acolor;
            frameWidth = aframeWidth;
            frameHeight = aframeHeight;
            playerSpeed = 5;

            // paddle movement keys
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

        public Vector2 PaddleStart()
        {
            // paddle starting position
            if (color == "red")
            {
                paddlePos.X = 0;
            }
            else if (color == "blue")
            {
                paddlePos.X = frameWidth - padWidth;
            }
            paddlePos.Y = frameHeight / 2 - padHeight / 2;

            return paddlePos;
        }

        public void PaddleMovement()
        {
            int frameHeight_padHeight = frameHeight - padHeight;

            // paddle movement up
            if (Keyboard.GetState().IsKeyDown(up))
            {
                paddlePos.Y -= playerSpeed;
                if (paddlePos.Y < 0)
                    paddlePos.Y = 0;
            }
            // paddle movement down
            else if (Keyboard.GetState().IsKeyDown(down))
            {
                paddlePos.Y += playerSpeed;
                if (paddlePos.Y > frameHeight_padHeight)
                    paddlePos.Y = frameHeight_padHeight;
            }
        }
    }
}
