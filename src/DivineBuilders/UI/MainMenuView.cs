using DivineBuilders.Configuration;
using Raylib_cs;

namespace DivineBuilders.UI;

public enum MenuAction
{
    None,
    Play,
    Quit
}

public sealed class MainMenuView
{
    private const int ButtonWidth = 440;
    private const int ButtonHeight = 52;
    private const int ButtonCenterX = GameConstants.ScreenWidth / 2;

    private static readonly (string Label, int Y, MenuAction Action)[] Buttons =
    {
        ("Play", 390, MenuAction.Play),
        ("Settings", 320, MenuAction.None),
        ("Gallery", 250, MenuAction.None),
        ("I Suck (Q^(u)it)", 180, MenuAction.Quit)
    };

    private string? message;

    public MenuAction Update()
    {
        if (!Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            return MenuAction.None;
        }

        System.Numerics.Vector2 mouse = Raylib.GetMousePosition();
        foreach ((string label, int y, MenuAction action) in Buttons)
        {
            if (mouse.X < ButtonCenterX - ButtonWidth / 2 ||
                mouse.X > ButtonCenterX + ButtonWidth / 2 ||
                mouse.Y < GameConstants.ScreenHeight - y - ButtonHeight / 2 ||
                mouse.Y > GameConstants.ScreenHeight - y + ButtonHeight / 2)
            {
                continue;
            }

            message = label switch
            {
                "Settings" => "Settings selected.",
                "Gallery" => "Credits selected.",
                _ => null
            };
            return action;
        }

        return MenuAction.None;
    }

    public void Draw()
    {
        Raylib.ClearBackground(GameConstants.BackgroundColor);
        int panelX = ButtonCenterX - 250;
        int panelY = 65;
        Raylib.DrawRectangle(panelX, panelY, 500, 590, new Color(0, 0, 0, 190));

        DrawCenteredText("Not Ready Yet", 38, 595, GameConstants.TextColor);
        DrawCenteredText("Idk Leif, Isobel and Ethan made", 21, 545, GameConstants.HighlightColor);

        System.Numerics.Vector2 mouse = Raylib.GetMousePosition();
        foreach ((string label, int y, _) in Buttons)
        {
            int top = GameConstants.ScreenHeight - y - ButtonHeight / 2;
            bool hovered = mouse.X >= ButtonCenterX - ButtonWidth / 2 &&
                           mouse.X <= ButtonCenterX + ButtonWidth / 2 &&
                           mouse.Y >= top &&
                           mouse.Y <= top + ButtonHeight;
            Color buttonColor = hovered ? GameConstants.HighlightColor : GameConstants.ButtonColor;

            Raylib.DrawRectangle(ButtonCenterX - ButtonWidth / 2, top, ButtonWidth, ButtonHeight, buttonColor);
            Raylib.DrawRectangleLines(ButtonCenterX - ButtonWidth / 2, top, ButtonWidth, ButtonHeight, GameConstants.TextColor);
            DrawCenteredText(label, 18, y, GameConstants.TextColor);
        }

        DrawCenteredText(message ?? "Come play plssss", 12, 100, new Color(145, 145, 145, 255));
    }

    private static void DrawCenteredText(string text, int fontSize, int yFromBottom, Color color)
    {
        int x = (GameConstants.ScreenWidth - Raylib.MeasureText(text, fontSize)) / 2;
        int y = GameConstants.ScreenHeight - yFromBottom - fontSize / 2;
        Raylib.DrawText(text, x, y, fontSize, color);
    }
}
