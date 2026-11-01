using System;

namespace CatLauncher;

public static class CatLaunchRules
{
    public const float SurvivorChance = 0.01f;
    public const int MinimumFragments = 4;
    public const int MaximumFragments = 7;

    public static bool ShouldSpawnSurvivor(float roll) => roll >= 0f && roll < SurvivorChance;

    public static bool ShouldSpawnSurvivor(float roll, float chance) => roll >= 0f && roll < Math.Min(1f, Math.Max(0f, chance));

    public static int FragmentCount(float roll) => MinimumFragments + Math.Min(MaximumFragments - MinimumFragments, Math.Max(0, (int)(roll * (MaximumFragments - MinimumFragments + 1))));

    public static int ScaleFragmentCount(int count, float multiplier) => Math.Max(0, Math.Min(MaximumFragments * 3, (int)Math.Round(Math.Max(0, count) * Math.Max(0f, multiplier))));

    public static float ClampDebrisDistance(float distance) => Math.Min(2.5f, Math.Max(0.35f, distance));

    public static int NormalizeFacing(int facing) => ((facing % 4) + 4) % 4;

    public static int NormalizeColorIndex(int index, int colorCount) => colorCount <= 0 ? 0 : Math.Min(colorCount - 1, Math.Max(0, index));

    public static float FlightRotation(int facing, int spinDirection, int elapsedTicks) => NormalizeFacing(facing) * 90f + (spinDirection >= 0 ? 1f : -1f) * elapsedTicks * 2.5f;
}
