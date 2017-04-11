# Hellion

[![Build Status](https://travis-ci.org/Eastrall/Hellion.svg?branch=develop)](https://travis-ci.org/Eastrall/Hellion)
[![experimental](http://badges.github.io/stability-badges/dist/experimental.svg)](https://github.com/Eastrall/Hellion/tree/develop)
[![discord](https://discordapp.com/api/guilds/294405146300121088/widget.png)](https://discord.gg/zAT6Az2)


Hellion is a FlyForFun V15 emulator built with C# and using the .NET Core 1.0 Framework.

This project has been created for learning purposes about the network and game logic problematics on the server-side.

We choose to use the [Ether.Network][ethernetwork] because it provides a clients management system and also a robust packet management system entirely customisable.

## Details

- Language: `C#`
- Framework target : `.NET Core 1.0.3`
- Application type: `Console`
- Database type : `MySQL`
- Configuration files type: `JSON`
- External libraries used:
	- [Ether.Network][ethernetwork]
	- EntityFrameworkCore


## Project features

- Login Server
- Cluster Server
    - Create/Delete character
    - LoginProtect (On/Off on configuration file)
    - Select character
- World Server
    - Data loading
      - Defines / Texts
      - Items
      - Maps
      - Npc
      - Dialogs
    - Player login
    - Player visibility with other players
    - Player moves (Mouse & keyboard)
    - Player attributes
    - Player flags
    - Chat
      - Normal chat
    - NPC
      - Visibility
      - Dialogs
    - Inventory
      - Move / Equip / Unequip
      - Save
    - Monsters
      - Visibility
      - Moves
    - Flying system (need review)
    
## Todo list

- NPC
    - Shops
- Inventory
  - Use items
  - Sets bonus
- Battle system
- Classes
- Skills
- Quest system
- Friends
- Mails
- Guilds

## How to use

1. Clone this repository
2. Install MySQL Server on your computer/server
3. Execute the `sql/hellion.sql` file in your MySQL server to setup the database.
4. Configure the 3 servers (Login, Cluster, World)
5. Restore all dependencies using `restore.bat`
6. Start the servers
   - Start `binary/LoginServer.bat`
   - Start `binary/ClusterServer.bat`
   - Start `binary/WorldServer.bat`
7. You are now ready to play!

[ethernetwork]: https://github.com/Eastrall/Ether.Network
