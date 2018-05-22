﻿using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire
using RPG.CameraUI;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        // Config
        [SerializeField] int enemyLayerNumber = 9;
        [SerializeField] float attackRadius = 2f;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        // State
        public bool wonGame { get; private set; }
        public float healthAsPercentage
        {
            get
            {
                return currentHealtPoints / maxHealthPoints;
            }
        }

        int counter;
        GameObject currentTarget;
        float currentHealtPoints;
        float lastHitTime = 0;
        GameObject weaponObject;

        // Cached components references
        CameraRaycaster cameraRaycaster;

        // Messages and methods
        void IDamageable.TakeDamage(float damage)
        {
            currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
            if (currentHealtPoints == 0)
            {
                wonGame = true;
            }
        }

        // Use this for initialization
        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            SetInitialWinConditionVariables();
            EquipWeapon(currentWeaponConfig);
            OverrideAnimatorController();
        }

        void OverrideAnimatorController()
        {
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = currentWeaponConfig.GetAttackAnimClip(); // remove const
        }

        void SetInitialWinConditionVariables()
        {
            wonGame = false;
            counter = 0;
        }

        void SetCurrentMaxHealth()
        {
            currentHealtPoints = maxHealthPoints;
        }

        public void EquipWeapon(Weapon weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); // empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.weaponGrip.transform.position;
            weaponObject.transform.localRotation = currentWeaponConfig.weaponGrip.transform.rotation;
            OverrideAnimatorController();
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length <= 0, "No DominantHand script found on player, please add one");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand scripts found on player, please have just one");
            return dominantHands[0].gameObject;
        }

        void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick; // registering
        }

        // Update is called once per frame
        void Update()
        {
            ScriptableObject.CreateInstance<Weapon>();
        }

        public void DisableControl()
        {
            GetComponent<PlayerMovement>().enabled = false;
        }

        public void EnableControl()
        {
            GetComponent<PlayerMovement>().enabled = true;
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

        void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayerNumber)
            {
                GameObject enemy = raycastHit.collider.gameObject;

                // Check if enemy is in range
                if ((enemy.transform.position - transform.position).magnitude > attackRadius)
                {
                    return;
                }

                currentTarget = enemy;

                if ((Time.time - lastHitTime) > minTimeBetweenHits)
                {
                    // Damage the enemy
                    IDamageable damageable = currentTarget.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damagePerHit);
                    }
                    lastHitTime = Time.time;
                }
            }
        }

        void OnDrawGizmos()
        {
            // Draw attack radius sphere
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}
