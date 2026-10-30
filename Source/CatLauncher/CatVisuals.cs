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

    public static Color ColorFor(int index) => Colors[Mathf.Clamp(index, 0, Colors.Length - 1)];
}
