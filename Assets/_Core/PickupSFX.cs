using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] int pickupScoreValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().AddToScore(pickupScoreValue);
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, 0.5f);
            Destroy(gameObject);
        }
    }
}
