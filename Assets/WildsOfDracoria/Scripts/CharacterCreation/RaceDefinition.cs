using System;
using UnityEngine;

namespace WildsOfDracoria.CharacterCreation
{
    [Serializable]
    public class RaceDefinition
    {
        public string raceId;
        public string displayName;
        public string shortDescription;
        public string homelandName;
        public string culturalTheme;
        public string startingBonusFlavorText;
        public Color previewColor = Color.white;
        public Vector3 previewScale = Vector3.one;

        public RaceDefinition(string raceId, string displayName, string shortDescription, string homelandName, string culturalTheme, string startingBonusFlavorText, Color previewColor, Vector3 previewScale)
        {
            this.raceId = RaceIds.Normalize(raceId);
            this.displayName = displayName;
            this.shortDescription = shortDescription;
            this.homelandName = homelandName;
            this.culturalTheme = culturalTheme;
            this.startingBonusFlavorText = startingBonusFlavorText;
            this.previewColor = previewColor;
            this.previewScale = previewScale;
        }
    }
}