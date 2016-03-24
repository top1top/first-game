using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace урок3
{
    class AnimatedSprite
    {
        Texture2D idleTexture;
        Texture2D runTexture;

        public Rectangle rect;


        bool isRunning;
        bool isRunningLeft;
        bool isJumping;

        float ySpeed;
        float maxYSpeed = 10;
        float g = 0.2f;
        Game1 game;

        public AnimatedSprite(Rectangle rect, Texture2D idle, Texture2D run, Game1 game)
        {
            this.rect = rect;
            this.idleTexture = idle;
            this.runTexture = run;
            this.game = game;

        }
        public void Run(bool left)
        {
            if(!isRunning)
            {
                isRunning = true;
            }
            isRunningLeft = left;
        }
        public void Jump()
        {
            if(!isJumping && ySpeed==0)
            {
                ySpeed=maxYSpeed;
                isJumping=true;

            }
        }
        public void Stop()
        {
            isRunning = false;

        }

        public void ApplyGravity(GameTime gameTime)
        {
            ySpeed = ySpeed - g * gameTime.ElapsedGameTime.Milliseconds / 10;
            float dy = ySpeed * gameTime.ElapsedGameTime.Milliseconds / 10;

            Rectangle nextPosition = rect;
            nextPosition.Offset(0, -(int)Math.Floor(dy));

            if (nextPosition.Top > 0 && !game.CollidesWithLevel(nextPosition))
                rect = nextPosition;

            bool collidesOnFallDown = game.CollidesWithLevel(nextPosition) && ySpeed < 0;

            if ( collidesOnFallDown)
            {
                isJumping = false;
                ySpeed = 0;
            }

        }
        public void Update(GameTime gameTime)
        {
            if (isRunning)
            {
                int dx = 3 * gameTime.ElapsedGameTime.Milliseconds / 10;
                if (!isRunningLeft)
                    dx = -dx;

                Rectangle nextPosition = rect;
                nextPosition.Offset(dx, 0);

                Rectangle screenRect = game.GetScreenRect(nextPosition);
                if (screenRect.Left > 0 && screenRect.Right < game.Width && !game.CollidesWithLevel(nextPosition) && nextPosition.Right< 50*40)
                {
                    rect = nextPosition;
                }
            }

            ApplyGravity(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

 
            Rectangle screenRect = game.GetScreenRect(rect);


            if (isJumping)
            {
                spriteBatch.Draw(runTexture, screenRect, Color.White);
            }
            else
            {
                if (isRunning)
                {
                    spriteBatch.Draw(runTexture, screenRect,Color.White);
                }
                else
                {
                    spriteBatch.Draw(idleTexture, screenRect, Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
