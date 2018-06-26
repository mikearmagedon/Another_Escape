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
            float damageToDeal = abilityUseParams.baseDamage + config.GetExtraDamage();
            abilityUseParams.target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
