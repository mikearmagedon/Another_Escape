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
        [SerializeField] float attackRadius = 5f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float damagePerHit = 5f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] Weapon weaponInUse = null;

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
        }

        private void EquipWeapon()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            var dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.weaponGrip.transform.position;
            weapon.transform.localRotation = weaponInUse.weaponGrip.transform.rotation;
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
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius)
            {
                // TODO Range attack: spawn projectile

                if ((Time.time - lastHitTime) > minTimeBetweenHits)
                {
                    // Damage the player
                    IDamageable damageable = player.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damagePerHit);
                    }
                    lastHitTime = Time.time;
                }
            }

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        void OnDrawGizmos()
        {
            // Draw attack radius sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw chase radius sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
