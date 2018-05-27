using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

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

        // State
        State state = State.idle;
        float distanceToPlayer;
        float currentWeaponRange;

        // Cached components references
        GameObject player = null;
        Character character;

        // Messages and methods
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            character = GetComponent<Character>();
        }

        void Update()
        {
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                state = State.patrolling;
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
            }
        }        

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            while (distanceToPlayer > currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
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
