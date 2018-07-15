using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class TextTrigger : MonoBehaviour
{
    [SerializeField] Text textBox;
    [SerializeField][TextArea] string displayText;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!textBox.enabled)
            {
                textBox.enabled = true;
            }

            textBox.text = displayText;
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
