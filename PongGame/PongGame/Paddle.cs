using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
    public class Paddle : Sprite
    {
        private int _minHeight;
        private int _maxHeight;

        private int _speed;
        private Vector2 _startPosition;

        public Paddle(Texture2D image, Vector2 position, Color color, int minHeight, int maxHeight, int speed) 
            : base(image, position, color)
        {
            _minHeight = minHeight;
            _maxHeight = maxHeight;
            _speed = speed;

            _startPosition = position;
        }

        public void MoveUp()
        {
            if (Position.Y == _minHeight)
            {
                return;
            }
            else if (Position.Y - _speed < _minHeight)
            {
                Position.Y = _minHeight;
            }
            else
            {
                Position.Y -= _speed;
            }
        }

        public void MoveDown()
        {
            if (Position.Y == _maxHeight)
            {
                return;
            }
            else if (Position.Y + Image.Height + _speed > _maxHeight)
            {
                Position.Y = _maxHeight - Image.Height;
            }
            else
            {
                Position.Y += _speed;
            }
        }

        public void Reset()
        {
            Position = _startPosition;   
        }
    }
}
