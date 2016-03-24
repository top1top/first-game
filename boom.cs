using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace урок3
{
    class boom
    {
        public Rectangle Rect { get; set; }

        Texture2D texture;
        Game1 game;

        public boom(Rectangle rect, Texture2D texture, Game1 game)
        {
            this.Rect = rect;
            this.texture = texture;
            this.game = game;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = game.GetScreenRect(Rect);
            spriteBatch.Draw(texture, screenRect, Color.White);
        }
    }
}
