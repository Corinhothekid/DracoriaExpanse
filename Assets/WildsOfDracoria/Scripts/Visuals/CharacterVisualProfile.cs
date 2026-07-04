using System;
using WildsOfDracoria.CharacterCreation;

namespace WildsOfDracoria.Visuals
{
    [Serializable]
    public class CharacterVisualProfile
    {
        public string race = RaceIds.Human;
        public string bodyType = "average";
        public string skinToneId = "warm";
        public string hairStyleId = "hair_short";
        public string hairColorId = "brown";
        public string eyeColorId = "hazel";
        public string faceId = "face_soft";
        public string facialHairId = "facial_none";
        public string outfitId = "outfit_traveler";
        public string armorSetId = "armor_none";
        public string capeId = "cape_none";
        public string weaponVisualId = "weapon_training_sword";

        public void Normalize()
        {
            race = RaceIds.Normalize(race);
            bodyType = NormalizeId(bodyType, "average");
            skinToneId = NormalizeId(skinToneId, "warm");
            hairStyleId = NormalizeId(hairStyleId, "hair_short");
            hairColorId = NormalizeId(hairColorId, "brown");
            eyeColorId = NormalizeId(eyeColorId, "hazel");
            faceId = NormalizeId(faceId, "face_soft");
            facialHairId = NormalizeId(facialHairId, "facial_none");
            outfitId = NormalizeId(outfitId, "outfit_traveler");
            armorSetId = NormalizeId(armorSetId, "armor_none");
            capeId = NormalizeId(capeId, "cape_none");
            weaponVisualId = NormalizeId(weaponVisualId, "weapon_training_sword");
        }

        private static string NormalizeId(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim().ToLowerInvariant().Replace(" ", "_");
        }
    }
}