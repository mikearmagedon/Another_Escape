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

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particleFXPrefab;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

        public void AttachAbilityTo(GameObject gameObjectToAttachTo)
        {
            behaviour = GetBehaviourComponent(gameObjectToAttachTo);
            behaviour.SetConfig(this);
        }

        public void Use(AbilityUseParams abilityUseParams)
        {
            behaviour.Use(abilityUseParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticleFXPrefab()
        {
            return particleFXPrefab;
        }

        public AudioClip GetRandomAudioClip()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
