using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        const float PARTICLE_CLEAN_UP_DELAY = 10f;
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        protected AbilityConfig config;

        public abstract void Use(GameObject target = null);

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            GameObject particleFXPrefab = config.GetParticleFXPrefab();
            var particleFXInstance = Instantiate(particleFXPrefab, transform.position, particleFXPrefab.transform.rotation, transform);
            particleFXInstance.GetComponent<ParticleSystem>().Play();
            Destroy(particleFXInstance, PARTICLE_CLEAN_UP_DELAY);
        }

        protected void PlayAudioClip()
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(config.GetRandomAudioClip());
        }

        protected void PlayAnimationClip()
        {
            Assert.IsNotNull(GetComponent<Character>().GetOverrideController(), "Please provide " + gameObject + " with an animator override controller.");
            var animatorOverrideController = GetComponent<Character>().GetOverrideController();
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = config.GetAnimationClip();
            animator.SetTrigger(ATTACK_TRIGGER);
        }
    }
}
