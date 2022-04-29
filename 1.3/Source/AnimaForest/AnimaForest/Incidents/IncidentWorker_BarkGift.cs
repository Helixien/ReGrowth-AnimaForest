using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace AnimaForest
{
    public class IncidentWorker_BarkGift : IncidentWorker_AnimaForestHarmony
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = parms.target as Map;
            var trees = map.listerThings.AllThings.Where(x => x is Plant && x.def.plant.IsTree).ToList();
            var woods = new List<Thing>();
            var centerCell = FindCenterColony(map);
            trees = trees.OrderBy(x => x.Position.DistanceTo(centerCell)).Take(Rand.RangeInclusive(5, 13)).ToList();
            foreach (var tree in trees)
            {
                var wood = ThingMaker.MakeThing(ThingDefOf.WoodLog);
                wood.stackCount = Rand.RangeInclusive(5, 15);
                GenPlace.TryPlaceThing(wood, tree.Position, tree.Map, ThingPlaceMode.Direct);
                woods.Add(wood);
            }
            SendStandardLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.PositiveEvent, parms, woods);
            return true;
        }
        public static IntVec3 FindCenterColony(Map map)
        {
            var colonyPawns = map.mapPawns.FreeColonists.Select(x => x.Position);
            if (colonyPawns.Any())
            {
                var x_Averages = colonyPawns.OrderBy(x => x.x);
                var x_average = x_Averages.ElementAt(x_Averages.Count() / 2).x;
                var z_Averages = colonyPawns.OrderBy(x => x.z);
                var z_average = z_Averages.ElementAt(z_Averages.Count() / 2).z;
                var middleCell = new IntVec3(x_average, 0, z_average);
                return middleCell;
            }
            return map.Center;
        }
    }
}
