using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace CatLauncher;

public sealed class Projectile_CatShell : Projectile_Explosive
{
    private int colorIndex = -1;
    private int visualFacing = -1;
    private int spinDirection;
    private int firstDrawTick = -1;
    private bool launchSoundPlayed;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref colorIndex, "colorIndex", -1);
        Scribe_Values.Look(ref visualFacing, "visualFacing", -1);
        Scribe_Values.Look(ref spinDirection, "spinDirection", 0);
        Scribe_Values.Look(ref firstDrawTick, "firstDrawTick", -1);
        Scribe_Values.Look(ref launchSoundPlayed, "launchSoundPlayed", false);
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        InitializeVisuals();
        var graphic = GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Animal/Cat/Cat", ShaderDatabase.Cutout, new Vector2(0.65f, 0.65f), CatVisuals.ColorFor(colorIndex));
        var elapsedTicks = Find.TickManager.TicksGame - firstDrawTick;
        var rotation = Quaternion.AngleAxis(CatLaunchRules.FlightRotation(visualFacing, spinDirection, elapsedTicks), Vector3.up);
        Graphics.DrawMesh(MeshPool.plane10, drawLoc, rotation, graphic.MatAt(new Rot4(visualFacing)), 0);
    }

    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        var impactCell = Position;
        var impactMap = Map;
        if (impactMap is not null) DefDatabase<SoundDef>.GetNamed("Pawn_Cat_Death").PlayOneShot(new TargetInfo(impactCell, impactMap));
        base.Impact(hitThing, blockedByShield);
        if (impactMap is null || !impactCell.InBounds(impactMap)) return;
        var bloodiness = CatLauncherMod.Settings.BloodinessMultiplier;
        if (bloodiness > 0f) FilthMaker.TryMakeFilth(impactCell, impactMap, ThingDefOf.Filth_Blood);
        var fragments = CatLaunchRules.ScaleFragmentCount(CatLaunchRules.FragmentCount(Rand.Value), bloodiness);
        for (var i = 0; i < fragments; i++)
        {
            var angle = Rand.Range(0f, 360f) * Mathf.Deg2Rad;
            var distance = CatLaunchRules.ClampDebrisDistance(Rand.Range(0.35f, 2.5f));
            var cell = impactCell + new IntVec3(Mathf.RoundToInt(Mathf.Cos(angle) * distance), 0, Mathf.RoundToInt(Mathf.Sin(angle) * distance));
            if (cell.InBounds(impactMap))
            {
                FilthMaker.TryMakeFilth(cell, impactMap, ThingDefOf.Filth_Blood);
                MoteMaker.MakeStaticMote(cell.ToVector3(), impactMap, DefDatabase<ThingDef>.GetNamed("Mote_CatFragment"), 0.35f);
            }
        }
        if (CatLaunchRules.ShouldSpawnSurvivor(Rand.Value, CatLauncherMod.Settings.SurvivorChance)) SpawnSurvivor(impactCell, impactMap, colorIndex);
    }

    private static void SpawnSurvivor(IntVec3 center, Map map, int catColorIndex)
    {
        // This runs after the explosion resolves. Keep the survivor beyond the blast radius,
        // then apply only its deliberate non-lethal injury.
        foreach (var cell in GenRadial.RadialCellsAround(center, 4f, true))
        {
            var dx = cell.x - center.x;
            var dz = cell.z - center.z;
            if (dx * dx + dz * dz <= 6 || !cell.InBounds(map) || !cell.Standable(map) || cell.GetFirstPawn(map) is not null) continue;
            var cat = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.GetNamed("Cat"));
            var tint = cat.GetComp<CompSurvivorCatTint>();
            if (tint is not null) tint.ColorIndex = catColorIndex;
            GenSpawn.Spawn(cat, cell, map);
            // The rare survivor is a small jackpot moment: reuse RimWorld's restrained trade-confirmation chime.
            DefDatabase<SoundDef>.GetNamed("ExecuteTrade").PlayOneShot(new TargetInfo(cell, map));
            cat.TakeDamage(new DamageInfo(DamageDefOf.Blunt, 1f));
            return;
        }
    }

    private void InitializeVisuals()
    {
        if (colorIndex < 0) colorIndex = Rand.RangeInclusive(0, CatVisuals.Colors.Length - 1);
        if (visualFacing < 0) visualFacing = Rand.RangeInclusive(0, 3);
        if (spinDirection == 0) spinDirection = Rand.Bool ? 1 : -1;
        if (firstDrawTick < 0) firstDrawTick = Find.TickManager.TicksGame;
        if (!launchSoundPlayed && Map is not null)
        {
            DefDatabase<SoundDef>.GetNamed("Pawn_Cat_Call").PlayOneShot(new TargetInfo(Position, Map));
            launchSoundPlayed = true;
        }
    }
}
