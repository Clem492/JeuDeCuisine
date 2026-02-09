using UnityEngine;

public class borne : MonoBehaviour
{
    public bool borneOccuper;

    private void Start()
    {
        borneOccuper = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            borneOccuper = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            borneOccuper = false;
        }
    }
}
