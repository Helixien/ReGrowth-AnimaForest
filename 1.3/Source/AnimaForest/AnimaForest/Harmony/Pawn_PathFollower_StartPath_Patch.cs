using HarmonyLib;
using System.Linq;
using Verse;
using Verse.AI;

namespace AnimaForest
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "Notify_DamageTaken")]
    public class Pawn_JobTracker_Notify_DamageTaken_Patch
    {
        public static void Prefix()
        {
            Pawn_PathFollower_StartPath_Patch.notifyDamageTaken = true;
        }
        public static void Postfix()
        {
            Pawn_PathFollower_StartPath_Patch.notifyDamageTaken = false;
        }
    }

    [HarmonyPatch(typeof(Pawn_PathFollower), "StartPath")]
    public class Pawn_PathFollower_StartPath_Patch
    {
        public static bool notifyDamageTaken;
        private static void Postfix(Pawn_PathFollower __instance, Pawn ___pawn, LocalTargetInfo dest, PathEndMode peMode)
        {
            if (!notifyDamageTaken && ___pawn.def == AF_DefOf.RG_ArchoEntity_Conservator)
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
