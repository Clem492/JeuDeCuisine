using NootColis;
using NootColis.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStock : MonoBehaviour
{
    public Stack<GameObject> steack;
    public Stack<GameObject> pain;
    public Stack<GameObject> salade;
    public Stack<GameObject> champi;

    //=============
    //     Laser
    //=============
    [SerializeField] Transform laserSteack;
    [SerializeField] Transform laserPain;
    [SerializeField] Transform laserSalade;
    [SerializeField] Transform laserChampi;

    //=============
    //    Prefab
    //=============
    [SerializeField] GameObject steackPrefab;
    [SerializeField] GameObject painPrefab;
    [SerializeField] GameObject saladePrefab;
    [SerializeField] GameObject champiPrefab;

    RaycastHit hit;

    private void Start()
    {
        NootColisAPI.GetStreamOfColis("Resto");
        steack = new Stack<GameObject>();
        pain = new Stack<GameObject>();
        salade = new Stack<GameObject>();
        champi = new Stack<GameObject>();

      

        for(int i = 0; i < 2; i++)
        {
            if (Physics.Raycast(laserSteack.position, Vector3.down * 300, out hit))
            {
                GameObject objectInstantiate = Instantiate(steackPrefab, hit.point, Quaternion.Euler(-90, 0, 0));
                steack.Push(objectInstantiate);
            }
            if (Physics.Raycast(laserPain.position, Vector3.down * 300, out hit))
            {
                GameObject objectInstantiate = Instantiate(painPrefab, hit.point, Quaternion.identity);
                pain.Push(objectInstantiate);
            }
            if (Physics.Raycast(laserSalade.position, Vector3.down * 300, out hit))
            {
                GameObject objectInstantiate = Instantiate(saladePrefab, hit.point, Quaternion.identity);
                salade.Push(objectInstantiate);
            }
            if (Physics.Raycast(laserChampi.position, Vector3.down * 300, out hit))
            {
                GameObject objectInstantiate = Instantiate(champiPrefab, hit.point, Quaternion.identity);
                champi.Push(objectInstantiate);
            }
        }

    }

    private void Update()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (NootColisAPI.GetInboxCount("Resto") > 0)
        {
            Colis colis = NootColisAPI.PopColis("Resto");
            string truc = colis.contenu;
            string[] tabTrucs;
            tabTrucs = truc.Split("_");
            int nb = Int32.Parse(tabTrucs[1]);
            if (tabTrucs[0]=="Steack")
            {
              
                
                for (int i = 0; i < nb; i++)
                {
                    if (Physics.Raycast(laserSteack.position, Vector3.down * 300, out hit))
                    {
                        GameObject objectInstantiate = Instantiate(steackPrefab, hit.point, Quaternion.Euler(-90, 0, 0));
                        steack.Push(objectInstantiate);
                    }
                }
                    
            }
            if (tabTrucs[0] == "Pain")
            {
                
                for (int i = 0; i < nb; i++)
                {
                    if (Physics.Raycast(laserPain.position, Vector3.down * 300, out hit))
                    {
                        GameObject objectInstantiate = Instantiate(painPrefab, hit.point, Quaternion.identity);
                        pain.Push(objectInstantiate);
                    }
                }
                
            }
            if (tabTrucs[0] == "Salade")
            {
                for (int i = 0; i < nb; i++)
                {
                    if (Physics.Raycast(laserSalade.position, Vector3.down * 300, out hit))
                    {
                        GameObject objectInstantiate = Instantiate(saladePrefab, hit.point, Quaternion.identity);
                        salade.Push(objectInstantiate);
                    }
                }
                    
            }
            if (tabTrucs[0] == "Champi")
            {
                for (int i = 0; i < nb; i++)
                {
                    if (Physics.Raycast(laserChampi.position, Vector3.down * 300, out hit))
                    {
                        GameObject objectInstantiate = Instantiate(champiPrefab, hit.point, Quaternion.identity);
                        champi.Push(objectInstantiate);
                    }
                }
                    
            }
            
            

        }
    }

}
