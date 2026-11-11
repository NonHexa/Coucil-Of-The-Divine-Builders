using Raylib_cs;

namespace DivineBuilders.Configuration;

public static class GameConstants
{
    public const int ScreenWidth = 1280;
    public const int ScreenHeight = 720;
    public const string ScreenTitle = "Divenie somethoing";
    public const string BackgroundPath = "";

    public static readonly Color BackgroundColor = new(12, 15, 22, 255);
    public static readonly Color TextColor = new(225, 218, 199, 255);
    public static readonly Color HighlightColor = new(150, 35, 35, 255);
    public static readonly Color ButtonColor = new(25, 27, 33, 255);

    public static readonly int[] NoEasy =
    {
        50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 41, 40, 39, 38, 37, 36, 35,
        34, 33, 32, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 23, 22, 21, 20,
        19, 18, 17, 16, 15, 14, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 5
    };

    public static readonly int[] NoMedium =
    {
        100, 96, 93, 89, 85, 82, 79, 75, 72, 69, 66, 63, 60, 57, 54, 52, 49,
        46, 44, 42, 39, 37, 35, 33, 31, 29, 27, 25, 23, 22, 20, 19, 17, 16,
        15, 14, 12, 11, 10, 10, 9, 8, 7, 7, 6, 6, 6, 5, 5, 5, 5
    };

    public static readonly int[] NoHard =
    {
        150, 141, 133, 125, 118, 111, 104, 97, 91, 85, 79, 74, 69, 64, 59,
        55, 51, 47, 43, 40, 36, 33, 30, 28, 25, 23, 21, 19, 17, 16, 14, 13,
        12, 11, 10, 9, 8, 8, 7, 7, 6, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5
    };
}
