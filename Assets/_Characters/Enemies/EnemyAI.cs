using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        // Config
        [SerializeField] float chaseRadius = 10f;

        // State
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

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        }        

        void OnDrawGizmos()
        {
            // Draw attack radius sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponConfig.GetMaxAttackRange());

            // Draw chase radius sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
