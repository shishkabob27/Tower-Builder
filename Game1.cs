using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.IO;
using System.Globalization;

namespace Tower_Builder
{
    public class Game1 : Game
    {
        private List<SaveFile> saveFiles;

        private bool storagePrompted = false;

        private bool savedGame;

        private bool touchedSun;

        private int leftoverRibbons;

        private int loadSlot;

        private int loadSlotPadding;

        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        private Rectangle safe;

        private Viewport defaultViewport;

        private Viewport leftViewport;

        private Viewport rightViewport;

        private int tempX;

        private int tempY;

        private Texture2D buttonA;

        private Texture2D buttonB;

        private Texture2D buttonX;

        private Texture2D buttonLS;

        private Texture2D buttonLT;

        private Texture2D buttonLB;

        private Texture2D buttonRT;

        private Texture2D buttonRB;

        private Texture2D buttonStart;

        private string animalList;

        private Animal animal;

        private List<Animal> animals;

        private Texture2D animalTexture1;
        private Texture2D animalTexture2;
        private Texture2D animalTexture3;

        private Texture2D animalTexture4;

        private Texture2D pixel;

        private List<Player> players = new List<Player>();

        private Dictionary<string, int> powers;

        private int jumpVelocity;

        private int fallVelocity;

        private int delayAmount;

        private int screen = 1;

        private int fadeIn;

        private List<GameOver> gameOvers;

        private List<int> tutorials;

        private string tutorialText;

        private Rectangle tutorialBack;

        private Rectangle tutorialFront;

        private Texture2D tutorialLeft;

        private Texture2D tutorialRight;

        private bool gamePaused;

        private bool pauseQuit;

        private bool pauseHelp;

        private bool pauseSave;

        private bool unPauseGame;

        private int pauseItem;

        private Rectangle pauseBack;

        private Rectangle pauseFront;

        private int menuItem;

        private Texture2D screen1;

        private Texture2D screen2;

        private Texture2D screen3;

        private Texture2D screen4;

        private Texture2D titleArt;

        private Texture2D arrowRight;

        private Texture2D arrowLeft;

        private Texture2D boxBubBlock;

        private Texture2D boxUnderground;

        private Vector2 screenPosition = new Vector2(0f, 720f);

        private List<List<Block>> blocks;

        private Texture2D blockTexture;

        private Texture2D editBlockTexture;

        private Texture2D editPreviewTexture;

        private Texture2D delBlockTexture;

        private List<Color> blockColors;

        private Texture2D truckTexture1;

        private Texture2D truckTexture2;

        private List<List<Rectangle>> trucks;

        private Texture2D sunTexture;

        private List<Rectangle> sun;

        private List<ParticleEngine> blockParticles;

        private List<List<Text>> floatingText;

        private Texture2D parachuteTexture;

        private int screenHeight;

        private int screenWidth;

        private int bgHeight;

        private List<Rectangle> background;

        private List<Rectangle> solidGround;

        private Texture2D bgGradient;

        private Texture2D groundTexture;

        private List<List<Vector2>> stars;

        private Texture2D cloudTexture;

        private List<List<Rectangle>> clouds;

        private Texture2D ribbonTexture;

        private Ribbon ribbon;

        private List<List<Ribbon>> ribbons;

        private Random randy;

        private Texture2D hatTexture;

        private List<Texture2D> hats;

        private List<int> hatOffset;

        private int collisionPadding = 6;

        private bool fallHit;

        private bool jumpHit;

        private Block block;

        private List<Vector2> editBox;

        private List<List<Block>> blocksToRemove;

        private int blockParticleLimit;

        private Rectangle eRectangle;

        private Rectangle bRectangle;

        private Rectangle pRectangle;

        private List<Text> messages;

        private Text message1;

        private Text message2;

        private SpriteFont numFont;

        private SpriteFont tutorialFont;

        private List<SoundEffect> sfxBlocks;

        private List<SoundEffect> sfxBreaks;

        private SoundEffect sfxRibbonBreak;

        private SoundEffect sfxPickup;

        private SoundEffect sfxPauseOpen;

        private SoundEffect sfxPauseClose;

        private SoundEffect sfxBurning;

        private SoundEffect sfxMenuScreen;

        private SoundEffect sfxMenuItem;

        private SoundEffect sfxAnimalChange;

        private SoundEffect sfxGhostChange;

        private Song musicGameplay;

