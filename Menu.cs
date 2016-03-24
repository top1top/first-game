using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace урок3
{
    class Menu
    {
        public List<MenuItem> Items { get; set; }
        SpriteFont font;

        int currentItem;
        KeyboardState oldState;
        Texture2D tex;
        public Menu()
        {
            Items = new List<MenuItem>();
        }
        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.Enter))
                Items[currentItem].OnClick();

            int delta = 0;
            if (state.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
                delta--;

            if (state.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
                delta++;

            currentItem += delta;
            bool ok=false;
            while(!ok)
            {
                if (currentItem < 0)
                    currentItem = Items.Count - 1;
                else if (currentItem > Items.Count - 1)
                    currentItem = 0;
                else if (Items[currentItem].Active == false)
                    currentItem += delta;
                else ok = true;
            }

            oldState = state;
        }
        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("MenuFont");
            tex = Content.Load<Texture2D>("map");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int y=80;
            foreach (MenuItem Item in Items)
            {
                Color color = Color.Aquamarine;
                if (Item.Active == false)
                    color = Color.White;
                if (Item == Items[currentItem])
                    color = Color.Red;
                spriteBatch.DrawString(font,Item.Name,new Vector2(40,y),color);
                y+=70;
            }
            spriteBatch.End();
        }

    }
}
