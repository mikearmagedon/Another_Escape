using UnityEngine;

namespace RPG.Characters
{
    public class AreaOfEffectBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams abilityUseParams)
        {
            DealRadialDamage(abilityUseParams);
            PlayAudioClip();
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams abilityUseParams)
        {
            Collider[] targets = Physics.OverlapSphere(
                transform.position,
                (config as AreaOfEffectConfig).GetRadius(),
                LayerMask.GetMask("Enemy")
            );

            foreach (Collider target in targets)
            {
                float damageToDeal = abilityUseParams.baseDamage + (config as AreaOfEffectConfig).GetDamageToEachTarget();
                target.gameObject.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
            }
        }
    }
}
