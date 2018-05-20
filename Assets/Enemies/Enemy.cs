using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float healthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }

    [SerializeField] float maxHealthPoints = 100f;

    float currentHealtPoints = 100f;
    // Use this for initialization
    void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}
}
