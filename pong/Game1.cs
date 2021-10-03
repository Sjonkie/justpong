using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace pong
{


    public class Game1 : Game
    {
        // Declare sprites batch
        private GraphicsDeviceManager _graphics;
        private SpriteBatch sprites;
        
        // Declare comic sans font
        private SpriteFont comic;
        
        // Declare sprites
        private Texture2D redPad, bluePad, ball, logo;

        // Declare music
        private Song menu_music;
        
        // Declare rectangles and vectors for sprites
        private Vector2 ballPos;
        private Rectangle logoPos;
        
        // Declare vectors for text
        private Vector2 startString1, startString2;

        // Declare random value
        private Random randomVal;
        private double theta;

        // Declare frame width & height values
        public int frameWidth = 600;
        public int frameHeight = 480;
        // Declare object size values
        private int padWidth = 16;
        private int padHeight = 96;
        private int ballDim = 16;

        // Declare text messegas
        private string start1 ="PRESS 1 FOR LIVES";
        private string start2 = "PRESS 2 FOR SCORE";
        private string blueWin ="BLUE WINS";
        private string redWin ="RED WINS";
        private string pressEnter = "PRESS TAB";
        
        // Declare lives and scores
        private int[] lives, score;
        private Rectangle scoreBall;

        // Declare object speeds
        private float ballSpeed, critEdge;
        private double critEffect;

        // Declare start positions objects
        private int ballStartX, ballStartY;
        
        // Declare gamestage
        private int gameStage;

        // Declare paddle objects
        paddle redPaddle, bluePaddle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            // initialise frame width & height
            _graphics.PreferredBackBufferWidth = frameWidth;
            _graphics.PreferredBackBufferHeight = frameHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }


        protected override void Initialize()
        {   
           
            // initialise ball start location
            ballStartY = frameHeight / 2 - ballDim / 2;
            ballStartX = frameWidth / 2 - ballDim / 2;
            ballPos = new Vector2(ballStartX, ballStartY);

            // initialise lives & score
            lives = new int[2];
            lives[0] = 3;
            lives[1] = 3;

            score = new int[2];
            score[0] = 0;
            score[1] = 0;

            // initialise ball speed
            ballSpeed = 5.0f;

            // initialise random angle
            randomVal = new Random();
            theta = randomVal.NextDouble() * Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) * 2 - Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) + randomVal.Next(2) * Math.PI;
            theta = 0;


            // initialise edge effect
            critEdge = 0.25f;
            critEffect = Math.PI / 8;

            // initialise paddles
            redPaddle = new paddle(padWidth, padHeight, frameWidth, frameHeight, "red");
            bluePaddle = new paddle(padWidth, padHeight, frameWidth, frameHeight, "blue");
            redPaddle.PaddleStart();
            bluePaddle.PaddleStart();

            base.Initialize();
        }


        

        protected override void LoadContent()
        {
            sprites = new SpriteBatch(GraphicsDevice);

            // load call for sprites and music
            redPad = Content.Load<Texture2D>("rodeSpeler");
            bluePad = Content.Load<Texture2D>("blauweSpeler");
            ball = Content.Load<Texture2D>("bal");
            logo = Content.Load<Texture2D>("logo");
            comic = Content.Load<SpriteFont>("comicfont");
            menu_music = Content.Load<Song>("Nameless Song");
           
            
        }

        protected void BallMovement()
        {
            // Updating the position of the ball
            ballPos.X += (float)(ballSpeed * Math.Cos(theta));
            ballPos.Y += (float)(-ballSpeed * Math.Sin(theta));
        }

        protected void BallCollision()
        {
            // Floor and roof collision detection and correction
            if (ballPos.Y < 0)
            {
                ballPos.Y = 0;
                theta = (2 * Math.PI - theta);
            }
            else if (ballPos.Y > frameHeight - ballDim)
            {
                ballPos.Y = frameHeight - ballDim;
                theta = (2 * Math.PI - theta);
            }

            // Paddle collision detection and correction including edge effect (Player 1)
            if (ballPos.X < padWidth & ballPos.Y >= redPaddle.paddlePos.Y + critEdge * padHeight & ballPos.Y <= redPaddle.paddlePos.Y + (1 - critEdge) * padHeight)
            {
                ballPos.X = padWidth;
                theta = (Math.PI - theta);
                ballSpeed += 0.2f;
            }
            else if (ballPos.X < padWidth & ballPos.Y >= -ballDim + redPaddle.paddlePos.Y & ballPos.Y - ballDim <= redPaddle.paddlePos.Y + critEdge * padHeight)
            {
                ballPos.X =  padWidth;
                theta = (Math.PI - theta + critEffect);
                ballSpeed += 0.4f;
            }
            else if (ballPos.X < padWidth & ballPos.Y >= (1 - critEdge) * padHeight + redPaddle.paddlePos.Y & ballPos.Y <= redPaddle.paddlePos.Y + padHeight)
            {
                ballPos.X = padWidth;
                theta = (Math.PI - theta - critEffect);
                ballSpeed += 0.4f;
            }
            
            // Paddle collision detection and correction including edge effect (Player 2)
            if (ballPos.X > frameWidth - padWidth - ballDim & ballPos.Y >= bluePaddle.paddlePos.Y + critEdge * padHeight & ballPos.Y <= bluePaddle.paddlePos.Y + (1 - critEdge) * padHeight)
            {
                ballPos.X = frameWidth - padWidth - ballDim;
                theta = (Math.PI - theta);
                ballSpeed += 0.2f;
            }
            else if (ballPos.X > frameWidth - padWidth - ballDim & ballPos.Y >= - ballDim + bluePaddle.paddlePos.Y & ballPos.Y <= bluePaddle.paddlePos.Y + critEdge * padHeight)
            {
                ballPos.X = frameWidth - padWidth - ballDim;
                theta = (Math.PI - theta + critEffect);
                ballSpeed += 0.4f;
            }
            else if (ballPos.X > frameWidth - padWidth - ballDim & ballPos.Y >= (1 - critEdge) * padHeight + bluePaddle.paddlePos.Y & ballPos.Y <=  bluePaddle.paddlePos.Y + padHeight)
            {
                ballPos.X = frameWidth - padWidth - ballDim;
                theta = (Math.PI - theta - critEffect);
                ballSpeed += 0.4f;
            }

            AngleCheck();


        }
        protected void AngleCheck()
        {
            // check too ensure that the angle does not become too steep
            if (theta % (2 * Math.PI) > Math.PI / 3 & theta % (2 * Math.PI) < Math.PI / 2)
            {
                theta = Math.PI / 3;
            }
            else if (theta % (2 * Math.PI) > Math.PI / 2 & theta % (2 * Math.PI) < 2 * Math.PI / 3)
            {
                theta = 2 * Math.PI / 3;
            }
            else if (theta % (2 * Math.PI) > 4 * Math.PI / 3 & theta % (2 * Math.PI) < 3 * Math.PI / 2)
            {
                theta = 4 * Math.PI / 3;
            }
            else if (theta % (2 * Math.PI) > 3 * Math.PI / 2 & theta % (2 * Math.PI) < 5 * Math.PI / 3)
            {
                theta = 5 * Math.PI / 3;
            }
        }
        
        protected void Reset()
        {
            // Method that resets the gameworld
            redPaddle.PaddleStart();
            bluePaddle.PaddleStart();
            ballPos.X = ballStartX;
            ballPos.Y = ballStartY;
            ballSpeed = 5.0f;
            theta = randomVal.NextDouble() * Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) * 2 - Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) + randomVal.Next(2) * Math.PI;
        }
        

        void Menu_music()
        {
            // Method that starts the music
            MediaPlayer.Play(menu_music);
            MediaPlayer.IsRepeating = true;
        }

        void BackToMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                //reset score lives and positions
                score[0] = 0;
                score[1] = 0;
                lives[0] = 3;
                lives[1] = 3;
                Reset();
                
                //back to menu
                gameStage = 0;
            }
        }


        protected override void Update(GameTime gameTime)
        {
            //exit game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            BackToMenu();

            // gameStages
            if (gameStage == 0)
            {
                Menu_music();

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    gameStage++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                    gameStage = 4;
            }
            else if (gameStage == 1)
            {
                redPaddle.PaddleMovement();
                bluePaddle.PaddleMovement();
                BallMovement();
                BallCollision();
                
                // lives tracker
                if (ballPos.X > frameWidth - ballDim)
                {
                    lives[1] -= 1;
                    if (lives[1] == 0)
                        gameStage = 3;
                    else
                        Reset();
                }
                if (ballPos.X < 0)
                {
                    lives[0] -= 1;
                    if (lives[0] == 0)
                        gameStage = 2;
                    else
                        Reset();
                }
            }
            else if ((gameStage == 2 || gameStage == 3) && Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                // Restart game back to menu
                Reset();
                lives[0] = 3;
                lives[1] = 3;
                gameStage = 0;

            }
            else if (gameStage == 4)
            {
                // score gameloop
                redPaddle.PaddleMovement();
                bluePaddle.PaddleMovement();
                BallMovement();
                BallCollision();

                // score tracker
                if (ballPos.X > frameWidth - ballDim)
                {
                    score[0] += 1;
                    Reset();
                }
                if (ballPos.X < 0)
                {
                    score[1] += 1;
                    Reset();
                }
            }
            base.Update(gameTime);
        }

        void DrawMenu()
        {
            // Background
            GraphicsDevice.Clear(Color.LightBlue);

            int logosize = 220;
            
            // start logo
            logoPos = new Rectangle((frameWidth / 2) - (logosize / 2), 0, logosize, logosize);
            sprites.Draw(logo, logoPos, null, Color.White);

            // draw menu messages
            Vector2 ssize1 = comic.MeasureString(start1);
            startString1 = new Vector2((frameWidth / 2) - (ssize1.X / 2), logosize);
            
            Vector2 ssize2 = comic.MeasureString(start2);
            startString2 = new Vector2((frameWidth / 2) - (ssize2.X / 2), logosize + ssize1.Y);
            
            
            sprites.DrawString(comic, start1, startString1, Color.Red);
            sprites.DrawString(comic, start2, startString2, Color.Red);
        }

        void DrawGameLives()
        {
            // draw function for sprites in game
            sprites.Draw(ball, ballPos, null, Color.White);
            sprites.Draw(redPad, redPaddle.paddlePos, null, Color.White);
            sprites.Draw(bluePad, bluePaddle.paddlePos, null, Color.White);


            // Drawing the score in loops. The first loop draws 3 black scores first. The other two loops overlay this with colored scores based on the scores stored in score[].
            for (int i = 3; i > 0; i--)
            {
                scoreBall = new Rectangle(frameWidth / 15 + (2 * i - 1) * ballDim, frameHeight / 20, ballDim, ballDim);
                sprites.Draw(ball, scoreBall, null, Color.Black);
                scoreBall = new Rectangle(frameWidth / 15 * 14 - ballDim - (2 * i - 1) * ballDim, frameHeight / 20, ballDim, ballDim);
                sprites.Draw(ball, scoreBall, null, Color.Black);
            }
            for (int i = lives[0]; i > 0; i--)
            {
                scoreBall = new Rectangle(frameWidth / 15 + (2 * i - 1) * ballDim, frameHeight / 20, ballDim, ballDim);
                sprites.Draw(ball, scoreBall, null, Color.Red);
            }
            for (int i = lives[1]; i > 0; i--)
            {
                scoreBall = new Rectangle(frameWidth / 15 * 14 - ballDim - (2 * i - 1) * ballDim, frameHeight / 20, ballDim, ballDim);
                sprites.Draw(ball, scoreBall, null, Color.Blue);
            }
        }

        void DrawGameScore()
        {
            //draw function for sprites in game
            sprites.Draw(ball, ballPos, null, Color.White);
            sprites.Draw(redPad, redPaddle.paddlePos, null, Color.White);
            sprites.Draw(bluePad, bluePaddle.paddlePos, null, Color.White);

            string redScore = score[0].ToString();
            string blueScore = score[1].ToString();
            
            Vector2 blueScoreSize = comic.MeasureString(blueScore);
            Vector2 redScorePos = new Vector2(frameWidth / 15, frameHeight / 20);
            Vector2 blueScorePos = new Vector2((frameWidth / 15) * 14 - blueScoreSize.X, frameHeight / 20);
            
            sprites.DrawString(comic, redScore, redScorePos, Color.Red);
            sprites.DrawString(comic, blueScore, blueScorePos, Color.Blue);

        }

        void DrawWinRed()
        {
            // Text for when player 1 wins
            int winPosY = frameHeight / 12;
            Vector2 redSize1 = comic.MeasureString(redWin);
            Vector2 redSize2 = comic.MeasureString(pressEnter);
            Vector2 winPos = new Vector2((frameWidth / 2) - (redSize1.X / 2), winPosY);
            Vector2 winPos2 = new Vector2((frameWidth / 2) - (redSize2.X / 2), winPosY + redSize1.Y);
            sprites.DrawString(comic, redWin, winPos, Color.Red);
            sprites.DrawString(comic, pressEnter, winPos2, Color.Red);
        }

        void DrawWinBlue()
        {
            // text for when blue wins
            int winPosY = frameHeight / 12;
            Vector2 blueSize1 = comic.MeasureString(blueWin);
            Vector2 blueSize2 = comic.MeasureString(pressEnter);
            Vector2 winPos = new Vector2((frameWidth / 2) - (blueSize1.X / 2), winPosY);
            Vector2 winPos2 = new Vector2((frameWidth / 2) - (blueSize2.X / 2), winPosY + blueSize1.Y);
            sprites.DrawString(comic, blueWin, winPos, Color.Blue);
            sprites.DrawString(comic, pressEnter, winPos2, Color.Blue);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            //Draw background
            GraphicsDevice.Clear(Color.White);

            // Draw menu
            sprites.Begin();
            if (gameStage == 0)
            {
                DrawMenu();
            }
            
            // Draw gamemode lives
            else if (gameStage == 1)
                DrawGameLives();
            
            // draw player 2 wins
            else if (gameStage == 2)
            {
                DrawGameLives();
                DrawWinBlue();  
            }
            // draw player 1 wins
            else if (gameStage == 3)
            {
                DrawGameLives();
                DrawWinRed();
            }

            // draw gamemode score
            else if (gameStage == 4)
            {
                DrawGameScore();
            }

            sprites.End();
            base.Draw(gameTime);
        }
    }
}
