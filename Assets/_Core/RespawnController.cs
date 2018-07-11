using UnityEngine;
using System.Collections;
public class RespawnController : MonoBehaviour
{
    public CheckPoint respawningCheckPoint = null;

    public delegate void MyDelegate();
    public event MyDelegate onRespawn;

    Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
        respawningCheckPoint.onRespawn += OnRespawn;
    }

    public void OnRespawn()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
        onRespawn();
    }
}
