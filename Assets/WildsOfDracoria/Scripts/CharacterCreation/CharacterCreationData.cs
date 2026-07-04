using System;

namespace WildsOfDracoria.CharacterCreation
{
    [Serializable]
    public class CharacterCreationData
    {
        public string characterName = "Corey";
        public string familyName = "Kranert";
        public string race = RaceIds.Human;
        public string bodyType = "Average";
        public string skinTone = "Warm";
        public string hairStyle = "Short";
        public string hairColor = "Brown";
        public string facialHairStyle = "None";
        public string eyeColor = "Hazel";
        public string startingHomeland = "Ironhaven";

        public string FullName => $"{characterName} {familyName}".Trim();
        public string HouseName => $"House {familyName}".Trim();
    }
}