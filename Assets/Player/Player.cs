using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public bool wonGame { get; private set; }
    public float healthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }

    [SerializeField] float maxHealthPoints = 100f;

    int counter;
    float currentHealtPoints = 100f;

    void IDamageable.TakeDamage(float damage)
    {
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
    }

    // Use this for initialization
    void Start()
    {
        wonGame = false;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}

    public void DisableControl()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void EnableControl()
    {
        GetComponent<PlayerMovement>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
            counter++;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            if (counter == 3)
            {
                wonGame = true;
            }
        }
    }
}
