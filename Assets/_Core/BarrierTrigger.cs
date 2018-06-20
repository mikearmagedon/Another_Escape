using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class BarrierTrigger : MonoBehaviour
{
    [SerializeField] int requiredAmountToUnlock;
    [SerializeField] GameObject gate;
    [SerializeField] AudioClip openingGateSFX;
    [SerializeField] Text textBox;

    AudioSource audioSource;
    Vector3 startingPosition;
    Vector3 endingPosition;
    bool isLocked = true;
    float period;
    float timeStartedLerping;

    const float tau = 2f * Mathf.PI;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        textBox.text = requiredAmountToUnlock.ToString();
        startingPosition = gate.transform.position;
        endingPosition = startingPosition + (2 * Vector3.up);
        period = openingGateSFX.length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isLocked && (FindObjectOfType<GameManager>().score >= requiredAmountToUnlock))
            {
                GetComponentInChildren<Canvas>().enabled = false;
                isLocked = false;
                timeStartedLerping = Time.time;
                audioSource.PlayOneShot(openingGateSFX);
                StartCoroutine(UnlockGate());
            }
        }
    }

    IEnumerator UnlockGate()
    {
        while (true)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / period;
            gate.transform.position = Vector3.Lerp(startingPosition, endingPosition, percentageComplete);
            yield return new WaitForEndOfFrame();
            if (percentageComplete >= 1.0f)
            {
                StopAllCoroutines();
            }
        }
    }
}
