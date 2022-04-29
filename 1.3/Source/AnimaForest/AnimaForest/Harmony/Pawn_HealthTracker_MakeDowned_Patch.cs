using HarmonyLib;
using Verse;

namespace AnimaForest
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
    public static class Pawn_HealthTracker_MakeDowned_Patch
    {
        private static void Postfix(Pawn ___pawn, DamageInfo? dinfo, Hediff hediff)
        {
            if (___pawn.Downed && ___pawn.def == AF_DefOf.RG_ArchoEntity_Conservator)
            {
                ___pawn.Kill(dinfo, hediff);
            }
        }
    }
}
