using RimWorld;
using UnityEngine;
using Verse;

namespace CatLauncher;

public sealed class Projectile_CatShell : Projectile_Explosive
{
    private static readonly Color[] CatColors = { new(0.95f, 0.48f, 0.16f), new(0.18f, 0.18f, 0.18f), new(0.88f, 0.82f, 0.7f), new(0.45f, 0.36f, 0.25f) };
    private int colorIndex = -1;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref colorIndex, "colorIndex", -1);
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        if (colorIndex < 0) colorIndex = Rand.RangeInclusive(0, CatColors.Length - 1);
        var graphic = GraphicDatabase.Get<Graphic_Single>("Things/Pawn/Animal/Cat/Cat", ShaderDatabase.Cutout, new Vector2(0.65f, 0.65f), CatColors[colorIndex]);
        Graphics.DrawMesh(MeshPool.plane10, drawLoc, ExactRotation, graphic.MatSingle, 0);
    }

    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        var impactCell = Position;
        var impactMap = Map;
        base.Impact(hitThing, blockedByShield);
        if (impactMap is null || !impactCell.InBounds(impactMap)) return;
        FilthMaker.TryMakeFilth(impactCell, impactMap, ThingDefOf.Filth_Blood);
        var fragments = CatLaunchRules.FragmentCount(Rand.Value);
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
        if (CatLaunchRules.ShouldSpawnSurvivor(Rand.Value)) SpawnSurvivor(impactCell, impactMap);
    }

    private static void SpawnSurvivor(IntVec3 center, Map map)
    {
        foreach (var cell in GenRadial.RadialCellsAround(center, 2f, true))
        {
            if (cell == center || !cell.InBounds(map) || !cell.Standable(map) || cell.GetFirstPawn(map) is not null) continue;
            var cat = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.GetNamed("Cat"));
            GenSpawn.Spawn(cat, cell, map);
            cat.TakeDamage(new DamageInfo(DamageDefOf.Blunt, 1f));
            return;
        }
    }
}
