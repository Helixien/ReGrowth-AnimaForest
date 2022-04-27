using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace AnimaForest
{
    [HarmonyPatch(typeof(MeditationUtility), "CountsAsArtificialBuilding", new Type[] { typeof(ThingDef), typeof(Faction) })]
    public static class MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch
    {
        public static ThingDef stuffDef;
        public static void Postfix(ref bool __result, ThingDef def)
        {
            if (__result && stuffDef == ThingDefOf.Jade)
            {
                __result = false;
            }
            if (stuffDef is null && def.MadeFromStuff)
            {
                Log.Error("[Regrowth Anima] Missing stuff def info for " + def);
            }
        }
    }

    [HarmonyPatch(typeof(MeditationUtility), "CountsAsArtificialBuilding", new Type[] { typeof(Thing) })]
    public static class MeditationUtility_CountsAsArtificialBuilding_Thing_Patch
    {
        private static void Prefix(Thing t)
        {
            MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch.stuffDef = t.Stuff;
        }
        public static void Postfix(ref bool __result, Thing t)
        {
            if (__result)
            {
                __result = t.Stuff != ThingDefOf.Jade;
            }
            MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch.stuffDef = null;
        }
    }

    [HarmonyPatch(typeof(Designator_Place), "DrawBeforeGhost")]
    public static class Designator_Place_Patch
    {
        private static void Prefix(Designator_Place __instance)
        {
            MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch.stuffDef = __instance.StuffDef;
        }

        private static void Postfix(Designator_Place __instance)
        {
            MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch.stuffDef = null;
        }
    }

    [HarmonyPatch(typeof(Designator_Install), "DrawGhost")]
    public static class Designator_Install_Patch
    {
        private static void Prefix(Designator_Install __instance)
        {
            MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch.stuffDef = __instance.StuffDef;
        }

        private static void Postfix(Designator_Install __instance)
        {
            MeditationUtility_CountsAsArtificialBuilding_ThingDef_Patch.stuffDef = null;
        }
    }
}
