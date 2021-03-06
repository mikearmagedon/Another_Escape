﻿using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            SelfHeal();
            PlayAudioClip();
            PlayParticleEffect();
            PlayAnimationClip();
        }

        private void SelfHeal()
        {
            GetComponent<HealthSystem>().Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
