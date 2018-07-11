using UnityEngine;

public class WaterDamage : MonoBehaviour {

    public float damage;

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }
}
