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

        // State
        public bool wonGame { get; set; }

        int counter;

        // Cached components references
        WeaponSystem weaponSystem;

        // Messages and methods
        // Use this for initialization
        void Start()
        {
            SetInitialWinConditionVariables();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        public void DisableControl()
        {
            GetComponent<PlayerController>().enabled = false;
        }

        public void EnableControl()
        {
            GetComponent<PlayerController>().enabled = true;
        }

        void SetInitialWinConditionVariables()
        {
            wonGame = false;
            counter = 0;
        }

        // Update is called once per frame
        void Update()
        {
            ScriptableObject.CreateInstance<WeaponConfig>();
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
            foreach (var target in targets)
            {
                weaponSystem.AttackTarget(target.gameObject);
            }
        }

        private Collider[] FindTargetsInRange()
        {
            Assert.IsFalse(enemyLayerMask == 0, "Please set enemyLayerMask to the Enemy layer");
            return Physics.OverlapSphere(transform.position, weaponSystem.GetCurrentWeapon().GetMaxAttackRange(), enemyLayerMask);
        }
    }
}
