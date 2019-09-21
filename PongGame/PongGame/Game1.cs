using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PongGame
{
    public enum GameState
    {
        Playing,
        Paused,
        LeftWins,
        RightWins
    }

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

        GameState currentState = GameState.Playing;

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

            if (currentState == GameState.Playing)
            {
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
                if (IsKeyPressed(Keys.Space))
                {
                    currentState = GameState.Paused;
                }

                int ballUpdateValue = ball.Update(leftPaddle, rightPaddle);
                if (ballUpdateValue > 0)
                {
                    leftScore++;

                    if (leftScore == scoreToReach)
                    {
                        currentState = GameState.LeftWins;
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
                        currentState = GameState.RightWins;
                    }
                    else
                    {
                        ResetGame();
                    }
                }
            }
            else if (currentState == GameState.Paused)
            {
                if (IsKeyPressed(Keys.Space))
                {
                    currentState = GameState.Playing;
                }
            }
            else
            {
                if (IsKeyPressed(Keys.Space))
                {
                    currentState = GameState.Playing;
                    ResetEverything();
                }
            }

            lastKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        private void ResetEverything()
        {
            ResetGame();
            leftScore = 0;
            rightScore = 0;
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
            spriteBatch.DrawString(mainFont, tempString, new Vector2(screenWidth / 2 - mainFont.MeasureString(tempString).X / 2, 50), Color.Black);

            if (currentState == GameState.Paused)
            {
                tempString = "Press space to continue";
                spriteBatch.DrawString(mainFont, tempString, new Vector2(screenWidth / 2 - mainFont.MeasureString(tempString).X / 2, screenHeight / 2 - mainFont.MeasureString(tempString).Y / 2), Color.Red);
            }
            else if (currentState == GameState.LeftWins)
            {
                tempString = "Left Won! Press space to restart";
                spriteBatch.DrawString(mainFont, tempString, new Vector2(screenWidth / 2 - mainFont.MeasureString(tempString).X / 2, screenHeight / 2 - mainFont.MeasureString(tempString).Y / 2), Color.Red);
            }
            else if (currentState == GameState.RightWins)
            {
                tempString = "Right Won! Press space to restart";
                spriteBatch.DrawString(mainFont, tempString, new Vector2(screenWidth / 2 - mainFont.MeasureString(tempString).X / 2, screenHeight / 2 - mainFont.MeasureString(tempString).Y / 2), Color.Red);

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
