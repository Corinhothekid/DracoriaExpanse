using System.Collections.Generic;
using System.Linq;

namespace WildsOfDracoria.Combat
{
    public static class WeaponDatabase
    {
        private static readonly List<WeaponData> Weapons = new List<WeaponData>
        {
            new WeaponData("Training Sword", 12, 14f, 2.1f, 0.65f, "Swordsmanship"),
            new WeaponData("Rusty Axe", 16, 20f, 1.9f, 0.95f, "Swordsmanship"),
            new WeaponData("Wooden Bow", 8, 12f, 6f, 0.85f, "Archery")
        };

        public static WeaponData GetWeapon(string weaponName)
        {
            return Weapons.FirstOrDefault(weapon => weapon.weaponName == weaponName) ?? Weapons[0];
        }

        public static IReadOnlyList<WeaponData> GetAllWeapons()
        {
            return Weapons;
        }
    }
}
