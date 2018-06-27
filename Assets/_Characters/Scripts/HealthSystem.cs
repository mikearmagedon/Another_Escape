﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using RPG.Characters;

public class HealthSystem : MonoBehaviour
{
    // Config
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] Image healthOrb;
    [SerializeField] float healtRegenStartDelay = 5f;
    [SerializeField] float regenHealthPointsPerSecond = 5f;
    [SerializeField] AudioClip[] damageSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] float deathVanishSeconds = 2.0f;

    // State
    public float HealthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }
    const string DEATH_TRIGGER = "Death";
    float currentHealtPoints;
    bool isHealing = false; // TODO consider using State enum

    // Cached components references
    Animator animator;
    AudioSource audioSource;
    Character characterMovement;

    // Messages and methods
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterMovement = GetComponent<Character>();

        currentHealtPoints = maxHealthPoints;
	}

	void Update()
    {
        if (GetComponent<PlayerController>())
        {
            HandleHealthRegen();
        }

        UpdateHealthBar();
	}

    void HandleHealthRegen()
    {
        if (!(GetComponent<PlayerController>().isInCombat))
        {
            if (!isHealing && currentHealtPoints < maxHealthPoints)
            {
                StartCoroutine(RegenerateHealth());
            }
        }
        else
        {
            if (isHealing)
            {
                isHealing = false;
                StopAllCoroutines();
            }
        }
    }

    IEnumerator RegenerateHealth()
    {
        isHealing = true;
        yield return new WaitForSeconds(healtRegenStartDelay);
        while (currentHealtPoints < maxHealthPoints)
        {
            float healthPointsToRegen = regenHealthPointsPerSecond * Time.deltaTime;
            Heal(healthPointsToRegen);
            yield return null;
        }
        isHealing = false;
    }

    void UpdateHealthBar()
    {
        if (healthOrb) // NPCs may not have health bars to update
        {
            healthOrb.fillAmount = HealthAsPercentage;
        }
    }

    public void TakeDamage(float damage)
    {
        bool characterDies = ((currentHealtPoints - damage) <= 0);
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
        AudioClip clip = damageSounds[Random.Range(0, damageSounds.Length)];
        audioSource.PlayOneShot(clip);
        if (characterDies)
        {
            StartCoroutine(KillCharacter());
        }
    }

    public void Heal(float amountToHeal)
    {
        currentHealtPoints = Mathf.Clamp(currentHealtPoints + amountToHeal, 0f, maxHealthPoints);
    }

    IEnumerator KillCharacter()
    {
        var playerComponent = GetComponent<PlayerController>();
        if (playerComponent && playerComponent.isActiveAndEnabled)
        {
            playerComponent.enabled = false;
        }

        characterMovement.Kill();
        animator.SetTrigger(DEATH_TRIGGER);

        audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
        audioSource.Play(); // override any playing sounds
        yield return new WaitForSecondsRealtime(audioSource.clip.length);

        if (playerComponent)
        {
            playerComponent.wonGame = true;
        }
        else // assuming is enemy for now, reconsider for other NPCs
        {
            Destroy(gameObject, deathVanishSeconds);
        }

    }
}
