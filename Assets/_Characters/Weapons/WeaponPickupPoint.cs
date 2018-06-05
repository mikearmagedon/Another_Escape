using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        void Update()
        {
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
            Instantiate(weaponPrefab, gameObject.transform);
        }

        void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<PlayerController>().GetComponent<WeaponSystem>().EquipWeapon(weaponConfig);
            AudioSource.PlayClipAtPoint(pickUpSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
