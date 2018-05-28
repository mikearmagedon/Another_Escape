using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour
    {
        // Config
        [SerializeField] LayerMask enemyLayerMask;

        // State
        public bool wonGame { get; set; }
        int counter;

        // Cached components references
        WeaponSystem weaponSystem;
        Character character;

        void Start()
        {
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            SetInitialWinConditionVariables();
        }

        void Update()
        {
            ScriptableObject.CreateInstance<WeaponConfig>();
            if (Input.GetMouseButton(0))
            {
                OnMouseClick();
            }
        }

        // Fixed update is called in sync with physics
        void FixedUpdate()
        {
            ProcessKeyboardMovement();
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

        void ProcessKeyboardMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            character.Move(movement, false);
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
            weaponSystem.AttackTargets(targets);
        }

        private Collider[] FindTargetsInRange()
        {
            Assert.IsFalse(enemyLayerMask == 0, "Please set enemyLayerMask to the Enemy layer");
            return Physics.OverlapSphere(transform.position, weaponSystem.GetCurrentWeapon().GetMaxAttackRange(), enemyLayerMask);
        }
    }
}
