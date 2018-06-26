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
            Collider[] targets = Physics.OverlapSphere(transform.position, config.GetRadius(), LayerMask.GetMask("Enemy"));

            foreach (Collider target in targets)
            {
                float damageToDeal = abilityUseParams.baseDamage + config.GetDamageToEachTarget();
                target.gameObject.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
            }
        }
    }
}
