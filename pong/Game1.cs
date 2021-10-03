using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace pong
{


    public class Game1 : Game
    {

        //sprites batch
        private GraphicsDeviceManager _graphics;
        private SpriteBatch sprites;
        //comic sans font
        private SpriteFont comic;
        //sprites
        private Texture2D redPad, bluePad, ball, logo;

        //music
        private Song menu_music;
        //rectangles and vectors for sprites
        private Vector2 ballPos;
        private Rectangle logoPos;
        //vectors for text
        private Vector2 startString1, startString2;

        //random value
        private Random randomVal;
        private double theta;

        //frame with height values
        public int frameWidth = 600;
        public int frameHeight = 480;
        //object size values
        private int padWidth = 16;
        private int padHeight = 96;
        private int ballDim = 16;

        //text messegas
        private string start1 ="PRESS 1 FOR LIVES";
        private string start2 = "PRESS 2 FOR SCORE";
        private string blueWin ="BLUE WINS";
        private string redWin ="RED WINS";
        private string pressEnter = "PRESS TAB";
        //lives
        /*int redlives, bluelives;*/
        private int[] lives;
        private int[] score;
        private Rectangle scoreBall;

        // object speeds
        private float ballSpeed, critEdge;
        private double critEffect;

        // start positions objects
        private int ballStartY;
        private int ballStartX;
        private int gameStage;
        private int winPosY;

        paddle redPaddle;
        paddle bluePaddle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            //frame with height and with
            _graphics.PreferredBackBufferWidth = frameWidth;
            _graphics.PreferredBackBufferHeight = frameHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = false;



        }


        protected override void Initialize()
        {   
            // TODO: Add your initialization logic here
            //lives

            ballStartY = frameHeight / 2 - ballDim / 2;
            ballStartX = frameWidth / 2 - ballDim / 2;


            // lives and removal
            lives = new int[2];
            lives[0] = 3;
            lives[1] = 3;

            // score 
            score = new int[2];
            score[0] = 0;
            score[1] = 0;

            // object speeds
            ballSpeed = 5.0f;


            // load rectangle location for sprites
            ballPos = new Vector2(ballStartX, ballStartY);

            // random angle
            randomVal = new Random();
            theta = randomVal.NextDouble() * Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) * 2 - Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) + randomVal.Next(2) * Math.PI;
            theta = 0;

            winPosY = frameHeight / 12;

            critEdge = 0.25f;
            critEffect = Math.PI / 8;

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

            // Paddle collision detection and correction
            // Red (Player 1)
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

            // Blue (Player 2)
            // Blue (Player 2)
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
            redPaddle.PaddleStart();
            bluePaddle.PaddleStart();
            ballPos.X = ballStartX;
            ballPos.Y = ballStartY;
            ballSpeed = 5.0f;
            theta = randomVal.NextDouble() * Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) * 2 - Math.Atan((1.1 * frameWidth) / (1.1 * frameHeight)) + randomVal.Next(2) * Math.PI;
        }
        

        void Menu_music()
        {
            MediaPlayer.Play(menu_music);
            MediaPlayer.IsRepeating = true;
        }

        void BackToMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                //resest score lives and positions
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
                
                // lives counter (updated with the array)

                if (ballPos.X > frameWidth - ballDim)
                {
                    /*bluelives -= 1;*/
                    lives[1] -= 1;
                    if (lives[1] == 0)
                        gameStage = 3;
                    else
                        Reset();
                 
                }
                if (ballPos.X < 0)
                {
                    /*redlives -= 1;*/
                    lives[0] -= 1;
                    if (lives[0] == 0)
                        gameStage = 2;
                    else
                        Reset();
                }
            }
            else if ((gameStage == 2 || gameStage == 3) && Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                Reset();
                lives[0] = 3;
                lives[1] = 3;
                gameStage = 0;

            }
            else if (gameStage == 4)
            {
                redPaddle.PaddleMovement();
                bluePaddle.PaddleMovement();
                BallMovement();
                BallCollision();
                if (ballPos.X > frameWidth - ballDim)
                {
                    /*bluescore += 1*/
                    score[0] += 1;
                    Reset();
                }
                if (ballPos.X < 0)
                {
                    /*redscore += 1*/
                    score[1] += 1;
                    Reset();
                }
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void DrawMenu()
        {
            int logosize = 220;
            //start logo
            logoPos = new Rectangle((frameWidth / 2) - (logosize / 2), 0, logosize, logosize);
            sprites.Draw(logo, logoPos, null, Color.White);

            //press 1,2 to start text
            int startPosY = logosize;
            
            Vector2 ssize1 = comic.MeasureString(start1);
            startString1 = new Vector2((frameWidth / 2) - (ssize1.X / 2), startPosY);
            Vector2 ssize2 = comic.MeasureString(start2);
            startString2 = new Vector2((frameWidth / 2) - (ssize2.X / 2), startPosY + ssize1.Y);
            
            
            sprites.DrawString(comic, start1, startString1, Color.Red);
            sprites.DrawString(comic, start2, startString2, Color.Red);
            //background
            GraphicsDevice.Clear(Color.LightBlue);
        }

        void DrawGameLives()
        {
            //draw function for sprites in game
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

            GraphicsDevice.Clear(Color.White);
        }

        void DrawGameScore()
        {
            //draw function for sprites in game
            sprites.Draw(ball, ballPos, null, Color.White);
            sprites.Draw(redPad, redPaddle.paddlePos, null, Color.White);
            sprites.Draw(bluePad, bluePaddle.paddlePos, null, Color.White);
            GraphicsDevice.Clear(Color.White);

            string redscore = score[0].ToString();
            string bluescore = score[1].ToString();
            
            Vector2 rscoresize = comic.MeasureString(redscore);
            Vector2 bscoresize = comic.MeasureString(bluescore);
            Vector2 rscorepos = new Vector2(frameWidth / 15, frameHeight / 20);
            Vector2 bscorepos = new Vector2((frameWidth / 15) * 14 - bscoresize.X, frameHeight / 20);
            
            sprites.DrawString(comic, redscore, rscorepos, Color.Red);
            sprites.DrawString(comic, bluescore, bscorepos, Color.Blue);

        }

        void DrawWinRed()
        {
            //text for when red wins
            int winPosY = frameHeight / 12;
            Vector2 rsize = comic.MeasureString(redWin);
            Vector2 rsize2 = comic.MeasureString(pressEnter);
            Vector2 winPos = new Vector2((frameWidth / 2) - (rsize.X / 2), winPosY);
            Vector2 winPos2 = new Vector2((frameWidth / 2) - (rsize2.X / 2), winPosY + rsize.Y);
            sprites.DrawString(comic, redWin, winPos, Color.Red);
            sprites.DrawString(comic, pressEnter, winPos2, Color.Red);
        }

        void DrawWinBlue()
        {
            //text for when blue wins
            Vector2 bsize = comic.MeasureString(blueWin);
            Vector2 bsize2 = comic.MeasureString(pressEnter);
            Vector2 winPos = new Vector2((frameWidth / 2) - (bsize.X / 2), winPosY);
            Vector2 winPos2 = new Vector2((frameWidth / 2) - (bsize2.X / 2), winPosY + bsize.Y);
            sprites.DrawString(comic, blueWin, winPos, Color.Blue);
            sprites.DrawString(comic, pressEnter, winPos2, Color.Blue);
        }


        protected override void Draw(GameTime gameTime)
        {

            sprites.Begin();
            if (gameStage == 0)
            {
                DrawMenu();
            }
            else if (gameStage == 1)
                DrawGameLives();

            else if (gameStage == 2)
            {
                DrawGameLives();
                DrawWinBlue();  
            }
            else if (gameStage == 3)
            {
                DrawGameLives();
                DrawWinRed();
            }
            else if (gameStage == 4)
            {
                DrawGameScore();
            }

            sprites.End();
            base.Draw(gameTime);
        }
    }
}
