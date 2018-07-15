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
                float targetHealth = target.GetComponent<HealthSystem>().HealthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                // test if target is out of range
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }

            float characterHealth = GetComponent<HealthSystem>().HealthAsPercentage;
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
            foreach (var target in targets)
            {
                this.target = target.gameObject;
            }

            var animationClip = currentWeaponConfig.GetRandomAttackAnimClip();
            float animationClipTime = animationClip.length / character.GetAnimSpeedMultiplier();
            float timeToWait = animationClipTime + currentWeaponConfig.GetTimeBetweenAnimationCycles();

            bool isTimeToHitAgain = (Time.time - lastHitTime) > timeToWait;
            if (isTimeToHitAgain)
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
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
            bool attackerStillAlive = GetComponent<HealthSystem>().HealthAsPercentage > Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().HealthAsPercentage > Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                var animationClip = currentWeaponConfig.GetRandomAttackAnimClip();
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
        }

        // Attack animation callback
        void Hit()
        {
            bool targetStillAlive = target && target.GetComponent<HealthSystem>().HealthAsPercentage > Mathf.Epsilon;
            if (targetStillAlive)
            {
                audioSource.PlayOneShot(currentWeaponConfig.GetAttackAudioClip());
                target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
            }
            else
            {
                audioSource.PlayOneShot(currentWeaponConfig.GetSwingAudioClip());
            }
        }

        // Attack animation callback
        // TODO spawn projectile
        void Shoot()
        {
            Hit();
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
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetRandomAttackAnimClip();
        }

        float CalculateDamage()
        {
            return baseDamage + currentWeaponConfig.GetWeaponDamage();
        }
    }
}
