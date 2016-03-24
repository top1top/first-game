using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace урок3
{
    class Gem
    {
        public Rectangle Rect {get;set;}//свойство, отвещающее за положение и размер одного блока
        
        Texture2D texture;
        Game1 game;

        int dy;

        public Gem(Rectangle rect, Texture2D texture, Game1 game)//текстура и положение нового блока
        {
            this.Rect = rect;
            this.texture = texture;
            this.game = game;
        }
        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalGameTime.TotalSeconds * 3 + Rect.X;
            dy = (int)(Math.Sin(t) * 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(Rect.X, Rect.Y + dy, Rect.Width, Rect.Height);
            Rectangle screenRect = game.GetScreenRect(r);
            spriteBatch.Draw(texture, screenRect, Color.White);//end и begin в одном месте для увеличеня производительности
        }
    }
}
