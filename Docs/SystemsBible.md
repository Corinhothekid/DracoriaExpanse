# Systems Bible

This document summarizes the major systems for Wilds of Dracoria. Each system should eventually become its own full specification before implementation.

## Development Rule

Build foundations first. Avoid feature sprawl. Every new system must connect to the living world, economy, professions, community, or legacy.

## System 001: Character and Identity

Players do not select a permanent class.

Character includes:

- Character name
- Family name
- Race
- Appearance
- Homeland
- Health
- Stamina
- Mana or Focus if needed later
- Gold
- Inventory
- Equipment
- Skills
- Professions
- Reputation
- Titles
- Guild
- Home
- Ships
- Mounts
- Journal
- Legacy records

Identity card should show who the player is, not only a level number.

## System 002: Skills

Every action feeds a skill.

Skill families:

- Combat
- Gathering
- Crafting
- Trade
- Civic
- Exploration
- Sailing

Every skill has:

- Name
- Level
- Current XP
- XP to next level
- Knowledge unlocks
- Mastery rank
- Techniques

Design rule:

> Every player can learn anything. Nobody becomes the best at everything quickly.

There are no hard profession locks. Mastery requires time, accomplishment, knowledge, and reputation.

## System 003: Professions

Professions are careers, not side menus.

Profession families:

- Combat: soldier, knight, ranger, mage, mercenary, monster hunter.
- Gathering: fisherman, miner, farmer, lumberjack, hunter, herbalist.
- Crafting: blacksmith, cook, shipwright, carpenter, tailor, alchemist, engineer.
- Commerce: merchant, broker, banker, shop owner, caravan master.
- Civic: mayor, architect, treasurer, quartermaster, diplomat, city planner.
- Exploration: navigator, cartographer, archaeologist, scholar, treasure hunter.

Every profession needs:

- XP
- Knowledge
- Mastery
- Reputation
- Contracts
- Equipment
- Bosses or major challenges
- Legendary materials
- Endgame activities

Examples:

- Fishermen hunt Leviathans, Kraken, Sea Serpents, Ghost Whales.
- Blacksmiths fight or harvest Fire Elementals, Crystal Titans, Living Forges.
- Lumberjacks face Ancient Treants and Living Forests.
- Farmers deal with Blight Queens, Locust Swarms, corrupted groves.
- Merchants face logistics, contracts, risk, route planning, and shipping challenges.

## System 004: Combat

Combat must be mobile-friendly, responsive, and expandable.

Core combat elements:

- Basic attacks
- Blocking
- Dodging
- Stamina management
- Weapon skills
- Enemy AI
- Damage system
- Targeting
- Health and stamina UI

No class restrictions. Weapons and skills shape the player's combat role.

Roles emerge naturally:

- Tank
- Damage
- Support
- Healer
- Control
- Scout

Combat should support professions rather than dominate them.

## System 005: Economy

The economy belongs to players.

Four pillars:

1. Creation: fishing, mining, farming, logging, hunting, exploration.
2. Production: blacksmithing, cooking, shipbuilding, tailoring, construction.
3. Transportation: caravans, ships, ports, warehouses, roads.
4. Consumption: food eaten, gear repaired, ships maintained, buildings supplied.

Design rules:

- Wolves should not magically drop piles of coins.
- Finished goods should usually require multiple professions.
- Local markets matter.
- No global auction house at first.
- Goods physically move through the world.
- Money sinks must feel like world maintenance, not punishment.

Money sinks:

- Repairs
- Ship upkeep
- Property tax
- Stall rent
- Market license
- Business license
- Luxury goods
- Festival sponsorship
- Construction projects

## System 006: Local Markets and Player Shops

Every city can have a market.

Players rent or own stalls.

Shop data:

- Owner
- Shop name
- Location
- Inventory
- Prices
- Reputation
- Sales history
- Decorations
- Business type

Shops should become destinations. Players should say things like: go to Corey's Forge.

## System 007: Travel

Travel is gameplay.

Travel methods:

- Walking
- Mounts
- Caravans
- Stagecoaches
- Ferries
- Player ships
- Guild convoys
- NPC kingdom transport

NPC transport should exist early game:

- Safe
- Cheap
- Fixed schedule
- Slower
- Reliable

Player transport should become the premium experience:

- Faster
- Flexible
- Social
- Can carry cargo
- Can stop for fishing or exploration
- Can become a business

## System 008: Sailing and The Great Sea

