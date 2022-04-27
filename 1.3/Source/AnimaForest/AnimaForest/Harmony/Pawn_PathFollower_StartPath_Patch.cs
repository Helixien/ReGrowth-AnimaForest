using HarmonyLib;
using Verse;
using Verse.AI;

namespace AnimaForest
{
    [HarmonyPatch(typeof(Pawn_PathFollower), "StartPath")]
    public class Pawn_PathFollower_StartPath_Patch
    {
        private static void Postfix(Pawn_PathFollower __instance, Pawn ___pawn, LocalTargetInfo dest, PathEndMode peMode)
        {
            if (___pawn.kindDef == AF_DefOf.RG_ArchoEntity_Conservator)
            {
                var comp = ___pawn.GetComp<CompPawnTeleporter>();
                if (comp.CanUseTeleport() && Rand.Chance(comp.Props.nonCombatTeleportChance))
                {
                    comp.Teleport(___pawn, __instance.Destination.Cell);
                }
            }
        }
    }
}
