using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace урок3
{
    class flyBlock
    {
        public Rectangle Rect {get;set;}//свойство, отвещающее за положение и размер одного блока
        
        Texture2D texture;
        Game1 game;

        int dx;
        int fX;
        int fY;
        int der;


        public flyBlock(Rectangle rect, Texture2D texture, Game1 game,int x, int y,int n)//текстура и положение нового блока
        {
            this.Rect = rect;
            this.texture = texture;
            this.game = game;
            this.fX = x;
            this.fY = y;
            this.der= n;
        }

        public void Update(GameTime gameTime)
        {
            dx = der*3 * gameTime.ElapsedGameTime.Milliseconds / 30;
            if (Rect.X + dx > fX + 40 || Rect.X + dx < fX - 40)
            {
                der = -der;
            }

            Rect = new Rectangle(Rect.X+dx, Rect.Y, Rect.Width, Rect.Height);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = game.GetScreenRect(Rect);
            spriteBatch.Draw(texture, screenRect, Color.White);
        }
    }
}
