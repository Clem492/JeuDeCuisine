
using System.Collections.Generic;
using UnityEngine;

public class Plat : MonoBehaviour
{
    private BoxCollider collider;
    private Rigidbody rb;
    Stack<GameObject> pileBurger = new Stack<GameObject>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }
    public void RecupererStack(Stack<GameObject> burger)
    {
        pileBurger = burger;

        Vector3 size = collider.size;
        Vector3 center = collider.center;

        size.y = pileBurger.Peek().transform.position.y / 2;
        center.y = pileBurger.Peek().transform.position.y / 4;

        collider.size = size;
        collider.center = center;
    }

    private void Update()
    {
        rb.linearVelocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PNJ"))
        {
            Debug.Log("bro what are you doing");
            GameObject.FindWithTag("Player").GetComponent<Cuisine>().valider();
        }
    }
}
