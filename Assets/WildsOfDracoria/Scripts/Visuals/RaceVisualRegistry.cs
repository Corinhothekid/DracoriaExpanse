using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WildsOfDracoria.CharacterCreation;

namespace WildsOfDracoria.Visuals
{
    public static class RaceVisualRegistry
    {
        private static readonly string[] CommonSkinTones = { "fair", "warm", "olive", "deep" };
        private static readonly string[] CommonHair = { "hair_short", "hair_long", "hair_spiky" };
        private static readonly string[] CommonFaces = { "face_soft", "face_sharp", "face_bold" };

        private static readonly List<RaceVisualSettings> Settings = new List<RaceVisualSettings>
        {
            new RaceVisualSettings(RaceIds.Human, 1f, 1f, 1f, "warm", new Color(0.22f, 0.45f, 0.9f), CommonSkinTones, CommonHair, CommonFaces),
            new RaceVisualSettings(RaceIds.Elf, 1.08f, 0.92f, 1.02f, "fair", new Color(0.18f, 0.72f, 0.38f), new[] { "fair", "warm", "moonlit", "olive" }, CommonHair, CommonFaces),
            new RaceVisualSettings(RaceIds.Orc, 1.06f, 1.18f, 1.04f, "green", new Color(0.36f, 0.62f, 0.24f), new[] { "green", "olive", "deep", "ash" }, CommonHair, CommonFaces),
            new RaceVisualSettings(RaceIds.Goblin, 0.78f, 0.82f, 1.12f, "green", new Color(0.42f, 0.78f, 0.3f), new[] { "green", "olive", "warm", "ash" }, CommonHair, CommonFaces),
            new RaceVisualSettings(RaceIds.Dragonborn, 1.12f, 1.1f, 1.06f, "ember", new Color(0.8f, 0.24f, 0.16f), new[] { "ember", "ash", "gold", "deep" }, new[] { "hair_none", "hair_spiky", "hair_crest" }, CommonFaces)
        };

        public static RaceVisualSettings Get(string race)
        {
            var normalizedRace = RaceIds.Normalize(race);
            return Settings.FirstOrDefault(setting => setting.race == normalizedRace) ?? Settings[0];
        }
    }
}