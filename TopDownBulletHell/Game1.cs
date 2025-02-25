using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private GameManager gameManager;
    private IntroScreen introScreen;

    private Texture2D playerTexture, bulletTexture, enemyTexture, introBackground;
    private SpriteFont font;
    private bool isIntroFinished;

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
    // introBackground = Content.Load<Texture2D>("introBackground");
    font = Content.Load<SpriteFont>("gamefont");

    gameManager = new GameManager(playerTexture, bulletTexture, enemyTexture);
    introScreen = new IntroScreen(null, font); // Pass null for introBackground
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