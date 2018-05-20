﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    // Config
    [SerializeField] int enemyLayerNumber = 9;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxHealthPoints = 100f;

    // State
    public bool wonGame { get; private set; }
    public float healthAsPercentage
    {
        get
        {
            return currentHealtPoints / maxHealthPoints;
        }
    }

    int counter;
    GameObject currentTarget;
    float currentHealtPoints;
    float lastHitTime = 0;

    // Cached components references
    CameraRaycaster cameraRaycaster;

    // Messages and methods
    void IDamageable.TakeDamage(float damage)
    {
        currentHealtPoints = Mathf.Clamp(currentHealtPoints - damage, 0f, maxHealthPoints);
    }

    // Use this for initialization
    void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick; // registering

        currentHealtPoints = maxHealthPoints;
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

    private void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayerNumber)
        {
            GameObject enemy = raycastHit.collider.gameObject;

            // Check if enemy is in range
            if ((enemy.transform.position - transform.position).magnitude > attackRadius)
            {
                return;
            }

            currentTarget = enemy;

            // Face the enemy
            Vector3 faceEnemy = (currentTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(faceEnemy), 0.2f);

            if ((Time.time - lastHitTime) > minTimeBetweenHits)
            {
                // Damage the enemy
                Component damageableComponent = currentTarget.GetComponent(typeof(IDamageable));
                if (damageableComponent)
                {
                    (damageableComponent as IDamageable).TakeDamage(damagePerHit);
                }
                lastHitTime = Time.time;
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw attack radius sphere
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
