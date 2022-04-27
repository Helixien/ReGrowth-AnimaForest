using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AnimaForest
{
    public class CompProperties_PawnTeleporter : CompProperties
    {
        public CompProperties_PawnTeleporter()
        {
            this.compClass = typeof(CompPawnTeleporter);
        }

        public float minDistance;

        public int cooldown;

        public bool disableManhunterState = false;

        public float nonCombatTeleportChance;
    }
    public class CompPawnTeleporter : ThingComp
    {
        public bool disappear = false;

        public int appearInTick = 0;

        private int readyToUseTicks = 0;
        public CompProperties_PawnTeleporter Props
        {
            get
            {
                return (CompProperties_PawnTeleporter)this.props;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.parent is Pawn pawn && pawn.health.summaryHealth.SummaryHealthPercent < 0.5f
                    && CanUseTeleport())
            {
                var hostiles = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget).Where(x
                    => x.HostileTo(pawn));
                IntVec3 loc = IntVec3.Invalid;
                if (CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) =>
                    hostiles.Where(y => y.Position.DistanceTo(x) > Props.minDistance).Count() == 0 && x.Walkable(this.parent.Map) && !x.Fogged(this.parent.Map),
                    pawn.Map, out loc))
                {
                    Teleport(pawn, loc);
                }
            }
        }

        public bool CanUseTeleport()
        {
            return Find.TickManager.TicksGame >= readyToUseTicks;
        }

        public void Teleport(Pawn pawn, IntVec3 loc)
        {
            Log.Message("Teleporting");
            FleckMaker.Static(pawn.Position, pawn.Map, FleckDefOf.PsycastAreaEffect, 10f);
            disappear = true;
            appearInTick = Find.TickManager.TicksGame + (60 * Rand.RangeInclusive(1, 10));
            var mapComp = pawn.Map.GetComponent<AnimaForestTracker>();
            if (mapComp.pawnsToTeleport == null) mapComp.pawnsToTeleport = new Dictionary<Pawn, IntVec3>();
            mapComp.pawnsToTeleport[pawn] = loc;
            pawn.DeSpawn(DestroyMode.Vanish);
            readyToUseTicks = Find.TickManager.TicksGame + Props.cooldown;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref readyToUseTicks, "readyToUseTicks", 0);
            Scribe_Values.Look<int>(ref appearInTick, "appearInTick", 0);
            Scribe_Values.Look<bool>(ref disappear, "disappear", false);
        }
    }
}

