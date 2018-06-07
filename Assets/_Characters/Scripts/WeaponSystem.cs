using System.Collections;
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
        AudioSource audioSource;
        Character character;
        float lastHitTime;

        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        const string ATTACK_TRIGGER = "Attack";

        void Start()
        {
            character = GetComponent<Character>();
            animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
            EquipWeapon(currentWeaponConfig);
            SetAttackAnimation();
        }

        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;

            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                // test if target is dead
                float targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                // test if target is out of range
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }

            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;
            if (characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
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
            weaponObject = Instantiate(weaponPrefab);
            weaponObject.transform.SetParent(dominantHand.transform, true);
            weaponObject.transform.localPosition = currentWeaponConfig.weaponGrip.transform.position;
            weaponObject.transform.localRotation = currentWeaponConfig.weaponGrip.transform.rotation;
        }

        public void StopAttacking()
        {
            StopAllCoroutines();
        }

        public void AttackTargets(Collider[] targets)
        {
            if ((Time.time - lastHitTime) > currentWeaponConfig.GetTimeBetweenAnimationCycles())
            {
                animator.SetTrigger(ATTACK_TRIGGER);
				audioSource.PlayOneShot(currentWeaponConfig.GetAttackAudioClip());
                foreach (var target in targets)
                {
                    // Damage the enemy
                    var damageable = target.GetComponent<HealthSystem>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(CalculateDamage());
                    }
                }
                lastHitTime = Time.time;
            }
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage > Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage > Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                var animationClip = currentWeaponConfig.GetAttackAnimClip();
                float animationClipTime = animationClip.length / character.GetAnimSpeedMultiplier();
                float timeToWait = animationClipTime + currentWeaponConfig.GetTimeBetweenAnimationCycles();

                bool isTimeToHitAgain = (Time.time - lastHitTime) > timeToWait;

                if (isTimeToHitAgain)
                {
                    AttackTargetOnce();

                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        void AttackTargetOnce()
        {
            SetAttackAnimation();
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = currentWeaponConfig.GetDamageDelay();
			audioSource.PlayOneShot(currentWeaponConfig.GetAttackAudioClip());
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length <= 0, "No DominantHand script found on " + gameObject.name + ", please add one.");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand scripts found on " + gameObject.name + ", please have just one.");
            return dominantHands[0].gameObject;
        }

        void SetAttackAnimation()
        {
            Assert.IsNotNull(character.GetOverrideController(), "Please provide " + gameObject + " with an animator override controller.");
            var animatorOverrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        float CalculateDamage()
        {
            return baseDamage + currentWeaponConfig.GetWeaponDamage();
        }
    }
}
