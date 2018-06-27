using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams abilityUseParams)
        {
            if (abilityUseParams.target != null)
            {
                DealDamage(abilityUseParams);
            }
            PlayAudioClip();
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams abilityUseParams)
        {
            float damageToDeal = abilityUseParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            abilityUseParams.target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
