using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        const float PARTICLE_CLEAN_UP_DELAY = 10f;

        protected AbilityConfig config;

        public abstract void Use(AbilityUseParams useParams);

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
    }
}
