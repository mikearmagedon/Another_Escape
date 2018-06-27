using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.CrossPlatformInput;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(AbilitySystem))]
    public class PlayerController : MonoBehaviour
    {
        // Config
        [SerializeField] LayerMask enemyLayerMask;

        // State
        public bool wonGame { get; set; }
        [HideInInspector] public bool isInCombat = false; // TODO consider using State enum
        Collider[] targets;

        // Cached components references
        Character character;
        WeaponSystem weaponSystem;
        AbilitySystem abilitySystem;

        void Start()
        {
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            abilitySystem = GetComponent<AbilitySystem>();
            
            SetInitialWinConditionVariables();
        }

        void Update()
        {
            FindTargetsInRange();
            ProcessMouseClick();
            ProcessAbilityKey();
        }

        // Fixed update is called in sync with physics
        void FixedUpdate()
        {
            ProcessKeyboardMovement();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                if (FindObjectOfType<GameManager>().score <= 3)
                {
                    wonGame = true;
                }
            }
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
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            character.Move(movement, false);
        }

        void ProcessMouseClick()
        {
            if (CrossPlatformInputManager.GetButtonDown("Attack"))
            {
                weaponSystem.AttackTargets(targets);
            }
            else if (CrossPlatformInputManager.GetButtonDown("Special Ability 0"))
            {
                if (targets.Length != 0)
                {
                    abilitySystem.AttemptSpecialAbility(0, targets[0].gameObject);
                }
                else
                {
                    abilitySystem.AttemptSpecialAbility(0);
                }
            }
        }

        private void ProcessAbilityKey()
        {
            if (CrossPlatformInputManager.GetButtonDown("Special Ability 1"))
            {
                abilitySystem.AttemptSpecialAbility(1);
            }
            else if (CrossPlatformInputManager.GetButtonDown("Special Ability 2"))
            {
                abilitySystem.AttemptSpecialAbility(2);
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
