using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{
    // Config
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 5f;
    [SerializeField] float chaseRadius = 10f;
    [SerializeField] float damagePerHit = 5f;
    [SerializeField] float minTimeBetweenHits = 0.5f;

    // State
    public float healthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }

    float currentHealtPoints;
    float lastHitTime = 0;

    // Cached components references
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    // Messages and methods
    void IDamageable.TakeDamage(float damage)
    {
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
        if (currentHealtPoints <= 0) { Destroy(gameObject); }
    }

    // Use this for initialization
    void Start()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");

        currentHealtPoints = maxHealthPoints;
	}
	
	// Update is called once per frame
	void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position,transform.position);
		if (distanceToPlayer <= attackRadius)
        {
            // TODO Range attack: spawn projectile
            
            // Face the player
            Vector3 facePlayer = (player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facePlayer), 0.2f);

            if ((Time.time - lastHitTime) > minTimeBetweenHits)
            {
                // Damage the player
                IDamageable damageable = player.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damagePerHit);
                }
                lastHitTime = Time.time;
            }
        }

        if (distanceToPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    void OnDrawGizmos()
    {
        // Draw attack radius sphere
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw chase radius sphere
        Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
