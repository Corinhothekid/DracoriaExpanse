using System.Collections.Generic;

namespace WildsOfDracoria.CharacterCreation
{
    public static class CharacterAppearanceOptions
    {
        private static readonly List<CharacterAppearancePreset> Presets = new List<CharacterAppearancePreset>
        {
            new CharacterAppearancePreset("Average", "Warm", "Short", "Brown", "None", "Hazel"),
            new CharacterAppearancePreset("Tall", "Fair", "Long", "Black", "Trimmed", "Blue"),
            new CharacterAppearancePreset("Stout", "Deep", "Braided", "Auburn", "Full", "Green"),
            new CharacterAppearancePreset("Lean", "Olive", "Shaved", "Silver", "None", "Amber")
        };

        public static IReadOnlyList<CharacterAppearancePreset> All => Presets;
    }
}