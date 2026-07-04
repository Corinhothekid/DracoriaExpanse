using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WildsOfDracoria.CharacterCreation
{
    public static class RaceRegistry
    {
        private static readonly List<RaceDefinition> Definitions = new List<RaceDefinition>
        {
            new RaceDefinition(RaceIds.Human, "Human", "Adaptable frontier settlers and city builders.", "Ironhaven", "Harbor villages, banners, practical craft, and old roads.", "Flavor: flexible ambition and strong house traditions.", new Color(0.22f, 0.45f, 0.9f), Vector3.one),
            new RaceDefinition(RaceIds.Elf, "Elf", "Long-lived wardens of ancient groves and starlit courts.", "Elderglen", "Moonlit forests, living wood, silver script, and quiet magic.", "Flavor: patience, memory, and graceful mastery.", new Color(0.18f, 0.72f, 0.38f), new Vector3(0.92f, 1.08f, 0.92f)),
            new RaceDefinition(RaceIds.Orc, "Orc", "Strong clanfolk shaped by honor, craft, and hard country.", "Stonejaw Hold", "Clan fires, mountain passes, drums, iron, and oath marks.", "Flavor: endurance, loyalty, and fearless work.", new Color(0.36f, 0.62f, 0.24f), new Vector3(1.12f, 1.06f, 1.12f)),
            new RaceDefinition(RaceIds.Goblin, "Goblin", "Sharp-eyed tinkerers, traders, scouts, and risk-takers.", "Cinderwharf", "Workshops, bright coins, clever tools, busy docks, and quick deals.", "Flavor: invention, wit, and finding opportunity first.", new Color(0.42f, 0.78f, 0.3f), new Vector3(0.78f, 0.78f, 0.78f)),
            new RaceDefinition(RaceIds.Dragonborn, "Dragonborn", "Proud descendants of old draconic bloodlines and frontier oaths.", "Ashpeak Enclave", "Scale banners, ember stone, ancient vows, and ceremonial armor.", "Flavor: presence, legacy, and fierce house memory.", new Color(0.8f, 0.24f, 0.16f), new Vector3(1.08f, 1.12f, 1.08f))
        };

        public static IReadOnlyList<RaceDefinition> All => Definitions;

        public static RaceDefinition Get(string raceIdOrName)
        {
            var normalizedId = RaceIds.Normalize(raceIdOrName);
            return Definitions.FirstOrDefault(race => race.raceId == normalizedId) ?? Definitions[0];
        }
    }
}