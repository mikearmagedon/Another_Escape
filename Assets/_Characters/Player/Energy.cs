using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        // Config
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenEnergyPointsPerSecond = 5f;

        // State
        public float EnergyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }
        float currentEnergyPoints;

        // Cached components references

        // Messages and methods
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
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

        private void AddEnergy()
        {
            float energyPointsToRegen = regenEnergyPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + energyPointsToRegen, 0, maxEnergyPoints);
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        void UpdateEnergyBar()
        {
            if (energyBar)
            {
                float xValue = -(EnergyAsPercentage / 2f) - 0.5f;
                energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
            }
        }
    }
}