The ocean is its own world.

Ship classes:

- Rowboat
- Fishing Boat
- Merchant Sloop
- Cargo Ship
- Warship
- Expedition Ship
- Flagship

Ship data:

- Name
- Builder
- Owner
- Captain
- Crew history
- Durability
- Speed
- Cargo capacity
- Upgrades
- Battle history
- Voyage log

Crew roles:

- Captain
- Navigator
- Helmsman
- Sailmaster
- Fisherman
- Cook
- Carpenter
- Blacksmith
- Medic
- Marine

The sea has weather, currents, storms, fish migrations, pirates, sea monsters, shipwrecks, hidden islands, and trade lanes.

## System 009: Navigators and Charter Services

Some players make a living moving other players and cargo.

Services:

- Passenger ferry
- Private charter
- Cargo route
- Expedition guide
- Fishing voyage
- Dangerous sea crossing

Navigator reputation matters:

- On-time arrivals
- Safe travel record
- Ships lost
- Passengers transported
- Storms survived
- Routes discovered

Shipping companies can become server-famous businesses.

## System 010: Homesteads and Communities

Small-scale civilization bridge between solo player and guild city.

Scale:

- Homestead: 1 to 4 players
- Community: 5 to 30 players
- Guild City: 30 to 150+ players

Community features:

- Shared road
- Shared dock
- Community hall
- Small market square
- Shared forge
- Garden/farm
- Fishing pier
- Windmill
- Notice board
- Festival grounds

Communities feed cities and give small friend groups meaningful goals.

## System 011: Guilds

Guilds are organizations, not chat rooms.

Guild data:

- Name
- Banner
- Colors
- Motto
- Government type
- Ranks
- Departments
- Treasury
- Warehouse
- Reputation
- Cities
- Ships
- Contracts
- Calendar
- Alliances

Guild types:

- Military
- Merchant
- Crafting
- Exploration
- Naval
- Civic

Guilds can found settlements and eventually cities.

## System 012: Living Cities

A city is a character.

City progression:

```text
Camp
Settlement
Village
Town
City
Great City
Legendary City
```

City features:

- Districts
- Markets
- Citizens
- Government
- Taxes
- Public buildings
- Harbors
- Defense
- Culture scores
- History
- Tourism
- Festivals

District examples:

- Market
- Harbor
- Crafting
- Residential
- Military
- Temple
- University
- Agricultural
- Industrial
- Entertainment
- Government

Cities use curated templates to avoid messy player sprawl. Players choose district placement, upgrades, decorations, public projects, and priorities.

## System 013: Kingdoms and Crown Isles

Five kingdoms:

- Asteria: Humans
- Sylvandor: Elves
- Khar-Dur: Orcs
- Gearhollow: Goblins
- Drakenreach: Dragonborn

Crown Isles are the central weekly war zone.

Victory is decided through the Crown Beacon campaign, with points from:

- Holding forts
- Delivering supplies
- Repairing structures
- Capturing ports
- Naval control
- Scouting
- Cooking and supply
- Siege work
- Combat victories

Rewards are Kingdom Blessings, not overpowering buffs.

## System 014: Dynasties

Players create a family House.

House data:

- Family name
- Crest
- Motto
- Homeland
- Reputation
- Estate
- Members
- Elders
- Heirlooms
- Family businesses
- Family chronicle

Rerolling becomes continuing the family story, not losing everything.

## System 015: Living NPCs

NPCs are citizens, not only quest signs.

NPCs can have:

- Home
- Job
- Daily routine
- Personality
- Memory
- Relationships
- Local role

NPCs fill gaps in the world but do not replace players as the main economy drivers.

## System 016: Time and Living World

The world has:

- Day/night
- Seasons
- Weather
- Moon phases
- Tides
- World events
- Server anniversaries

Time changes professions, travel, markets, fishing, farming, sailing, and monster behavior.

## System 017: Reputation and Consequences

The world remembers how players behave.

Reputation types:

- Kingdom
- City
- Profession
- Personal
- Guild
- Criminal/Outlaw
- Merchant
- Community

Reputation is identity, not a simple good/evil meter.

## System 018: Server Chronicle

Every server writes its own history.

Chronicle entries include:

- Cities founded
- Wonders completed
- Sea monsters defeated
- First legendary item
- Great wars
- Famines
- Trade booms
- Families rising to fame
- Famous ships sunk or retired
- Kingdom victories

The world should remember players after they leave.
