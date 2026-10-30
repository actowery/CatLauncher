using UnityEngine;
using Verse;

namespace CatLauncher;

public sealed class CatLauncherMod : Mod
{
    public static CatLauncherSettings Settings { get; private set; } = new();

    public CatLauncherMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<CatLauncherSettings>();
    }

    public override string SettingsCategory() => "CatLauncher.SettingsCategory".Translate();

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing = new Listing_Standard();
        listing.Begin(inRect);
        listing.Label("CatLauncher.SurvivorChance".Translate(Settings.SurvivorChancePercent.ToString("0.0")));
        Settings.SurvivorChancePercent = listing.Slider(Settings.SurvivorChancePercent, 0f, 10f);
        listing.Gap();
        listing.Label("CatLauncher.Bloodiness".Translate(Settings.BloodinessPercent.ToString("0")));
        Settings.BloodinessPercent = listing.Slider(Settings.BloodinessPercent, 0f, 300f);
        listing.Gap();
        listing.Label("CatLauncher.SettingsHint".Translate());
        listing.End();
    }
}

public sealed class CatLauncherSettings : ModSettings
{
    public float SurvivorChancePercent = 1f;
    public float BloodinessPercent = 100f;

    public float SurvivorChance => Mathf.Clamp(SurvivorChancePercent, 0f, 10f) / 100f;
    public float BloodinessMultiplier => Mathf.Clamp(BloodinessPercent, 0f, 300f) / 100f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref SurvivorChancePercent, "survivorChancePercent", 1f);
        Scribe_Values.Look(ref BloodinessPercent, "bloodinessPercent", 100f);
    }
}
