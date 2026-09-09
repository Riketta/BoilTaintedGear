using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BoilTaintedGear
{
    /// <summary>Bill worker for the boil recipe. Vanilla destroys bill
    /// ingredients at completion through this virtual call
    /// (Toils_Recipe.ConsumeIngredients - RecipeWorker.ConsumeIngredient),
    /// so overriding it is the whole mod: the gear survives the job instead -
    /// taint cleared, durability charged - and stays where the pawn placed it
    /// for the next hauler to pick up. No Harmony, no patches, nothing
    /// hardcoded: only this mod's def uses this worker class, and the recipe
    /// itself stays productless exactly like vanilla's burn/destroy recipes.</summary>
    public class RecipeWorker_BoilGear : RecipeWorker
    {
        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
        {
            if (ingredient is Apparel apparel && BoilGearUtility.IsBoilable(apparel))
            {
                int charged = BoilGearUtility.Boil(apparel);
                if (DebugLog.VerboseEnabled)
                {
                    DebugLog.Verbose("boiled " + apparel.LabelCap + " (" + apparel.def.defName
                        + "), durability charged: " + charged
                        + ", now " + apparel.HitPoints + "/" + apparel.MaxHitPoints + " hp.");
                }
                return;
            }
            // Unreachable through the bill filter; keep vanilla consume
            // semantics so a weird ingredient can never silently vanish in a
            // bill that promises to hand the item back.
            DebugLog.Warning("skipped non-tainted ingredient, consuming it the vanilla way: " + ingredient);
            base.ConsumeIngredient(ingredient, recipe, map);
        }

        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            if (DebugLog.MessageEnabled && ingredients.Count > 0)
            {
                string who = billDoer?.LabelShortCap ?? "someone";
                DebugLog.Message(who + " boiled " + ingredients.Count + " tainted item(s).");
            }
        }
    }
}
