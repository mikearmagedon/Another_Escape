using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(movementVector * Time.deltaTime);
	}
}
