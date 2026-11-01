using UnityEngine;

namespace CatLauncher;

public static class CatVisuals
{
    public static readonly Color[] Colors =
    {
        new(0.95f, 0.48f, 0.16f),
        new(0.18f, 0.18f, 0.18f),
        new(0.88f, 0.82f, 0.7f),
        new(0.45f, 0.36f, 0.25f)
    };

    public static readonly string[] SurvivorTintHediffs =
    {
        "CatLauncher_CatTintOrange",
        "CatLauncher_CatTintBlack",
        "CatLauncher_CatTintCream",
        "CatLauncher_CatTintBrown"
    };

    public static Color ColorFor(int index) => Colors[CatLaunchRules.NormalizeColorIndex(index, Colors.Length)];

    public static string SurvivorTintHediffFor(int index) => SurvivorTintHediffs[CatLaunchRules.NormalizeColorIndex(index, SurvivorTintHediffs.Length)];
}
