using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class PlayerController : MonoBehaviour
    {
        // Config
        [SerializeField] LayerMask enemyLayerMask;

        // State
        public bool wonGame { get; set; }
        public bool isInCombat = false; // TODO consider using State enum
        Collider[] targets;

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
            FindTargetsInRange();
            ProcessMouseClick();
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
                ScoreManager.score++;
            }
            else if (other.gameObject.CompareTag("Finish"))
            {
                if (ScoreManager.score <= 3)
                {
                    wonGame = true;
                }
            }
        }

        void ProcessMouseClick()
        {
            if (Input.GetMouseButton(0))
            {
                weaponSystem.AttackTargets(targets);
            }
        }

        private void FindTargetsInRange()
        {
            Assert.IsFalse(enemyLayerMask == 0, "Please set enemyLayerMask to the Enemy layer");
            targets = Physics.OverlapSphere(transform.position, weaponSystem.GetCurrentWeapon().GetMaxAttackRange(), enemyLayerMask);
            if (targets.Length != 0)
            {
                isInCombat = true;
                StopAllCoroutines();
            }
            else
            {
                if (isInCombat)
                {
                    StartCoroutine(LeavingCombat());
                }
            }
        }

        IEnumerator LeavingCombat()
        {
            yield return new WaitForSeconds(2f);
            isInCombat = false;
        }
    }
}
