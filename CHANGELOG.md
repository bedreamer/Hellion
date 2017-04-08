## Hellion ChangeLog

### Update #17

...

### Update #16

Date: 08/04/2017

- World
  - Add `Hair Designer` NPC [PR #49](https://github.com/Eastrall/Hellion/pull/49)
  - Add `Makeup Artist` NPC [PR #50](https://github.com/Eastrall/Hellion/pull/50)
  - Idle Heal (HP, MP, FP)

### Update #15

Date: 25/03/2017

- Database
  - Add `StatPoints` field to `characters` table
  - Add `SkillPoints` field to `characters` table
- World
  - Monsters died and respawn after some time
  - Player win experience when killing monster
  - Player level-up

### Update #14

Date: 18/03/2017

- World
  - Load JobData (propJob.inc)
  - Damage formula
  - Miss / Blocking / Critical hit calculation
  - Knockback
  - Add Item refine / element / element refine

### Update #13

Date: 11/03/2017

- Common
  - Fix configuration hostname issue [PR #42](https://github.com/Eastrall/Hellion/pull/42) (by Almewty)
  - Remove unnecessary checks [Issue #44](https://github.com/Eastrall/Hellion/issues/44)
  - Rename ConfigurationManager to JsonHelper [Issue #43](https://github.com/Eastrall/Hellion/issues/43)
- World
  - Fix/optimize new walk algorithm [Issue #36](https://github.com/Eastrall/Hellion/issues/36)

### Update #12

Date: 04/03/2017

- Documentation
  - New packet documentation structure
- Remove ISC Project
- LoginServer
  - Built in with ISC
- World
  - Skills and Skill levels loading

### Update #11

Date : 24/02/2017

- World
  - WIP: New mover walk process algorithm in progress

### Update #10

Date: 17/02/2017

- Common
  - Fix database dump `sql/hellion.sql`
- Cluster
  - Fix delete character issue
  - Fix create character issue
- World
  - Monsters follow player when attacked


### Update #9 (10/02/2017)

- World
  - Command: /createitem
  - Save inventory items in database
  - Fly moves (need review in some points)
  - Follow movers (foot only)
  - Begin of fight system

### Update #8 (03/02/2017)

- Common
  - Add official packet and snapshot headers

- World
  - Improve moving system
  - Add Moving and and motion flags management
  - Moves with keyboard (wsad)
  - Jump

### Update #7 (27/01/2017)

- Common
  - Downgrade to .NET Core 1.0.3 LTS to avoid compatibility problems during releases.
  - Create `Hellion.Database` project to handle everyting realated witht the database.

- World
  - Npc data loading
  - Npc oral dialog
  - Npc dialog box
  - Teleport command

### Update #6 (13/01/2017)

- Common
  - NPC Dialog structure

- World
  - Add all GM and admin commands verification
  - Monster moves
  - Dialog loader

### Update #5 (06/01/2017)

- World
  - Monster visibility

### Update #4 (30/12/2016)

- Common
  - Update to .NET Core 1.1
  - Remove unused dependencies
  - Rgn file structure

- Tools
  - DefineToConst : Converts a flyff header file (.h) to a C# const file.
  
- Cluster
  - FIX: HP/MP/FP at character creation
  
- World
  - Chat system (normal chat)
  - Load region from .rgn files
  - Begin of monster respawner


### Update #3 (23/12/2016)

- Common
  - Add ResourceTable reader (to read files like propItem.txt, propSkills.txt, etc...)
  - Add WldFile structure

- LoginServer
  - Fix bug "Account already connected"

- WorldServer
  - Data loading
    - Items (propItem.txt)
  - Inventory
    - Viewable equiped items (Current player and others)
    - Item move
    - Equip/Unequip
  - Player
    - Save informations in database at logout


### Update #2 (16/12/2016)


- Common
  - New incoming packet management using attributes
  - Change folder structure

- World Server
    - Data loading
        - Defines loading
        - Map loading (.dyo)
    - Player login
    - Player visibility with other players
    - Player moves
    - NPC

### Update #1 (09/12/2016)

- InterServer Communication
- Login Server
    - Connect
    - Disconnect
- Cluster Server
    - Character List
    - Create character
    - Delete character
    - Login Protect (On/Off)
