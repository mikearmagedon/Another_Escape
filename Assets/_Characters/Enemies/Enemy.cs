using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.Characters.ThirdPerson;

using RPG.Core; // TODO consider re-wire

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        // Config
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float damagePerHit = 5f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        // State
        public float healthAsPercentage
        {
            get
            {
                return currentHealtPoints / maxHealthPoints;
            }
        }

        float currentHealtPoints;
        float lastHitTime = 0;

        // Cached components references
        AICharacterControl aiCharacterControl = null;
        GameObject player = null;
        Animator animator;

        // Messages and methods
        void IDamageable.TakeDamage(float damage)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
            if (currentHealtPoints <= 0) { Destroy(gameObject); }
        }

        // Use this for initialization
        void Start()
        {
            aiCharacterControl = GetComponent<AICharacterControl>();
            player = GameObject.FindGameObjectWithTag("Player");

            currentHealtPoints = maxHealthPoints;

            EquipWeapon();
            SetupRuntimeAnimator();
        }

        void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = currentWeaponConfig.GetAttackAnimClip(); // remove const
        }

        private void EquipWeapon()
        {
            var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();
            var dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = currentWeaponConfig.weaponGrip.transform.position;
            weapon.transform.localRotation = currentWeaponConfig.weaponGrip.transform.rotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length <= 0, "No DominantHand script found on " + gameObject.name + ", please add one");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand scripts found on " + gameObject.name + ", please have just one");
            return dominantHands[0].gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (IsTargetInRange(player))
            {
                AttackTarget(player);
            }

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return (distanceToTarget <= currentWeaponConfig.GetMaxAttackRange());
        }

        void AttackTarget(GameObject target)
        {
            if ((Time.time - lastHitTime) > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                // Damage the enemy
                IDamageable damageable = target.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    animator.SetTrigger("Attack"); // TODO make const
                    damageable.TakeDamage(damagePerHit);
                }
                lastHitTime = Time.time;
            }
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
