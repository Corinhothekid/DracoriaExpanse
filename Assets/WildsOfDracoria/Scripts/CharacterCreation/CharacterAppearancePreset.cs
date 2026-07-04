namespace WildsOfDracoria.CharacterCreation
{
    public class CharacterAppearancePreset
    {
        public string bodyType;
        public string skinTone;
        public string hairStyle;
        public string hairColor;
        public string facialHairStyle;
        public string eyeColor;

        public CharacterAppearancePreset(string bodyType, string skinTone, string hairStyle, string hairColor, string facialHairStyle, string eyeColor)
        {
            this.bodyType = bodyType;
            this.skinTone = skinTone;
            this.hairStyle = hairStyle;
            this.hairColor = hairColor;
            this.facialHairStyle = facialHairStyle;
            this.eyeColor = eyeColor;
        }
    }
}