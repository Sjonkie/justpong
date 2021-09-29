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
        private Texture2D redpad, bluepad, ball, logo;

        //music
        private Song menu_music;
        //rectangles and vectors for sprites
        private Vector2 ballPos;
        private Rectangle logoPos;
        //vectors for text
        private Vector2 startstring;

        //random value
        private Random randomVal;
        private double theta;

        //frame with height values
        public int framex = 600;
        public int framey = 480;
        //object size values
        private int padx = 16;
        private int pady = 96;
        private int ballxy = 16;

        //text messegas
        private string start ="PRESS SPACE TO START";
        private string bluewins ="BLUE WINS";
        private string redwins ="RED WINS";
        private string pressenter = "PRESS ENTER";

        //lives
        /*int redlives, bluelives;*/
        private int[] score;
        private Rectangle scoreball;

        // object speeds
        private int playerSpeed;
        private float ballSpeed, critEdge;
        private double critEffect;

        // start positions objects
        private int startbally;
        private int startballx;
        private int gamestage;
        private int winPosY;

        paddle redpaddle;
        paddle bluepaddle;

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

            startbally = framey / 2 - ballxy / 2;
            startballx = framex / 2 - ballxy / 2;


            // lives and removal
            score = new int[2];
            score[0] = 3;
            score[1] = 3;

            // object speeds
            playerSpeed = 5;
            ballSpeed = 5.0f;


            // load rectangle location for sprites
            ballPos = new Vector2(startballx, startbally);

            // random angle
            randomVal = new Random();
            theta = randomVal.NextDouble() * Math.Atan((1.1 * framex) / (1.1 * framey)) * 2 - Math.Atan((1.1 * framex) / (1.1 * framey)) + randomVal.Next(2) * Math.PI;
            theta = 0;

            winPosY = framey / 12;

            critEdge = 0.25f;
            critEffect = Math.PI / 6;

            redpaddle = new paddle(padx, pady, framex, framey, "red");
            bluepaddle = new paddle(padx, pady, framex, framey, "blue");

            redpaddle.paddlestart();
            bluepaddle.paddlestart();

            base.Initialize();
        }


        

        protected override void LoadContent()
        {
            sprites = new SpriteBatch(GraphicsDevice);

            // load call for sprites
            redpad = Content.Load<Texture2D>("rodeSpeler");
            bluepad = Content.Load<Texture2D>("blauweSpeler");
            ball = Content.Load<Texture2D>("bal");
            logo = Content.Load<Texture2D>("logo");
            comic = Content.Load<SpriteFont>("comicfont");
            menu_music = Content.Load<Song>("Nameless Song");
           
            
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
            if (ballPos.X < padx & ballPos.Y >= redpaddle.startpad.Y + critEdge * pady & ballPos.Y <= redpaddle.startpad.Y + (1 - critEdge) * pady)
            {
                ballPos = new Vector2(padx, ballPos.Y);
                theta = (Math.PI - theta);
                ballSpeed += 0.2f;
            }
            else if (ballPos.X < padx & ballPos.Y >= redpaddle.startpad.Y & ballPos.Y <= redpaddle.startpad.Y + critEdge * pady)
            {
                ballPos = new Vector2(padx, ballPos.Y);
                theta = (Math.PI - theta + critEffect);
                ballSpeed += 0.4f;
            }
            else if (ballPos.X < padx & ballPos.Y >= (1 - critEdge) * redpaddle.startpad.Y & ballPos.Y <= redpaddle.startpad.Y + pady)
            {
                ballPos = new Vector2(padx, ballPos.Y);
                theta = (Math.PI - theta - critEffect);
                ballSpeed += 0.4f;
            }

            // Blue (Player 2)
            // Blue (Player 2)
            if (ballPos.X > framex - padx - ballxy & ballPos.Y >= bluepaddle.startpad.Y + critEdge * pady & ballPos.Y <= bluepaddle.startpad.Y + (1 - critEdge) * pady)
            {
                ballPos = new Vector2(framex - padx - ballxy, ballPos.Y);
                theta = (Math.PI - theta);
                ballSpeed += 0.2f;
            }
            else if (ballPos.X > framex - padx - ballxy & ballPos.Y >= bluepaddle.startpad.Y & ballPos.Y <= bluepaddle.startpad.Y + critEdge * pady)
            {
                ballPos = new Vector2(framex - padx - ballxy, ballPos.Y);
                theta = (Math.PI - theta + critEffect);
                ballSpeed += 0.4f;
            }
            else if (ballPos.X > framex - padx - ballxy & ballPos.Y >= (1 - critEdge) * bluepaddle.startpad.Y & ballPos.Y <= bluepaddle.startpad.Y + pady)
            {
                ballPos = new Vector2(framex - padx - ballxy, ballPos.Y);
                theta = (Math.PI - theta - critEffect);
                ballSpeed += 0.4f;
            }


        }
        
        protected void Reset()
        {
            redpaddle.paddlestart();
            bluepaddle.paddlestart();
            ballPos.X = startballx;
            ballPos.Y = startbally;
            ballSpeed = 5.0f;
            theta = randomVal.NextDouble() * Math.Atan((1.1 * framex) / (1.1 * framey)) * 2 - Math.Atan((1.1 * framex) / (1.1 * framey)) + randomVal.Next(2) * Math.PI;
        }
        
        void Menu_music()
        {
            MediaPlayer.Play(menu_music);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {


            //exit game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            if (gamestage == 0)
            {
                Menu_music();

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gamestage++;
                }
            }
            else if (gamestage == 1)
            {
                redpaddle.paddlemovement();
                bluepaddle.paddlemovement();
                BallMovement();
                BallCollision();
                
                // lives counter (updated with the array)

                if (ballPos.X > framex - ballxy)
                {
                    /*bluelives -= 1;*/
                    score[1] -= 1;
                    if (score[1] == 0)
                        gamestage = 3;
                    else
                        Reset();
                 
                }
                if (ballPos.X < 0)
                {
                    /*redlives -= 1;*/
                    score[0] -= 1;
                    if (score[0] == 0)
                        gamestage = 2;
                    else
                        Reset();
                }
            }
            else if ((gamestage == 2 || gamestage == 3) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Reset();
                score[0] = 3;
                score[1] = 3;
                gamestage = 0;

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void Drawmenu()
        {
            int logosize = 220;
            //start logo
            logoPos = new Rectangle((framex / 2) - (logosize / 2), 0, logosize, logosize);
            sprites.Draw(logo, logoPos, null, Color.White);

            //press space text
            int startPosY = logosize;
            Vector2 ssize = comic.MeasureString(start);
            startstring = new Vector2((framex / 2) - (ssize.X / 2), startPosY);
            sprites.DrawString(comic, start, startstring, Color.Red);
            //background
            GraphicsDevice.Clear(Color.LightBlue);
        }

        void Drawgame1()
        {
            //draw function for sprites in game
            sprites.Draw(ball, ballPos, null, Color.White);
            sprites.Draw(redpad, redpaddle.startpad, null, Color.White);
            sprites.Draw(bluepad, bluepaddle.startpad, null, Color.White);


            // Drawing the score in loops. The first loop draws 3 black scores first. The other two loops overlay this with colored scores based on the scores stored in score[].
            for (int i = 3; i > 0; i--)
            {
                scoreball = new Rectangle(framex / 15 + (2 * i - 1) * ballxy, framey / 20, ballxy, ballxy);
                sprites.Draw(ball, scoreball, null, Color.Black);
                scoreball = new Rectangle(framex / 15 * 14 - ballxy - (2 * i - 1) * ballxy, framey / 20, ballxy, ballxy);
                sprites.Draw(ball, scoreball, null, Color.Black);
            }
            for (int i = score[0]; i > 0; i--)
            {
                scoreball = new Rectangle(framex / 15 + (2 * i - 1) * ballxy, framey / 20, ballxy, ballxy);
                sprites.Draw(ball, scoreball, null, Color.Red);
            }
            for (int i = score[1]; i > 0; i--)
            {
                scoreball = new Rectangle(framex / 15 * 14 - ballxy - (2 * i - 1) * ballxy, framey / 20, ballxy, ballxy);
                sprites.Draw(ball, scoreball, null, Color.Blue);
            }

            GraphicsDevice.Clear(Color.White);
        }

        void DrawWinred()
        {
            //text for when red wins
            int winPosY = framey / 12;
            Vector2 rsize = comic.MeasureString(redwins);
            Vector2 rsize2 = comic.MeasureString(pressenter);
            Vector2 winPos = new Vector2((framex / 2) - (rsize.X / 2), winPosY);
            Vector2 winPos2 = new Vector2((framex / 2) - (rsize2.X / 2), winPosY + rsize.Y);
            sprites.DrawString(comic, redwins, winPos, Color.Red);
            sprites.DrawString(comic, pressenter, winPos2, Color.Red);
        }

        void DrawWinblue()
        {
            //text for when blue wins
            Vector2 bsize = comic.MeasureString(bluewins);
            Vector2 bsize2 = comic.MeasureString(pressenter);
            Vector2 winPos = new Vector2((framex / 2) - (bsize.X / 2), winPosY);
            Vector2 winPos2 = new Vector2((framex / 2) - (bsize2.X / 2), winPosY + bsize.Y);
            sprites.DrawString(comic, bluewins, winPos, Color.Blue);
            sprites.DrawString(comic, pressenter, winPos2, Color.Blue);
        }


        protected override void Draw(GameTime gameTime)
        {

            sprites.Begin();
            if (gamestage == 0)
            {
                Drawmenu();
            }
            else if (gamestage == 1)
                Drawgame1();

            else if (gamestage == 2)
            {
                Drawgame1();
                DrawWinblue();  
            }
            else if (gamestage == 3)
            {
                Drawgame1();
                DrawWinred();
            }

            sprites.End();
            base.Draw(gameTime);
        }
    }
}
