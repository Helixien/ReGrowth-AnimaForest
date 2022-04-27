using System;
using RimWorld;
using Verse;

namespace AnimaForest
{
	[DefOf]
	public static class AF_DefOf
	{
		static AF_DefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AF_DefOf));
		}

		public static BiomeDef RG_AnimaForest;
		public static TerrainDef RG_AnimaSoilCracked;
		public static ThingDef RG_Jadeite;
		public static WeatherDef RG_PsychicFog;
		public static HediffDef RG_PsychicFogEffect;
		public static WeatherDef RG_PsychicStorm;
		public static MentalStateDef RG_Wander_Psychotic_Short;

		public static GameConditionDef RG_PsychicStormGC;
		public static GameConditionDef RG_PsychicFlareGC;
		public static IncidentDef RG_PsychicShriek;
		public static IncidentDef RG_AnimalInsanityPulse;
		public static ThingSetMakerDef RG_ExposedOreDeposite;
		public static GameConditionDef RG_AnimaSoothe;
		public static HediffDef RG_PsychicBrainworm;
		public static HediffDef RG_PsychicBrainwormParalysis;
		public static MentalStateDef WanderConfused;
		public static PawnKindDef RG_ArchoEntity_Conservator;
	}
}
