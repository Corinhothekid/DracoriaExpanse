using System.Collections.Generic;
using System.Linq;

namespace WildsOfDracoria.Professions
{
    public static class ProfessionRegistry
    {
        private static readonly List<ProfessionDefinition> Definitions = new List<ProfessionDefinition>
        {
            new ProfessionDefinition(ProfessionIds.Fishing, "Fishing", "Catch fish, discover waters, and eventually face legendary sea creatures.", true),
            new ProfessionDefinition(ProfessionIds.Mining, "Mining", "Extract ore, stone, gems, and rare materials from the world.", false),
            new ProfessionDefinition(ProfessionIds.Blacksmithing, "Blacksmithing", "Forge tools, weapons, armor, fittings, and world-building materials.", false),
            new ProfessionDefinition(ProfessionIds.Cooking, "Cooking", "Prepare meals that support travel, work, combat, and community life.", false),
            new ProfessionDefinition(ProfessionIds.Sailing, "Sailing", "Operate vessels, read water conditions, and support sea travel when sailing is built later.", false),
            new ProfessionDefinition(ProfessionIds.Merchanting, "Merchanting", "Move goods, run shops, manage routes, and build reputation through trade.", false),
            new ProfessionDefinition(ProfessionIds.Logging, "Logging", "Harvest wood and forest materials for construction, crafting, and ships.", false),
            new ProfessionDefinition(ProfessionIds.Farming, "Farming", "Grow crops, tend land, and support settlements with food and trade goods.", false),
            new ProfessionDefinition(ProfessionIds.Hunting, "Hunting", "Track beasts, gather hides and meat, and protect settlements from wildlife.", false),
            new ProfessionDefinition(ProfessionIds.Navigation, "Navigation", "Chart routes, guide travelers, and master dangerous journeys.", false)
        };

        public static IReadOnlyList<ProfessionDefinition> All => Definitions;

        public static ProfessionDefinition GetDefinition(string professionId)
        {
            var normalizedId = ProfessionIds.Normalize(professionId);
            return Definitions.FirstOrDefault(definition => definition.professionId == normalizedId);
        }
    }
}
