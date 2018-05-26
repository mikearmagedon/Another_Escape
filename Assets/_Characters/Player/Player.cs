using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire
using RPG.CameraUI;

namespace RPG.Characters
{
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        // Config
        [SerializeField] LayerMask enemyLayerMask;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] Weapon currentWeaponConfig;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        // State
        public bool wonGame { get; set; }

        int counter;
        GameObject currentTarget;
        float lastHitTime = 0;
        GameObject weaponObject;

        // Cached components references
        CameraRaycaster cameraRaycaster;
        Animator animator;

        // Messages and methods
        // Use this for initialization
        void Start()
        {
            SetInitialWinConditionVariables();
            EquipWeapon(currentWeaponConfig);
            SetupRuntimeAnimator();
        }

        public void DisableControl()
        {
            GetComponent<PlayerController>().enabled = false;
        }

        public void EnableControl()
        {
            GetComponent<PlayerController>().enabled = true;
        }

        void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = currentWeaponConfig.GetAttackAnimClip(); // remove const
        }

        void SetInitialWinConditionVariables()
        {
            wonGame = false;
            counter = 0;
        }

        public void EquipWeapon(Weapon weaponConfig)
        {
            Assert.IsNotNull(weaponConfig, "Please assign a weapon to the player");
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); // empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.weaponGrip.transform.position;
            weaponObject.transform.localRotation = currentWeaponConfig.weaponGrip.transform.rotation;
            SetupRuntimeAnimator();
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length <= 0, "No DominantHand script found on player, please add one");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand scripts found on player, please have just one");
            return dominantHands[0].gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            ScriptableObject.CreateInstance<Weapon>();
            if (Input.GetMouseButton(0))
            {
                OnMouseClick();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Pickup"))
            {
                Destroy(other.gameObject);
                counter++;
            }
            else if (other.gameObject.CompareTag("Finish"))
            {
                if (counter == 3)
                {
                    wonGame = true;
                }
            }
        }

        void OnMouseClick()
        {
            Collider[] targets = FindTargetsInRange();
            AttackTargets(targets);
        }

        private Collider[] FindTargetsInRange()
        {
            Assert.IsFalse(enemyLayerMask == 0, "Please set enemyLayerMask to the Enemy layer");
            return Physics.OverlapSphere(transform.position, currentWeaponConfig.GetMaxAttackRange(), enemyLayerMask);
        }

        void AttackTargets(Collider[] targets)
        {
            if ((Time.time - lastHitTime) > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); // TODO make const
                foreach (var target in targets)
                {
                    // Damage the enemy
                    var damageable = target.GetComponent<HealthSystem>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damagePerHit);
                    }
                }
                lastHitTime = Time.time;
            }
        }

        void OnDrawGizmos()
        {
            // Draw attack radius sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponConfig.GetMaxAttackRange());
        }
    }
}
