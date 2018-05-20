using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{
    public float healthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 5f;

    float currentHealtPoints = 100f;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    void IDamageable.TakeDamage(float damage)
    {
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
    }

    // Use this for initialization
    void Start()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position,transform.position);
		if (distanceToPlayer <= attackRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
	}
}
