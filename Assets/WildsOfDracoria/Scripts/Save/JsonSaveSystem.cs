using System.IO;
using UnityEngine;
using WildsOfDracoria.Data;

namespace WildsOfDracoria.Save
{
    public static class JsonSaveSystem
    {
        private const string SaveFileName = "wilds_of_dracoria_character.json";

        public static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        public static bool HasSave()
        {
            return File.Exists(SavePath);
        }

        public static void Save(CharacterData characterData)
        {
            if (characterData == null)
            {
                return;
            }

            characterData.EnsureVisualProfile();
            characterData.NormalizeInventory();
            characterData.EnsureContracts();
            var json = JsonUtility.ToJson(characterData, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Saved Wilds of Dracoria character to {SavePath}");
        }

        public static CharacterData Load()
        {
            if (!File.Exists(SavePath))
            {
                return null;
            }

            var json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<CharacterData>(json);
            data.EnsureDefaultSkills();
            data.EnsureDefaultProfessions();
            data.EnsureVisualProfile();
            data.NormalizeInventory();
            data.EnsureContracts();
            return data;
        }
    }
}
