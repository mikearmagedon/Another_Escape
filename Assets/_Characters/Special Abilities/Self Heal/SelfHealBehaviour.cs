using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config;

        public void SetConfig(SelfHealConfig selfHealConfig)
        {
            config = selfHealConfig;
        }

        public void Use(AbilityUseParams abilityUseParams)
        {
            SelfHeal(abilityUseParams);
            PlayParticleEffect();
        }

        private void SelfHeal(AbilityUseParams abilityUseParams)
        {
            GetComponent<HealthSystem>().Heal(config.GetExtraHealth());
        }

        private void PlayParticleEffect()
        {
            GameObject particleFXPrefab = config.GetParticleFXPrefab();
            var particleFXInstance = Instantiate(particleFXPrefab, transform.position, Quaternion.identity, transform);
            var particleSystem = particleFXInstance.GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(particleFXInstance, particleSystem.main.startLifetime.constantMax);
        }
    }
}
