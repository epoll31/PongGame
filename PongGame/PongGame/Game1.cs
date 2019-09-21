using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int screenWidth = 1100;
        const int screenHeight = 700;
        const int paddleWidth = 20;
        const int paddleHeight = 100;
        const int ballSize = 20;

        Texture2D paddleImage;
        Texture2D ballImage;
        SpriteFont mainFont;

        Paddle leftPaddle;
        Paddle rightPaddle;

        Ball ball;

        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;

        int leftScore = 0;
        int rightScore = 0;
        const int scoreToReach = 5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = screenWidth,
                PreferredBackBufferHeight = screenHeight
            };
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            paddleImage = Sprite.CreateRectangleTexture(GraphicsDevice, paddleWidth, paddleHeight, Color.White);
            ballImage = Sprite.CreateSquareTexture(GraphicsDevice, ballSize, Color.White);
            mainFont = Content.Load<SpriteFont>("MainFont");

            leftPaddle = new Paddle(paddleImage, new Vector2(30, screenHeight / 2 - paddleHeight / 2), Color.Black, 30, screenHeight - 30, 5);
            rightPaddle = new Paddle(paddleImage, new Vector2(screenWidth - paddleWidth - 30, screenHeight / 2 - paddleHeight / 2), Color.Black, 30, screenHeight - 30, 5);

            ball = new Ball(ballImage, Color.Red, MathHelper.Pi * 3 / 8, 5, 0, screenWidth, 0, screenHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
            {
                leftPaddle.MoveUp();
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                leftPaddle.MoveDown();
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                rightPaddle.MoveUp();
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                rightPaddle.MoveDown();
            }

            int ballUpdateValue = ball.Update(leftPaddle, rightPaddle);
            if (ballUpdateValue > 0)
            {
                leftScore++;

                if (leftScore == scoreToReach)
                {
                    LeftWins();
                }
                else
                {
                    ResetGame();
                }
            }
            else if (ballUpdateValue < 0)
            {
                rightScore++;

                if (rightScore == scoreToReach)
                {
                    RightWins();
                }
                else
                {
                    ResetGame();
                }
            }

            lastKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        private void LeftWins()
        {
            
        }

        private void RightWins()
        {
            
        }

        private void ResetGame()
        {
            leftPaddle.Reset();
            rightPaddle.Reset();
            ball.Reset();
        }

        private bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            leftPaddle.Draw(spriteBatch);
            rightPaddle.Draw(spriteBatch);

            ball.Draw(spriteBatch);

            string tempString = $"Left Score: {leftScore}";
            spriteBatch.DrawString(mainFont, tempString, new Vector2(50, 50), Color.Black);
            tempString = $"Right Score: {rightScore}";
            spriteBatch.DrawString(mainFont, tempString, new Vector2(screenWidth - mainFont.MeasureString(tempString).X - 50, 50), Color.Black);
            tempString = $"First to {scoreToReach} wins";
            spriteBatch.DrawString(mainFont, tempString, new Vector2(screenWidth/2 - mainFont.MeasureString(tempString).X/2, 50), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
