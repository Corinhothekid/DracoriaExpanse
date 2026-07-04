using UnityEngine;
using WildsOfDracoria.Combat;

namespace WildsOfDracoria.AI
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private EnemyState state = EnemyState.Patrol;
        [SerializeField] private float patrolRadius = 4f;
        [SerializeField] private float idleSeconds = 1.5f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float chaseSpeed = 3.8f;
        [SerializeField] private float detectionRange = 8f;
        [SerializeField] private float attackRange = 1.6f;
        [SerializeField] private int attackDamage = 8;
        [SerializeField] private float attackCooldown = 1.25f;
        [SerializeField] private float returnDistance = 12f;

        private EnemyHealth health;
        private Transform player;
        private IDamageable playerDamageable;
        private Vector3 spawnPoint;
        private Vector3 patrolPoint;
        private float idleUntil;
        private float nextAttackTime;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
            spawnPoint = transform.position;
            PickPatrolPoint();
        }

        private void Start()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                playerDamageable = playerObject.GetComponent<IDamageable>();
            }
        }

        private void Update()
        {
            if (health.IsDead())
            {
                return;
            }

            var distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : float.MaxValue;
            var distanceToSpawn = Vector3.Distance(transform.position, spawnPoint);

            if (player != null && distanceToPlayer <= attackRange)
            {
                state = EnemyState.Attack;
            }
            else if (player != null && distanceToPlayer <= detectionRange && distanceToSpawn <= returnDistance)
            {
                state = EnemyState.Chase;
            }
            else if (distanceToSpawn > patrolRadius * 1.5f)
            {
                state = EnemyState.Return;
            }
            else if (state == EnemyState.Chase || state == EnemyState.Attack || state == EnemyState.Return)
            {
                state = EnemyState.Patrol;
            }

            switch (state)
            {
                case EnemyState.Idle:
                    UpdateIdle();
                    break;
                case EnemyState.Patrol:
                    MoveToward(patrolPoint, moveSpeed);
                    if (Vector3.Distance(transform.position, patrolPoint) <= 0.35f)
                    {
                        state = EnemyState.Idle;
                        idleUntil = Time.time + idleSeconds;
                    }
                    break;
                case EnemyState.Chase:
                    MoveToward(player.position, chaseSpeed);
                    break;
                case EnemyState.Attack:
                    AttackPlayer();
                    break;
                case EnemyState.Return:
                    MoveToward(spawnPoint, moveSpeed);
                    if (Vector3.Distance(transform.position, spawnPoint) <= 0.35f)
                    {
                        state = EnemyState.Patrol;
                        PickPatrolPoint();
                    }
                    break;
            }
        }

        private void UpdateIdle()
        {
            if (Time.time < idleUntil)
            {
                return;
            }

            PickPatrolPoint();
            state = EnemyState.Patrol;
        }

        private void AttackPlayer()
        {
            if (player == null)
            {
                state = EnemyState.Return;
                return;
            }

            Face(player.position);
            if (Time.time < nextAttackTime)
            {
                return;
            }

            nextAttackTime = Time.time + attackCooldown;
            playerDamageable?.TakeDamage(attackDamage);
        }

        private void MoveToward(Vector3 target, float speed)
        {
            var direction = target - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.01f)
            {
                return;
            }

            var movement = direction.normalized * speed * Time.deltaTime;
            transform.position += movement;
            Face(target);
        }

        private void Face(Vector3 target)
        {
            var direction = target - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 12f * Time.deltaTime);
        }

        private void PickPatrolPoint()
        {
            var offset = Random.insideUnitCircle * patrolRadius;
            patrolPoint = spawnPoint + new Vector3(offset.x, 0f, offset.y);
        }
    }
}
