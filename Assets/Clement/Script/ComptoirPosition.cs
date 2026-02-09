using UnityEngine;

public class ComptoirPosition : MonoBehaviour
{
    public bool ComptoirOccuper;

    private void Start()
    {
        ComptoirOccuper = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            ComptoirOccuper = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            ComptoirOccuper = false;
        }
    }
}
