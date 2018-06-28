using UnityEngine;

namespace RPG.Characters
{
    public class AreaOfEffectBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            DealRadialDamage();
            PlayAudioClip();
            PlayParticleEffect();
            PlayAnimationClip();
        }

        private void DealRadialDamage()
        {
            Collider[] targets = Physics.OverlapSphere(
                transform.position,
                (config as AreaOfEffectConfig).GetRadius(),
                LayerMask.GetMask("Enemy")
            );

            foreach (Collider target in targets)
            {
                float damageToDeal = (config as AreaOfEffectConfig).GetDamageToEachTarget();
                target.gameObject.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
            }
        }
    }
}
