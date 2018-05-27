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

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
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
            audioSource.PlayOneShot(pickUpSFX);
        }
    }
}