        private Song musicMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);   
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            screenWidth = (this.graphics.PreferredBackBufferWidth = 1280);
            screenHeight = (this.graphics.PreferredBackBufferHeight = 720);
            int num = (int)((float)this.screenWidth - (float)this.screenWidth * 0.9f) / 2;
            int num2 = (int)((float)this.screenHeight - (float)this.screenHeight * 0.9f) / 2;
            this.safe = new Rectangle(num, num2, (int)((float)this.screenWidth * 0.9f), (int)((float)this.screenHeight * 0.9f));
            this.saveFiles = new List<SaveFile>();
            SaveFile saveFile = new SaveFile();
            saveFile.Initialize("blank", "blank");
            this.saveFiles.Add(saveFile);
            for (int i = 1; i <= 3; i++)
            {
                saveFile = new SaveFile();
                saveFile.Initialize("Craftimals_Blocks_" + i, "Craftimals_Powers_" + i);
                this.saveFiles.Add(saveFile);
            }
        }

        protected override void Initialize()
        {
            Window.Title = "Craftimals: Build to the Sun";
            if (this.screen < 5)
            {
                this.InitializeMenus();
            }
            else if (this.screen == 5)
            {
                this.InitializeGameplay();
            }
            base.Initialize();
        }

        private void InitializeMenus()
        {
            this.menuItem = 1;
            this.loadSlot = 0;
            this.loadSlotPadding = 0;
            this.touchedSun = false;
            this.fadeIn = 255;
            this.animals = new List<Animal>();
            this.animalList = "bear,raccoon,pig,tiger,frog";
            this.messages = new List<Text>();
            this.numFont = base.Content.Load<SpriteFont>("fonts/number");
            this.tutorialFont = base.Content.Load<SpriteFont>("fonts/tutorial");
        }

        private void InitializeGameplay()
        {
            this.tutorials = new List<int>();
            this.gamePaused = (this.unPauseGame = (this.pauseQuit = (this.pauseHelp = false)));
            this.pauseItem = 1;
            this.gameOvers = new List<GameOver>();
            this.fadeIn = 255;
            this.powers = new Dictionary<string, int>();
            this.powers.Add("maxCarry", 3);
            this.powers.Add("maxJumps", 1);
            this.powers.Add("editRadius", 0);
            this.powers.Add("colorRange", 2);
            this.powers.Add("hatRange", 0);
            this.jumpVelocity = 16;
            this.fallVelocity = 12;
            this.delayAmount = 10;
            this.blocks = new List<List<Block>>();
            List<Block> list = new List<Block>();
            this.blocks.Add(list);
            list = new List<Block>();
            this.blocks.Add(list);
            this.blockColors = new List<Color>();
            this.blockColors.Add(Color.White);
            this.blockColors.Add(Color.Gray);
            this.blockColors.Add(Color.Black);
            this.blockColors.Add(Color.Purple);
            this.blockColors.Add(Color.Red);
            this.blockColors.Add(Color.Pink);
            this.blockColors.Add(Color.Green);
            this.blockColors.Add(Color.Cyan);
            this.blockColors.Add(Color.DarkBlue);
            this.blockColors.Add(Color.SaddleBrown);
            this.blockColors.Add(Color.Orange);
            this.blockColors.Add(Color.Yellow);
            this.editBox = new List<Vector2>();
            this.blocksToRemove = new List<List<Block>>();
            list = new List<Block>();
            this.blocksToRemove.Add(list);
            list = new List<Block>();
            this.blocksToRemove.Add(list);
            this.blockParticles = new List<ParticleEngine>();
            this.hats = new List<Texture2D>();
            this.hatOffset = new List<int>();
            this.floatingText = new List<List<Text>>();
            this.ribbons = new List<List<Ribbon>>();
            List<Ribbon> list2 = new List<Ribbon>();
            this.ribbons.Add(list2);
            list2 = new List<Ribbon>();
            this.ribbons.Add(list2);
            this.trucks = new List<List<Rectangle>>();
            List<Rectangle> list3 = new List<Rectangle>();
            this.trucks.Add(list3);
            list3 = new List<Rectangle>();
            this.trucks.Add(list3);
            this.sun = new List<Rectangle>();
            this.background = new List<Rectangle>();
            this.solidGround = new List<Rectangle>();
            this.stars = new List<List<Vector2>>();
            List<Vector2> list4 = new List<Vector2>();
            this.stars.Add(list4);
            list4 = new List<Vector2>();
            this.stars.Add(list4);
            this.clouds = new List<List<Rectangle>>();
            List<Rectangle> list5 = new List<Rectangle>();
            this.clouds.Add(list5);
            list5 = new List<Rectangle>();
            this.clouds.Add(list5);
            this.randy = new Random();
        }

        protected override void LoadContent()
        {
            if (this.screen < 5)
            {
                this.LoadMenuContent();
            }
            else if (this.screen == 5)
            {
                this.LoadGameplayContent();
            }
        }

        private void LoadMenuContent()
        {
            this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
            this.pixel = new Texture2D(base.GraphicsDevice, 1, 1, true, 0);
            this.pixel.SetData<Color>(new Color[]
            {
                Color.White
            });
            this.editPreviewTexture = base.Content.Load<Texture2D>("tools/edit-preview");
            this.buttonA = base.Content.Load<Texture2D>("buttons/a-button");
            this.buttonB = base.Content.Load<Texture2D>("buttons/b-button");
            this.buttonX = base.Content.Load<Texture2D>("buttons/x-button");
            this.buttonLS = base.Content.Load<Texture2D>("buttons/ls-button");
            this.buttonLT = base.Content.Load<Texture2D>("buttons/lt-button");
            this.buttonLB = base.Content.Load<Texture2D>("buttons/lb-button");
            this.buttonRT = base.Content.Load<Texture2D>("buttons/rt-button");
            this.buttonRB = base.Content.Load<Texture2D>("buttons/rb-button");
            this.buttonStart = base.Content.Load<Texture2D>("buttons/start-button");
            this.screen1 = base.Content.Load<Texture2D>("menu/screen1");
            this.screen2 = base.Content.Load<Texture2D>("menu/screen2");
            this.screen3 = base.Content.Load<Texture2D>("menu/screen3");
            this.screen4 = base.Content.Load<Texture2D>("menu/screen4");
            this.titleArt = base.Content.Load<Texture2D>("menu/title");
            this.arrowLeft = base.Content.Load<Texture2D>("menu/arrow-left");
            this.arrowRight = base.Content.Load<Texture2D>("menu/arrow-right");
            this.boxBubBlock = base.Content.Load<Texture2D>("menu/bub-block");
            this.boxUnderground = base.Content.Load<Texture2D>("menu/going-underground");
            this.tutorialLeft = base.Content.Load<Texture2D>("menu/tutorial-left");
            this.tutorialRight = base.Content.Load<Texture2D>("menu/tutorial-right");
            foreach (string text in this.animalList.Split(new char[]
            {
                ','
            }))
            {
                this.animalTexture1 = base.Content.Load<Texture2D>("animals/" + text + "-right");
                this.animalTexture2 = base.Content.Load<Texture2D>("animals/" + text + "-left");
                this.animalTexture3 = base.Content.Load<Texture2D>("animals/ghost/" + text + "-right");
                this.animalTexture4 = base.Content.Load<Texture2D>("animals/ghost/" + text + "-left");
                this.animal = new Animal();
                this.animal.Initialize(this.animalTexture1, this.animalTexture2, this.animalTexture3, this.animalTexture4);
                this.animals.Add(this.animal);
            }
            this.sfxMenuScreen = base.Content.Load<SoundEffect>("sounds/whoosh");
            this.sfxMenuItem = base.Content.Load<SoundEffect>("sounds/click");
            this.sfxAnimalChange = base.Content.Load<SoundEffect>("sounds/pop1");
            this.sfxGhostChange = base.Content.Load<SoundEffect>("sounds/pop2");
            this.musicMenu = base.Content.Load<Song>("music/menu");
            MediaPlayer.Play(this.musicMenu);
            MediaPlayer.IsRepeating = true;
        }

        private void LoadGameplayContent()
        {
            this.defaultViewport = base.GraphicsDevice.Viewport;
            this.leftViewport = this.defaultViewport;
            this.rightViewport = this.defaultViewport;
            if (this.players.Count > 1)
            {
                this.leftViewport.Width = this.leftViewport.Width / 2 - 1;
                this.rightViewport.Width = this.rightViewport.Width / 2 - 1;
                this.rightViewport.X = this.leftViewport.Width + 2;
            }
            this.bgHeight = 100000;
            this.background.Add(new Rectangle(0, this.screenHeight - this.bgHeight - this.screenHeight / 4, this.screenWidth, this.bgHeight));
            this.solidGround.Add(new Rectangle(0, this.background[0].Y + this.background[0].Height, this.screenWidth, this.screenHeight));
            if (this.players.Count > 0)
            {
                this.background.Add(new Rectangle(-this.screenWidth / 2, this.screenHeight - this.bgHeight - this.screenHeight / 4, this.screenWidth, this.bgHeight));
                this.solidGround.Add(new Rectangle(0, this.background[1].Y + this.background[1].Height, this.screenWidth, this.screenHeight));
            }
            this.hatTexture = base.Content.Load<Texture2D>("hats/none");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(0);
            this.hatTexture = base.Content.Load<Texture2D>("hats/baseball-cap");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(10);
            this.hatTexture = base.Content.Load<Texture2D>("hats/top-hat");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(15);
            this.hatTexture = base.Content.Load<Texture2D>("hats/sun-hat");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(20);
            this.hatTexture = base.Content.Load<Texture2D>("hats/nurse-cap");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(15);
            this.hatTexture = base.Content.Load<Texture2D>("hats/police-cap");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(40);
            this.hatTexture = base.Content.Load<Texture2D>("hats/flight-cap");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(20);
            this.hatTexture = base.Content.Load<Texture2D>("hats/snow-cap");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(40);
            this.hatTexture = base.Content.Load<Texture2D>("hats/pirate-hat");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(20);
            this.hatTexture = base.Content.Load<Texture2D>("hats/feather-band");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(10);
            this.hatTexture = base.Content.Load<Texture2D>("hats/bunny-ears");
            this.hats.Add(this.hatTexture);
            this.hatOffset.Add(20);
            if (this.players[0].Ghost)
            {
                this.background[0] = new Rectangle(0, this.screenHeight - this.bgHeight - this.screenHeight / 4 + 25, this.screenWidth, this.bgHeight);
                this.solidGround[0] = new Rectangle(0, this.background[0].Y + this.background[0].Height + 25, this.screenWidth, this.screenHeight);
                this.players[0].Initialize(this.animals[this.players[0].AnimalIndex].GhostSpriteRight, this.animals[this.players[0].AnimalIndex].GhostSpriteLeft, this.hats[0], new Vector2((float)(this.leftViewport.Width / 2 - this.animalTexture1.Width / 2), (float)(this.solidGround[0].Y - this.animalTexture1.Height - 50)), this.leftViewport);
            }
            else
            {
                this.players[0].Initialize(this.animals[this.players[0].AnimalIndex].NormalSpriteRight, this.animals[this.players[0].AnimalIndex].NormalSpriteLeft, this.hats[0], new Vector2((float)(this.leftViewport.Width / 2 - this.animalTexture1.Width / 2), (float)(this.solidGround[0].Y - this.animalTexture1.Height)), this.leftViewport);
            }
            this.editBox.Add(Vector2.Zero);
            GameOver gameOver = new GameOver();
            gameOver.Initialize(this.pixel, this.numFont, this.leftViewport, this.safe);
            this.gameOvers.Add(gameOver);
            if (this.players.Count > 1)
            {
                if (this.players[1].Ghost)
                {
                    this.background[1] = new Rectangle(-this.screenWidth / 2, this.screenHeight - this.bgHeight - this.screenHeight / 4 + 25, this.screenWidth, this.bgHeight);
                    this.solidGround[1] = new Rectangle(0, this.background[1].Y + this.background[1].Height + 25, this.screenWidth, this.screenHeight);
                    this.players[1].Initialize(this.animals[this.players[1].AnimalIndex].GhostSpriteRight, this.animals[this.players[1].AnimalIndex].GhostSpriteLeft, this.hats[0], new Vector2((float)(this.rightViewport.Width / 2 - this.animalTexture1.Width / 2), (float)(this.solidGround[1].Y - this.animalTexture1.Height - 50)), this.rightViewport);
                }
                else
                {
                    this.players[1].Initialize(this.animals[this.players[1].AnimalIndex].NormalSpriteRight, this.animals[this.players[1].AnimalIndex].NormalSpriteLeft, this.hats[0], new Vector2((float)(this.rightViewport.Width / 2 - this.animalTexture1.Width / 2), (float)(this.solidGround[1].Y - this.animalTexture1.Height)), this.rightViewport);
                }
                this.editBox.Add(Vector2.Zero);
                gameOver = new GameOver();
                gameOver.Initialize(this.pixel, this.numFont, this.rightViewport, this.safe);
                this.gameOvers.Add(gameOver);
            }
            this.blockTexture = base.Content.Load<Texture2D>("tools/block");
            this.editBlockTexture = base.Content.Load<Texture2D>("tools/edit-block");
            this.truckTexture1 = base.Content.Load<Texture2D>("tools/truck1");
            this.truckTexture2 = base.Content.Load<Texture2D>("tools/truck2");
            this.sunTexture = base.Content.Load<Texture2D>("background/sun");
            for (int i = 0; i < this.players.Count; i++)
            {
                this.trucks[i].Add(new Rectangle(this.background[i].X + this.safe.X, this.solidGround[i].Y - this.truckTexture1.Height + 3, this.truckTexture1.Width, this.truckTexture1.Height));
                this.trucks[i].Add(new Rectangle(this.background[i].X + this.background[i].Width - this.truckTexture2.Width - this.safe.X, this.solidGround[i].Y - this.truckTexture2.Height + 3, this.truckTexture2.Width, this.truckTexture2.Height));
                this.sun.Add(new Rectangle(this.background[i].X, this.background[i].Y - this.sunTexture.Height, this.sunTexture.Width, this.sunTexture.Height));
            }
            this.delBlockTexture = base.Content.Load<Texture2D>("tools/delete");
            this.parachuteTexture = base.Content.Load<Texture2D>("tools/parachute");
            this.bgGradient = base.Content.Load<Texture2D>("background/sky");
            this.groundTexture = base.Content.Load<Texture2D>("background/ground");
            this.cloudTexture = base.Content.Load<Texture2D>("background/cloud");
            this.ribbonTexture = base.Content.Load<Texture2D>("background/ribbon");
            for (int j = 0; j <= 250; j++)
            {
                Vector2 vector;
                vector = new((float)this.randy.Next(0, this.screenWidth), (float)this.randy.Next(0, this.bgHeight / 3));
                this.stars[0].Add(vector);
                this.stars[1].Add(vector);
            }
            for (int k = 0; k <= 70; k++)
            {
                int num = this.randy.Next(this.cloudTexture.Width / 2, this.cloudTexture.Width);
                Rectangle rectangle;
                rectangle = new(this.randy.Next(-this.cloudTexture.Width / 2, this.screenWidth - this.cloudTexture.Width / 2), this.randy.Next(this.bgHeight / 2, this.bgHeight - this.screenHeight), num, num * this.cloudTexture.Height / this.cloudTexture.Width);
                for (int i = 0; i < this.clouds[0].Count; i++)
                {
                    if (rectangle.Intersects(this.clouds[0][i]))
                    {
                        k--;
                        break;
                    }
                }
                if (k == this.clouds[0].Count)
                {
                    this.clouds[0].Add(rectangle);
                    this.clouds[1].Add(rectangle);
                }
            }
            if (this.loadSlot > 0)
            {
                this.LoadData();
                this.tutorials = this.saveFiles[this.loadSlot].TutorialSteps;
                this.leftoverRibbons = this.saveFiles[this.loadSlot].LeftoverRibbons;
            }
            else
            {
                this.leftoverRibbons = 20;
                this.loadSlot = 1;
            }
            for (int i = 0; i < this.players.Count; i++)
            {
                if (this.leftoverRibbons >= 20)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)(this.bgHeight - this.screenHeight)), this.background[i], "maxCarry", 10);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["maxCarry"] = 10;
                }
                if (this.leftoverRibbons >= 19)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)(this.bgHeight - this.screenHeight * 3)), this.background[i], "hatRange", 1);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 1;
                }
                if (this.leftoverRibbons >= 18)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.95f), this.background[i], "colorRange", 5);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["colorRange"] = 5;
                }
                if (this.leftoverRibbons >= 17)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.9f), this.background[i], "hatRange", 2);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 2;
                }
                if (this.leftoverRibbons >= 16)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.85f), this.background[i], "maxCarry", 20);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["maxCarry"] = 20;
                }
                if (this.leftoverRibbons >= 15)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.8f), this.background[i], "hatRange", 3);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 3;
                }
                if (this.leftoverRibbons >= 14)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.75f), this.background[i], "maxCarry", 40);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["maxCarry"] = 40;
                }
                if (this.leftoverRibbons >= 13)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.7f), this.background[i], "hatRange", 4);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 4;
                }
                if (this.leftoverRibbons >= 12)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.65f), this.background[i], "colorRange", 8);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["colorRange"] = 8;
                }
                if (this.leftoverRibbons >= 11)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.6f), this.background[i], "hatRange", 5);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 5;
                }
                if (this.leftoverRibbons >= 10)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.55f), this.background[i], "maxJumps", 2);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["maxJumps"] = 2;
                }
                if (this.leftoverRibbons >= 9)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.5f), this.background[i], "hatRange", 6);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 6;
                }
                if (this.leftoverRibbons >= 8)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.45f), this.background[i], "editRadius", 1);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["editRadius"] = 1;
                }
                if (this.leftoverRibbons >= 7)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.4f), this.background[i], "hatRange", 7);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 7;
                }
                if (this.leftoverRibbons >= 6)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.35f), this.background[i], "maxCarry", 50);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["maxCarry"] = 60;
                }
                if (this.leftoverRibbons >= 5)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.3f), this.background[i], "hatRange", 8);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 8;
                }
                if (this.leftoverRibbons >= 4)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.25f), this.background[i], "colorRange", 11);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["colorRange"] = 11;
                }
                if (this.leftoverRibbons >= 3)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.2f), this.background[i], "hatRange", 9);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 9;
                }
                if (this.leftoverRibbons >= 2)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.15f), this.background[i], "maxCarry", 500);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["maxCarry"] = 500;
                }
                if (this.leftoverRibbons >= 1)
                {
                    this.ribbon = new Ribbon();
                    this.ribbon.Initialize(this.ribbonTexture, new Vector2(0f, (float)this.bgHeight * 0.1f), this.background[i], "hatRange", 10);
                    this.ribbons[i].Add(this.ribbon);
                }
                else
                {
                    this.powers["hatRange"] = 10;
                }
            }
            List<Texture2D> list = new List<Texture2D>();
            list.Add(base.Content.Load<Texture2D>("tools/edit-preview"));
            this.blockParticles.Add(new ParticleEngine(list));
            this.blockParticles.Add(new ParticleEngine(list));
            this.sfxBlocks = new List<SoundEffect>();
            SoundEffect soundEffect = base.Content.Load<SoundEffect>("sounds/block1");
            this.sfxBlocks.Add(soundEffect);
            soundEffect = base.Content.Load<SoundEffect>("sounds/block2");
            this.sfxBlocks.Add(soundEffect);
            soundEffect = base.Content.Load<SoundEffect>("sounds/block3");
            this.sfxBlocks.Add(soundEffect);
            soundEffect = base.Content.Load<SoundEffect>("sounds/block4");
            this.sfxBlocks.Add(soundEffect);
            this.sfxBreaks = new List<SoundEffect>();
            //soundEffect = base.Content.Load<SoundEffect>("sounds/break1");
            this.sfxBreaks.Add(soundEffect);
            //soundEffect = base.Content.Load<SoundEffect>("sounds/break2");
            this.sfxBreaks.Add(soundEffect);
            //soundEffect = base.Content.Load<SoundEffect>("sounds/break3");
            this.sfxBreaks.Add(soundEffect);
            //soundEffect = base.Content.Load<SoundEffect>("sounds/break4");
            this.sfxBreaks.Add(soundEffect);
            //this.sfxRibbonBreak = base.Content.Load<SoundEffect>("sounds/glass");
            this.sfxPickup = base.Content.Load<SoundEffect>("sounds/pickup");
            this.sfxPauseOpen = base.Content.Load<SoundEffect>("sounds/paper1");
            this.sfxPauseClose = base.Content.Load<SoundEffect>("sounds/paper2");
            //this.sfxBurning = base.Content.Load<SoundEffect>("sounds/fire");
            this.musicGameplay = base.Content.Load<Song>("music/gameplay");
            MediaPlayer.Play(this.musicGameplay);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.screen < 5)
            {
                this.UpdateMenus(gameTime);
            }
            else if (this.screen == 5)
            {
                this.UpdateGameplay(gameTime);
            }
            for (int i = 0; i < this.players.Count; i++)
            {
                this.players[i].UpdateController();
            }
            /*
            foreach (SignedInGamer signedInGamer in Gamer.SignedInGamers)
            {
                if (this.screen < 5)
                {
                    signedInGamer.Presence.PresenceMode = 46;
                }
                else if (this.screen == 5 && this.players.Count == 1)
                {
                    signedInGamer.Presence.PresenceMode = 1;
                }
                else if (this.screen == 5 && this.players.Count > 1)
                {
                    signedInGamer.Presence.PresenceMode = 2;
                }
            }
            */
            base.Update(gameTime);
        }

        private void UpdateMenus(GameTime gameTime)
        {
            if (this.screen == 1)
            {
                if (this.players.Count < 1)
                {
                    for (PlayerIndex playerIndex = PlayerIndex.One; playerIndex <= PlayerIndex.Four; playerIndex++)
                    {
                        if (GamePad.GetState(playerIndex).Buttons.A == ButtonState.Pressed || GamePad.GetState(playerIndex).Buttons.Start == ButtonState.Pressed || GamePad.GetState(playerIndex).Buttons.Y == ButtonState.Pressed || GamePad.GetState(playerIndex).Buttons.X == ButtonState.Pressed || GamePad.GetState(playerIndex).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            Player player = new Player();
                            player.Preset(playerIndex);
                            this.players.Add(player);
                            this.screen = 2;
                            this.sfxMenuScreen.Play();
                            break;
                        }
                    }
                }
            }
            else if (this.screen == 2 && this.players.Count > 0 && this.screenPosition.Y == 0f && this.screenPosition.X == 0f)
            {
                if (!this.storagePrompted)
                {
                    this.PromptMe();
                }
                if (this.players.Count == 2)
                {
                    this.players.RemoveAt(1);
                }
                try
                {
                    if (true) //Global.SaveDevice.IsReady
                    {
                        if (((double)this.players[0].currentGamePadState.ThumbSticks.Left.Y > 0.8 && (double)this.players[0].previousGamePadState.ThumbSticks.Left.Y < 0.8) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Up) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Up)))
                        {
                            if (this.loadSlot == 1)
                            {
                                this.loadSlot = 3;
                            }
                            else if (this.loadSlot > 1)
                            {
                                this.loadSlot--;
                            }
                            else if (this.menuItem == 1)
                            {
                                this.menuItem = 4;
                            }
                            else
                            {
                                this.menuItem--;
                            }
                            this.sfxMenuItem.Play();
                        }
                        else if (((double)this.players[0].currentGamePadState.ThumbSticks.Left.Y < -0.8 && (double)this.players[0].previousGamePadState.ThumbSticks.Left.Y > -0.8) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Down) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Down)))
                        {
                            if (this.loadSlot == 3)
                            {
                                this.loadSlot = 1;
                            }
                            else if (this.loadSlot < 3 && this.loadSlot > 0)
                            {
                                this.loadSlot++;
                            }
                            else if (this.menuItem == 4)
                            {
                                this.menuItem = 1;
                            }
                            else
                            {
                                this.menuItem++;
                            }
                            this.sfxMenuItem.Play();
                        }
                        if ((this.players[0].currentGamePadState.Buttons.A == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.A == null) || (this.players[0].currentGamePadState.Buttons.Start == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.Start == 0) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Enter) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Enter)))
                        {
                            if (this.menuItem == 1)
                            {
                                this.screen = 3;
                                this.sfxMenuScreen.Play();
                                this.touchedSun = false;
                                for (int i = 0; i < this.players.Count; i++)
                                {
                                    this.players[i].AnimalIndex = 0;
                                    this.players[i].Ghost = false;
                                }
                            }
                            else if (this.menuItem == 2)
                            {
                                if (this.menuItem == 2 && false) // Guide.IsTrialMode
                                {
                                    this.PurchaseGame();
                                }
                                else if (this.loadSlot == 0)
                                {
                                    this.loadSlot = 1;
                                    this.loadSlotPadding = 70;
                                }
                                else
                                {
                                    this.screen = 3;
                                    this.sfxMenuScreen.Play();
                                    this.touchedSun = this.saveFiles[this.loadSlot].TouchedSun;
                                    for (int i = 0; i < this.players.Count; i++)
                                    {
                                        this.players[i].AnimalIndex = 0;
                                        this.players[i].Ghost = false;
                                    }
                                }
                            }
                            else if (this.menuItem == 3)
                            {
                                this.screen = 4;
                                this.sfxMenuScreen.Play();
                            }
                            else if (this.menuItem == 4)
                            {
                                base.Exit();
                            }
                        }
                        else if (this.menuItem == 2 && ((this.players[0].currentGamePadState.Buttons.B == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.B == 0) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Escape) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Escape))))
                        {
                            this.loadSlot = 0;
                            this.loadSlotPadding = 0;
                        }
                    }
                }
                catch
                {
                }
            }
            else if (this.screen == 3 && this.screenPosition.X == -1280f)
            {
                for (PlayerIndex playerIndex = PlayerIndex.One; playerIndex <= PlayerIndex.Four; playerIndex++)
                {
                    if (this.players.Count < 2 && GamePad.GetState(playerIndex).Buttons.A == ButtonState.Pressed && this.players[0].Index != playerIndex)
                    {
                        Player player = new Player();
                        player.Preset(playerIndex);
                        this.players.Add(player);
                    }
                }
                if (this.players.Count > 1 && this.players[1].currentGamePadState.Buttons.B == ButtonState.Pressed)
                {
                    this.players.RemoveAt(1);
                }
                for (int i = 0; i < this.players.Count; i++)
                {
                    if ((double)this.players[i].currentGamePadState.ThumbSticks.Left.X > 0.8 && (double)this.players[i].previousGamePadState.ThumbSticks.Left.X < 0.8)
                    {
                        if (this.players[i].AnimalIndex < this.animals.Count - 1)
                        {
                            this.players[i].AnimalIndex++;
                        }
                        else
                        {
                            this.players[i].AnimalIndex = 0;
                        }
                        this.sfxAnimalChange.Play();
                    }
                    else if ((double)this.players[i].currentGamePadState.ThumbSticks.Left.X < -0.8 && (double)this.players[i].previousGamePadState.ThumbSticks.Left.X > -0.8)
                    {
                        if (this.players[i].AnimalIndex > 0)
                        {
                            this.players[i].AnimalIndex--;
                        }
                        else
                        {
                            this.players[i].AnimalIndex = this.animals.Count - 1;
                        }
                        this.sfxAnimalChange.Play();
                    }
                    else if (this.touchedSun && this.players[i].currentGamePadState.Buttons.X == ButtonState.Pressed && this.players[i].previousGamePadState.Buttons.X == 0)
                    {
                        if (this.players[i].Ghost)
                        {
                            this.players[i].Ghost = false;
                        }
                        else
                        {
                            this.players[i].Ghost = true;
                        }
                        this.sfxGhostChange.Play();
                    }
                }
                if ((this.players[0].currentGamePadState.Buttons.Start == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.Start == 0) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Enter) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Enter)))
                {
                    this.screen = 5;
                    this.Initialize();
                }
                else if ((this.players[0].currentGamePadState.Buttons.B == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.B == 0) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Escape) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Escape)))
                {
                    this.screen = 2;
                    this.loadSlot = 0;
                    this.loadSlotPadding = 0;
                    this.sfxMenuScreen.Play();
                }
            }
            else if (this.screen == 4 && this.screenPosition.X == 1280f)
            {
                if ((this.players[0].currentGamePadState.Buttons.B == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.B == 0) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Escape) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Escape)))
                {
                    this.screen = 2;
                    this.sfxMenuScreen.Play();
                }
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            /*
            for (int i = 0; i < this.players.Count; i++)
            {
                if ((false || !GamePad.GetState(this.players[i].Index).IsConnected) && !this.gamePaused) //Guide.IsVisible
                {
                    this.gamePaused = true;
                    this.pauseItem = 1;
                    this.pauseQuit = (this.pauseHelp = (this.pauseSave = false));
                    this.savedGame = false;
                }
            }
            */
            if (!this.gamePaused && ((this.players[0].currentGamePadState.Buttons.Start == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.Start == 0) || (this.players[0].currentKeyboardState.IsKeyDown(Keys.Escape) && !this.players[0].previousKeyboardState.IsKeyDown(Keys.Escape))))
            {
                this.gamePaused = true;
                this.pauseItem = 1;
                this.pauseQuit = (this.pauseHelp = (this.pauseSave = false));
                this.savedGame = false;
                this.sfxPauseOpen.Play();
            }
            else if (this.gamePaused)
            {
                if ((double)this.players[0].currentGamePadState.ThumbSticks.Left.Y > 0.8 && (double)this.players[0].previousGamePadState.ThumbSticks.Left.Y < 0.8)
                {
                    if (this.pauseSave)
                    {
                        if (true)
                        {
                            if (this.loadSlot == 1)
                            {
                                this.loadSlot = 3;
                            }
                            else
                            {
                                this.loadSlot--;
                            }
                            this.savedGame = false;
                            this.sfxMenuItem.Play();
                        }
                    }
                    else
                    {
                        if (this.pauseItem == 1)
                        {
                            this.pauseItem = 4;
                        }
                        else
                        {
                            this.pauseItem--;
                        }
                        this.pauseQuit = false;
                        this.pauseHelp = false;
                        this.sfxMenuItem.Play();
                    }
                }
                else if ((double)this.players[0].currentGamePadState.ThumbSticks.Left.Y < -0.8 && (double)this.players[0].previousGamePadState.ThumbSticks.Left.Y > -0.8)
                {
                    if (this.pauseSave)
                    {
                        if (true)
                        {
                            if (this.loadSlot == 3)
                            {
                                this.loadSlot = 1;
                            }
                            else
                            {
                                this.loadSlot++;
                            }
                            this.savedGame = false;
                            this.sfxMenuItem.Play();
                        }
                    }
                    else
                    {
                        if (this.pauseItem == 4)
                        {
                            this.pauseItem = 1;
                        }
                        else
                        {
                            this.pauseItem++;
                        }
                        this.pauseQuit = false;
                        this.pauseHelp = false;
                        this.sfxMenuItem.Play();
                    }
                }
                else if ((this.pauseItem == 1 && this.players[0].currentGamePadState.Buttons.A == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.A == null) || (this.players[0].currentGamePadState.Buttons.Start == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.Start == 0))
                {
                    this.gamePaused = false;
                    for (int i = 0; i < this.players.Count; i++)
                    {
                        this.players[i].Editing = false;
                        this.players[i].Deleting = 0;
                    }
                    this.unPauseGame = true;
                    this.sfxPauseClose.Play();
                }
                else if (this.pauseItem == 2 && this.players[0].currentGamePadState.Buttons.A == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.A == 0)
                {
                    if (false) //Guide.IsTrialMode
                    {
                        this.PurchaseGame();
                    }
                    else if (!this.pauseSave)
                    {
                        this.pauseSave = true;
                    }
                    else
                    {
                        this.SaveData();
                    }
                }
                else if (this.pauseItem == 3 && this.players[0].currentGamePadState.Buttons.A == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.A == 0)
                {
                    this.pauseHelp = true;
                }
                else if (this.pauseItem == 4 && this.players[0].currentGamePadState.Buttons.A == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.A == 0)
                {
                    if (this.pauseQuit)
                    {
                        this.screenPosition = Vector2.Zero;
                        this.screen = 2;
                        this.Initialize();
                    }
                    else
                    {
                        this.pauseQuit = true;
                    }
                }
                else if (this.players[0].currentGamePadState.Buttons.B == ButtonState.Pressed && this.players[0].previousGamePadState.Buttons.B == 0)
                {
                    try
                    {
                        if (true)
                        {
                            this.pauseQuit = (this.pauseHelp = (this.pauseSave = false));
                            this.savedGame = false;
                        }
                    }
                    catch
                    {
                        this.pauseQuit = (this.pauseHelp = (this.pauseSave = false));
                    }
                }
            }
            else if (!this.gamePaused)
            {
                this.UpdatePlayer(gameTime);
            }
            if (this.unPauseGame && this.players[0].currentGamePadState.Buttons.A == 0)
            {
                this.unPauseGame = false;
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            int i = 0;
            while (i < this.players.Count)
            {
                if (this.players[i].HitBox.Intersects(this.sun[i]) && this.gameOvers[i].Step == 0 && !this.players[i].Ghost)
                {
                    if (this.players[i].HitBox.Y < this.sun[i].Y + this.sun[i].Height - 25)
                    {
                        this.touchedSun = true;
                        this.gameOvers[i].Step = 1;
                        this.sfxBurning.Play();
                    }
                    goto IL_42F;
                }
                if (this.gameOvers[i].Step == 1)
                {
                    this.gameOvers[i].Update();
                    this.players[i].Opacity -= 5;
                    this.blockParticles[i].Update(this.background[i]);
                }
                else
                {
                    if (this.gameOvers[i].Step == 2)
                    {
                        this.players[i].Ghost = true;
                        if (i == 0)
                        {
                            this.background[i] = new Rectangle(0, this.screenHeight - this.bgHeight - this.screenHeight / 4 + 25, this.screenWidth, this.bgHeight);
                            this.solidGround[i] = new Rectangle(0, this.background[i].Y + this.background[i].Height + 25, this.screenWidth, this.screenHeight);
                            this.players[0].Initialize(this.animals[this.players[0].AnimalIndex].GhostSpriteRight, this.animals[this.players[0].AnimalIndex].GhostSpriteLeft, this.hats[0], new Vector2((float)(this.leftViewport.Width / 2 - this.animalTexture1.Width / 2), (float)(this.solidGround[0].Y - this.animalTexture1.Height - 50)), this.leftViewport);
                        }
                        else if (i == 1)
                        {
                            this.background[1] = new Rectangle(-this.screenWidth / 2, this.screenHeight - this.bgHeight - this.screenHeight / 4 + 25, this.screenWidth, this.bgHeight);
                            this.solidGround[1] = new Rectangle(0, this.background[1].Y + this.background[1].Height + 25, this.screenWidth, this.screenHeight);
                            this.players[1].Initialize(this.animals[this.players[1].AnimalIndex].GhostSpriteRight, this.animals[this.players[1].AnimalIndex].GhostSpriteLeft, this.hats[0], new Vector2((float)(this.rightViewport.Width / 2 - this.animalTexture1.Width / 2), (float)(this.solidGround[1].Y - this.animalTexture1.Height - 50)), this.rightViewport);
                        }
                        this.gameOvers[i].Step = 3;
                        goto IL_42F;
                    }
                    if (this.gameOvers[i].Step == 3)
                    {
                        this.gameOvers[i].Update();
                        goto IL_42F;
                    }
                    goto IL_42F;
                }
            IL_14D4:
                i++;
                continue;
            IL_42F:
                if (this.players[i].Carrying > 0 && (double)this.players[i].currentGamePadState.Triggers.Left > 0.1 && (double)this.players[i].previousGamePadState.Triggers.Left <= 0.1)
                {
                    if (this.players[i].ColorIndex == 0)
                    {
                        this.players[i].ColorIndex = this.powers["colorRange"];
                    }
                    else
                    {
                        this.players[i].ColorIndex--;
                    }
                    if (!this.tutorials.Contains(4))
                    {
                        this.tutorials.Add(4);
                    }
                }
                else if (this.players[i].Carrying > 0 && (double)this.players[i].currentGamePadState.Triggers.Right > 0.1 && (double)this.players[i].previousGamePadState.Triggers.Right <= 0.1)
                {
                    if (this.players[i].ColorIndex == this.powers["colorRange"])
                    {
                        this.players[i].ColorIndex = 0;
                    }
                    else
                    {
                        this.players[i].ColorIndex++;
                    }
                    if (!this.tutorials.Contains(4))
                    {
                        this.tutorials.Add(4);
                    }
                }
                if (this.players[i].currentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed && this.players[i].previousGamePadState.Buttons.LeftShoulder == ButtonState.Released)
                {
                    if (this.players[i].HatIndex == 0)
                    {
                        this.players[i].HatIndex = this.powers["hatRange"];
                    }
                    else
                    {
                        this.players[i].HatIndex--;
                    }
                    this.players[i].Hat = this.hats[this.players[i].HatIndex];
                    if (!this.tutorials.Contains(7) && this.players[i].HatIndex != 0)
                    {
                        this.tutorials.Add(7);
                    }
                }
                else if (this.players[i].currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed && this.players[i].previousGamePadState.Buttons.RightShoulder == ButtonState.Released)
                {
                    if (this.players[i].HatIndex == this.powers["hatRange"])
                    {
                        this.players[i].HatIndex = 0;
                    }
                    else
                    {
                        this.players[i].HatIndex++;
                    }
                    this.players[i].Hat = this.hats[this.players[i].HatIndex];
                    if (!this.tutorials.Contains(7) && this.players[i].HatIndex != 0)
                    {
                        this.tutorials.Add(7);
                    }
                }
                if (this.powers["editRadius"] == 1 && (this.players[i].Editing || this.players[i].Deleting > 0) && this.players[i].currentGamePadState.Buttons.LeftStick == ButtonState.Pressed && this.players[i].previousGamePadState.Buttons.LeftStick == ButtonState.Released)
                {
                    if (this.players[i].EditRadius == 1.8f)
                    {
                        this.players[i].EditRadius = 4.1f;
                    }
                    else
                    {
                        this.players[i].EditRadius = 1.8f;
                    }
                    if (!this.tutorials.Contains(8))
                    {
                        this.tutorials.Add(8);
                    }
                }
                if (this.players[i].Carrying > 0 && (this.players[i].currentGamePadState.Buttons.X == ButtonState.Pressed || Mouse.GetState().LeftButton == ButtonState.Pressed))
                {
                    if (this.players[i].currentGamePadState.Buttons.X == ButtonState.Pressed)
                    {
                        this.editBox[i] = new Vector2((float)(this.players[i].HitBox.X + this.players[i].HitBox.Width / 2 - this.blockTexture.Width / 2) + this.players[i].currentGamePadState.ThumbSticks.Left.X * (float)this.blockTexture.Width * this.players[i].EditRadius, (float)this.players[i].HitBox.Y - this.players[i].currentGamePadState.ThumbSticks.Left.Y * (float)this.blockTexture.Height * this.players[i].EditRadius);
                    }
                    else
                    {
                        var mousex = (float)this.players[i].currentMouseState.X;
                        var mousey = (float)this.players[i].currentMouseState.Y;
                        mousex -= (float)this.players[i].Position.X + 48;
                        mousey -= (float)this.players[i].Position.Y + 38;
                        mousex /= 80;
                        mousey /= 80;
                        mousex = Clamp(mousex, -1f, 1f);
                        mousey = Clamp(mousey, -1f, 1f);
                        this.editBox[i] = new Vector2((float)(this.players[i].HitBox.X + this.players[i].HitBox.Width / 2 - this.blockTexture.Width / 2) + mousex * (float)this.blockTexture.Width * this.players[i].EditRadius, (float)this.players[i].HitBox.Y + mousey * (float)this.blockTexture.Height * this.players[i].EditRadius);
                    }
                    this.players[i].Editing = true;
                }
                else if (this.players[i].currentGamePadState.Buttons.B == ButtonState.Pressed || Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    if (this.players[i].currentGamePadState.Buttons.B == ButtonState.Pressed)
                    {
                        this.editBox[i] = new Vector2((float)(this.players[i].HitBox.X + this.players[i].HitBox.Width / 2 - this.blockTexture.Width / 2) + this.players[i].currentGamePadState.ThumbSticks.Left.X * (float)this.blockTexture.Width * this.players[i].EditRadius, (float)this.players[i].HitBox.Y - this.players[i].currentGamePadState.ThumbSticks.Left.Y * (float)this.blockTexture.Height * this.players[i].EditRadius);
                    }
                    else
                    {
                        var mousex = (float)this.players[i].currentMouseState.X;
                        var mousey = (float)this.players[i].currentMouseState.Y;
                        mousex -= (float)this.players[i].Position.X + 48;
                        mousey -= (float)this.players[i].Position.Y + 38;
                        mousex /= 80;
                        mousey /= 80;
                        mousex = Clamp(mousex, -1f, 1f);
                        mousey = Clamp(mousey, -1f, 1f);
                        this.editBox[i] = new Vector2((float)(this.players[i].HitBox.X + this.players[i].HitBox.Width / 2 - this.blockTexture.Width / 2) + mousex * (float)this.blockTexture.Width * this.players[i].EditRadius, (float)this.players[i].HitBox.Y + mousey * (float)this.blockTexture.Height * this.players[i].EditRadius);
                    }
                    this.players[i].Deleting = 1;
                }
                else if ((this.players[i].currentGamePadState.Buttons.B != null && this.players[i].previousGamePadState.Buttons.B == ButtonState.Pressed) || (this.players[i].currentMouseState.RightButton == ButtonState.Released && this.players[i].previousMouseState.RightButton == ButtonState.Pressed))
                {
                    this.players[i].Deleting = 2;
                    this.blocksToRemove[0] = new List<Block>();
                    this.blocksToRemove[1] = new List<Block>();
                    this.eRectangle = new Rectangle((int)this.editBox[i].X, (int)this.editBox[i].Y, this.blockTexture.Width, this.blockTexture.Height);

                }
                else if (this.players[i].Carrying > 0 && ((this.players[i].currentGamePadState.Buttons.X != null && this.players[i].previousGamePadState.Buttons.X == ButtonState.Pressed) || (this.players[i].currentMouseState.LeftButton == ButtonState.Released && this.players[i].previousMouseState.LeftButton == ButtonState.Pressed)))
                {
                    this.eRectangle = new Rectangle((int)this.editBox[i].X, (int)this.editBox[i].Y, this.blockTexture.Width, this.blockTexture.Height);
                    if (!this.eRectangle.Intersects(this.solidGround[i]) && !this.eRectangle.Intersects(this.sun[i]))
                    {
                        if (!this.eRectangle.Intersects(this.players[i].HitBox) || (this.players[i].Ghost && (this.players[i].currentGamePadState.ThumbSticks.Left.X != 0f || this.players[i].currentGamePadState.ThumbSticks.Left.Y != 0f)))
                        {
                            if (this.players.Count > 1)
                            {
                                if (i == 0)
                                {
                                    this.tempY = this.solidGround[1].Y - this.players[1].HitBox.Y;
                                    this.tempY = this.solidGround[0].Y - this.tempY;
                                    int num = this.players[1].HitBox.X - this.background[1].X;
                                    num = this.background[0].X + num;
                                    this.pRectangle = new Rectangle(num, this.tempY, this.players[1].HitBox.Width, this.players[1].HitBox.Height);
                                }
                                else if (i == 1)
                                {
                                    this.tempY = this.solidGround[0].Y - this.players[0].HitBox.Y;
                                    this.tempY = this.solidGround[1].Y - this.tempY;
                                    int num = this.players[0].HitBox.X - this.background[0].X;
                                    num = this.background[1].X + num;
                                    this.pRectangle = new Rectangle(num, this.tempY, this.players[0].HitBox.Width, this.players[0].HitBox.Height);
                                }
                                if (!this.eRectangle.Intersects(this.pRectangle))
                                {
                                    this.block = new Block();
                                    this.block.Initialize(this.blockTexture, new Vector2(this.editBox[i].X - (float)this.background[i].X, this.editBox[i].Y - (float)this.background[i].Y), this.background[0], this.blockColors[this.players[i].ColorIndex]);
                                    this.blocks[0].Add(this.block);
                                    this.block = new Block();
                                    this.block.Initialize(this.blockTexture, new Vector2(this.editBox[i].X - (float)this.background[i].X, this.editBox[i].Y - (float)this.background[i].Y), this.background[1], this.blockColors[this.players[i].ColorIndex]);
                                    this.blocks[1].Add(this.block);
                                    this.players[i].Carrying--;
                                    this.sfxBlocks[this.randy.Next(0, this.sfxBlocks.Count)].Play();
                                    if (!this.tutorials.Contains(2))
                                    {
                                        this.tutorials.Add(2);
                                    }
                                }
                            }
                            else
                            {
                                this.block = new Block();
                                this.block.Initialize(this.blockTexture, new Vector2(this.editBox[i].X - (float)this.background[i].X, this.editBox[i].Y - (float)this.background[i].Y), this.background[0], this.blockColors[this.players[i].ColorIndex]);
                                this.blocks[0].Add(this.block);
                                this.players[i].Carrying--;
                                this.sfxBlocks[this.randy.Next(0, this.sfxBlocks.Count)].Play();
                                if (!this.tutorials.Contains(2))
                                {
                                    this.tutorials.Add(2);
                                }
                            }
                        }
                    }
                    this.players[i].Editing = false;
                    this.players[i].Delay = this.delayAmount;
                }
                this.UpdateMovement(i);
                this.blockParticles[i].Update(this.background[i]);
                for (int j = 0; j < this.trucks[i].Count; j++)
                {
                    if (this.players[i].HitBox.Intersects(this.trucks[i][j]) && this.players[i].Carrying < this.powers["maxCarry"])
                    {
                        int num2 = 0;
                        if (j == 0)
                        {
                            num2 = this.players[i].HitBox.Width / 2;
                        }
                        this.message1 = new Text();
                        this.message1.Initialize(this.numFont, new Vector2((float)(this.players[i].View.X + this.players[i].HitBox.X + num2), this.players[i].Position.Y - 10f), "+" + (this.powers["maxCarry"] - this.players[i].Carrying), 6);
                        this.messages.Add(this.message1);
                        this.sfxPickup.Play();
                        this.players[i].Carrying = this.powers["maxCarry"];
                        if (!this.tutorials.Contains(1))
                        {
                            this.tutorials.Add(1);
                        }
                    }
                }
                this.players[i].Update();
                goto IL_14D4;
            }
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }

        private void UpdateMovement(int p)
        {
            if ((this.players[p].Jump < this.powers["maxJumps"] && ((this.players[p].currentGamePadState.Buttons.A == ButtonState.Pressed && this.players[p].previousGamePadState.Buttons.A == null) || this.players[p].currentKeyboardState.IsKeyDown(Keys.Space)) && !this.players[p].previousKeyboardState.IsKeyDown(Keys.Space)) || (!this.unPauseGame && this.players[p].Jump == 0 && this.players[p].currentGamePadState.Buttons.A == ButtonState.Pressed))
            {
                this.players[p].Jump++;
                this.players[p].VelocityUp = this.jumpVelocity;
                this.players[p].VelocityDown = 0;
            }
            if (!this.players[p].Editing && this.players[p].Delay <= 0 && this.players[p].Deleting == 0)
            {
                if (this.players[p].currentGamePadState.ThumbSticks.Left.X < 0f || this.players[p].currentKeyboardState.IsKeyDown(Keys.A))
                {
                    if (this.players[p].Position.X + (float)(this.players[p].Width / 2) > (float)(this.players[p].View.Width / 2 - this.players[p].WalkingSpeed * 2) && this.players[p].Position.X + (float)(this.players[p].Width / 2) < (float)(this.players[p].View.Width / 2 + this.players[p].WalkingSpeed * 2) && this.background[p].X < -1)
                    {
                        if (this.players[p].currentKeyboardState.IsKeyDown(Keys.A))
                        {
                            this.background[p] = new Rectangle(this.background[p].X - (int)(-1 * (float)this.players[p].WalkingSpeed), this.background[p].Y, this.background[p].Width, this.background[p].Height);
                        }
                        else
                        {
                            this.background[p] = new Rectangle(this.background[p].X - (int)(this.players[p].currentGamePadState.ThumbSticks.Left.X * (float)this.players[p].WalkingSpeed), this.background[p].Y, this.background[p].Width, this.background[p].Height);
                        }
                        this.players[p].Scrolling = true;
                    }
                    else
                    {
                        Player player = this.players[p];
                        if (this.players[p].currentKeyboardState.IsKeyDown(Keys.A))
                        {
                            player.Position.X = player.Position.X + -1 * (float)this.players[p].WalkingSpeed;
                        }
                        else
                        {
                            player.Position.X = player.Position.X + this.players[p].currentGamePadState.ThumbSticks.Left.X * (float)this.players[p].WalkingSpeed;
                        }
                        this.players[p].Scrolling = false;
                    }
                }
                else if (this.players[p].currentGamePadState.ThumbSticks.Left.X > 0f || this.players[p].currentKeyboardState.IsKeyDown(Keys.D))
                {
                    if (this.players[p].Position.X + (float)(this.players[p].Width / 2) > (float)(this.players[p].View.Width / 2 - this.players[p].WalkingSpeed * 2) && this.players[p].Position.X + (float)(this.players[p].Width / 2) < (float)(this.players[p].View.Width / 2 + this.players[p].WalkingSpeed * 2) && this.background[p].X + this.background[p].Width > this.players[p].View.Width + 1)
                    {
                        if (this.players[p].currentKeyboardState.IsKeyDown(Keys.D))
                        {
                            this.background[p] = new Rectangle(this.background[p].X - (int)(1 * (float)this.players[p].WalkingSpeed), this.background[p].Y, this.background[p].Width, this.background[p].Height);
                        }
                        else
                        {
                            this.background[p] = new Rectangle(this.background[p].X - (int)(this.players[p].currentGamePadState.ThumbSticks.Left.X * (float)this.players[p].WalkingSpeed), this.background[p].Y, this.background[p].Width, this.background[p].Height);
                        }
                        this.players[p].Scrolling = true;
                    }
                    else
                    {
                        Player player2 = this.players[p];
                        if (this.players[p].currentKeyboardState.IsKeyDown(Keys.D))
                        {
                            player2.Position.X = player2.Position.X + 1 * (float)this.players[p].WalkingSpeed;
                        }
                        else
                        {
                            player2.Position.X = player2.Position.X + this.players[p].currentGamePadState.ThumbSticks.Left.X * (float)this.players[p].WalkingSpeed;
                        }
                        this.players[p].Scrolling = false;
                    }
                }
                if (this.players[p].Ghost)
                {
                    if (this.players[p].currentGamePadState.ThumbSticks.Left.Y > 0f)
                    {
                        this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y + (int)(this.players[p].currentGamePadState.ThumbSticks.Left.Y * (float)this.players[p].WalkingSpeed), this.background[p].Width, this.background[p].Height);
                    }
                    else if (this.players[p].currentGamePadState.ThumbSticks.Left.Y < 0f && this.players[p].Position.Y + (float)this.players[p].Height < (float)this.solidGround[p].Y)
                    {
                        this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y + (int)(this.players[p].currentGamePadState.ThumbSticks.Left.Y * (float)this.players[p].WalkingSpeed), this.background[p].Width, this.background[p].Height);
                    }
                }
            }
            this.players[p].HitBox = new Rectangle((int)this.players[p].Position.X + 16, (int)this.players[p].Position.Y + 16, 56, 52);
            this.RemoveBlocks(p);
            if (this.players[p].Deleting == 2)
            {
                this.players[p].Deleting = 0;
            }
            this.jumpHit = (this.fallHit = false);
            if (!this.players[p].Ghost)
            {
                for (int i = 0; i < this.blocks[p].Count; i++)
                {
                    this.players[p].HitBox = new Rectangle((int)this.players[p].Position.X + 16, (int)this.players[p].Position.Y + 16, 56, 52);
                    this.blocks[p][i].Update(this.background[p]);
                    if (this.players[p].HitBox.Intersects(new Rectangle((int)this.blocks[p][i].Position.X, (int)this.blocks[p][i].Position.Y, this.blockTexture.Width, this.blockTexture.Height)))
                    {
                        if ((float)(this.players[p].HitBox.Y + this.players[p].HitBox.Height) > this.blocks[p][i].Position.Y && (float)(this.players[p].HitBox.Y + this.players[p].HitBox.Height) < this.blocks[p][i].Position.Y + (float)this.blocks[p][i].Height && (float)(this.players[p].HitBox.X + this.players[p].HitBox.Width) > this.blocks[p][i].Position.X + (float)this.collisionPadding && (float)this.players[p].HitBox.X < this.blocks[p][i].Position.X + (float)this.blocks[p][i].Width - (float)this.collisionPadding)
                        {
                            this.fallHit = true;
                            this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y + (int)((float)(this.players[p].HitBox.Y + this.players[p].HitBox.Height) - this.blocks[p][i].Position.Y) - 1, this.background[p].Width, this.background[p].Height);
                            if (this.players[p].VelocityUp < 1)
                            {
                                this.players[p].Jump = 0;
                                this.players[p].VelocityDown = 0;
                            }
                            if (!this.tutorials.Contains(3))
                            {
                                this.tutorials.Add(3);
                            }
                        }
                        else if ((float)this.players[p].HitBox.Y < this.blocks[p][i].Position.Y + (float)this.blocks[p][i].Height && (float)this.players[p].HitBox.Y > this.blocks[p][i].Position.Y && (float)(this.players[p].HitBox.X + this.players[p].HitBox.Width) > this.blocks[p][i].Position.X + (float)this.collisionPadding && (float)this.players[p].HitBox.X < this.blocks[p][i].Position.X + (float)this.blocks[p][i].Width - (float)this.collisionPadding)
                        {
                            this.jumpHit = true;
                            this.players[p].VelocityUp = 0;
                            if (!this.fallHit && this.players[p].HitBox.Y < this.solidGround[p].Y - this.players[p].HitBox.Height)
                            {
                                this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y - (int)(this.blocks[p][i].Position.Y + (float)this.blocks[p][i].Height - (float)this.players[p].HitBox.Y), this.background[p].Width, this.background[p].Height);
                            }
                        }
                        else if ((float)(this.players[p].HitBox.X + this.players[p].HitBox.Width) > this.blocks[p][i].Position.X && (float)this.players[p].HitBox.X < this.blocks[p][i].Position.X && (float)(this.players[p].HitBox.Y + this.players[p].HitBox.Height) > this.blocks[p][i].Position.Y + (float)this.collisionPadding && (float)this.players[p].HitBox.Y < this.blocks[p][i].Position.Y + (float)this.blocks[p][i].Height - (float)this.collisionPadding)
                        {
                            if (this.players[p].Scrolling)
                            {
                                this.background[p] = new Rectangle(this.background[p].X + (int)((float)(this.players[p].HitBox.X + this.players[p].HitBox.Width) - this.blocks[p][i].Position.X), this.background[p].Y, this.background[p].Width, this.background[p].Height);
                            }
                            else
                            {
                                this.players[p].Position.X = this.players[p].Position.X - ((float)(this.players[p].HitBox.X + this.players[p].HitBox.Width) - this.blocks[p][i].Position.X);
                            }
                        }
                        else if ((float)this.players[p].HitBox.X < this.blocks[p][i].Position.X + (float)this.blocks[p][i].Width && (float)this.players[p].HitBox.X > this.blocks[p][i].Position.X && (float)(this.players[p].HitBox.Y + this.players[p].HitBox.Height) > this.blocks[p][i].Position.Y + (float)this.collisionPadding && (float)this.players[p].HitBox.Y < this.blocks[p][i].Position.Y + (float)this.blocks[p][i].Height - (float)this.collisionPadding)
                        {
                            if (this.players[p].Scrolling)
                            {
                                this.background[p] = new Rectangle(this.background[p].X - (int)(this.blocks[p][i].Position.X + (float)this.blocks[p][i].Width - (float)this.players[p].HitBox.X), this.background[p].Y, this.background[p].Width, this.background[p].Height);
                            }
                            else
                            {
                                this.players[p].Position.X = this.blocks[p][i].Position.X + (float)this.blocks[p][i].Width - ((float)this.players[p].HitBox.X - this.players[p].Position.X);
                            }
                        }
                    }
                }
                if (this.players[p].VelocityUp > 0 && !this.jumpHit)
                {
                    this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y + this.players[p].VelocityUp, this.background[p].Width, this.background[p].Height);
                    this.players[p].VelocityUp--;
                }
                else if (this.players[p].VelocityUp < 1 && this.players[p].HitBox.Y + this.players[p].HitBox.Height < this.solidGround[p].Y && !this.fallHit)
                {
                    this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y - this.players[p].VelocityDown, this.background[p].Width, this.background[p].Height);
                    if (this.players[p].VelocityDown < this.fallVelocity)
                    {
                        this.players[p].VelocityDown++;
                    }
                    if (this.players[p].Jump < 1)
                    {
                        this.players[p].Jump = 1;
                    }
                }
            }
            else if (this.players[p].HitBox.Intersects(this.sun[p]) && this.players[p].currentGamePadState.ThumbSticks.Left.Y > 0f)
            {
                this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y - (int)(this.players[p].currentGamePadState.ThumbSticks.Left.Y * (float)this.players[p].WalkingSpeed), this.background[p].Width, this.background[p].Height);
            }
            if (this.players[p].VelocityUp < 1 && this.players[p].Position.Y + (float)this.players[p].Height >= (float)this.solidGround[p].Y)
            {
                this.background[p] = new Rectangle(this.background[p].X, this.background[p].Y + (int)(this.players[p].Position.Y + (float)this.players[p].Height - (float)this.solidGround[p].Y), this.background[p].Width, this.background[p].Height);
                this.players[p].Jump = 0;
                this.players[p].VelocityDown = 0;
            }
            this.solidGround[p] = new Rectangle(0, this.background[p].Y + this.background[p].Height, this.screenWidth, this.screenHeight);
            this.trucks[p][0] = new Rectangle(this.background[p].X + this.safe.X, this.solidGround[p].Y - this.truckTexture1.Height + 3, this.truckTexture1.Width, this.truckTexture1.Height);
            this.trucks[p][1] = new Rectangle(this.background[p].X + this.background[p].Width - this.truckTexture2.Width - this.safe.X, this.solidGround[p].Y - this.truckTexture2.Height + 3, this.truckTexture2.Width, this.truckTexture2.Height);
            this.sun[p] = new Rectangle(this.background[p].X, this.background[p].Y - this.sun[p].Height, this.sun[p].Width, this.sun[p].Height);
            if (this.background[p].X > 0)
            {
                this.background[p] = new Rectangle(0, this.background[p].Y, this.background[p].Width, this.background[p].Height);
            }
            else if (this.background[p].X + this.background[p].Width < this.players[p].View.Width)
            {
                this.background[p] = new Rectangle(this.background[p].X + (this.players[p].View.Width - (this.background[p].X + this.background[p].Width)), this.background[p].Y, this.background[p].Width, this.background[p].Height);
            }
            for (int i = 0; i < this.blocks[p].Count; i++)
            {
                this.blocks[p][i].Update(this.background[p]);
            }
            for (int i = 0; i < this.ribbons[p].Count; i++)
            {
                this.ribbons[p][i].Update(this.background[p]);
                this.CheckRibbons(p, i);
            }
            this.players[p].Position.X = MathHelper.Clamp(this.players[p].Position.X, -16f, (float)(this.players[p].View.Width - this.players[p].Width + 28));
        }

        private void RemoveBlocks(int p)
        {
            if (this.players[p].Deleting == 2)
            {
                for (int i = 0; i < this.blocks[p].Count; i++)
                {
                    this.bRectangle = new Rectangle((int)this.blocks[p][i].Position.X, (int)this.blocks[p][i].Position.Y, this.blockTexture.Width, this.blockTexture.Height);
                    if (this.bRectangle.Intersects(this.eRectangle)) //&& (this.players[p].currentGamePadState.ThumbSticks.Left.X != 0f || this.players[p].currentGamePadState.ThumbSticks.Left.Y != 0f))
                    {
                        this.blocksToRemove[0].Add(this.blocks[0][i]);
                        if (this.players.Count > 1)
                        {
                            this.blocksToRemove[1].Add(this.blocks[1][i]);
                        }
                        if (!this.tutorials.Contains(5))
                        {
                            this.tutorials.Add(5);
                        }
                    }
                }
                if (this.blocksToRemove[0].Count > 0)
                {
                    this.sfxBreaks[this.randy.Next(0, this.sfxBreaks.Count)].Play();
                }
                this.blockParticleLimit = 0;
                for (int i = 0; i < this.blocksToRemove[p].Count; i++)
                {
                    this.blocks[0].Remove(this.blocksToRemove[0][i]);
                    if (this.blockParticleLimit < 40)
                    {
                        this.blockParticles[0].Create(this.background[0], new Vector2(this.blocksToRemove[0][i].PositionX + (float)(this.blocksToRemove[0][i].Width / 2), this.blocksToRemove[0][i].PositionY + (float)(this.blocksToRemove[0][i].Height / 2)), this.blocksToRemove[0][i].Color, 20, 5f, 50);
                    }
                    if (this.players.Count > 1)
                    {
                        this.blocks[1].Remove(this.blocksToRemove[1][i]);
                        if (this.blockParticleLimit < 40)
                        {
                            this.blockParticles[1].Create(this.background[1], new Vector2(this.blocksToRemove[0][i].PositionX + (float)(this.blocksToRemove[0][i].Width / 2), this.blocksToRemove[0][i].PositionY + (float)(this.blocksToRemove[0][i].Height / 2)), this.blocksToRemove[0][i].Color, 20, 5f, 50);
                        }
                    }
                    this.blockParticleLimit++;
                }
                this.players[p].Delay = this.delayAmount;
            }
        }

        private void CheckRibbons(int p, int i)
        {
            if (this.players[p].HitBox.Intersects(this.ribbons[p][i].HitBox))
            {
                for (int j = this.screenWidth / 20; j <= this.screenWidth; j += this.screenWidth / 20)
                {
                    this.blockParticles[0].Create(this.background[0], new Vector2((float)j, this.ribbons[0][i].PositionY + (float)(this.ribbons[0][i].Height / 2)), Color.White, 12, 5f, 150);
                    this.blockParticles[0].Create(this.background[0], new Vector2((float)j, this.ribbons[0][i].PositionY + (float)(this.ribbons[0][i].Height / 2)), Color.Black, 12, 5f, 150);
                    if (this.players.Count == 2)
                    {
                        this.blockParticles[1].Create(this.background[1], new Vector2((float)j, this.ribbons[1][i].PositionY + (float)(this.ribbons[1][i].Height / 2)), Color.White, 12, 5f, 150);
                        this.blockParticles[1].Create(this.background[1], new Vector2((float)j, this.ribbons[1][i].PositionY + (float)(this.ribbons[1][i].Height / 2)), Color.Black, 12, 5f, 150);
                    }
                }
                this.powers[this.ribbons[p][i].Effect] = this.ribbons[p][i].EffectValue;
                string text = "";
                this.message1 = new Text();
                if (this.ribbons[p][i].Effect == "maxCarry")
                {
                    text = "CARRY LIMIT INCREASED TO " + this.ribbons[p][i].EffectValue + "!";
                    this.message2 = new Text();
                    this.message2.Initialize(this.numFont, new Vector2((float)(this.players[p].View.X + this.players[p].HitBox.X), this.players[p].Position.Y - 10f), "+" + (this.powers["maxCarry"] - this.players[p].Carrying), 6);
                    this.messages.Add(this.message2);
                    this.players[p].Carrying = this.powers["maxCarry"];
                }
                else if (this.ribbons[p][i].Effect == "hatRange")
                {
                    text = "NEW HAT UNLOCKED!";
                }
                else if (this.ribbons[p][i].Effect == "colorRange")
                {
                    text = "NEW COLORS UNLOCKED!";
                }
                else if (this.ribbons[p][i].Effect == "editRadius")
                {
                    text = "BUILDING RADIUS INCREASED!";
                }
                else if (this.ribbons[p][i].Effect == "maxJumps")
                {
                    text = "DOUBLE JUMP UNLOCKED!";
                }
                this.message1.Initialize(this.numFont, new Vector2((float)(this.screenWidth / 2) - this.numFont.MeasureString(text).X / 2f, (float)(this.screenHeight / 3)), text, 1);
                this.messages.Add(this.message1);
                this.ribbons[0].Remove(this.ribbons[0][i]);
                if (this.players.Count == 2)
                {
                    this.ribbons[1].Remove(this.ribbons[1][i]);
                }
                //this.sfxRibbonBreak.Play();
                if (!this.tutorials.Contains(6))
                {
                    this.tutorials.Add(6);
                }
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (this.screen == 5)
            {
                this.DrawGameplay();
            }
            else if (this.screen < 5)
            {
                this.DrawMenus();
            }
            base.Draw(gameTime);
        }

        private void DrawMenus()
        {
            base.GraphicsDevice.Clear(new Color(147, 223, 255));
            this.spriteBatch.Begin();
            if (this.screen == 1)
            {
                this.spriteBatch.Draw(this.screen1, Vector2.Zero, Color.White);
                this.spriteBatch.Draw(this.titleArt, new Vector2((float)(this.safe.X + 420), (float)(this.safe.Y + 135)), Color.White);
            }
            else if (this.screen == 2)
            {
                if (this.screenPosition.Y > 0f)
                {
                    this.screenPosition.Y = this.screenPosition.Y - 40f;
                    this.spriteBatch.Draw(this.screen1, new Vector2(this.screenPosition.X, this.screenPosition.Y - 720f), Color.White);
                    this.spriteBatch.Draw(this.screen2, this.screenPosition, Color.White);
                }
                else if (this.screenPosition.X < 0f)
                {
                    this.screenPosition.X = this.screenPosition.X + 40f;
                    this.spriteBatch.Draw(this.screen2, this.screenPosition, Color.White);
                    this.spriteBatch.Draw(this.screen3, new Vector2(this.screenPosition.X + 1280f, this.screenPosition.Y), Color.White);
                }
                else if (this.screenPosition.X > 0f)
                {
                    this.screenPosition.X = this.screenPosition.X - 40f;
                    this.spriteBatch.Draw(this.screen2, this.screenPosition, Color.White);
                    this.spriteBatch.Draw(this.screen4, new Vector2(this.screenPosition.X - 1280f, this.screenPosition.Y), Color.White);
                }
                else
                {
                    this.spriteBatch.Draw(this.screen2, Vector2.Zero, Color.White);
                    if (this.menuItem == 1)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(770f, (float)(this.safe.Y + 106)), Color.White);
                    }
                    else if (this.menuItem == 2)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(770f, (float)(this.safe.Y + 156)), Color.Pink);
                    }
                    else if (this.menuItem == 3)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(770f, (float)(this.safe.Y + 206 + this.loadSlotPadding)), Color.Yellow);
                    }
                    else if (this.menuItem == 4)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(770f, (float)(this.safe.Y + 256 + this.loadSlotPadding)), Color.Cyan);
                    }
                    if (this.loadSlot == 1)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(805f, (float)(this.safe.Y + 186)), Color.Purple);
                    }
                    else if (this.loadSlot == 2)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(805f, (float)(this.safe.Y + 216)), Color.Purple);
                    }
                    else if (this.loadSlot == 3)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2(805f, (float)(this.safe.Y + 246)), Color.Purple);
                    }
                    this.spriteBatch.DrawString(this.tutorialFont, "Start a New Game", new Vector2(800f, (float)(this.safe.Y + 90)), Color.Black);
                    if (false) //Guide.IsTrialMode
                    {
                        this.spriteBatch.DrawString(this.tutorialFont, "Buy Full Game", new Vector2(800f, (float)(this.safe.Y + 150)), Color.Black);
                    }
                    else
                    {
                        this.spriteBatch.DrawString(this.tutorialFont, "Load Game", new Vector2(800f, (float)(this.safe.Y + 140)), Color.Black);
                        if (this.loadSlot > 0)
                        {
                            this.spriteBatch.DrawString(this.tutorialFont, "Slot 1: " + this.saveFiles[1].SlotMessage, new Vector2(830f, (float)(this.safe.Y + 170)), Color.Purple);
                            this.spriteBatch.DrawString(this.tutorialFont, "Slot 2: " + this.saveFiles[2].SlotMessage, new Vector2(830f, (float)(this.safe.Y + 200)), Color.Purple);
                            this.spriteBatch.DrawString(this.tutorialFont, "Slot 3: " + this.saveFiles[3].SlotMessage, new Vector2(830f, (float)(this.safe.Y + 230)), Color.Purple);
                        }
                    }
                    this.spriteBatch.DrawString(this.tutorialFont, "More Information", new Vector2(800f, (float)(this.safe.Y + 190 + this.loadSlotPadding)), Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "Return to Dashboard", new Vector2(800f, (float)(this.safe.Y + 240 + this.loadSlotPadding)), Color.Black);
                }
            }
            else if (this.screen == 3)
            {
                if (this.screenPosition.X > -1280f)
                {
                    this.screenPosition.X = this.screenPosition.X - 40f;
                    this.spriteBatch.Draw(this.screen2, this.screenPosition, Color.White);
                    this.spriteBatch.Draw(this.screen3, new Vector2(this.screenPosition.X + 1280f, this.screenPosition.Y), Color.White);
                }
                else
                {
                    this.spriteBatch.Draw(this.screen3, Vector2.Zero, Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Select a Craftimal", new Vector2((float)(this.screenWidth / 2) - this.tutorialFont.MeasureString("Select a Craftimal").X / 2f, (float)(this.safe.Y + 50)), Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "Then Press            to Start Game", new Vector2((float)(this.screenWidth / 2) - this.tutorialFont.MeasureString("Then Press         to Start Game").X / 2f, (float)(this.safe.Y + 90)), Color.Black);
                    this.spriteBatch.Draw(this.buttonStart, new Vector2((float)(this.screenWidth / 2 - this.buttonStart.Width / 2), (float)(this.safe.Y + 90)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.8f, 0, 0f);
                    if (this.players[0].Ghost)
                    {
                        this.spriteBatch.Draw(this.animals[this.players[0].AnimalIndex].GhostSpriteRight, new Vector2((float)(this.screenWidth / 2 - 200), (float)(this.safe.Y + 260)), Color.White);
                    }
                    else
                    {
                        this.spriteBatch.Draw(this.animals[this.players[0].AnimalIndex].NormalSpriteRight, new Vector2((float)(this.screenWidth / 2 - 200), (float)(this.safe.Y + 260)), Color.White);
                    }
                    this.spriteBatch.Draw(this.arrowLeft, new Vector2((float)(this.screenWidth / 2 - 200 - this.arrowLeft.Width * 2), (float)(this.safe.Y + 260 + this.animalTexture1.Height / 2)), Color.White);
                    this.spriteBatch.Draw(this.arrowRight, new Vector2((float)(this.screenWidth / 2 - 200 + 105), (float)(this.safe.Y + 260 + this.animalTexture1.Height / 2)), Color.White);
                    if (this.players.Count > 1)
                    {
                        if (this.players[1].Ghost)
                        {
                            this.spriteBatch.Draw(this.animals[this.players[1].AnimalIndex].GhostSpriteRight, new Vector2((float)(this.screenWidth / 2 + 200 - this.animalTexture1.Width), (float)(this.safe.Y + 260)), Color.White);
                        }
                        else
                        {
                            this.spriteBatch.Draw(this.animals[this.players[1].AnimalIndex].NormalSpriteRight, new Vector2((float)(this.screenWidth / 2 + 200 - this.animalTexture1.Width), (float)(this.safe.Y + 260)), Color.White);
                        }
                        this.spriteBatch.Draw(this.arrowLeft, new Vector2((float)(this.screenWidth / 2 + 200 - this.animalTexture1.Width - this.arrowLeft.Width * 2), (float)(this.safe.Y + 260 + this.animalTexture1.Height / 2)), Color.White);
                        this.spriteBatch.Draw(this.arrowRight, new Vector2((float)(this.screenWidth / 2 + 200 - this.animalTexture1.Width + 105), (float)(this.safe.Y + 260 + this.animalTexture1.Height / 2)), Color.White);
                    }
                    else
                    {
                        this.spriteBatch.Draw(this.buttonA, new Vector2((float)(this.screenWidth / 2 + 200 - this.buttonA.Width - this.arrowLeft.Width * 2), (float)(this.safe.Y + 280)), Color.White);
                        this.spriteBatch.DrawString(this.tutorialFont, "Player 2 Join!", new Vector2((float)(this.screenWidth / 2 + 200 - this.buttonA.Width / 2) - this.tutorialFont.MeasureString("Player 2 Join!").X / 2f - (float)(this.arrowLeft.Width * 2), (float)(this.safe.Y + 235)), Color.Black);
                    }
                    if (this.touchedSun)
                    {
                        this.spriteBatch.Draw(this.buttonX, new Vector2((float)(this.safe.X + 110), (float)(this.safe.Y + 452)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.9f, 0, 0f);
                        this.spriteBatch.DrawString(this.tutorialFont, "Toggle Ghost Mode!", new Vector2((float)(this.safe.X + 113 + this.buttonX.Width), (float)(this.safe.Y + 458)), Color.Black);
                    }
                }
            }
            else if (this.screen == 4)
            {
                if (this.screenPosition.X < 1280f)
                {
                    this.screenPosition.X = this.screenPosition.X + 40f;
                    this.spriteBatch.Draw(this.screen2, this.screenPosition, Color.White);
                    this.spriteBatch.Draw(this.screen4, new Vector2(this.screenPosition.X - 1280f, this.screenPosition.Y), Color.White);
                }
                else
                {
                    this.spriteBatch.Draw(this.screen4, Vector2.Zero, Color.White);
                    this.spriteBatch.Draw(this.boxBubBlock, new Vector2((float)(this.safe.X + 100), (float)(this.safe.Y + 50)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Roppy Chop Studios is a small, independent developer", new Vector2((float)(this.safe.X + this.boxBubBlock.Width + 110), (float)(this.safe.Y + 50)), Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "whose first game, a 4-player battle called Bub Block,", new Vector2((float)(this.safe.X + this.boxBubBlock.Width + 110), (float)(this.safe.Y + 75)), Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "debuted on the Xbox indie channel in November 2011.", new Vector2((float)(this.safe.X + this.boxBubBlock.Width + 110), (float)(this.safe.Y + 100)), Color.Black);
                    this.spriteBatch.Draw(this.boxUnderground, new Vector2((float)(this.safe.X + 100), (float)(this.safe.Y + 250)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Boost Power! is the composer behind Craftimals and has", new Vector2((float)(this.safe.X + this.boxBubBlock.Width + 110), (float)(this.safe.Y + 250)), Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "two other albums to his credit: the Bub Block soundtrack", new Vector2((float)(this.safe.X + this.boxBubBlock.Width + 110), (float)(this.safe.Y + 275)), Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "and the electronic compilation Going Underground.", new Vector2((float)(this.safe.X + this.boxBubBlock.Width + 110), (float)(this.safe.Y + 300)), Color.Black);
                }
            }
            if (this.fadeIn >= 0)
            {
                this.fadeIn -= 5;
            }
            this.spriteBatch.Draw(this.pixel, new Rectangle(0, 0, this.screenWidth, this.screenHeight), new Color(0, 0, 0, this.fadeIn));
            this.spriteBatch.End();
        }

        private void DrawGameplay()
        {
            base.GraphicsDevice.Clear(Color.Black);
            for (int i = 0; i < this.players.Count; i++)
            {
                base.GraphicsDevice.Viewport = this.players[i].View;
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(this.bgGradient, this.background[i], Color.White);
                for (int j = 0; j < this.stars[i].Count; j++)
                {
                    this.spriteBatch.Draw(this.pixel, new Rectangle(this.background[i].X + (int)this.stars[i][j].X, this.background[i].Y + (int)this.stars[i][j].Y, 2, 2), new Color(230, 230, 230));
                }
                for (int j = 0; j < this.clouds[i].Count; j++)
                {
                    this.spriteBatch.Draw(this.cloudTexture, new Rectangle(this.background[i].X + this.clouds[i][j].X, this.background[i].Y + this.clouds[i][j].Y, this.clouds[i][j].Width, this.clouds[i][j].Height), Color.White);
                }
                this.spriteBatch.Draw(this.groundTexture, new Rectangle(this.background[i].X, this.solidGround[i].Y - 40, this.screenWidth, this.groundTexture.Height), Color.White);
                this.spriteBatch.Draw(this.truckTexture1, this.trucks[i][0], Color.White);
                this.spriteBatch.Draw(this.truckTexture2, this.trucks[i][1], Color.White);
                this.spriteBatch.Draw(this.sunTexture, this.sun[i], Color.White);
                for (int j = 0; j < this.ribbons[i].Count; j++)
                {
                    this.ribbons[i][j].Draw(this.spriteBatch);
                }
                for (int j = 0; j < this.blocks[i].Count; j++)
                {
                    this.blocks[i][j].Draw(this.spriteBatch);
                }
                this.blockParticles[i].Draw(this.spriteBatch);
                for (int j = 0; j < this.players.Count; j++)
                {
                    if (i != j)
                    {
                        this.tempY = (int)((float)this.solidGround[j].Y - this.players[j].Position.Y);
                        this.tempY = this.solidGround[i].Y - this.tempY;
                        this.tempX = (int)(this.players[j].Position.X - (float)this.background[j].X);
                        this.tempX = this.background[i].X + this.tempX;
                        this.players[j].Draw(this.spriteBatch, new Vector2((float)this.tempX, (float)this.tempY), this.parachuteTexture);
                    }
                }
                this.players[i].Draw(this.spriteBatch, this.players[i].Position, this.parachuteTexture);
                if (this.gameOvers[i].Step > 0)
                {
                    this.gameOvers[i].Draw(this.spriteBatch);
                }
                else
                {
                    if (this.players[i].Editing)
                    {
                        this.spriteBatch.Draw(this.editBlockTexture, new Rectangle((int)this.editBox[i].X, (int)this.editBox[i].Y, this.blockTexture.Width, this.blockTexture.Height), this.blockColors[this.players[i].ColorIndex]);
                    }
                    else if (this.players[i].Deleting > 0)
                    {
                        this.spriteBatch.Draw(this.delBlockTexture, new Rectangle((int)this.editBox[i].X, (int)this.editBox[i].Y, this.blockTexture.Width, this.blockTexture.Height), Color.White);
                    }
                    else if (this.players[i].Carrying > 0)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Rectangle((int)(this.players[i].Position.X + 37f), (int)(this.players[i].Position.Y - (float)this.editPreviewTexture.Height - (float)this.hatOffset[this.players[i].HatIndex]), this.editPreviewTexture.Width, this.editPreviewTexture.Height), this.blockColors[this.players[i].ColorIndex]);
                    }
                    this.spriteBatch.DrawString(this.tutorialFont, "height: " + Convert.ToInt32((float)(this.background[i].Y + 99460) / 19.99f), new Vector2((float)this.safe.X, (float)this.safe.Y), Color.White);
                }
                this.spriteBatch.End();
            }
            base.GraphicsDevice.Viewport = this.defaultViewport;
            this.spriteBatch.Begin();
            for (int i = 0; i < this.messages.Count; i++)
            {
                if (!this.gamePaused)
                {
                    this.messages[i].Update();
                }
                this.messages[i].Draw(this.spriteBatch);
                if (this.messages[i].Opacity <= 0)
                {
                    this.messages.RemoveAt(i);
                    i--;
                }
            }
            if (!this.tutorials.Contains(1))
            {
                this.tutorialText = "touch the wheelbarrow to pick up blocks.";
                this.DrawTutorial(this.tutorialText);
            }
            else if (!this.tutorials.Contains(2))
            {
                this.tutorialText = "hold     and move left stick      then let go of     to place a block.";
                this.DrawTutorial(this.tutorialText);
                this.spriteBatch.Draw(this.buttonX, new Vector2((float)(this.tutorialFront.X + 49), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonLS, new Vector2((float)(this.tutorialFront.X + 252), (float)(this.tutorialFront.Y + 7)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonX, new Vector2((float)(this.tutorialFront.X + 413), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
            }
            else if (!this.tutorials.Contains(3))
            {
                this.tutorialText = "press     to jump on top of a block.";
                this.DrawTutorial(this.tutorialText);
                this.spriteBatch.Draw(this.buttonA, new Vector2((float)(this.tutorialFront.X + 53), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
            }
            else if (!this.tutorials.Contains(4))
            {
                this.tutorialText = "press left trigger     or right     while carrying a block to change its color.";
                this.DrawTutorial(this.tutorialText);
                this.spriteBatch.Draw(this.buttonLT, new Vector2((float)(this.tutorialFront.X + 157), (float)(this.tutorialFront.Y + 2)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.4f, 0, 0f);
                this.spriteBatch.Draw(this.buttonRT, new Vector2((float)(this.tutorialFront.X + 256), (float)(this.tutorialFront.Y + 2)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.4f, 0, 0f);
            }
            else if (!this.tutorials.Contains(5))
            {
                this.tutorialText = "hold     and move left stick      then let go of     to delete a block.";
                this.DrawTutorial(this.tutorialText);
                this.spriteBatch.Draw(this.buttonB, new Vector2((float)(this.tutorialFront.X + 49), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonLS, new Vector2((float)(this.tutorialFront.X + 252), (float)(this.tutorialFront.Y + 7)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonB, new Vector2((float)(this.tutorialFront.X + 413), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
            }
            else if (!this.tutorials.Contains(6))
            {
                this.tutorialText = "now build steps to the first checkpoint!";
                this.DrawTutorial(this.tutorialText);
            }
            else if (!this.tutorials.Contains(7) && this.powers["hatRange"] > 0)
            {
                this.tutorialText = "press left bumper           or right           to change your hat.";
                this.DrawTutorial(this.tutorialText);
                this.spriteBatch.Draw(this.buttonLB, new Vector2((float)(this.tutorialFront.X + 166), (float)(this.tutorialFront.Y + 11)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonRB, new Vector2((float)(this.tutorialFront.X + 305), (float)(this.tutorialFront.Y + 11)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
            }
            else if (!this.tutorials.Contains(8) && this.powers["editRadius"] > 0)
            {
                this.tutorialText = "click the left stick      while holding     or     to change build radius.";
                this.DrawTutorial(this.tutorialText);
                this.spriteBatch.Draw(this.buttonLS, new Vector2((float)(this.tutorialFront.X + 168), (float)(this.tutorialFront.Y + 7)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonX, new Vector2((float)(this.tutorialFront.X + 330), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
                this.spriteBatch.Draw(this.buttonB, new Vector2((float)(this.tutorialFront.X + 380), (float)(this.tutorialFront.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.5f, 0, 0f);
            }
            if (this.fadeIn >= 0 && !this.gamePaused)
            {
                this.fadeIn -= 5;
            }
            this.spriteBatch.Draw(this.pixel, new Rectangle(0, 0, this.screenWidth, this.screenHeight), new Color(0, 0, 0, this.fadeIn));
            if (this.gamePaused)
            {
                this.spriteBatch.Draw(this.pixel, new Rectangle(0, 0, this.screenWidth, this.screenHeight), new Color(0, 0, 0, 100));
                this.pauseBack = new Rectangle(this.screenWidth / 2 - 150, this.screenHeight / 2 - 105, 300, 210);
                this.pauseFront = new Rectangle(this.pauseBack.X + 2, this.pauseBack.Y + 2, this.pauseBack.Width - 4, this.pauseBack.Height - 4);
                this.spriteBatch.Draw(this.pixel, this.pauseBack, Color.White);
                this.spriteBatch.Draw(this.pixel, this.pauseFront, Color.Black);
                if (this.pauseItem == 1)
                {
                    this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(this.pauseFront.X + 20), (float)(this.pauseFront.Y + 17)), Color.White);
                }
                else if (this.pauseItem == 2)
                {
                    this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(this.pauseFront.X + 20), (float)(this.pauseFront.Y + 67)), Color.Pink);
                }
                else if (this.pauseItem == 3)
                {
                    this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(this.pauseFront.X + 20), (float)(this.pauseFront.Y + 117)), Color.Yellow);
                }
                else if (this.pauseItem == 4)
                {
                    this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(this.pauseFront.X + 20), (float)(this.pauseFront.Y + 167)), Color.Cyan);
                }
                this.spriteBatch.DrawString(this.tutorialFont, "Continue", new Vector2((float)(this.pauseFront.X + 50), (float)(this.pauseFront.Y + 10)), Color.White);
                if (false) //Guide.IsTrialMode
                {
                    this.spriteBatch.DrawString(this.tutorialFont, "Buy Full Game to Save", new Vector2((float)(this.pauseFront.X + 50), (float)(this.pauseFront.Y + 60)), Color.White);
                }
                else
                {
                    this.spriteBatch.DrawString(this.tutorialFont, "Save", new Vector2((float)(this.pauseFront.X + 50), (float)(this.pauseFront.Y + 60)), Color.White);
                }
                this.spriteBatch.DrawString(this.tutorialFont, "Help", new Vector2((float)(this.pauseFront.X + 50), (float)(this.pauseFront.Y + 110)), Color.White);
                this.spriteBatch.DrawString(this.tutorialFont, "Quit to Menu", new Vector2((float)(this.pauseFront.X + 50), (float)(this.pauseFront.Y + 160)), Color.White);
                if (this.pauseSave)
                {
                    Rectangle rectangle;
                    rectangle = new Rectangle(this.pauseBack.X + this.pauseBack.Width - 50, this.pauseBack.Y - 100, 300, 140);
                    Rectangle rectangle2;
                    rectangle2 = new Rectangle(rectangle.X + 2, rectangle.Y + 2, rectangle.Width - 4, rectangle.Height - 4);
                    this.spriteBatch.Draw(this.pixel, rectangle, Color.White);
                    this.spriteBatch.Draw(this.pixel, rectangle2, Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "Choose a save file:", new Vector2((float)(rectangle.X + 10), (float)(rectangle.Y + 10)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Slot 1: " + this.saveFiles[1].SlotMessage, new Vector2((float)(rectangle.X + 50), (float)(rectangle.Y + 40)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Slot 2: " + this.saveFiles[2].SlotMessage, new Vector2((float)(rectangle.X + 50), (float)(rectangle.Y + 70)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Slot 3: " + this.saveFiles[3].SlotMessage, new Vector2((float)(rectangle.X + 50), (float)(rectangle.Y + 100)), Color.White);
                    if (this.loadSlot <= 1)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(rectangle.X + 20), (float)(rectangle.Y + 47)), Color.Purple);
                    }
                    else if (this.loadSlot == 2)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(rectangle.X + 20), (float)(rectangle.Y + 77)), Color.Purple);
                    }
                    else if (this.loadSlot == 3)
                    {
                        this.spriteBatch.Draw(this.editPreviewTexture, new Vector2((float)(rectangle.X + 20), (float)(rectangle.Y + 107)), Color.Purple);
                    }
                    if (this.savedGame)
                    {
                        Rectangle rectangle3;
                        rectangle3 = new Rectangle(rectangle.X + rectangle.Width - 80, rectangle.Y + rectangle.Height - 2, 80, 45);
                        Rectangle rectangle4;
                        rectangle4 = new Rectangle(rectangle3.X + 2, rectangle3.Y + 2, rectangle3.Width - 4, rectangle3.Height - 4);
                        this.spriteBatch.Draw(this.pixel, rectangle3, Color.White);
                        this.spriteBatch.Draw(this.pixel, rectangle4, Color.Black);
                        this.spriteBatch.DrawString(this.tutorialFont, "Saved!", new Vector2((float)(rectangle3.X + 10), (float)(rectangle3.Y + 10)), Color.White);
                    }
                }
                else if (this.pauseHelp)
                {
                    Rectangle rectangle5;
                    rectangle5 = new Rectangle(this.pauseBack.X + this.pauseBack.Width - 50, this.pauseBack.Y - 150, 340, 250);
                    if (this.powers["hatRange"] > 0)
                    {
                        rectangle5 = new Rectangle(this.pauseBack.X + this.pauseBack.Width - 50, this.pauseBack.Y - 150, 350, 290);
                    }
                    Rectangle rectangle6;
                    rectangle6 = new Rectangle(rectangle5.X + 2, rectangle5.Y + 2, rectangle5.Width - 4, rectangle5.Height - 4);
                    this.spriteBatch.Draw(this.pixel, rectangle5, Color.White);
                    this.spriteBatch.Draw(this.pixel, rectangle6, Color.Black);
                    this.spriteBatch.Draw(this.buttonLS, new Vector2((float)(rectangle5.X + 10), (float)(rectangle5.Y + 10)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.75f, 0, 0f);
                    this.spriteBatch.DrawString(this.tutorialFont, "move", new Vector2((float)(rectangle5.X + 60), (float)(rectangle5.Y + 13)), Color.White);
                    if (this.powers["editRadius"] > 0)
                    {
                        this.spriteBatch.DrawString(this.tutorialFont, "(click to change radius)", new Vector2((float)(rectangle5.X + 118), (float)(rectangle5.Y + 13)), Color.White);
                    }
                    this.spriteBatch.Draw(this.buttonA, new Vector2((float)(rectangle5.X + 14), (float)(rectangle5.Y + 55)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.75f, 0, 0f);
                    this.spriteBatch.DrawString(this.tutorialFont, "jump", new Vector2((float)(rectangle5.X + 60), (float)(rectangle5.Y + 55)), Color.White);
                    if (this.powers["maxJumps"] > 1)
                    {
                        this.spriteBatch.DrawString(this.tutorialFont, "(x2 for double jump)", new Vector2((float)(rectangle5.X + 118), (float)(rectangle5.Y + 55)), Color.White);
                    }
                    else
                    {
                        this.spriteBatch.DrawString(this.tutorialFont, "(hold to bounce)", new Vector2((float)(rectangle5.X + 118), (float)(rectangle5.Y + 55)), Color.White);
                    }
                    this.spriteBatch.Draw(this.buttonX, new Vector2((float)(rectangle5.X + 14), (float)(rectangle5.Y + 95)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.75f, 0, 0f);
                    this.spriteBatch.DrawString(this.tutorialFont, "place block", new Vector2((float)(rectangle5.X + 60), (float)(rectangle5.Y + 95)), Color.White);
                    this.spriteBatch.Draw(this.buttonB, new Vector2((float)(rectangle5.X + 14), (float)(rectangle5.Y + 135)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.75f, 0, 0f);
                    this.spriteBatch.DrawString(this.tutorialFont, "delete block", new Vector2((float)(rectangle5.X + 60), (float)(rectangle5.Y + 135)), Color.White);
                    this.spriteBatch.Draw(this.buttonLT, new Vector2((float)(rectangle5.X + 14), (float)(rectangle5.Y + 175)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.6f, 0, 0f);
                    this.spriteBatch.Draw(this.buttonRT, new Vector2((float)(rectangle5.X + 54), (float)(rectangle5.Y + 175)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.6f, 0, 0f);
                    this.spriteBatch.DrawString(this.tutorialFont, "change color", new Vector2((float)(rectangle5.X + 95), (float)(rectangle5.Y + 190)), Color.White);
                    if (this.powers["hatRange"] > 0)
                    {
                        this.spriteBatch.Draw(this.buttonLB, new Vector2((float)(rectangle5.X + 10), (float)(rectangle5.Y + 248)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.6f, 0, 0f);
                        this.spriteBatch.Draw(this.buttonRB, new Vector2((float)(rectangle5.X + 80), (float)(rectangle5.Y + 248)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.6f, 0, 0f);
                        this.spriteBatch.DrawString(this.tutorialFont, "change hat", new Vector2((float)(rectangle5.X + 155), (float)(rectangle5.Y + 246)), Color.White);
                    }
                }
                else if (this.pauseQuit)
                {
                    Rectangle rectangle7;
                    rectangle7 = new Rectangle(this.pauseBack.X + this.pauseBack.Width - 50, this.pauseBack.Y + this.pauseBack.Height - 50, 320, 120);
                    Rectangle rectangle8;
                    rectangle8 = new Rectangle(rectangle7.X + 2, rectangle7.Y + 2, rectangle7.Width - 4, rectangle7.Height - 4);
                    this.spriteBatch.Draw(this.pixel, rectangle7, Color.White);
                    this.spriteBatch.Draw(this.pixel, rectangle8, Color.Black);
                    this.spriteBatch.DrawString(this.tutorialFont, "Make sure to save your game!", new Vector2((float)(rectangle7.X + 10), (float)(rectangle7.Y + 10)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Do you really want to quit?", new Vector2((float)(rectangle7.X + 10), (float)(rectangle7.Y + 40)), Color.White);
                    this.spriteBatch.DrawString(this.tutorialFont, "Yes         No", new Vector2((float)(rectangle7.X + 190), (float)(rectangle7.Y + 80)), Color.White);
                    this.spriteBatch.Draw(this.buttonA, new Vector2((float)(rectangle7.X + 150), (float)(rectangle7.Y + 77)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.75f, 0, 0f);
                    this.spriteBatch.Draw(this.buttonB, new Vector2((float)(rectangle7.X + 235), (float)(rectangle7.Y + 77)), default(Rectangle?), Color.White, 0f, Vector2.Zero, 0.75f, 0, 0f);
                }
            }
            this.spriteBatch.End();
        }

        private void DrawTutorial(string txt)
        {
            this.tutorialBack = new Rectangle(this.screenWidth / 2 - (int)(this.tutorialFont.MeasureString(txt).X / 2f) - 5, this.safe.Y + this.safe.Height - 60, (int)this.tutorialFont.MeasureString(txt).X + 10, 50);
            this.spriteBatch.Draw(this.pixel, this.tutorialBack, Color.Black);
            this.tutorialFront = new Rectangle(this.tutorialBack.X + 2, this.tutorialBack.Y + 2, this.tutorialBack.Width - 4, this.tutorialBack.Height - 4);
            this.spriteBatch.Draw(this.pixel, this.tutorialFront, Color.White);
            this.spriteBatch.Draw(this.tutorialLeft, new Vector2((float)(this.tutorialBack.X - this.tutorialLeft.Width + 2), (float)(this.tutorialBack.Y - 1)), Color.White);
            this.spriteBatch.Draw(this.tutorialRight, new Vector2((float)(this.tutorialBack.X + this.tutorialBack.Width - 2), (float)(this.tutorialBack.Y - 1)), Color.White);
            this.spriteBatch.DrawString(this.tutorialFont, txt, new Vector2((float)(this.tutorialBack.X + 5), (float)(this.tutorialBack.Y)), Color.Black);
        }

        private void PurchaseGame()
        {
            /*
            SignedInGamer signedInGamer = Gamer.SignedInGamers[this.players[0].Index];
            if (signedInGamer != null && signedInGamer.Privileges.AllowPurchaseContent)
            {
                Guide.ShowMarketplace(this.players[0].Index);
            }
            else
            {
                Guide.ShowSignIn(1, true);
            }
            */
        }

        private void PromptMe()
        {
            try
            {
                int i;
                for (i = 1; i <= 3; i++)
                {
                    if (File.Exists("save/Craftimals_Powers_" + i))
                    {
                        using (StreamReader streamReader = new StreamReader("save/Craftimals_Powers_" + i))
                        {
                            //int i;
                            this.saveFiles[i].TouchedSun = bool.Parse(streamReader.ReadLine());
                            this.saveFiles[i].LeftoverRibbons = int.Parse(streamReader.ReadLine());
                            int num = int.Parse(streamReader.ReadLine());
                            for (i = 0; i < num; i++)
                            {
                                this.saveFiles[i].TutorialSteps.Add(int.Parse(streamReader.ReadLine()));
                            }
                            this.saveFiles[i].Update();
                        }
                    }
                }
            }
            catch
            {
                this.storagePrompted = false;
            }
        }

        private void LoadData()
        {
            string generalSave = "save/Craftimals_Blocks_" + loadSlot;
            if (File.Exists(generalSave))
            {
                using (StreamReader streamReader = new StreamReader(generalSave))
                {
                    int num = int.Parse(streamReader.ReadLine());
                    for (int i = 0; i < num; i++)
                    {
                        Vector2 position = new(float.Parse(streamReader.ReadLine(), CultureInfo.InvariantCulture), float.Parse(streamReader.ReadLine(), CultureInfo.InvariantCulture));
                        Color color = new(int.Parse(streamReader.ReadLine()), int.Parse(streamReader.ReadLine()), int.Parse(streamReader.ReadLine()));
                        Block block = new Block();
                        block.Initialize(this.blockTexture, position, this.background[0], color);
                        this.blocks[0].Add(block);
                        if (this.players.Count > 1)
                        {
                            block = new Block();
                            block.Initialize(this.blockTexture, position, this.background[1], color);
                            this.blocks[1].Add(block);
                        }
                    }
                }
            }
        }

        private void SaveData()
        {

            if (!Directory.Exists("save"))
            {
                Directory.CreateDirectory("save");
            }

            string powerSave = "save/Craftimals_Powers_" + loadSlot;
            string blockSave = "save/Craftimals_Blocks_" + loadSlot;

            using (StreamWriter streamWriter = new StreamWriter(powerSave))
            {
                streamWriter.WriteLine(this.touchedSun);
                this.saveFiles[this.loadSlot].TouchedSun = this.touchedSun;
                streamWriter.WriteLine(this.ribbons[0].Count);
                this.saveFiles[this.loadSlot].LeftoverRibbons = this.ribbons[0].Count;
                streamWriter.WriteLine(this.tutorials.Count);
                this.saveFiles[this.loadSlot].TutorialSteps = new List<int>();
                for (int i = 0; i < this.tutorials.Count; i++)
                {
                    streamWriter.WriteLine(this.tutorials[i]);
                    this.saveFiles[this.loadSlot].TutorialSteps.Add(this.tutorials[i]);
                }
                this.saveFiles[this.loadSlot].Update();
                this.savedGame = true;
            }

            using (StreamWriter streamWriter = new StreamWriter(blockSave))
            {
                streamWriter.WriteLine(this.blocks[0].Count);
                for (int i = 0; i < this.blocks[0].Count; i++)
                {
                    streamWriter.WriteLine(this.blocks[0][i].PositionX.ToString(CultureInfo.InvariantCulture));
                    streamWriter.WriteLine(this.blocks[0][i].PositionY.ToString(CultureInfo.InvariantCulture));
                    streamWriter.WriteLine((int)this.blocks[0][i].Color.R);
                    streamWriter.WriteLine((int)this.blocks[0][i].Color.G);
                    streamWriter.WriteLine((int)this.blocks[0][i].Color.B);
                }
            }
        }
    }
}
