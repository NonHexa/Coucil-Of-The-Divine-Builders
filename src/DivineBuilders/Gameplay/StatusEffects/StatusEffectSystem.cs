namespace DivineBuilders.Gameplay.StatusEffects;

public static class StatusEffectSystem
{
    public const int StatCount = 7;

    private static readonly double[,] Multipliers =
    {
        { 0.8, 1, 1, 1, 1, 1, 0.8 },
        { 0.7, 1, 1, 1, 1, 0.8, 1 },
        { 1, 1, 1, 0.8, 1, 1, 1 },
        { 0, 0, 0, 0, 0, 0, 0 },
        { 1.3, 1, 1, 0.5, 1, 0.5, 0.5 },
        { 0.5, 1, 1, 1, 1, 0.5, 1.15 },
        { 0.7, 1, 1, 1, 1, 0.7, 0.8 },
        { 1, 1, 1, 1, 1, 1, 0.8 },
        { 0.7, 1, 1, 1, 1, 1, 2 },
        { 0.5, 1.4, 1.4, 0.6, 0.5, 1, 1.2 },
        { 0.6, 1.1, 1.1, 0.6, 0.6, 0.8, 0.9 },
        { 0.9, 1, 1, 1, 1, 0.9, 1 },
        { 0.8, 1.5, 1.2, 1, 1, 1, 1 },
        { 1, 1, 1, 0.9, 0.9, 1, 1 },
        { 1, 1, 1, 1, 1, 1, 0.8 },
        { 0.8, 1, 1, 1, 1, 0.75, 0.7 },
        { 0.8, 1, 1, 0.8, 0.8, 0.85, 0.7 },
        { 1, 1, 1, 1, 1, 1, 0 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 1, 1.2, 1.2, 1, 1, 1, 1 },
        { 1, 0.5, 0.6, 1, 1, 1, 1 }
    };

    private static readonly int[][] Exclusivities =
    {
        new[] { 2 }, Array.Empty<int>(), new[] { 0, 6 }, Array.Empty<int>(),
        new[] { 8 }, new[] { 15 }, new[] { 2 }, Array.Empty<int>(),
        new[] { 4 }, new[] { 13 }, Array.Empty<int>(), new[] { 14 },
        new[] { 12 }, Array.Empty<int>(), new[] { 11 }, Array.Empty<int>(),
        Array.Empty<int>(), Array.Empty<int>(), new[] { 19 }, new[] { 18 },
        new[] { 12 }
    };

    public static int EffectCount => Multipliers.GetLength(0);

    public static void Apply(int effectIndex, double[] stats, bool[] activeEffects)
    {
        ArgumentNullException.ThrowIfNull(stats);
        ArgumentNullException.ThrowIfNull(activeEffects);

        if ((uint)effectIndex >= EffectCount)
        {
            throw new ArgumentOutOfRangeException(nameof(effectIndex));
        }

        if (stats.Length != StatCount)
        {
            throw new ArgumentException($"Expected exactly {StatCount} stats.", nameof(stats));
        }

        if (activeEffects.Length != EffectCount)
        {
            throw new ArgumentException($"Expected exactly {EffectCount} active-effect flags.", nameof(activeEffects));
        }

        if (activeEffects[effectIndex])
        {
            return;
        }

        foreach (int conflictingEffect in Exclusivities[effectIndex])
        {
            if (!activeEffects[conflictingEffect])
            {
                continue;
            }

            for (int statIndex = 0; statIndex < StatCount; statIndex++)
            {
                double multiplier = Multipliers[conflictingEffect, statIndex];
                if (multiplier == 0)
                {
                    throw new InvalidOperationException(
                        $"Cannot remove status effect {conflictingEffect} because it has a zero multiplier.");
                }

                stats[statIndex] /= multiplier;
            }

            activeEffects[conflictingEffect] = false;
        }

        for (int statIndex = 0; statIndex < StatCount; statIndex++)
        {
            stats[statIndex] *= Multipliers[effectIndex, statIndex];
        }

        activeEffects[effectIndex] = true;
    }
}
