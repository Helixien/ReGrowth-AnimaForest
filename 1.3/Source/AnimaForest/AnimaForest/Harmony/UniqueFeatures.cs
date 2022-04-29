using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using RimWorld.Planet;
using System.Linq;
using System;
using RimWorld.BaseGen;
using Verse.AI;

namespace AnimaForest
{

    [HarmonyPatch(typeof(Fire))]
    [HarmonyPatch("SpawnSetup")]
    public static class Fire_SpawnSetup_Patch
    {
        public static void Postfix(Fire __instance, bool respawningAfterLoad)
        {
            if (!respawningAfterLoad && __instance.Map?.Biome == AF_DefOf.RG_AnimaForest)
            {
                if (__instance.Map.weatherManager.RainRate < 0.01f && __instance.Map.weatherManager.curWeather != AF_DefOf.Rain)
                {
                    __instance.Map.weatherManager.TransitionTo(AF_DefOf.Rain);
                }
            }
        }
    }

    [HarmonyPatch]
    public static class CastAbility_Patch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AccessTools.GetDeclaredMethods(typeof(Ability)).Where(x => x.Name == "Activate"))
            {
                yield return method;
            }
        }
        public static void Postfix(Ability __instance)
        {
            var hediff = __instance.pawn.health.hediffSet.GetFirstHediffOfDef(AF_DefOf.RG_PsychicBrainworm);
            if (hediff != null)
            {
                __instance.pawn.health.RemoveHediff(hediff);
                hediff = HediffMaker.MakeHediff(AF_DefOf.RG_PsychicBrainwormParalysis, __instance.pawn);
                __instance.pawn.health.AddHediff(hediff);
            }
        }
    }
}
