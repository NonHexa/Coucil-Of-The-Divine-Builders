using System.Numerics;
using DivineBuilders.Configuration;
using Raylib_cs;

namespace DivineBuilders.World;

public sealed class CircleOfSquaresView
{
    private const float Speed = 500f;
    private const int SmallGridSize = 40;
    private const int SquareSize = 1000;
    private const int BorderThickness = 12;

    private readonly float circleRadius = SquareSize * 50f;
    private readonly float centerX = 400f;
    private readonly float centerY = 300f;
    private readonly int smallSquaresPerSide = SquareSize / SmallGridSize;
    private readonly List<Rectangle> blueSquares = new();
    private Vector2 playerPosition = new(400, 300);
    private readonly float playerRadius = 15f;
    private Camera2D camera;

    public CircleOfSquaresView()
    {
        camera = new Camera2D
        {
            Target = playerPosition,
            Offset = new Vector2(GameConstants.ScreenWidth / 2f, GameConstants.ScreenHeight / 2f),
            Rotation = 0,
            Zoom = 1
        };

        int gridRadius = (int)Math.Ceiling(circleRadius / SquareSize);
        for (int column = -gridRadius; column <= gridRadius; column++)
        {
            for (int row = -gridRadius; row <= gridRadius; row++)
            {
                float offsetX = column * SquareSize;
                float offsetY = row * SquareSize;
                float farthestCornerX = Math.Abs(offsetX) + SquareSize / 2f;
                float farthestCornerY = Math.Abs(offsetY) + SquareSize / 2f;

                if (farthestCornerX * farthestCornerX + farthestCornerY * farthestCornerY > circleRadius * circleRadius)
                {
                    continue;
                }

                blueSquares.Add(new Rectangle(
                    centerX + offsetX - SquareSize / 2f,
                    centerY + offsetY - SquareSize / 2f,
                    SquareSize,
                    SquareSize));
            }
        }
    }

    public void UpdateAndDraw(float deltaTime)
    {
        Vector2 nextPosition = playerPosition;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) nextPosition.X += Speed * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Left)) nextPosition.X -= Speed * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) nextPosition.Y -= Speed * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) nextPosition.Y += Speed * deltaTime;

        Vector2 fromCenter = nextPosition - new Vector2(centerX, centerY);
        float distanceFromCenter = fromCenter.Length();
        float maxPlayerDistance = circleRadius - playerRadius;
        if (distanceFromCenter > maxPlayerDistance)
        {
            nextPosition = new Vector2(centerX, centerY) + fromCenter / distanceFromCenter * maxPlayerDistance;
        }

        playerPosition = nextPosition;
        camera.Target.X += (playerPosition.X - camera.Target.X) * 0.1f;
        camera.Target.Y += (playerPosition.Y - camera.Target.Y) * 0.1f;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        Raylib.BeginMode2D(camera);

        float viewLeft = camera.Target.X - camera.Offset.X / camera.Zoom;
        float viewRight = camera.Target.X + camera.Offset.X / camera.Zoom;
        float viewTop = camera.Target.Y - camera.Offset.Y / camera.Zoom;
        float viewBottom = camera.Target.Y + camera.Offset.Y / camera.Zoom;

        foreach (Rectangle square in blueSquares)
        {
            if (!IsVisible(square, viewLeft, viewRight, viewTop, viewBottom))
            {
                continue;
            }

            for (int division = 1; division < smallSquaresPerSide; division++)
            {
                int x = (int)(square.X + division * SmallGridSize);
                int y = (int)(square.Y + division * SmallGridSize);
                Raylib.DrawLine(x, (int)square.Y, x, (int)(square.Y + square.Height), Color.Orange);
                Raylib.DrawLine((int)square.X, y, (int)(square.X + square.Width), y, Color.Orange);
            }

            Raylib.DrawRectangleLines((int)square.X, (int)square.Y, (int)square.Width, (int)square.Height, Color.Blue);
        }

        for (int border = 0; border < BorderThickness; border++)
        {
            Raylib.DrawCircleLines(
                (int)centerX,
                (int)centerY,
                (int)circleRadius - BorderThickness / 2 + border,
                Color.Blue);
        }

        Raylib.DrawCircleV(playerPosition, playerRadius, new Color(0, 128, 0, 255));
        Raylib.EndMode2D();

        Raylib.DrawText("Arrow keys: move | Esc: menu", 10, 10, 20, Color.DarkGray);
        Raylib.EndDrawing();
    }

    private static bool IsVisible(Rectangle square, float left, float right, float top, float bottom)
    {
        return square.X + square.Width >= left &&
               square.X <= right &&
               square.Y + square.Height >= top &&
               square.Y <= bottom;
    }
}
