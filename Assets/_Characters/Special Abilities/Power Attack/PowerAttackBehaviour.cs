using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        public void SetConfig(PowerAttackConfig powerAttackConfig)
        {
            config = powerAttackConfig;
        }
    }
}
