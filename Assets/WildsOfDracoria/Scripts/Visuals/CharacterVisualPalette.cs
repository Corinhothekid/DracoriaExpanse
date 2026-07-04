using UnityEngine;

namespace WildsOfDracoria.Visuals
{
    public static class CharacterVisualPalette
    {
        public static Color Skin(string id)
        {
            switch (Normalize(id))
            {
                case "fair": return new Color(0.95f, 0.78f, 0.65f);
                case "olive": return new Color(0.66f, 0.48f, 0.32f);
                case "deep": return new Color(0.38f, 0.24f, 0.18f);
                case "green": return new Color(0.38f, 0.62f, 0.32f);
                case "ash": return new Color(0.48f, 0.48f, 0.46f);
                case "ember": return new Color(0.74f, 0.28f, 0.16f);
                case "gold": return new Color(0.86f, 0.62f, 0.22f);
                case "moonlit": return new Color(0.72f, 0.76f, 0.92f);
                default: return new Color(0.78f, 0.56f, 0.38f);
            }
        }

        public static Color Hair(string id)
        {
            switch (Normalize(id))
            {
                case "black": return new Color(0.04f, 0.035f, 0.03f);
                case "silver": return new Color(0.78f, 0.82f, 0.86f);
                case "auburn": return new Color(0.55f, 0.18f, 0.08f);
                case "white": return new Color(0.9f, 0.9f, 0.86f);
                case "blue": return new Color(0.16f, 0.32f, 0.8f);
                default: return new Color(0.28f, 0.14f, 0.06f);
            }
        }

        public static Color Eye(string id)
        {
            switch (Normalize(id))
            {
                case "blue": return new Color(0.22f, 0.48f, 1f);
                case "green": return new Color(0.18f, 0.8f, 0.32f);
                case "amber": return new Color(1f, 0.62f, 0.16f);
                case "gold": return new Color(1f, 0.82f, 0.18f);
                case "red": return new Color(0.9f, 0.12f, 0.08f);
                default: return new Color(0.45f, 0.28f, 0.12f);
            }
        }

        private static string Normalize(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? string.Empty : id.Trim().ToLowerInvariant().Replace(" ", "_");
        }
    }
}