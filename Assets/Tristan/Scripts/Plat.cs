
using System.Collections.Generic;
using UnityEngine;

public class Plat : MonoBehaviour
{
    private BoxCollider collider;
    private Rigidbody rb;
    private Ray ray;
    private Stack<GameObject> pileBurger = new Stack<GameObject>();
    private Commande commande;
    private Collider[] tabSphere;
    private Cuisine cuisine;
    private void Start()
    {
        commande = GameObject.FindWithTag("commande").GetComponent<Commande>();
        cuisine= GameObject.FindWithTag("Player").GetComponent<Cuisine>();
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
    
    public void PNJRecup()
    {
        tabSphere = Physics.OverlapSphere(transform.position, 1);
        
        foreach(Collider co in tabSphere)
        {
            if (co.transform.gameObject.CompareTag("PNJ") && co.transform.gameObject == commande.fileClients.Peek())
            {
                Debug.Log("c'est validé mon gas !!");
                cuisine.valider();
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1);
    }
}
