using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        public Transform weaponGrip;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip[] attackAnimation;
        [SerializeField] AudioClip attackSFX;
        [SerializeField] AudioClip swingSFX;
        [SerializeField] float timeBetweenAnimationCycles = 2f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float weaponDamage = 1f;

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetTimeBetweenAnimationCycles()
        {
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetRandomAttackAnimClip()
        {
            return attackAnimation[Random.Range(0, attackAnimation.Length)];
        }
		
		public AudioClip GetAttackAudioClip()
        {
            return attackSFX;
        }

        public AudioClip GetSwingAudioClip()
        {
            return swingSFX;
        }
    }
}
