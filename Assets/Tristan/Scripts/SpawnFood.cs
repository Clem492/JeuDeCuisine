using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    [SerializeField] Cuisine cuisine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    public void Spawn(GameObject go , Stack<GameObject> pileGo)
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, new Vector3(0, -25, 0), Color.red);
        Physics.Raycast(transform.position, Vector3.down, out hit, 25);
        
        if(go.CompareTag("Pain") && cuisine.sensPain == true)
        {
            pileGo.Push(Instantiate(go, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(180, 0, 0)));
        }
        else
        {
            //instancie le game object
            pileGo.Push(Instantiate(go, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity));
        }
            
        
        
    }
}
