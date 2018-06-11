using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;

    void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(pickupSFX, transform.position, 0.5f);
    }
}
