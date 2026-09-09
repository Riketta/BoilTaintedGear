using Verse;

namespace BoilTaintedGear
{
    /// <summary>Optional layered logging, off by default (see mod settings).
    /// Basic: mod loading, settings changes, every boiled item.
    /// Verbose: additionally per-item durability details and fallback paths.
    /// Errors are always logged regardless of level.</summary>
    internal static class DebugLog
    {
        private const string Prefix = "[BoilTaintedGear] ";

        private static bool AtLeast(DebugLogLevel level)
        {
            return (BoilTaintedGearMod.Settings?.debugLevel ?? (int)DebugLogLevel.Off) >= (int)level;
        }

        /// <summary>Hot-path guards: check these BEFORE building log strings
        /// so disabled logging costs nothing but a property read. Verbose
        /// implies Message.</summary>
        public static bool MessageEnabled => AtLeast(DebugLogLevel.Basic);

        public static bool VerboseEnabled => AtLeast(DebugLogLevel.Verbose);

        /// <summary>Basic-level message (player-visible state changes).</summary>
        public static void Message(string message)
        {
            if (AtLeast(DebugLogLevel.Basic))
            {
                Log.Message(Prefix + message);
            }
        }

        /// <summary>Verbose-level message (per-item details, edge cases).</summary>
        public static void Verbose(string message)
        {
            if (AtLeast(DebugLogLevel.Verbose))
            {
                Log.Message(Prefix + message);
            }
        }

        /// <summary>Unconditional warning (degraded behavior, not a crash).</summary>
        public static void Warning(string message)
        {
            Log.Warning(Prefix + message);
        }
    }
}
