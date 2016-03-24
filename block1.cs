using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace урок3
{
    class block1
    {
        public Rectangle Rect {get;set;}//свойство, отвещающее за положение и размер одного блока
        
        Texture2D texture;
        Game1 game;

        public block1(Rectangle rect, Texture2D texture, Game1 game)//текстура и положение нового блока
        {
            this.Rect = rect;
            this.texture = texture;
            this.game = game;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenRect = game.GetScreenRect(Rect);
            spriteBatch.Draw(texture, screenRect, Color.White);//end и begin в одном месте для увеличеня производительности
        }
    }

}
