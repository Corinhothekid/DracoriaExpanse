using System;

namespace WildsOfDracoria.Combat
{
    [Serializable]
    public class WeaponData
    {
        public string weaponName;
        public int damage;
        public float staminaCost;
        public float attackRange;
        public float attackCooldown;
        public string skillType;

        public WeaponData(string weaponName, int damage, float staminaCost, float attackRange, float attackCooldown, string skillType)
        {
            this.weaponName = weaponName;
            this.damage = damage;
            this.staminaCost = staminaCost;
            this.attackRange = attackRange;
            this.attackCooldown = attackCooldown;
            this.skillType = skillType;
        }
    }
}
