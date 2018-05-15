using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour
{
    int counter;

    public bool wonGame { get; private set; }

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
        GetComponent<ThirdPersonUserControl>().enabled = false;
    }

    public void EnableControl()
    {
        GetComponent<ThirdPersonUserControl>().enabled = true;
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
