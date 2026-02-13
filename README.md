# Burglin'Ware

A cheat mod for Burglin' Gnomes, built in C# for MelonLoader.

![Version](https://img.shields.io/badge/version-2.0-blue)
![Game](https://img.shields.io/badge/game-Burglin'%20Gnomes-green)
![Framework](https://img.shields.io/badge/framework-MelonLoader-orange)

---

<img width="1920" height="1042" alt="image" src="https://github.com/user-attachments/assets/34a1685b-1f03-4c0f-86dc-49d080522028" />
<img width="1907" height="1055" alt="image" src="https://github.com/user-attachments/assets/db316e16-f595-4cac-8912-994d47fb5d60" />
<img width="1920" height="1038" alt="image" src="https://github.com/user-attachments/assets/bafbf635-3d2e-4bb8-bf3a-095c8d489348" />

---

## Installation

1. Install [MelonLoader](https://melonwiki.xyz/) for Burglin' Gnomes
2. Download `GnomeCheat.dll`
3. Place it in `[Game Directory]/Mods/`
4. Launch the game
5. Press **INSERT** or **N** to toggle the menu

---

## Controls

| Key | Action |
|-----|--------|
| INSERT | Toggle menu |
| WASD/ZQSD | Fly / Noclip movement |
| Space | Fly up |
| Left Ctrl | Fly down |
| Left Shift | Fly speed boost (x2) |

---

## Features

### Tab 1 - Items

Spawn any resource item from the game's internal item database.

- Dynamic item list loaded from game resources
- Scrollable selection list
- Spawns selected item 2m in front of player

### Tab 2 - Objects

Spawn 30+ interactive objects with one click.

| Category | Objects |
|----------|---------|
| Weapons | Knife, Gun, Taser, Grenade, PepperSpray |
| Traps | Mousetrap |
| Toys | BearToy, CatToy, BallToy, RobotToy |
| Food | Egg, Bread |
| Electronics | Toaster, Radio, RecordPlayer, GameConsole, GameConsoleJoystick, Hairdryer |
| Kitchen | Fork, Pan, Teapot, CatBowl |
| Home | Globe, ClockTable, Slipper, Underpants, TrashBucket, Plunger, ToiletPaper |

### Tab 3 - Self (Local Player)

Full control over your own character.

**Toggles:**
- God Mode — continuous invincibility with auto-respawn
- Fly Mode — free-flight with camera-relative WASD movement, shift for speed boost
- Noclip — walk through walls (layer-based, no physics disruption)
- ~~Invisibility (Network) — hidden from all players via render + scale manipulation~~
- Infinite Arms — max reach distance, no arm break/dismemberment
- ~~Strong Arms — carry any object regardless of weight~~
- No Ragdoll — prevents ragdolling
- Anti-Kidnap — auto-untie, blocks spider/vacuum/human grabs
- Infinite Stamina — stamina never depletes

**Actions:**
- Full Heal / Kill Player
- Force Ragdoll / Unragdoll

**Speed:** adjustable multiplier (1x to 10x) with slider and quick presets

**Disguise:** Bob Disguise, Mole Disguise — replaces player model with NPC model while hiding your own

### Tab 4 - Teleport

- Teleport to Spawn
- Teleport Forward 10m
- Teleport Up 5m

### Tab 5 - Online (Multiplayer)

Player list with Steam name resolution and `[YOU]` indicator for local player.

**Basic Actions:** Heal, Kill, Ragdoll, Unragdoll

**Troll Options:**

| Action | Description |
|--------|-------------|
| Launch Up | Ragdoll + 50m upward force |
| Explode | Explosion force on ragdoll |
| Spin | Random angular velocity |
| Freeze | Zero all ragdoll velocity |
| Dismember Head | Remove head via network RPC |
| Dismember Legs | Remove both legs via network RPC |
| Throw Knife | Spawn and throw knife at player |
| Shoot | Spawn and shoot gun at player |
| Grenade | Spawn grenade at player position and instant-explode |
| Blind | Trigger blind effect on player |
| RC Kamikaze | Drive RC car into player at max speed and explode |
| Tornado | Continuous ragdoll + spin effect |
| Freeze Player | Lock player in place |
| Spectate | Attach camera to player |
| Headstand | Continuously teleport on top of player |

**Spawn NPC on Player:** select enemy type (Redcap, Rat, Roach, Cat, Human, Mole) with quantity slider and spawn them at the target player's position.

**Kidnapper Options:** Spider Kidnap, Vacuum Kidnap, Human Kidnap, Release All

**Chaos:**

| Action | Description |
|--------|-------------|
| Set on Fire / Extinguish | Apply or remove fire effect |
| Stun (5s / 15s) | Stun player for duration |
| Tie Up / Untie | Bind or free player |
| Rain Grenades | Spawn multiple grenades above player |

**Teleport:** Teleport target to you, or yourself to target

**Admin:** Kick Player (host only)

### Tab 6 — NPCs

Two sub-tabs: **NPCs** and **Spawner**.

**NPC Control:**

| NPC | Actions |
|-----|---------|
| Bob (Gnome Thief) | TP to me, TP away, Kill, Make drop items |
| Human (Neighbor) | TP to me, TP away, Kill, Release player, Drop gun, Make naked |
| Cat | TP to me, Cat attack, Spam meow, Spam hiss |

- Find All NPCs (debug log)
- Kill All Enemies — iterates all GameEntityAI and kills them
- Despawn All Enemies — calls AiDirector.DespawnEnemies()

**Enemy Spawner:**

Select enemy type from: Redcap, Rat, Roach, Cat, Human, Mole. Adjust quantity with slider (1-50). Quick spawn buttons: x1, x5, x10, x25. Enemies spawn at local player position with random offset, using reflection to access AiDirector enemy configs.

### Tab 7 — Miscs

Five sub-tabs: **RC Cars**, **Vacuum**, **Spider**, **Mass**, **World**.

**RC Cars:**
- Auto-detection of all RC Cars in scene with driver display
- Per-car controls: TP to me, TP me to car, Launch up, Flip, Spin, Freeze, Super Speed, Duplicate, Eject driver, Delete
- Spawn RC Car (also available when no cars exist)
- Mass actions: Launch all, Super Speed all, Delete all

**Vacuum:**
- Status display (ON/OFF)
- TP Vacuum to me / TP me to Vacuum
- Toggle ON/OFF
- Release all players

**Spider:**
- Status display (holding player or not)
- TP Spider to me / TP me to Spider
- Kill Spider
- Release player

**Mass Actions (Host):**
- Kill All, Heal All, Ragdoll All, Launch All
- Set All on Fire, Extinguish All
- Stun All, Tie All, Untie All
- TP All to Me

**Chaos:**
- Item Rain (30 items)
- Grenade Rain (15 grenades)
- LAG ALL (500 objects) / MEGA LAG (2000 objects)

**World:**
- Open All Doors (force push)
- Explode All Grenades

**Exploits:**
- Crash All Players — ToggleGraphics corruption + network resync causes NullReferenceException on all clients

### Tab 8 — Visuals (ESP)

Four sub-tabs: **Players**, **NPCs**, **Items**, **Settings**.

**Player ESP:** boxes, names, health bars, distance, tracers

**NPC ESP:** toggle per NPC type (Bob, Human, Spider) with boxes, names, distance, tracers

**Item ESP:** names, distance, tracers

**Settings:**
- Max render distance slider
- Quick toggles: All ON/OFF, All Tracers ON/OFF, All Boxes ON/OFF

---

## Disclaimer

This mod is for educational and entertainment purposes only. Use at your own risk.

---

## Support

For bugs, suggestions, or feedback, open an issue on GitHub.
