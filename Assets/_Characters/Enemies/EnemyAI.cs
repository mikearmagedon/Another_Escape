using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    // EnemyAI states
    enum State { idle, patrolling, chasing, attacking};

    [SelectionBase]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour
    {
        // Config
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] AudioClip combatMusic;
        [HideInInspector] public bool isDead;
        [HideInInspector] public string enemyName;

        // State
        State state = State.idle;
        float distanceToPlayer;
        float currentWeaponRange;
        int nextWaypointIndex;

        // Cached components references
        GameObject player = null;
        Character character;
        //AudioManager audioManager;

        private void Awake()
        {
            isDead = false;
            enemyName = gameObject.name;
        }

        // Messages and methods
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            character = GetComponent<Character>();
            //audioManager = AudioManager.instance;
        }

        void Update()
        {
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }

            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }

            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player);
            }
        }      
        
        IEnumerator Patrol()
        {
            state = State.patrolling;
            while (patrolPath != null)
            {
                Vector3 nextWaypointPosition = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypointWhenClose(nextWaypointPosition);
                yield return new WaitForSeconds(waypointDwellTime);
            }

            //audioManager.PlayMusicBattle(combatMusic, false);
        }

        void CycleWaypointWhenClose(Vector3 nextWaypointPosition)
        {
            if (Vector3.Distance(transform.position, nextWaypointPosition) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            while (distanceToPlayer > currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                //audioManager.PlayMusicBattle(combatMusic, true);
                yield return new WaitForEndOfFrame();
            }

            //audioManager.PlayMusicBattle(combatMusic, true);
        }

        void OnDrawGizmos()
        {
            // Draw attack radius sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase radius sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
