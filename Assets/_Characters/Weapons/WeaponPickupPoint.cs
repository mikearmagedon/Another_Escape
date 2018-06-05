using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        AudioSource audioSource;
        GameObject weapon;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            DestroyChildren();
            InstantiateWeapon();
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
            weapon = Instantiate(weaponPrefab, gameObject.transform);
        }

        void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<PlayerController>().GetComponent<WeaponSystem>().EquipWeapon(weaponConfig);
            audioSource.PlayOneShot(pickUpSFX);
            weapon.SetActive(false);
            Destroy(gameObject, pickUpSFX.length);
        }
    }
}
