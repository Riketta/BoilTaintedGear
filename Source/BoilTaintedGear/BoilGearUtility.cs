using RimWorld;
using UnityEngine;
using Verse;

namespace BoilTaintedGear
{
    /// <summary>Taint checks and the boil transform itself, shared by the
    /// bill filter and the recipe worker.</summary>
    internal static class BoilGearUtility
    {
        /// <summary>True when the thing is apparel that both can carry and
        /// currently carries corpse taint. Taint only ever lands on Apparel
        /// whose def cares about it (ApparelProperties.careIfWornByCorpse,
        /// default true - special items opt out via XML), so the same two
        /// fields gate the bill filter. Called from ingredient scans over
        /// candidate items: deliberately branch-cheap, no allocations.</summary>
        public static bool IsBoilable(Thing t)
        {
            return t is Apparel apparel
                && !apparel.Destroyed
                && apparel.def.apparel != null
                && apparel.def.apparel.careIfWornByCorpse
                && apparel.WornByCorpse;
        }

        /// <summary>Cleans one piece of tainted gear: clears the corpse taint
        /// and charges the configured durability cost - a share of the item's
        /// CURRENT hit points, rounded up, floored so an item can never be
        /// boiled to death. Quality, color, style and any mod comps survive
        /// untouched because the item itself survives the job.</summary>
        /// <returns>Hit points actually charged.</returns>
        public static int Boil(Apparel apparel)
        {
            int before = Mathf.Clamp(apparel.HitPoints, 1, Mathf.Max(1, apparel.MaxHitPoints));
            int cost = Mathf.CeilToInt(before * BoilTaintedGearMod.DurabilityCostPercent / 100f);
            int after = Mathf.Max(1, before - cost);

            apparel.WornByCorpse = false;
            apparel.HitPoints = after;
            // The rotten tint is derived from the taint flag at draw time;
            // dropping the cached graphics re-renders the item in its normal
            // color (same call vanilla's styling station uses after recolors).
            apparel.Notify_ColorChanged();

            return before - after;
        }
    }
}
