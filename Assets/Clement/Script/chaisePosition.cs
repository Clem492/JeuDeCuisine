using UnityEngine;

public class chaisePosition : MonoBehaviour
{
    public bool chaiseOccuper;

    private void Start()
    {
        chaiseOccuper = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            chaiseOccuper = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            chaiseOccuper = false;
        }
    }
}
