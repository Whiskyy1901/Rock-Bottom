# ROCK BOTTOM

A text-based caveman RPG for the terminal, written in C#. You wake up in a cave, bruised and confused. Explore the wilds, fight beasts, upgrade your gear, and track down whatever is shaking the ground.

## Running the game

1. Download the latest `.exe` from the project's **Releases** page.
2. Run it by double-clicking it, or from a terminal.

A terminal with colour support is recommended (Windows Terminal and the default console both work).

Saves are written to a `Saves` folder next to wherever you run the game from, so keep the exe in its own folder.

## How to play

Type a letter and press Enter to pick a menu option.

### Main menu

| Key | Action |
|-----|--------|
| E | Explore: random fight, coin stash, or healing bottle |
| R | Rest: restore full health (25% chance of being ambushed) |
| C | Camp shop: spend coins on upgrades |
| L | Lair of the T-Rex: appears once unlocked, until you beat the boss |
| T | Text speed: cycle Normal, Fast, Instant |
| Q | Save and quit |

### Combat

| Key | Action |
|-----|--------|
| A | Attack: deal full damage, take a full hit back |
| D | Defend: deal half damage, take a much weaker hit, 30% chance of a counter strike |
| R | Run: 2 in 3 chance to escape (not possible against the boss) |
| H | Heal: drink a bottle to restore health |

Attacks have a 10% chance to miss and a 10% chance to crit for double damage. You also have a 10% chance to dodge a normal enemy attack.

Winning fights earns coins and XP.

### Shop

| Item | Price | Effect |
|------|-------|--------|
| Armour | 100 × (armour + 1) | Reduces damage taken |
| Weapon | 50 × damage | Increases damage |
| Heals | 30 + 10 × difficulty | One more healing bottle |
| Difficulty | 500 × (difficulty + 1) | Tougher beasts, bigger coin rewards |

### The T-Rex

Every victory counts toward unlocking the boss. After 7 victories the Lair of the T-Rex opens, with a few story scenes along the way.

Beat it to see the ending. You can then keep exploring or quit.

## Saving

The game saves automatically after every action and when you quit. At startup you can pick an existing character by id or type `new` to start a new game. Dead characters stay in the list but can't be loaded, and finished characters are marked as victorious.
