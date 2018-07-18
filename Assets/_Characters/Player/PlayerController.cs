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
        [HideInInspector] [SerializeField] public Vector3 position;

        // State
        public bool wonGame { get; set; }
        Collider[] targets;

        // Cached components references
        Character character;
        WeaponSystem weaponSystem;
        AbilitySystem abilitySystem;
        SaveLoad saveLoad;

        void Start()
        {
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            abilitySystem = GetComponent<AbilitySystem>();
            saveLoad = FindObjectOfType<SaveLoad>();

            position = transform.position;
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
                if (GameManager.instance.score <= 3)
                {
                    wonGame = true;
                }
            }

            if (other.gameObject.CompareTag("Checkpoint"))
            {
                position = gameObject.transform.position;
                saveLoad.Save();
            }

            if (other.gameObject.CompareTag("Trap"))
            {
                GetComponent<HealthSystem>().TakeDamage(20f);
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

        void ProcessAbilityKey()
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

        void FindTargetsInRange()
        {
            Assert.IsFalse(enemyLayerMask == 0, "Please set enemyLayerMask to the Enemy layer");
            targets = Physics.OverlapSphere(transform.position, weaponSystem.GetCurrentWeapon().GetMaxAttackRange(), enemyLayerMask);
        }
    }
}

