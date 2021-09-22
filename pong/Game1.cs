using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pong
{


    public class Game1 : Game
    {
        enum GameState
        {
            start,
            play,
            end
        }

        //sprites batch
        private GraphicsDeviceManager _graphics;
        private SpriteBatch sprites;
        //sprites
        private Texture2D redpad;
        private Texture2D bluepad;
        private Texture2D ball;
        //rectangles and vectors for sprites
        private Vector2 redPos, bluePos;
        private Rectangle rposscore1, rposscore2, rposscore3;
        private Rectangle bposscore1, bposscore2, bposscore3;
        private Vector2 ballPos;

        //random value
        private Random randomVal;
        private double theta;
        
        //frame with height values
        int framex = 600;
        int framey = 480;
        //object size values
        int padx = 16;
        int pady = 96;
        int ballxy = 16;

        //text messegas
        string start = "press space to start the game";
        string bluewins = "Red was bad so blue is the winner";
        string Redwins = "Blue wasn't good enough red is the winner";

        //lives
        int redlives, bluelives;

        // object speeds
        private int playerSpeed;
        private float ballSpeed;
        private KeyboardState currentKBstate, previousKBstate;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            //frame with height and with
            _graphics.PreferredBackBufferWidth = framex;
            _graphics.PreferredBackBufferHeight = framey;

            Content.RootDirectory = "Content";
            IsMouseVisible = false;

        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //lives

            int startbally = framey / 2 - ballxy / 2;
            int startballx = framex / 2 - ballxy / 2;


            // lives and removal
            redlives = 3;
            bluelives = 3;

            // object speeds
            playerSpeed = 5;
            ballSpeed = 5.0f;

            //object start position values
            int startpady = framey / 2 - pady / 2;
            int startpadx = framex - padx;

            // load rectangle location for sprites
            redPos = new Vector2(0, startpady);
            bluePos = new Vector2(startpadx, startpady);
            ballPos = new Vector2(startballx, startbally);

            rposscore1 = new Rectangle(framex / 15 + 4 * ballxy, framey / 20, ballxy, ballxy);
            rposscore2 = new Rectangle(framex / 15 + 2 * ballxy, framey / 20, ballxy, ballxy);
            rposscore3 = new Rectangle(framex / 15, framey / 20, ballxy, ballxy);

            bposscore1 = new Rectangle(framex / 15*14 - ballxy - ballxy*4, framey/20, ballxy, ballxy);
            bposscore2 = new Rectangle(framex / 15*14 - ballxy - ballxy*2, framey/20, ballxy, ballxy);
            bposscore3 = new Rectangle(framex / 15*14 - ballxy, framey/20, ballxy, ballxy);


            // random angle
            
            randomVal = new Random();
            theta = randomVal.NextDouble() * 2 * Math.PI;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sprites = new SpriteBatch(GraphicsDevice);

            // load call for sprites
            redpad = Content.Load<Texture2D>("rodeSpeler");
            bluepad = Content.Load<Texture2D>("blauweSpeler");
            ball = Content.Load<Texture2D>("bal");

        }

        protected void playerMovement()
        {
            // Moving the red paddle
            if (currentKBstate.IsKeyDown(Keys.W))
            {
                redPos -= new Vector2(0, playerSpeed);
                if (redPos.Y < 0)
                { redPos = new Vector2(0, 0); }
            }
            else if (currentKBstate.IsKeyDown(Keys.S))
            {
                redPos += new Vector2(0, playerSpeed);
                if (redPos.Y > framey - pady)
                { redPos = new Vector2(0, framey - pady); }
            }

            // Moving the blue paddle
            if (currentKBstate.IsKeyDown(Keys.Up))
            {
                bluePos -= new Vector2(0, playerSpeed);
                if (bluePos.Y < 0)
                { bluePos = new Vector2(framex - padx, 0); }
            }
            else if (currentKBstate.IsKeyDown(Keys.Down))
            {
                bluePos += new Vector2(0, playerSpeed);
                if (bluePos.Y > framey - pady)
                { bluePos = new Vector2(framex - padx, framey - pady); }
            }
        }

        protected void BallMovement()
        {
            // Updating the position of the ball
            ballPos += new Vector2((float)(ballSpeed * Math.Cos(theta)), (float)(-ballSpeed * Math.Sin(theta)));
        }

        protected void BallCollision()
        {
            // Floor and roof collision detection and correction
            if (ballPos.Y < 0)
            {
                ballPos = new Vector2(ballPos.X, 0);
                theta = (2 * Math.PI - theta);
            }
            else if (ballPos.Y > framey - ballxy)
            {
                ballPos = new Vector2(ballPos.X, framey - ballxy);
                theta = (2 * Math.PI - theta);
            }

            // Paddle collision detection and correction
            // Red (Player 1)
            if (ballPos.X < padx & ballPos.Y >= redPos.Y & ballPos.Y <= redPos.Y + pady)
            {
                ballPos = new Vector2(padx, ballPos.Y);
                theta = (Math.PI - theta);
                ballSpeed += 0.2f;
            }
            // Blue (Player 2)
            else if (ballPos.X > framex - 32 & ballPos.Y >= bluePos.Y & ballPos.Y <= bluePos.Y + pady)
            {
                ballPos = new Vector2(framex - padx - ballxy, ballPos.Y);
                theta = (Math.PI - theta);
                ballSpeed += 0.2f;
            }
        }


        protected override void Update(GameTime gameTime)
        {
            //exit game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            currentKBstate = Keyboard.GetState();
            playerMovement();
            BallMovement();
            BallCollision();



            // lives counter
            int startbally = framey / 2 - ballxy / 2;
            int startballx = framex / 2 - ballxy / 2;
            if (ballPos.X > framex - ballxy)
            {
                bluelives -= 1;
                ballPos.X = startballx;
                ballPos.Y = startbally;
            }

            if (ballPos.X < 0)
            {
                redlives -= 1;
                ballPos.X = startballx;
                ballPos.Y = startbally;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);


            //draw function for sprites
            sprites.Begin();
            sprites.Draw(redpad, redPos, null, Color.White);
            sprites.Draw(bluepad, bluePos, null, Color.White);
            sprites.Draw(ball, ballPos, null, Color.White);

            // lives counter
            switch (redlives)
            {
                case 1:
                    sprites.Draw(ball, rposscore1, null, Color.Red);
                    sprites.Draw(ball, rposscore2, null, Color.Black);
                    sprites.Draw(ball, rposscore3, null, Color.Black);
                    break;

                case 2:
                    sprites.Draw(ball, rposscore1, null, Color.Red);
                    sprites.Draw(ball, rposscore2, null, Color.Red);
                    sprites.Draw(ball, rposscore3, null, Color.Black);
                    break;

                case 3:
                    sprites.Draw(ball, rposscore1, null, Color.Red);
                    sprites.Draw(ball, rposscore2, null, Color.Red);
                    sprites.Draw(ball, rposscore3, null, Color.Red);
                    break;
            }

            switch (bluelives)
            {
                case 1:
                    sprites.Draw(ball, bposscore1, null, Color.Blue);
                    sprites.Draw(ball, bposscore2, null, Color.Black);
                    sprites.Draw(ball, bposscore3, null, Color.Black);
                    break;

                case 2:
                    sprites.Draw(ball, bposscore1, null, Color.Blue);
                    sprites.Draw(ball, bposscore2, null, Color.Blue);
                    sprites.Draw(ball, bposscore3, null, Color.Black);
                    break;

                case 3:
                    sprites.Draw(ball, bposscore1, null, Color.Blue);
                    sprites.Draw(ball, bposscore2, null, Color.Blue);
                    sprites.Draw(ball, bposscore3, null, Color.Blue);
                    break;
            }


            base.Draw(gameTime);
            
            sprites.End();

            
        }
    }
}