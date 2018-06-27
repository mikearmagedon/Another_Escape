using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig powerAttackConfig)
        {
            config = powerAttackConfig;
        }

        public void Use(AbilityUseParams abilityUseParams)
        {
            DealDamage(abilityUseParams);
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams abilityUseParams)
        {
            float damageToDeal = abilityUseParams.baseDamage + config.GetExtraDamage();
            abilityUseParams.target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }

        private void PlayParticleEffect()
        {
            GameObject particleFXPrefab = config.GetParticleFXPrefab();
            var particleFXInstance = Instantiate(particleFXPrefab, transform.position, Quaternion.identity, transform);
            var particleSystem = particleFXInstance.GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(particleFXInstance, particleSystem.main.duration);
        }
    }
}
