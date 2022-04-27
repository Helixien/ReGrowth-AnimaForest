using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace AnimaForest
{
    public class AnimaForestTracker : MapComponent
    {
        private int lastHarmTick;

        public const int MinHarmonyDuration = GenDate.TicksPerDay * 7;
        public bool InHarmony => (Find.TickManager.TicksGame - lastHarmTick) >= MinHarmonyDuration;

        public Dictionary<Pawn, IntVec3> pawnsToTeleport = new Dictionary<Pawn, IntVec3>();

        private List<Pawn> pawnsToTeleportKeys;

        private List<IntVec3> pawnsToTeleportValues;
        public AnimaForestTracker(Map map) : base(map)
        {

        }
        public void RegisterHarm()
        {
            lastHarmTick = Find.TickManager.TicksGame;
            if (Rand.Chance(0.1f))
            {
                Utils.DoAnimaWrath(map);
            }
        }

        public int oreDepositIncidentEndTick;
        public IntVec3 exposedOreDepositPlace = IntVec3.Invalid;
        public IntVec3 StartExposedOreDepositIncident()
        {
            if (!exposedOreDepositPlace.IsValid && TryFindCell(out exposedOreDepositPlace))
            {
                oreDepositIncidentEndTick = Find.TickManager.TicksGame + 600;
                return exposedOreDepositPlace;
            }
            return IntVec3.Invalid;
        }
        public override void MapComponentTick()
        {
            base.MapComponentTick();
            if (pawnsToTeleport != null && pawnsToTeleport.Count > 0)
            {
                List<Pawn> keysToRemove = new List<Pawn>();
                foreach (var pawnData in pawnsToTeleport)
                {
                    var teleportComp = pawnData.Key?.TryGetComp<CompPawnTeleporter>();
                    if (teleportComp != null && teleportComp.disappear && Find.TickManager.TicksGame >= teleportComp.appearInTick)
                    {
                        GenPlace.TryPlaceThing(pawnData.Key, pawnData.Value, this.map,
                            ThingPlaceMode.Near, null, null, default(Rot4));
                        teleportComp.disappear = false;
                        if (teleportComp.Props.disableManhunterState && pawnData.Key.mindState?.mentalStateHandler?.CurStateDef == MentalStateDefOf.Manhunter)
                        {
                            pawnData.Key.mindState.mentalStateHandler.Reset();
                        }
                        keysToRemove.Add(pawnData.Key);
                        FleckMaker.Static(pawnData.Key.Position, this.map, FleckDefOf.PsycastAreaEffect, 10f);
                        //Log.Message("APPEARED");
                    }
                }
                foreach (var pawn in keysToRemove)
                {
                    pawnsToTeleport.Remove(pawn);
                }
            }

            if (exposedOreDepositPlace.IsValid)
            {
                if (oreDepositIncidentEndTick > Find.TickManager.TicksGame)
                {
                    Find.CameraDriver.shaker.DoShake(1f);
                    foreach (var cell in GenRadial.RadialCellsAround(exposedOreDepositPlace, ThingSetMaker_ExposedOreDeposit.MineablesCountRange.max / 4, true))
                    {
                        if (Rand.Chance(0.5f))
                        {
                            FleckMaker.ThrowDustPuffThick(cell.ToVector3Shifted() + new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f)), map, 1f, Color.green);
                        }
                    }
                }
                else
                {
                    List<Thing> list = AF_DefOf.RG_ExposedOreDeposite.root.Generate();
                    foreach (var cell in GenRadial.RadialCellsAround(exposedOreDepositPlace, ThingSetMaker_ExposedOreDeposit.MineablesCountRange.max / 4, true))
                    {
                        if (list.Any())
                        {
                            GenSpawn.Spawn(list[0], cell, map);
                            list.RemoveAt(0);
                        }
                    }

                    oreDepositIncidentEndTick = -1;
                    exposedOreDepositPlace = IntVec3.Invalid;
                }
            }
        }
        private bool TryFindCell(out IntVec3 cell)
        {
            int maxMineables = ThingSetMaker_ExposedOreDeposit.MineablesCountRange.max;
            return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, out cell, 10, default(IntVec3), -1, allowRoofedCells: true, allowCellsWithItems: false, allowCellsWithBuildings: false, colonyReachable: false, avoidColonistsIfExplosive: true, alwaysAvoidColonists: true, delegate (IntVec3 x)
            {
                int num = Mathf.CeilToInt(Mathf.Sqrt(maxMineables)) + 2;
                CellRect cellRect = CellRect.CenteredOn(x, num, num);
                int num2 = 0;
                foreach (IntVec3 item in cellRect)
                {
                    if (item.InBounds(map) && item.Standable(map))
                    {
                        num2++;
                    }
                }
                return num2 >= maxMineables;
            });
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastHarmTick, "lastHarmTick");
            Scribe_Values.Look(ref oreDepositIncidentEndTick, "oreDepositIncidentEndTick");
            Scribe_Values.Look(ref exposedOreDepositPlace, "exposedOreDepositPlace", IntVec3.Invalid);
            Scribe_Collections.Look(ref pawnsToTeleport, "pawnsToTeleport", LookMode.Deep, LookMode.Value, ref this.pawnsToTeleportKeys, ref this.pawnsToTeleportValues);
        }
    }
}
