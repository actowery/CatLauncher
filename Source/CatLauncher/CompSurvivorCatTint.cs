using UnityEngine;
using Verse;

namespace CatLauncher;

public sealed class CompProperties_SurvivorCatTint : CompProperties
{
    public CompProperties_SurvivorCatTint() => compClass = typeof(CompSurvivorCatTint);
}

public sealed class CompSurvivorCatTint : ThingComp
{
    public int ColorIndex = -1;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref ColorIndex, "catLauncherColorIndex", -1);
    }

    public override void PostDraw()
    {
        base.PostDraw();
        if (ColorIndex < 0 || parent is not Pawn pawn || pawn.Dead) return;

        var graphic = GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Animal/Cat/Cat", ShaderDatabase.Cutout, new Vector2(1f, 1f), CatVisuals.ColorFor(ColorIndex));
        var drawLoc = parent.DrawPos;
        drawLoc.y += 0.01f;
        Graphics.DrawMesh(MeshPool.plane10, drawLoc, Quaternion.identity, graphic.MatAt(parent.Rotation), 0);
    }
}
