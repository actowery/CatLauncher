using Verse;
using Verse.Sound;

namespace CatLauncher;

public sealed class Thing_CatLauncher : ThingWithComps
{
    public override void Notify_Equipped(Pawn pawn)
    {
        base.Notify_Equipped(pawn);
        if (pawn.Map is not null)
        {
            DefDatabase<SoundDef>.GetNamed("CatLauncher_Equip").PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
        }
    }
}
