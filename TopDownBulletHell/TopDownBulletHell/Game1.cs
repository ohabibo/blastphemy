using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private GameManager gameManager;
    private IntroScreen introScreen;

    private Texture2D playerTexture, bulletTexture, enemyTexture, enemyBulletTexture, introBackground, shieldTexture;
    private SpriteFont font;
    private bool isIntroFinished;
    private int playerLives = 3;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.PreferredBackBufferWidth = 1280; // Set resolution width
        graphics.PreferredBackBufferHeight = 720; // Set resolution height
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("player");
        bulletTexture = Content.Load<Texture2D>("bullet");
        enemyTexture = Content.Load<Texture2D>("enemy");
        enemyBulletTexture = Content.Load<Texture2D>("enemybullet1");
        font = Content.Load<SpriteFont>("gamefont");
        SpriteFont shieldFont = Content.Load<SpriteFont>("shieldfont"); // Load the new font
        Texture2D logoTexture = Content.Load<Texture2D>("logo");
        shieldTexture = Content.Load<Texture2D>("shield"); // Load the shield texture

        gameManager = new GameManager(playerTexture, bulletTexture, enemyTexture, enemyBulletTexture, font, shieldTexture, shieldFont);
        introScreen = new IntroScreen(null, logoTexture, font);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (!isIntroFinished)
        {
            introScreen.Update(gameTime);
            if (introScreen.IsFinished)
                isIntroFinished = true;
        }
        else
        {
            gameManager.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();
        if (!isIntroFinished)
        {
            introScreen.Draw(spriteBatch);
        }
        else
        {
            gameManager.Draw(spriteBatch);
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }
}