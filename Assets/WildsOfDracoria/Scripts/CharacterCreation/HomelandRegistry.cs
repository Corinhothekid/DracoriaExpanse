using System.Collections.Generic;

namespace WildsOfDracoria.CharacterCreation
{
    public static class HomelandRegistry
    {
        private static readonly List<string> HomelandNames = new List<string>
        {
            "Ironhaven",
            "Elderglen",
            "Stonejaw Hold",
            "Cinderwharf",
            "Ashpeak Enclave"
        };

        public static IReadOnlyList<string> All => HomelandNames;
    }
}