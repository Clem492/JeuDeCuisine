using UnityEngine;

public class SteackScript : MonoBehaviour
{

    [SerializeField] SpawnStock SpawnStock;
    private Renderer renderer;
    private Collider col;

    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        col = GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (SpawnStock.steack.Count >0)
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
