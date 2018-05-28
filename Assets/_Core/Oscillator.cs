using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    const float tau = 2f * Mathf.PI;

    float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPosition;

    // Use this for initialization
    void Start()
	{
        startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{
        if (period <= Mathf.Epsilon) { return; } // protect from period = 0

        float cycles = Time.time / period; // grows continually from 0
        movementFactor = Mathf.Sin(cycles * tau) / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
	}
}
