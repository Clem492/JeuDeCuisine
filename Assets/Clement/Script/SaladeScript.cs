using UnityEngine;

public class SaladeScript : MonoBehaviour
{
    [SerializeField] SpawnStock SpawnStock;
    private Renderer renderer;
    private Collider col;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (SpawnStock.salade.Count > 0)
        {
            renderer.enabled = true;
            col.enabled = true;
        }
        else
        {
            renderer.enabled = false;
            col.enabled = false;
        }
    }
}
