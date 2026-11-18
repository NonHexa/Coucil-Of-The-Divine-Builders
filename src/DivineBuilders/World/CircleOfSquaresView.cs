using System.Numerics;
using DivineBuilders.Configuration;
using DivineBuilders.Gameplay.StatusEffects;
using Raylib_cs;

namespace DivineBuilders.World;

public sealed class CircleOfSquaresView
{
    private const float Speed = 5000f;
    private const int SmallGridSize = GameConstants.FloorGridCellSize;
    private const int SquareSize = GameConstants.FloorSize;
    private const int BorderThickness = 12;

    private readonly int gridRadius;
    public int FloorNumber { get; }
    private readonly float centerX = 400f;
    private readonly float centerY = 300f;
    private readonly int smallSquaresPerSide = SquareSize / SmallGridSize;
    private readonly List<Rectangle> blueSquares = new();
    private Vector2 playerPosition = new(400, 300);
    private readonly float playerRadius = 15f;
    private readonly double[] playerStats = Enumerable.Repeat(100d, StatusEffectSystem.StatCount).ToArray();
    private readonly bool[] activeEffects = new bool[StatusEffectSystem.EffectCount];
    private readonly Rectangle downDoor;
    private Camera2D camera;

    public IReadOnlyList<double> PlayerStats => playerStats;
    public IReadOnlyList<bool> ActiveEffects => activeEffects;

    public void ApplyEffect(int effectIndex) => StatusEffectSystem.Apply(effectIndex, playerStats, activeEffects);

    public CircleOfSquaresView(int floorNumber = 0, GameConstants.Difficulty difficulty = GameConstants.Difficulty.Easy)
    {
        FloorNumber = floorNumber;
        gridRadius = GameConstants.GetFloorRadius(floorNumber, difficulty);
        int cellsPerFloorSide = SquareSize / SmallGridSize;
        int doorColumn;
        int doorRow;
        do
        {
            doorColumn = Random.Shared.Next(cellsPerFloorSide);
            doorRow = Random.Shared.Next(cellsPerFloorSide);
        } while (doorColumn == cellsPerFloorSide / 2 && doorRow == cellsPerFloorSide / 2);

        downDoor = new Rectangle(
            centerX - SquareSize / 2f + doorColumn * SmallGridSize,
            centerY - SquareSize / 2f + doorRow * SmallGridSize,
            SmallGridSize,
            SmallGridSize);

        camera = new Camera2D
        {
            Target = playerPosition,
            Offset = new Vector2(GameConstants.ScreenWidth / 2f, GameConstants.ScreenHeight / 2f),
            Rotation = 0,
            Zoom = 1
        };

        for (int column = -gridRadius; column <= gridRadius; column++)
        {
            for (int row = -gridRadius; row <= gridRadius; row++)
            {
                float offsetX = column * SquareSize;
                float offsetY = row * SquareSize;
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
        for (int effect = 0; effect < 9; effect++)
        {
            if (Raylib.IsKeyPressed((KeyboardKey)((int)KeyboardKey.One + effect)))
            {
                ApplyEffect(effect);
            }
        }

        Vector2 nextPosition = playerPosition;
        if (Raylib.IsKeyDown(KeyboardKey.Right)) nextPosition.X += Speed * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Left)) nextPosition.X -= Speed * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Up)) nextPosition.Y -= Speed * deltaTime;
        if (Raylib.IsKeyDown(KeyboardKey.Down)) nextPosition.Y += Speed * deltaTime;

        Vector2 fromCenter = nextPosition - new Vector2(centerX, centerY);
        float distanceFromCenter = fromCenter.Length();
        float mapHalfExtent = (gridRadius + 0.5f) * SquareSize - playerRadius;
        nextPosition.X = Math.Clamp(nextPosition.X, centerX - mapHalfExtent, centerX + mapHalfExtent);
        nextPosition.Y = Math.Clamp(nextPosition.Y, centerY - mapHalfExtent, centerY + mapHalfExtent);

        playerPosition = nextPosition;
        camera.Target.X += (playerPosition.X - camera.Target.X) * 0.1f;
        camera.Target.Y += (playerPosition.Y - camera.Target.Y) * 0.1f;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        Raylib.BeginMode2D(camera);

        // The centre tile is the lift/laser shaft, aligned to the tile grid.
        Raylib.DrawRectangle((int)(centerX - SquareSize / 2f), (int)(centerY - SquareSize / 2f), SquareSize, SquareSize, new Color(35, 22, 22, 250));

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

        float mapEdge = (gridRadius + 0.5f) * SquareSize;
        for (int border = 0; border < BorderThickness; border++)
            Raylib.DrawRectangleLines((int)(centerX - mapEdge + border), (int)(centerY - mapEdge + border),
                (int)(mapEdge * 2 - border * 2), (int)(mapEdge * 2 - border * 2), Color.Blue);

        // The laser occupies the centre cell of this floor; its beam continues between floors.
        Raylib.DrawRectangle((int)centerX - 6, (int)(centerY - SquareSize / 2f), 12, SquareSize, new Color(255, 45, 35, 110));
        Raylib.DrawLine((int)centerX, (int)(centerY - SquareSize / 2f), (int)centerX, (int)(centerY + SquareSize / 2f), Color.Red);

        Raylib.DrawRectangleRec(downDoor, new Color(230, 190, 35, 255));
        Raylib.DrawRectangleLinesEx(downDoor, 3, Color.DarkBrown);

        Raylib.DrawCircleV(playerPosition, playerRadius, new Color(0, 128, 0, 255));
        Raylib.EndMode2D();

        Raylib.DrawText("Arrow keys: move | 1-9: apply effect | Esc: menu", 10, 10, 20, Color.DarkGray);
        Raylib.DrawText($"Stats: {string.Join(" / ", playerStats.Select(value => value.ToString("0.#")))}", 10, 36, 18, Color.DarkGray);
        Raylib.DrawText($"Active effects: {string.Join(",", activeEffects.Select((active, index) => active ? (index + 1).ToString() : null).Where(value => value is not null))}", 10, 60, 18, Color.DarkGray);
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
