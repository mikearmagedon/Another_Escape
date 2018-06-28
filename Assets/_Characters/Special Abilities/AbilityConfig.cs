using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particleFXPrefab;
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] AnimationClip abilityAnimation;

        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

        public void AttachAbilityTo(GameObject gameObjectToAttachTo)
        {
            behaviour = GetBehaviourComponent(gameObjectToAttachTo);
            behaviour.SetConfig(this);
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
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

        public AnimationClip GetAnimationClip()
        {
            return abilityAnimation;
        }
    }
}
