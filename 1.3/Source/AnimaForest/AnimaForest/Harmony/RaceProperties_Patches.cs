using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AnimaForest
{
    [HarmonyPatch(typeof(RaceProperties), "IsFlesh", MethodType.Getter)]
    public static class RaceProperties_IsFlesh_Patch
    {
        public static void Postfix(RaceProperties __instance, ref bool __result)
        {
            if (__result && __instance.FleshType == AF_DefOf.RG_ArchoEntity)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(RaceProperties), "IsMechanoid", MethodType.Getter)]
    public static class RaceProperties_IsMechanoid_Patch
    {
        public static void Postfix(RaceProperties __instance, ref bool __result)
        {
            if (!__result && __instance.FleshType == AF_DefOf.RG_ArchoEntity)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(PawnRenderer), "CarryWeaponOpenly")]
    public static class PawnRenderer_CarryWeaponOpenly_Patch
    {
        public static void Postfix(Pawn ___pawn, ref bool __result)
        {
            if (!__result && ___pawn.def == AF_DefOf.RG_ArchoEntity_Conservator)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(RaceProperties), "SpecialDisplayStats")]
    public static class RaceProperties_SpecialDisplayStats_Patch
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, ThingDef parentDef, StatRequest req)
        {
            foreach (var entry in __result)
            {
                if (parentDef == AF_DefOf.RG_ArchoEntity_Conservator && entry.LabelCap == "StatsReport_LifeExpectancy".Translate().CapitalizeFirst())
                {
                    continue;
                }
                else
                {
                    yield return entry;
                }
            }
        }
    }
}
