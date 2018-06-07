using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        public Transform weaponGrip;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] AudioClip attackSFX;
        [SerializeField] float timeBetweenAnimationCycles = 2f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float weaponDamage = 1f;
        [SerializeField] float damageDelay = 0.5f;

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetTimeBetweenAnimationCycles()
        {
            return timeBetweenAnimationCycles;
        }

        public float GetDamageDelay()
        {
            if (damageDelay > attackAnimation.length)
            {
                Debug.LogAssertion("Damage delay is longer then attack animation length.");
            }
            return damageDelay;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }
		
		public AudioClip GetAttackAudioClip()
        {
            return attackSFX;
        }

        // So that the RPG Character Animation Pack cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
