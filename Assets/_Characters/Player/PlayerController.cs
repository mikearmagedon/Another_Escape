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
            SetInitialWinConditionVariables();

            abilities[0].AttachComponentTo(gameObject);
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
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            character.Move(movement, false);
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

        void ProcessMouseClick()
        {
            if (CrossPlatformInputManager.GetButtonDown("Attack"))
            {
                weaponSystem.AttackTargets(targets);
            }
            else if (CrossPlatformInputManager.GetButtonDown("Special Ability"))
            {
                AttemptSpecialAbility(0);
            }
        }

        void AttemptSpecialAbility(int abilityIndex)
        {
            if (energy.IsEnergyAvailable(abilities[abilityIndex].GetEnergyCost()))
            {
                energy.ConsumeEnergy(abilities[abilityIndex].GetEnergyCost());
                var abilityParams = new AbilityUseParams(targets[0].gameObject, 10f); // TODO remove magic number
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
