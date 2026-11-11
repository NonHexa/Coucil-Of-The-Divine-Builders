using DivineBuilders.Configuration;
using DivineBuilders.UI;
using DivineBuilders.World;
using Raylib_cs;

namespace DivineBuilders;

internal static class Program
{
    private static void Main()
    {
        Raylib.InitWindow(GameConstants.ScreenWidth, GameConstants.ScreenHeight, GameConstants.ScreenTitle);
        Raylib.SetTargetFPS(60);

        var menu = new MainMenuView();
        CircleOfSquaresView? game = null;
        bool running = true;

        while (running && !Raylib.WindowShouldClose())
        {
            if (game is null)
            {
                MenuAction action = menu.Update();
                switch (action)
                {
                    case MenuAction.Play:
                        game = new CircleOfSquaresView();
                        break;
                    case MenuAction.Quit:
                        running = false;
                        break;
                }

                if (game is null)
                {
                    Raylib.BeginDrawing();
                    menu.Draw();
                    Raylib.EndDrawing();
                }
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                game = null;
            }
            else
            {
                game.UpdateAndDraw(Raylib.GetFrameTime());
            }
        }

        Raylib.CloseWindow();
    }
}
