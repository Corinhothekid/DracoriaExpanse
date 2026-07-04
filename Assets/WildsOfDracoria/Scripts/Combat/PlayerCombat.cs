using UnityEngine;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI;

namespace WildsOfDracoria.Combat
{
    [RequireComponent(typeof(PlayerVitals))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private string equippedWeaponName = "Training Sword";
        [SerializeField] private LayerMask hittableLayers = ~0;
        [SerializeField] private float dodgeStaminaCost = 24f;
        [SerializeField] private float dodgeDistance = 3f;
        [SerializeField] private float dodgeDuration = 0.18f;

        private PlayerVitals vitals;
        private CharacterController characterController;
        private float nextAttackTime;
        private float dodgeEndsAt;
        private Vector3 dodgeVelocity;
        private bool externalBlockInput;
        private IDamageable currentTarget;

        public bool IsBlocking { get; private set; }
        public WeaponData EquippedWeapon { get; private set; }

        private void Awake()
        {
            vitals = GetComponent<PlayerVitals>();
            characterController = GetComponent<CharacterController>();
            var savedWeapon = GameManager.Instance?.CharacterData?.equippedWeapon;
            if (!string.IsNullOrWhiteSpace(savedWeapon))
            {
                equippedWeaponName = savedWeapon;
            }

            EquipWeapon(equippedWeaponName);
        }

        private void Update()
        {
            ReadCombatInput();
            ContinueDodge();
        }

        public void EquipWeapon(string weaponName)
        {
            EquippedWeapon = WeaponDatabase.GetWeapon(weaponName);
            equippedWeaponName = EquippedWeapon.weaponName;
            if (GameManager.Instance?.CharacterData != null)
            {
                GameManager.Instance.CharacterData.equippedWeapon = equippedWeaponName;
            }
        }

        public void SetBlockInput(bool isBlocking)
        {
            externalBlockInput = isBlocking;
        }

        public void BasicAttack()
        {
            if (Time.time < nextAttackTime || vitals.IsDead() || EquippedWeapon == null)
            {
                return;
            }

            if (!vitals.TrySpendStamina(EquippedWeapon.staminaCost))
            {
                GameManager.Instance.DialogueUI?.ShowLine("Not enough stamina.");
                return;
            }

            nextAttackTime = Time.time + EquippedWeapon.attackCooldown;
            var hitCenter = transform.position + Vector3.up + transform.forward * EquippedWeapon.attackRange * 0.55f;
            var hits = Physics.OverlapSphere(hitCenter, EquippedWeapon.attackRange, hittableLayers, QueryTriggerInteraction.Ignore);
            IDamageable bestTarget = null;
            var bestDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    continue;
                }

                var damageable = hit.GetComponentInParent<IDamageable>();
                if (damageable == null || damageable.IsDead())
                {
                    continue;
                }

                var distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTarget = damageable;
                }
            }

            if (bestTarget == null)
            {
                return;
            }

            currentTarget = bestTarget;
            CombatUI.Instance?.SetTarget(bestTarget);
            var damage = CalculateDamage();
            bestTarget.TakeDamage(damage);
        }

        public void Dodge()
        {
            if (vitals.IsDead() || Time.time < dodgeEndsAt || !vitals.TrySpendStamina(dodgeStaminaCost))
            {
                return;
            }

            dodgeEndsAt = Time.time + dodgeDuration;
            dodgeVelocity = transform.forward * (dodgeDistance / dodgeDuration);
        }

        public void NotifyEnemyDefeated(EnemyHealth enemy)
        {
            if (enemy == null)
            {
                return;
            }

            if (EquippedWeapon != null && EquippedWeapon.skillType == "Swordsmanship")
            {
                GameManager.Instance.GainSkillXP("Swordsmanship", enemy.SwordsmanshipXPReward);
            }

            GameManager.Instance.GainSkillXP("Endurance", enemy.EnduranceXPReward);
            enemy.GrantDrops();
            CombatUI.Instance?.ClearTarget(enemy);
        }

        private int CalculateDamage()
        {
            var skill = GameManager.Instance.CharacterData.GetSkill(EquippedWeapon.skillType);
            var skillBonus = skill != null ? Mathf.Max(0, skill.level - 1) : 0;
            return EquippedWeapon.damage + skillBonus;
        }

        private void ReadCombatInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                BasicAttack();
            }

            IsBlocking = Input.GetMouseButton(1) || externalBlockInput;

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Dodge();
            }
        }

        private void ContinueDodge()
        {
            if (Time.time >= dodgeEndsAt || characterController == null)
            {
                return;
            }

            characterController.Move(dodgeVelocity * Time.deltaTime);
        }
    }
}
