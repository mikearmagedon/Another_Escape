using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int counter;

	// Use this for initialization
	void Start()
    {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update()
    {
		
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
                print("Game Over");
            }
        }
    }
}
