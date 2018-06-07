using UnityEngine;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;
        [SerializeField] bool destroyAfterPickedUp = true;

        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        void InstantiateWeapon()
        {
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            weaponPrefab.transform.position = Vector3.zero;
            Instantiate(weaponPrefab, gameObject.transform);
        }

        void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<PlayerController>().GetComponent<WeaponSystem>().EquipWeapon(weaponConfig);
            if (destroyAfterPickedUp)
            {
                AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);
                Destroy(gameObject);
            }
            else
            {
                audioSource.PlayOneShot(pickUpSFX);
            }
        }
    }
}
