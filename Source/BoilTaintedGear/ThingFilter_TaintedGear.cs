using Verse;

namespace BoilTaintedGear
{
    /// <summary>A ThingFilter that only passes apparel currently carrying
    /// corpse taint. Instantiated from XML via the Class attribute on the
    /// recipe ingredient filter (&lt;filter Class="BoilTaintedGear.ThingFilter_TaintedGear"&gt;),
    /// which is the only reliable way to gate ingredients per-thing: taint is
    /// per-instance state that no def-level filter field can express.
    ///
    /// Every ingredient validation path consults this override
    /// (WorkGiver_DoBill.IsUsableIngredient, Bill.IsFixedOrAllowedIngredient,
    /// RecipeDef.PotentiallyMissingIngredients). The bill dialog's editable
    /// filter stays a plain ThingFilter seeded from defaultIngredientFilter,
    /// so players filter apparel exactly as usual - this check only removes
    /// untainted pieces the user filter would otherwise allow.
    ///
    /// Gated on the mod's Enabled switch so a disabled mod pauses its bills
    /// (they simply find no ingredients) instead of consuming gear nobody
    /// asked to boil.</summary>
    public class ThingFilter_TaintedGear : ThingFilter
    {
        public override bool Allows(Thing t)
        {
            if (!BoilTaintedGearMod.Active || !BoilGearUtility.IsBoilable(t))
            {
                return false;
            }
            return base.Allows(t);
        }
    }
}
