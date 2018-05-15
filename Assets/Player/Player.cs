using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public bool wonGame { get; private set; }

    [SerializeField] Camera mapCamera;
    
    int counter;

    private void Awake()
    {
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
        if (CrossPlatformInputManager.GetButtonDown("Map"))
        {
            mapCamera.enabled = !mapCamera.enabled;
        }
    }

    public void DisableControl()
    {
        GetComponent<FirstPersonController>().enabled = false;
    }

    public void EnableControl()
    {
        GetComponent<FirstPersonController>().enabled = true;
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
