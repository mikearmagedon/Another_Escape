using UnityEngine;

namespace RPG.Characters
{
    public class AreaOfEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaOfEffectConfig config;

        public void SetConfig(AreaOfEffectConfig areaOfEffectConfig)
        {
            config = areaOfEffectConfig;
        }

        public void Use(AbilityUseParams abilityUseParams)
        {
            DealRadialDamage(abilityUseParams);
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams abilityUseParams)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, config.GetRadius(), LayerMask.GetMask("Enemy"));

            foreach (Collider target in targets)
            {
                float damageToDeal = abilityUseParams.baseDamage + config.GetDamageToEachTarget();
                target.gameObject.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
            }
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
