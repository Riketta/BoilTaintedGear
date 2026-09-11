using UnityEngine;
using Verse;

namespace BoilTaintedGear
{
    /// <summary>Debug verbosity levels. Stored as int in settings so enum
    /// reordering can never corrupt saves.</summary>
    public enum DebugLogLevel
    {
        Off = 0,
        Basic = 1,
        Verbose = 2
    }

    public class BoilTaintedGearSettings : ModSettings
    {
        public bool enabled = true;

        /// <summary>Durability cost of one boil, in percent of the item's
        /// CURRENT hit points (default 20). Clamped 0..90 when read; boiling
        /// never destroys an item either way. The default is tuned so the
        /// average piece of raider loot (~84% of max HP, see vanilla
        /// gearHealthRange rolls) lands at ~67% - above both usability
        /// lines: safely clear of the 50% "ratty apparel" mood line and
        /// over the 60% market-value cliff of StatPart_Health (cleaned
        /// gear keeps roughly 60% of its trade value). Combat wear makes
        /// real-world loot land lower still.</summary>
        public int durabilityCostPercent = 20;

        public int debugLevel = (int)DebugLogLevel.Off;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref enabled, "enabled", true);
            Scribe_Values.Look(ref durabilityCostPercent, "durabilityCostPercent", 20);
            Scribe_Values.Look(ref debugLevel, "debugLevel", (int)DebugLogLevel.Off);
        }
    }

    public class BoilTaintedGearMod : Mod
    {
        public const string PackageId = "Riketta.BoilTaintedGear";

        public static BoilTaintedGearSettings Settings;

        /// <summary>Master switch, read by the bill filter on each call.
        /// Null-safe: without settings the bills stay available rather than
        /// silently idling.</summary>
        public static bool Active => Settings?.enabled ?? true;

        /// <summary>Durability cost per boil in percent of current hit
        /// points, read at the moment a job completes so setting changes
        /// apply from the next boiled item on.</summary>
        public static int DurabilityCostPercent =>
            Mathf.Clamp(Settings?.durabilityCostPercent ?? 20, 0, 90);

        public BoilTaintedGearMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<BoilTaintedGearSettings>();
            DebugLog.Message("loaded (enabled=" + (Settings.enabled ? "true" : "false")
                + ", durabilityCost=" + DurabilityCostPercent + "%"
                + ", debugLevel=" + (DebugLogLevel)Settings.debugLevel + ").");
        }

        public override string SettingsCategory()
        {
            return "BoilTaintedGear.SettingsCategory".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);

            bool enabled = Settings.enabled;
            list.CheckboxLabeled("BoilTaintedGear.Enabled".Translate(), ref enabled,
                "BoilTaintedGear.Enabled.Tip".Translate());
            Settings.enabled = enabled;
            list.Gap(6f);

            float cost = Settings.durabilityCostPercent;
            cost = list.SliderLabeled("BoilTaintedGear.DurabilityCost".Translate(cost),
                cost, 0f, 90f, 0.6f, "BoilTaintedGear.DurabilityCost.Tip".Translate());
            Settings.durabilityCostPercent = Mathf.RoundToInt(cost);
            list.Gap(6f);

            Rect debugRect = list.GetRect(30f);
            string levelName = ((DebugLogLevel)Settings.debugLevel).ToString();
            if (Widgets.ButtonText(debugRect, "BoilTaintedGear.DebugLevel".Translate(levelName)))
            {
                Settings.debugLevel = (Settings.debugLevel + 1) % 3;
            }
            TooltipHandler.TipRegion(debugRect, "BoilTaintedGear.DebugLevel.Tip".Translate());
            list.Gap(12f);

            GUI.color = ColoredText.SubtleGrayColor;
            list.Label("BoilTaintedGear.BehaviorNote".Translate());
            GUI.color = Color.white;
            list.End();
        }
    }
}
