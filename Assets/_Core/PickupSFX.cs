using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, 0.5f);
            ScoreManager.score++;
            Destroy(gameObject);
        }
    }
}
