
# Mobile MMORPG
A nameless (yet) customizable mobile mmorpg in .net core (Server) and Unity (Client)

## About

The infinite chunk based map loaded from tiled will be serve for this dungeon party oriented mmorpg, where players will kill creatures to gain power and items. The idea is to start as a simple casual mmorpg grinder with a simple historyline.

### Features in initial roadmap:

- Good Character Customization.
- Few monsters, but different in behaviour and strategy.
- Monsters drop materials, and thats it.
- Automatic Party, everyone gets EXP.
- Harvesting skills to collect materials.
- Crafting skills to craft items.
- PvP Zones.
- "Quest" System ( simplistic )
- Server Easily Moddable / Plugins System
- React WebPanel

## Requirements

- [Unity3d](#%20MobileMMORPG%20A%20very%20nameless%20%28yet%29%20mobile%20mmorpg%20in%20.net%20core%20%28Server%29%20and%20Unity%20%28Client%29%20%20##%20Requirements%20%20-Unity3d%20https://unity3d.com/pt/get-unity/download) 
- [Redis Database](https://redis.io/download)
- [.NET Core SDK](https://www.microsoft.com/net/download)
- [Microsoft .NET Framework 3.5](https://www.microsoft.com/pt-br/download/details.aspx?id=21)

### How to Run

- Start Redis Database
- Open the Server solution and click "Run Project"
- Open Unity, open the GameClient project.
  If the Game scene is not selected yet, click on the "Project" tab (could be in "Console" by default), go to Scenes and select "Game"
- Click the "Run" button to run the client.
- The initial accounts can be checked at `Storage/TestDataGenerator`.
