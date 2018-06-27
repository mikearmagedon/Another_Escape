using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.CrossPlatformInput;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Energy))]
    public class PlayerController : MonoBehaviour
    {
        // Config
        [SerializeField] LayerMask enemyLayerMask;
        [SerializeField] SpecialAbility[] abilities; // TODO move to its own class

        // State
        public bool wonGame { get; set; }
        [HideInInspector] public bool isInCombat = false; // TODO consider using State enum
        Collider[] targets;

        // Cached components references
        Energy energy;
        Character character;
        WeaponSystem weaponSystem;

        void Start()
        {
            energy = GetComponent<Energy>();
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            AttachInitialAbilities();
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
                AttemptSpecialAbility(0);
            }
        }

        private void ProcessAbilityKey()
        {
            if (CrossPlatformInputManager.GetButtonDown("Special Ability 1"))
            {
                AttemptSpecialAbility(1);
            }
            else if (CrossPlatformInputManager.GetButtonDown("Special Ability 2"))
            {
                AttemptSpecialAbility(2);
            }
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachComponentTo(gameObject);
            }
        }

        void AttemptSpecialAbility(int abilityIndex)
        {
            if (energy.IsEnergyAvailable(abilities[abilityIndex].GetEnergyCost()))
            {
                energy.ConsumeEnergy(abilities[abilityIndex].GetEnergyCost());
                var abilityParams = new AbilityUseParams();
                if (targets.Length != 0)
                {
                    abilityParams = new AbilityUseParams(targets[0].gameObject, 10f); // TODO remove magic number
                }
                else
                {
                    abilityParams = new AbilityUseParams(null, 10f); // TODO remove magic number
                }
                abilities[abilityIndex].Use(abilityParams);
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
