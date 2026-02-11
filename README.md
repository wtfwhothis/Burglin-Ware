# Burglin'Ware
a shit codded Burglin' Gnomes cheat made in C# for MelonLoader.

![Version](https://img.shields.io/badge/version-3.0-blue)
![Game](https://img.shields.io/badge/game-Burglin'%20Gnomes%20Demo-green)
![Framework](https://img.shields.io/badge/framework-MelonLoader-orange)

## Releasing at 10 stars.<br>

<img width="1907" height="1055" alt="image" src="https://github.com/user-attachments/assets/a3f23b37-2b7e-4287-b880-5e058f05f006" />
<img width="1920" height="1038" alt="image" src="https://github.com/user-attachments/assets/50b07903-509f-4590-99f3-2cc04759ca94" />

## 🚀 Features Overview

### 📦 **TAB 1: ITEMS**
Spawn any resource item in the game.

| Feature | Description |
|---------|-------------|
| **Load Items List** | Dynamically loads all available items from game resources |
| **Scrollable Selection** | Browse through all items with a scrollable list |
| **Spawn Item** | Spawns selected item 2m in front of player |

---

### 🔧 **TAB 2: OBJECTS**
Spawn 30+ interactive objects instantly.

#### Available Objects:
- **Weapons**: Knife, Gun, Taser, Grenade, PepperSpray
- **Traps**: Mousetrap
- **Toys**: BearToy, CatToy, BallToy, RobotToy
- **Food**: Egg, Bread
- **Electronics**: Toaster, Radio, RecordPlayer, GameConsole, GameConsoleJoystick, Hairdryer
- **Kitchen**: Fork, Pan, Teapot, CatBowl
- **Home Items**: Globe, ClockTable, Slipper, Underpants, TrashBucket, Plunger, ToiletPaper

| Feature | Description |
|---------|-------------|
| **Quick Spawn** | Spawn any object with one click |
| **Find All Objects** | Debug tool to search scene for objects |

---

### 👤 **TAB 3: PLAYER (Local)**
Complete control over your character.

| Feature | Description |
|---------|-------------|
| **God Mode** | Continuous invincibility (auto-respawn) |
| **Fly Mode** | Free-flight with WASD + Space/Ctrl controls |
| **Fly Speed Slider** | Adjust flight speed from 1 to 50 |
| **Full Heal** | Instantly restore health |
| **Kill Player** | Suicide command |
| **Force Ragdoll** | Enable ragdoll physics manually |
| **Unragdoll** | Disable ragdoll and return to normal |

---

### 🌐 **TAB 4: TELEPORT (Local)**
Instant teleportation commands.

| Feature | Description |
|---------|-------------|
| **Teleport to Spawn** | Return to player spawn point |
| **Teleport Forward 10m** | Move 10 meters forward |
| **Teleport Up 5m** | Move 5 meters upward |

---

### 👥 **TAB 5: ONLINE (Multiplayer)**
Full multiplayer player control and trolling.

#### Player Management
| Feature | Description |
|---------|-------------|
| **Player List** | Shows all connected players with Steam names |
| **Auto Name Loading** | Async Steam name fetching |
| **[YOU] Indicator** | Clearly shows which player is you |

#### Basic Actions
| Feature | Description |
|---------|-------------|
| **Heal** | Heal selected player |
| **Kill** | Kill selected player |
| **Ragdoll** | Force ragdoll on player |
| **Unragdoll** | Remove ragdoll from player |

#### 🎪 Troll Options
| Feature | Description |
|---------|-------------|
| **Launch Up** | Launch player 50m into the air |
| **Explode** | Apply massive explosion force to ragdoll |
| **Spin** | Apply random angular velocity |
| **Freeze** | Freeze player ragdoll in place |
| **Dismember Head** | Remove player's head |
| **Dismember Legs** | Remove both legs |

#### 👹 Kidnapper Options
| Feature | Description |
|---------|-------------|
| **Spider Kidnap** | Force spider to grab player |
| **Vacuum Kidnap** | Force vacuum to suck up player |
| **Human Kidnap** | Force neighbor to kidnap player |
| **Release All** | Free player from all kidnappers |

#### Teleport Options
| Feature | Description |
|---------|-------------|
| **Teleport to Me** | Bring selected player to your location |
| **Teleport Me to Them** | Go to selected player's location |

---

### 🤖 **TAB 6: NPCs**
Control all NPCs in the game.

#### 🎩 BOB (Gnome Thief)
| Feature | Description |
|---------|-------------|
| **TP Bob to Me** | Teleport Bob 3m in front of you |
| **TP Bob Away** | Send Bob to void (y=-1000) |
| **Kill Bob** | Destroy Bob permanently |
| **Make Bob Drop** | Force Bob to drop carried items |

#### 🏠 HUMAN (Neighbor)
| Feature | Description |
|---------|-------------|
| **TP Human to Me** | Teleport neighbor 3m in front of you |
| **TP Human Away** | Send neighbor to void (y=-1000) |
| **Kill Human** | Deal 999 damage to neighbor |
| **Release Player** | Free any kidnapped player |
| **Drop Gun** | Make neighbor drop weapon |
| **Make Naked** | Toggle naked/clothed mesh state |

#### General
| Feature | Description |
|---------|-------------|
| **Find All NPCs** | Debug search for Bob and Human in console |

---

### 🎉 **TAB 7: FUN**
Chaos and entertainment features.

#### 🚗 RC CARS (Sub-Tab)

##### Car List & Selection
| Feature | Description |
|---------|-------------|
| **Auto-Detection** | Finds all RC Cars in scene |
| **Driver Display** | Shows Steam name of current driver |
| **Scrollable List** | Browse all cars with status |

##### Individual Car Control
| Feature | Description |
|---------|-------------|
| **TP to Me** | Teleport car 3m in front of you |
| **TP Me to Car** | Teleport yourself to the car |
| **Launch Up** | Launch car 30m upward |
| **Flip** | Reset car rotation to upright |
| **Spin** | Apply random angular velocity |
| **Freeze** | Stop all velocity (linear & angular) |
| **Super Speed** | Boost forward velocity to 50 m/s |
| **Duplicate** | Clone the selected car (networked) |
| **Eject Driver** | Remove current driver |
| **Delete Car** | Destroy selected car |

##### Mass Actions
| Feature | Description |
|---------|-------------|
| **Launch All** | Launch every RC Car in scene |
| **Super Speed All** | Boost all cars simultaneously |
| **Delete All** | Remove all RC Cars from game |

#### 🌪️ VACUUM (Sub-Tab)
| Feature | Description |
|---------|-------------|
| **Status Display** | Shows if vacuum is ON or OFF |
| **TP Vacuum to Me** | Teleport vacuum 3m in front of you |
| **TP Me to Vacuum** | Teleport yourself to vacuum |
| **Toggle ON/OFF** | Switch vacuum power state |
| **Release All Players** | Free all trapped players |

#### 🕷️ SPIDER (Sub-Tab)
| Feature | Description |
|---------|-------------|
| **Status Display** | Shows if spider is holding a player |
| **TP Spider to Me** | Teleport spider 3m in front of you |
| **TP Me to Spider** | Teleport yourself to spider |
| **Kill Spider** | Destroy the spider |
| **Release Player** | Free trapped player |

---

## ⌨️ Controls

| Key | Action |
|-----|--------|
| **INSERT** | Toggle menu ON/OFF |
| **WASD** | Fly Mode movement (when enabled) |
| **SPACE** | Fly Mode up (when enabled) |
| **LEFT CTRL** | Fly Mode down (when enabled) |

---

## 📋 Installation

1. **Install MelonLoader** for Burglin' Gnomes Demo
2. Download `GnomeCheat.dll`
3. Place in `[Game Directory]/Mods/`
4. Launch game
5. Press **INSERT** to open menu

---

## ⚠️ Disclaimer

This mod is for **educational and entertainment purposes only**. Use responsibly in multiplayer games. Griefing or ruining others' gameplay experience is discouraged.

---

## 📫 Support

For bugs, suggestions, or feedback, please open an issue on GitHub.

---

**Enjoy the chaos! 🎮🔥**
