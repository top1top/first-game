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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D blockTexture1;
        Texture2D blockTexture2;
        Texture2D blockTexture3;
        Texture2D Map;

        Texture2D idleTexture;
        Texture2D runTexture;
        Texture2D gemTexture;
        Texture2D boomTexture;
        Texture2D winTexture;
        Texture2D hevelTexture;

        int levelLength;
        int levelHeight;

        SpriteFont font;

        AnimatedSprite hero;

        public int Width;
        public int Height;

        List<block1> blocks;
        List<Gem> gems;
        List<boom> booms;
        List<flyBlock> flyBlocks;

        KeyboardState oldState;

        int currentLevel=1;
        int maxLevel=2;

        int ScrollX;//смещение игровых в экранный
        int ScrollY;

        int Score;//очки

        Win win;

        Menu menu;
        GameState gameState = GameState.Menu;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Width = this.graphics.PreferredBackBufferWidth = 400;
            Height = this.graphics.PreferredBackBufferHeight = 400;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menu = new Menu();
            MenuItem newGame = new MenuItem("Start");
            MenuItem resumeGame = new MenuItem("Continue");
            MenuItem exitGame = new MenuItem(" Exit");

            resumeGame.Active = false;
            newGame.Click += new EventHandler(newGame_Click);
            resumeGame.Click+= new EventHandler(resumeGame_Click);
            exitGame.Click += new EventHandler(exitGame_Click);

            menu.Items.Add(newGame);
            menu.Items.Add(resumeGame);
            menu.Items.Add(exitGame);


            base.Initialize();
        }
        void exitGame_Click(object sender, EventArgs e)
        {
            this.Exit();
        }
        void resumeGame_Click(object sender, EventArgs e)
        {
            gameState = GameState.Game;
        }
        void newGame_Click(object sender, EventArgs e)
        {
            menu.Items[1].Active = true;
            gameState = GameState.Game;

            Rectangle rect = new Rectangle(10, 200, 60, 60);
            hero = new AnimatedSprite(rect, idleTexture, runTexture, this);
            CreateLevel();

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blockTexture1 = Content.Load<Texture2D>("Textures/pink");
            blockTexture2 = Content.Load<Texture2D>("Textures/orange");
            blockTexture3 = Content.Load<Texture2D>("Textures/yellow");
            Map = Content.Load<Texture2D>("map");
            //idleTexture = Content.Load<Texture2D>("Textures/staymen2");
            idleTexture = Content.Load<Texture2D>("Textures/hero");
            runTexture = Content.Load<Texture2D>("Textures/hero");
            gemTexture = Content.Load<Texture2D>("Textures/heart");
            boomTexture = Content.Load<Texture2D>("Textures/boom");
            winTexture = Content.Load<Texture2D>("Textures/win");
            hevelTexture = Content.Load<Texture2D>("Textures/hevel");

            font = Content.Load<SpriteFont>("GameFont");

            menu.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 
        public Rectangle GetScreenRect(Rectangle rect)//возвращает новый прямоугольник в экранных координатах
        {
            Rectangle r = rect;
            r.Offset(-ScrollX, -ScrollY);
            return r;
        }

        public void ScrollDxDy(int dx,int dy)//смещает экранн относительно начала уровня
        {

            if (ScrollX + dx > 0 && ScrollX + dx < 50*40 - 200)
                ScrollX += dx;
            if (ScrollY + dy > 0 && ScrollY + dy < 50*40 - 200)
                ScrollY += dy;
        }

        public bool CollidesWithLevel(Rectangle rect)
        {
            bool b=false;
            foreach (block1 block in blocks)
            {
                if (block.Rect.Intersects(rect))
                     b=true;
            }
            foreach (flyBlock flyBlock in flyBlocks)
            {
               if (flyBlock.Rect.Intersects(rect))
                   b = true;
            }
            return b;
        }
        void CreateLevel()
        {
            blocks = new List<block1>();
            gems = new List<Gem>();
            booms = new List<boom>();
            flyBlocks = new List<flyBlock>();

            if (currentLevel > 2)
            {
                currentLevel = 1;
                Score = 0;
            }

            string[] lines = File.ReadAllLines("content/levels/block"+currentLevel+".txt");

            levelLength = 40 * 50;
            levelHeight = 40 * 50;

            Rectangle heroRect= new Rectangle(50,200,36,36);
            hero.rect = heroRect;

            ScrollX = 0;
            ScrollY = 0;

            int x = 0;
            int y = 0;

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    Rectangle rect = new Rectangle (x,y,40,40);
                    if (c == 'X')
                    {             
                        block1 block = new block1(rect, blockTexture1,this);
                        blocks.Add(block);
                    }
                    if( c == 'Y' )
                    {
                        block1 block = new block1(rect, blockTexture2,this);
                        blocks.Add(block);
                    }
                    if (c == 'Z')
                    {
                        block1 block = new block1(rect, blockTexture3, this);
                        blocks.Add(block);
                    }
                    if (c == 'G')
                    {
                        Rectangle gemRect = new Rectangle(x,y,20,20);
                        Gem gem = new Gem(gemRect, gemTexture, this);
                        gems.Add(gem);
                    }
                    if (c == 'B')
                    {
                        Rectangle boomRect = new Rectangle(x, y, 30, 30);
                        boom boom = new boom(boomRect, boomTexture, this);
                        booms.Add(boom);
                    }
                    if (c == 'W')
                    {
                        Rectangle winRect = new Rectangle(x, y, 40, 40);
                       win = new Win(winRect,winTexture,this);
                    }
                    if (c == 'f')
                    {
                        Rectangle flyRect = new Rectangle(x, y, 120, 40);
                        flyBlock flyBlock = new flyBlock(flyRect, hevelTexture, this,x,y,1);
                        flyBlocks.Add(flyBlock);
                    }
                    x += 40;//увеличение на размер блока
                }
                x = 0;
                y += 40;
            }
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (gameState == GameState.Game)
                UpdateGameLogic(gameTime);
            else menu.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        private void UpdateGameLogic(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
                gameState = GameState.Menu;

            if (win != null)
            {
                if (win.Rect.Intersects(hero.rect))
                {
                    currentLevel++;
                    if (currentLevel > maxLevel)
                    {
                        Score = 0;
                        currentLevel = 1;
                        gameState = GameState.Menu;
                        menu.Items[1].Active = false;

                    }
                    else CreateLevel();
                }
            }
            foreach (Gem gem in gems)
            {
                gem.Update(gameTime);
            }
            foreach (flyBlock flyBlock in flyBlocks)
            {
                flyBlock.Update(gameTime);
            }
            if (state.IsKeyDown(Keys.Left))
            {
                hero.Run(false);
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                hero.Run(true);
            }
            else
            {
                hero.Stop();
            }
            if (state.IsKeyDown(Keys.Up))
            {
                hero.Jump();
            }

            Rectangle heroScreenRect = GetScreenRect(hero.rect);

            //перемещение окошечка под движение героя
            //где-то тут что-то не так :(

            if (heroScreenRect.Right < Width / 2  )
                ScrollDxDy(-3 * gameTime.ElapsedGameTime.Milliseconds / 10,0);

            if (heroScreenRect.Left > Width / 2 && hero.rect.Right < 50*40-200)
                ScrollDxDy(3 * gameTime.ElapsedGameTime.Milliseconds / 10,0);

            if (heroScreenRect.Bottom < Height / 2)
                ScrollDxDy(0,-3 * gameTime.ElapsedGameTime.Milliseconds / 10);

            if (heroScreenRect.Top > Height / 2 && hero.rect.Bottom<50*40-100)
                ScrollDxDy(0,3 * gameTime.ElapsedGameTime.Milliseconds / 10);

            hero.Update(gameTime);
            oldState = state;

            int i = 0;
            while (i < gems.Count)
            {
                if (gems[i].Rect.Intersects(hero.rect))
                {
                    gems.RemoveAt(i);
                    Score += 10;
                }
                else i++;
            }
            foreach (boom boom in booms)
            {
                if (boom.Rect.Intersects(hero.rect))
                {
                    Score = 0;
                    gameState = GameState.Menu;
                    menu.Items[1].Active = false;
                    //выход в меню
                }
            }
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            if (gameState == GameState.Game)
            {
                DrawGame();
            }
            else
            {
                GraphicsDevice.Clear(Color.Yellow);
                menu.Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            spriteBatch.Begin();
            foreach (block1 block in blocks)
            {
                block.Draw(spriteBatch);
            }
            foreach (Gem gem in gems)
            {
                gem.Draw(spriteBatch);
            }
            foreach (boom boom in booms)
            {
                boom.Draw(spriteBatch);
            }
            foreach (flyBlock flyBlock in flyBlocks)
            {
                flyBlock.Draw(spriteBatch);
            }
            if (win != null)
            {
               win.Draw(spriteBatch);
            }

            spriteBatch.End();

            hero.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Your score:   " + Score, Vector2.Zero, Color.Green);
            spriteBatch.End();
        }
    }
    enum GameState
    {
        Game,
        Menu
    }
}
