using System.Collections.Generic;
using System.Linq;

namespace WildsOfDracoria.Professions
{
    public static class ProfessionRegistry
    {
        private static readonly List<ProfessionDefinition> Definitions = new List<ProfessionDefinition>
        {
            new ProfessionDefinition("Fishing", true, "Gathering", "Catch fish, discover waters, and eventually face legendary sea creatures."),
            new ProfessionDefinition("Mining", false, "Gathering", "Extract ore, stone, gems, and rare materials from the world."),
            new ProfessionDefinition("Blacksmithing", false, "Crafting", "Forge tools, weapons, armor, fittings, and world-building materials."),
            new ProfessionDefinition("Cooking", false, "Crafting", "Prepare meals that support travel, work, combat, and community life."),
            new ProfessionDefinition("Farming", false, "Gathering", "Grow crops, tend land, and support settlements with food and trade goods."),
            new ProfessionDefinition("Logging", false, "Gathering", "Harvest wood and forest materials for construction, crafting, and ships."),
            new ProfessionDefinition("Merchant", false, "Commerce", "Move goods, run shops, manage routes, and build reputation through trade."),
            new ProfessionDefinition("Navigator", false, "Exploration", "Chart routes, guide travelers, and master dangerous journeys."),
            new ProfessionDefinition("Shipwright", false, "Crafting", "Build and maintain vessels for fishing, trade, travel, and war."),
            new ProfessionDefinition("Carpenter", false, "Crafting", "Shape wood into furniture, structures, tools, docks, and homes."),
            new ProfessionDefinition("Hunter", false, "Gathering", "Track beasts, gather hides and meat, and protect settlements from wildlife.")
        };

        public static IReadOnlyList<ProfessionDefinition> All => Definitions;

        public static ProfessionDefinition GetDefinition(string professionName)
        {
            return Definitions.FirstOrDefault(definition => definition.professionName == professionName);
        }
    }
}
