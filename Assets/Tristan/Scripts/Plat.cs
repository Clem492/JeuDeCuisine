
using System.Collections.Generic;
using UnityEngine;

public class Plat : MonoBehaviour
{
    private BoxCollider collider;
    Stack<GameObject> pileBurger = new Stack<GameObject>();

    private void Start()
    {
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
}
