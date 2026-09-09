# Boil Tainted Gear

A RimWorld 1.6 mod for cleaning looted apparel. Stoves and campfires gain a
**boil tainted gear** bill that washes corpse taint out of apparel - anything
that can become tainted (vanilla, DLC or modded) shows up in the bill filter
as usual, so you simply queue the pieces you want cleaned.

No dependencies, no research. Works with all DLC and mods by design, and is
safe to add or remove at any time.

## How to use

1. Build (or already own) an **electric stove, fueled stove or campfire** -
   no research is required, a campfire and a pot are enough for tribal
   colonies.
2. Open its bill list and add **boil tainted gear**. The ingredient filter
   lists apparel exactly like any other bill - restrict it by category,
   quality, hit points or whatever you need; untainted pieces can be selected
   in the dialog but are never actually boiled.
3. Pawns haul tainted apparel to the fire and boil it clean. The cleaned item
   keeps its quality, color, style and any mod-added parts, and is placed
   next to the fire for a hauler to carry back to your stockpile.

Boiling is deliberately not free: the scrubbing wears the item out, charging
**10% of the item's current durability** by default (rounded up, configurable
in the mod settings). An item is never destroyed by boiling - the worst case
leaves it at 1 hp.

## Mod settings

- **Enabled** - master switch. While off, boil bills find no ingredients and
  sit idle; they work again when re-enabled.
- **Durability cost** (default 10%) - share of *current* hit points charged
  per boil, rounded up. Applies from the next boiled item on.
- **Debug logging** - Off / Basic (mod load, every boiled item) / Verbose
  (per-item durability details and fallback paths).

## Known limitations

- Only apparel can become tainted in the game, so only apparel is accepted;
  the bill filter may show pieces whose def opted out of tainting
  (`careIfWornByCorpse`), but those are never actually boiled.
- Boiling one piece is one work iteration (one trip per item); use the bill
  filter and "do times"/"do forever" repeat modes to manage the queue.
- The bill cannot use the "do until you have X" repeat mode - there is no
  product to count, the same as vanilla's burn/destroy recipes.

## Technical notes

For modders and the curious - no def, DLC or mod lists are hardcoded:

- The bill is a single productless `RecipeDef` attached to `ElectricStove`,
  `FueledStove` and `Campfire` through vanilla's `recipeUsers` mechanism, so
  it behaves like any other recipe (work/skill/effect/sound, modded stoves
  can adopt it by adding the def to their recipe list).
- Vanilla destroys bill ingredients at job completion via the virtual
  `RecipeWorker.ConsumeIngredient`. The mod's worker overrides that hook:
  the ingredient is not destroyed but cleaned (`Apparel.WornByCorpse = false`)
  and charged the configured share of its current hit points, floored at
  1 hp. Quality, color, style and mod comps survive because the thing itself
  survives. No Harmony, no patches anywhere.
- The ingredient filter is a `ThingFilter` subclass
  (`ThingFilter_TaintedGear`) instantiated from XML via the `Class`
  attribute, because "currently tainted" is per-thing state no def-level
  filter field can express. The bill dialog's editable filter stays a plain
  `ThingFilter`, so players filter as usual; the subclass only ever removes
  untainted pieces.
- Taint itself is untouched vanilla state: it lands on apparel whose def
  cares (`ApparelProperties.careIfWornByCorpse`, default true) and is
  cleared here the same way vanilla resurrection clears it. The rotten tint
  is re-rendered away via `Notify_ColorChanged`.
- Debug logging (Basic/Verbose) covers load, settings and every boiled item.

## Build from source

Requires the .NET SDK and a RimWorld 1.6 install. Build the Release configuration for the dll you
ship - a plain `dotnet build` defaults to Debug:

```
cd Source/BoilTaintedGear
dotnet build -c Release -p:RimWorldDir="C:\Path\To\RimWorld"
```

The output lands in `Assemblies/BoilTaintedGear.dll`. Copy or symlink the whole
`BoilTaintedGear` folder into the game's `Mods` directory to try it.
