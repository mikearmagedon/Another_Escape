using UnityEngine;
using UnityEngine.UI;

public class TriggerText : MonoBehaviour
{
    [SerializeField] Text textBox;
    [SerializeField] string text;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!textBox.enabled)
            {
                textBox.enabled = true;
            }

            textBox.text = text;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (textBox.enabled)
            {
                textBox.enabled = false;
            }

            textBox.text = string.Empty;
        }
    }
}
