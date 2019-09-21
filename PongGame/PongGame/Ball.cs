using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
    public class Ball : Sprite
    {
        private float _rotation;
        private int _speed;
        private int _minWidth;
        private int _maxWidth;
        private int _minHeight;
        private int _maxHeight;

        private Random _random;
        private Vector2 _center;

        public Ball(Texture2D image, Color color, float rotation, int speed, int minWidth, int maxWidth, int minHeight, int maxHeight)
            : base(image, new Vector2((maxHeight - minWidth)/2 - image.Width/2, (maxHeight - minHeight)/2 - image.Height/2), color)
        {
            _rotation = rotation;
            _speed = speed;
            _minWidth = minWidth;
            _maxWidth = maxWidth;
            _minHeight = minHeight;
            _maxHeight = maxHeight;

            _random = new Random();
            _center = new Vector2((maxHeight - minWidth) / 2 - image.Width / 2, (maxHeight - minHeight) / 2 - image.Height / 2);
        }

        /// <summary>
        /// Returns a 0 if nobody wins
        /// Returns a -1 is left wins
        /// Returns a 1 if right wins
        /// </summary>
        /// <param name="leftPaddle"></param>
        /// <param name="rightPaddle"></param>
        /// <returns></returns>
        public int Update(Paddle leftPaddle, Paddle rightPaddle)
        {
            Vector2 nextPosition = Position += new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation)) * _speed;

            if (nextPosition.Y < _minHeight)
            {
                nextPosition.Y = _minHeight;
                _rotation = -_rotation;
            }
            else if (nextPosition.Y + Image.Height > _maxHeight)
            {
                nextPosition.Y = _maxHeight - Image.Height;
                _rotation = -_rotation;
            }
            else
            {
                Position = nextPosition;
            }

            if (Intersects(leftPaddle))
            {
                _rotation = (float)Math.PI - _rotation;
                Position.X = leftPaddle.Position.X + leftPaddle.Image.Width;
            }
            else if (Intersects(rightPaddle))
            {
                _rotation = (float)Math.PI - _rotation;
                Position.X = rightPaddle.Position.X - Image.Width;
            }

            if (Position.X < _minWidth)
            {
                return -1;
            }
            else if (Position.X + Image.Width > _maxWidth)
            {
                return 1;
            }

            return 0;
        }

        public void Reset() => Reset((float)(2 * Math.PI * _random.NextDouble()));
        public void Reset(float newRotation)
        {
            Position = _center;
            _rotation = newRotation;
        }
    }
}
