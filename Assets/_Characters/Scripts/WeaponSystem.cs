using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig;

        GameObject weaponObject;
        GameObject target;
        Animator animator;
        Character character;
        float lastHitTime;

        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        const string ATTACK = "Attack";

        // Use this for initialization
        void Start()
        {
            character = GetComponent<Character>();
            EquipWeapon(currentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            Assert.IsNotNull(weaponConfig, "Please assign a weapon to the player");
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); // empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.weaponGrip.transform.position;
            weaponObject.transform.localRotation = currentWeaponConfig.weaponGrip.transform.rotation;
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            // TODO use coroutine to setup repeating attack
            target = targetToAttack;
            print("Attacking" + target);
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length <= 0, "No DominantHand script found on player, please add one");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand scripts found on player, please have just one");
            return dominantHands[0].gameObject;
        }

        void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            var animatorOverrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        void AttackTargets(Collider[] targets)
        {
            if ((Time.time - lastHitTime) > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                animator.SetTrigger(ATTACK);
                foreach (var target in targets)
                {
                    // Damage the enemy
                    var damageable = target.GetComponent<HealthSystem>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(baseDamage);
                    }
                }
                lastHitTime = Time.time;
            }
        }
    }
}
