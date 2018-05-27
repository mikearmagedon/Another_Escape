using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RPG.Characters;

public class HealthSystem : MonoBehaviour
{
    // Config
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] RawImage healthBar;
    [SerializeField] AudioClip[] damageSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] float deathVanishSeconds = 2.0f;

    // State
    public float healthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }
    const string DEATH_TRIGGER = "Death";
    float currentHealtPoints;

    // Cached components references
    Animator animator;
    AudioSource audioSource;
    Character characterMovement;

    // Messages and methods
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterMovement = GetComponent<Character>();

        currentHealtPoints = maxHealthPoints;
	}
	
	// Update is called once per frame
	void Update()
    {
        UpdateHealthBar();
	}

    void UpdateHealthBar()
    {
        if (healthBar) // NPCs may not have health bars to update
        {
            float xValue = -(healthAsPercentage / 2f) - 0.5f;
            healthBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }

    public void TakeDamage(float damage)
    {
        bool characterDies = ((currentHealtPoints - damage) <= 0);
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
        //AudioClip clip = damageSounds[Random.Range(0, damageSounds.Length)];
        //audioSource.PlayOneShot(clip);
        if (characterDies)
        {
            StartCoroutine(KillCharacter());
        }
    }

    void Heal(float points)
    {
        currentHealtPoints = Mathf.Clamp(currentHealtPoints + points, 0f, maxHealthPoints);
    }

    IEnumerator KillCharacter()
    {
        characterMovement.Kill();
        animator.SetTrigger(DEATH_TRIGGER);

        var playerComponent = GetComponent<Player>();

        if (playerComponent && playerComponent.isActiveAndEnabled)
        {
            //audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
            //audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            playerComponent.wonGame = true;
        }
        else // assuming is enemy for now, reconsider for other NPCs
        {
            Destroy(gameObject, deathVanishSeconds);
        }

    }
}
