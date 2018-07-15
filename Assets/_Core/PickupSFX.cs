using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] int pickupScoreValue = 1;
    [HideInInspector] public bool isActive;
    [HideInInspector] public string pickupName;

    private void Awake()
    {
        isActive = true;
        pickupName = gameObject.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AddToScore(pickupScoreValue);
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, 0.5f);
            //Destroy(gameObject);
            isActive = false;
            gameObject.SetActive(false);
        }
    }
}
