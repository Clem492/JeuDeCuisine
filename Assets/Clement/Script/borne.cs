using UnityEngine;

public class borne : MonoBehaviour
{
    public bool borneOccuper;

    private void Start()
    {
        borneOccuper = false;
    }

   

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PNJ"))
        {
            borneOccuper = false;
        }
    }
}
