using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class AbilitySystem : MonoBehaviour
    {
        // Config
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyOrb;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenEnergyPointsPerSecond = 5f;
        [SerializeField] AudioClip outOfEnergyClip;

        // State
        public float EnergyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }
        [SerializeField]public float currentEnergyPoints;

        // Messages and methods
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        // Update is called once per frame
        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergy();
                UpdateEnergyBar();
            }
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            if (abilities[abilityIndex].GetEnergyCost() <= currentEnergyPoints)
            {
                ConsumeEnergy(abilities[abilityIndex].GetEnergyCost());
                abilities[abilityIndex].Use(target);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(outOfEnergyClip);
            }
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        void ConsumeEnergy(float amount)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        void AddEnergy()
        {
            float energyPointsToRegen = regenEnergyPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + energyPointsToRegen, 0, maxEnergyPoints);
        }

        void UpdateEnergyBar()
        {
            if (energyOrb)
            {
                energyOrb.fillAmount = EnergyAsPercentage;
            }
        }
    }
}
