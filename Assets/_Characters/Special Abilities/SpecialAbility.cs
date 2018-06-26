using UnityEngine;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public GameObject target;
        public float baseDamage;

        public AbilityUseParams(GameObject target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams abilityUseParams)
        {
            behaviour.Use(abilityUseParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }
    }

    public interface ISpecialAbility
    {
        void Use(AbilityUseParams abilityUseParams);
    }
}
