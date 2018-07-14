using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using RPG.Characters;

public class HealthSystem : MonoBehaviour
{
    // Config
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] Image healthBar;
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

    // Cached components references
    Animator animator;
    Character character;
    AudioManager audioManager;

    // Messages and methods
    void Start()
    {
        audioManager = AudioManager.instance;
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();

        currentHealtPoints = maxHealthPoints;
	}

	void Update()
    {
        UpdateHealthBar();
	}

    void UpdateHealthBar()
    {
        if (healthBar) // NPCs may not have health bars to update
        {
            healthBar.fillAmount = HealthAsPercentage;
        }
    }

    public void TakeDamage(float damage)
    {
        bool characterDies = ((currentHealtPoints - damage) <= 0);
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);

        if (characterDies && character.IsAlive())
        {
            StartCoroutine(KillCharacter());
        }
        else if (character.IsAlive())
        {
            AudioClip clip = damageSounds[Random.Range(0, damageSounds.Length)];
            audioManager.PlayMisc(clip);
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

        character.Kill();
        animator.SetTrigger(DEATH_TRIGGER);

        AudioClip clip = deathSounds[Random.Range(0, deathSounds.Length)];
        audioManager.PlayMisc(clip);
        yield return new WaitForSecondsRealtime(clip.length);

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
