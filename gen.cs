using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace CircleOfSquares
{
    class Program
    {
        const int ScreenWidth = 800;
        const int ScreenHeight = 600;
        const float Speed = 500f; // Pixels per second

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

            int smallerGrid = 40;
            int squareSize = 1000;
            float circleRadius = squareSize * 50f;
            float centerX = 400f;
            float centerY = 300f;
            int gridRadius = (int)Math.Ceiling(circleRadius / squareSize);
            int smallSquaresPerSide = squareSize / smallerGrid;
            List<Rectangle> blueSquares = new List<Rectangle>();

            for (int column = -gridRadius; column <= gridRadius; column++)
            {
                for (int row = -gridRadius; row <= gridRadius; row++)
                {
                    float offsetX = column * squareSize;
                    float offsetY = row * squareSize;
                    float farthestCornerX = Math.Abs(offsetX) + squareSize / 2f;
                    float farthestCornerY = Math.Abs(offsetY) + squareSize / 2f;

                    if (farthestCornerX * farthestCornerX + farthestCornerY * farthestCornerY > circleRadius * circleRadius)
                        continue;

                    Rectangle rect = new Rectangle(centerX + offsetX - squareSize / 2f, centerY + offsetY - squareSize / 2f, squareSize, squareSize);
                    blueSquares.Add(rect);
                }
            }

            int borderThickness = 12;

            // Main Game Loop
            while (!Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();

                // 1. Handle Input & Movement
                Vector2 nextPlayerPos = playerPos;
                if (Raylib.IsKeyDown(KeyboardKey.Right)) nextPlayerPos.X += Speed * deltaTime * 5;
                if (Raylib.IsKeyDown(KeyboardKey.Left)) nextPlayerPos.X -= Speed * deltaTime * 5;
                if (Raylib.IsKeyDown(KeyboardKey.Up)) nextPlayerPos.Y -= Speed * deltaTime * 5;
                if (Raylib.IsKeyDown(KeyboardKey.Down)) nextPlayerPos.Y += Speed * deltaTime * 5;

                Vector2 fromCenter = nextPlayerPos - new Vector2(centerX, centerY);
                float distanceFromCenter = fromCenter.Length();
                float maxPlayerDistance = circleRadius - playerRadius;
                if (distanceFromCenter > maxPlayerDistance)
                {
                    nextPlayerPos = new Vector2(centerX, centerY) + fromCenter / distanceFromCenter * maxPlayerDistance;
                }
                playerPos = nextPlayerPos;

                // 2. Update Camera (Lerp towards player)
                camera.Target.X += (playerPos.X - camera.Target.X) * 0.1f;
                camera.Target.Y += (playerPos.Y - camera.Target.Y) * 0.1f;

                // 3. Draw
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                // Start 2D Camera Mode
                Raylib.BeginMode2D(camera);

                // Draw the small-square grid inside each blue square visible to the camera.
                float viewLeft = camera.Target.X - camera.Offset.X / camera.Zoom;
                float viewRight = camera.Target.X + camera.Offset.X / camera.Zoom;
                float viewTop = camera.Target.Y - camera.Offset.Y / camera.Zoom;
                float viewBottom = camera.Target.Y + camera.Offset.Y / camera.Zoom;

                foreach (Rectangle square in blueSquares)
                {
                    if (square.X + square.Width < viewLeft || square.X > viewRight ||
                        square.Y + square.Height < viewTop || square.Y > viewBottom)
                        continue;

                    for (int division = 1; division < smallSquaresPerSide; division++)
                    {
                        int x = (int)(square.X + division * smallerGrid);
                        int y = (int)(square.Y + division * smallerGrid);
                        Raylib.DrawLine(x, (int)square.Y, x, (int)(square.Y + square.Height), Color.Orange);
                        Raylib.DrawLine((int)square.X, y, (int)(square.X + square.Width), y, Color.Orange);
                    }
                }

                foreach (Rectangle square in blueSquares)
                {
                    if (square.X + square.Width < viewLeft || square.X > viewRight ||
                        square.Y + square.Height < viewTop || square.Y > viewBottom)
                        continue;

                    Raylib.DrawRectangleLines((int)square.X, (int)square.Y, (int)square.Width, (int)square.Height, Color.Blue);
                }

                for (int border = 0; border < borderThickness; border++)
                {
                    Raylib.DrawCircleLines((int)centerX, (int)centerY, (int)circleRadius - borderThickness / 2 + border, Color.Blue);
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
    }
}