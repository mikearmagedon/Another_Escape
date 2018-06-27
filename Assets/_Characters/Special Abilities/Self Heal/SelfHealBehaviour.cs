using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams abilityUseParams)
        {
            SelfHeal(abilityUseParams);
            PlayAudioClip();
            PlayParticleEffect();
        }

        private void SelfHeal(AbilityUseParams abilityUseParams)
        {
            GetComponent<HealthSystem>().Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
