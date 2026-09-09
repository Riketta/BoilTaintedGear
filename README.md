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
**30% of the item's current durability** by default (rounded up, configurable
in the mod settings). An item is never destroyed by boiling - the worst case
leaves it at 1 hp. See *Balance* below for why 30%.

## Balance

One boil takes **3000 work** - 10x cooking a simple meal (300), 7.5x brewing
psychite tea (400), ~6.7x butchering a creature (450) and 50x just burning
the rags (60) - and needs a cook with **Cooking 2+** (the same vanilla skill
gate fine and lavish meals use). Speed follows the vanilla CookSpeed curve
(0.4x at skill 0 up to 1.6x at 20):

| Cooking skill | 2 | 5 | 10 | 20 |
|---|---|---|---|---|
| ticks per boiled item | ~5770 | ~4290 | 3000 | ~1880 |
| real time @1x speed | ~96 s | ~71 s | 50 s | ~31 s |
| in-game hours | ~2.3 | ~1.7 | ~1.2 | ~0.8 |

Cleaning a big raid's loot is a serious labor investment for rookie cooks -
and boiling trains Cooking as a side effect, so the chore eases as your cook
learns.

The durability charge defaults to **30% of the item's current hit points**
(configurable in the settings). That number is chosen so the *average* piece
of raider loot (about 84% of max, per the vanilla `gearHealthRange` rolls)
comes out at ~59% - right between vanilla's two usability lines. Above the
50% "ratty apparel" mood threshold, but through the 60% market-value cliff
(`StatPart_Health`: 60% HP sells for half, 50% HP for a tenth). Per tier:
elite gear (100%) cleans up to 70% and still sells well; pirate/mercenary
pieces (~97-98%) land at ~68%; scavenger gear (~74%) at ~52%; drifter rags
(~40%) turn into 28% junk - so it pays to cherry-pick what you boil. Armor
itself never degrades with HP in RimWorld, so a cleaned vest still protects
fully; the tax is mood and trade value. The charge stacks the usual apparel
policy pressure on top: repeatedly saved gear is still gear you'll want to
replace eventually.

## Mod settings

- **Enabled** - master switch. While off, boil bills find no ingredients and
  sit idle; they work again when re-enabled.
- **Durability cost** (default 30%) - share of *current* hit points charged
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
