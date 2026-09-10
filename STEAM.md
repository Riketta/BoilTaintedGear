# Steam Workshop description

Paste the text below into the Workshop item's description field when publishing
(the BBCode renders on Steam, but not in-game - `About/About.xml` carries its own
plain-text description).

```
[h3]Boil Tainted Gear[/h3]
Stoves and campfires gain a "boil tainted gear" bill that washes the death out of apparel. Looted tainted gear no longer has to be worn with disgust, sold cheap or burned - boil it clean and wear it.

[b]Balanced by design:[/b] no research and no ingredients - the price is paid in slow, skill-gated work at the stove and a permanent durability tax on every wash. Cleaned pieces end up usable but second-hand: good loot is worth saving, junk is not.

[h3]What it does[/h3]
[list][*]Electric stoves, fueled stoves and campfires gain a new bill: [b]boil tainted gear[/b].
[*]Anything that can become tainted shows up in the bill filter as usual - vanilla, DLC and modded apparel alike. Restrict the filter by category, quality or whatever you need; untainted pieces are never actually boiled.
[*]A pawn hauls the tainted apparel to the fire, boils it clean, and the item is placed next to the fire for a hauler to carry back. Quality, color, style and mod-added parts are all kept - only the taint goes.
[*]Boiling costs durability: a share of the item's [b]current[/b] hit points (25% by default, rounded up), tuned so the average piece of looted gear comes out wearable but clearly second-hand - good pieces survive nicely, junk stays junk. An item is never destroyed by boiling - the worst case leaves it at 1 hp.
[*]No research required: a campfire and a big pot are enough, so tribal colonies can clean loot from day one.[/list]

[h3]Settings[/h3]
Master switch, durability cost slider (0-90%, default 25%), and debug logging (Off / Basic / Verbose).

[h3]Things to keep in mind[/h3]
[list][*]Boiling is slow on purpose: one item takes ten times the work of cooking a simple meal, and the bill needs a cook with at least 2 Cooking skill. Low-skill cooks are much slower still - a fresh cook needs over two in-game hours per piece, a legendary one around half a minute. Cleaning a whole raid's loot is a serious labor commitment, not a side chore.
[*]The durability numbers are best-case: armor absorbs damage by taking hits itself, and gear left outdoors deteriorates - pieces that went through a real fight are already below their condition roll before the boil, so cleaned loot usually lands even lower than the percentages suggest.
[*]One boil is one work iteration per item - large piles of loot mean large piles of hauling and cooking work. Balance!
[*]The bill has no "do until you have X" repeat mode, since there is no product to count (same as vanilla's burn recipes).[/list]

[h3]Compatibility[/h3]
Requires RimWorld 1.6; all DLCs are optional, no other mods needed.
Nothing is hardcoded to specific items: the bill is an ordinary recipe attached through the vanilla recipeUsers mechanism, and the cleaning itself reuses the game's own taint flag. Safe to add or remove at any time.

Source code and details: [url]https://github.com/Riketta/BoilTaintedGear[/url]
```
