using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AnimaForest
{
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
