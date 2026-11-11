using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace CircleOfSquares
{
    class RectangleOutline
    {
        public Rectangle Rect;
        public Color Color;
        public int BorderWidth;

        public RectangleOutline(Rectangle rect, Color color, int borderWidth)
        {
            Rect = rect;
            Color = color;
            BorderWidth = borderWidth;
        }
    }

    class Program
    {
        const int ScreenWidth = 800;
        const int ScreenHeight = 600;
        const float Speed = 5f; // Pixels per second

        static void Main(string[] args)
        {
            // Initialize window
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Circle of Squares in C# Raylib");
            Raylib.SetTargetFPS(60);

            // Player setup
            Vector2 playerPos = new Vector2(400, 300);
            float playerRadius = 15f;
            Color laSalleGreen = new Color(0, 128, 0, 255);

            // Camera setup
            Camera2D camera = new Camera2D
            {
                Target = playerPos,
                Offset = new Vector2(ScreenWidth / 2.0f, ScreenHeight / 2.0f),
                Rotation = 0.0f,
                Zoom = 1.0f
            };

            // Generate grid shapes (equivalent to Arcade's ShapeElementList)
            List<RectangleOutline> gridShapes = new List<RectangleOutline>();
            
            int smallerGrid = 40;
            int squareSize = 1000;
            float circleRadius = squareSize * 50f;
            float centerX = 400f;
            float centerY = 300f;
            
            int gridRadiusSmall = (int)(circleRadius / smaller_grid(smallerGrid, squareSize, circleRadius)); // Adjusted for performance bounds
            // Using safer bounds for demonstration to avoid excessive memory overhead if radii are massive:
            int safeSmallRadius = 50; 
            int safeLargeRadius = 10;

            // Generate smaller grid outlines (Orange)
            for (int column = -safeSmallRadius; column <= safeSmallRadius; column++)
            {
                for (int row = -safeSmallRadius; row <= safeSmallRadius; row++)
                {
                    float offsetX = column * smallerGrid;
                    float offsetY = row * smallerGrid;
                    
                    if (offsetX * offsetX + offsetY * offsetY > (circleRadius * smallerGrid) * (circleRadius * smallerGrid))
                        continue;

                    Rectangle rect = new Rectangle(centerX + offsetX - squareSize / 2f, centerY + offsetY - squareSize / 2f, squareSize, squareSize);
                    gridShapes.Add(new RectangleOutline(rect, Color.Orange, 2));
                }
            }

            // Generate main grid outlines (Blue)
            int gridRadius = (int)(circleRadius / square_size_divisor(squareSize, circleRadius));
            for (int column = -safeLargeRadius; column <= safeLargeRadius; column++)
            {
                for (int row = -safeLargeRadius; row <= safeLargeRadius; row++)
                {
                    float offsetX = column * squareSize;
                    float offsetY = row * squareSize;
                    
                    if (offsetX * offsetX + offsetY * offsetY > circleRadius * circleRadius)
                        continue;

                    Rectangle rect = new Rectangle(centerX + offsetX - squareSize / 2f, centerY + offsetY - squareSize / 2f, squareSize, squareSize);
                    gridShapes.Add(new RectangleOutline(rect, Color.Blue, 2));
                }
            }

            // Main Game Loop
            while (!Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();

                // 1. Handle Input & Movement
                if (Raylib.IsKeyDown(KeyboardKey.Right)) playerPos.X += Speed * deltaTime * 5;
                if (Raylib.IsKeyDown(KeyboardKey.Left)) playerPos.X -= Speed * deltaTime * 5;
                if (Raylib.IsKeyDown(KeyboardKey.Up)) playerPos.Y -= Speed * deltaTime * 5;
                if (Raylib.IsKeyDown(KeyboardKey.Down)) playerPos.Y += Speed * deltaTime * 5;

                // 2. Update Camera (Lerp towards player)
                camera.Target.X += (playerPos.X - camera.Target.X) * 0.1f;
                camera.Target.Y += (playerPos.Y - camera.Target.Y) * 0.1f;

                // 3. Draw
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                // Start 2D Camera Mode
                Raylib.BeginMode2D(camera);

                // Draw batched grid shapes
                foreach (var shape in gridShapes)
                {
                    Raylib.DrawRectangleLines((int)shape.Rect.X, (int)shape.Rect.Y, (int)shape.Rect.Width, (int)shape.Rect.Height, shape.Color);
                }

                // Draw player
                Raylib.DrawCircleV(playerPos, playerRadius, laSalleGreen);

                // End 2D Camera Mode
                Raylib.EndMode2D();

                // UI / HUD text
                Raylib.DrawText("Use Arrow Keys to Move", 10, 10, 20, Color.DarkGray);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        static float smaller_grid(int smallerGrid, int squareSize, float circleRadius) => smallerGrid;
        static float square_size_divisor(int squareSize, float circleRadius) => squareSize;
    }
}