using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Cuisine : MonoBehaviour
{
    
    [SerializeField] GameObject prefabSteak, prefabPainHaut,prefabPainBas, prefabSalade, prefabChampi;
    [SerializeField] GameObject assiet;

    [SerializeField] GameObject cam;

    
    float rayRange = 10;


    //récupère les autres scripts
    [SerializeField] Commande commande;
    [SerializeField] SpawnFood spawnFood;
    [SerializeField] Poubelle poubelle;
    [SerializeField] ComptoirPosition comptoir;
    //poubelle
    [SerializeField] GameObject poubelleInterieur,poubelleExterieur;
    [SerializeField] RawImage poubelleImage;
    private bool poubelleInInventory;

    Stack<string> pileString = new Stack<string>();
    Stack<GameObject> pileGameObject = new Stack<GameObject>();

    List<string> listPainDansPile;
    //food
    private string Pain = "Pain";
    private string Viande = "Viande";
    private string Champignon = "Champignon";
    private string Salade = "Salade";
    private string Poubelle = "Poubelle";
    public bool sensPain = false;

    [SerializeField] TextMeshProUGUI instruction;

    [SerializeField] PNJManager pnjManager;
    public bool commandePrise;

    [SerializeField] Camion camion;
    private GameObject plat;
    private Vector3 posPlat;
    [SerializeField] GameObject platPrefab;

    public bool portePlat;
    private GameObject platTransporter;

    [SerializeField] SpawnStock spawnStock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        plat = GameObject.FindWithTag("Plat");
        posPlat = plat.transform.position;
        plat.GetComponent<Rigidbody>().isKinematic = true;

        portePlat = false;
        poubelleImage.enabled = false;
        poubelleInInventory = false;
        instruction.text = "";
        listPainDansPile = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {





        Raycast();
        retirer();
        VerifGiveUp();





    }
    private void Raycast()
    {
        //recupéré la nourriture
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * rayRange, Color.blue);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayRange))
        {
            //aficher text quand ont vise un ingredients
            if (hit.transform.gameObject.CompareTag(Pain) || hit.transform.gameObject.CompareTag(Viande) || hit.transform.gameObject.CompareTag(Champignon) || hit.transform.gameObject.CompareTag(Salade) || hit.transform.gameObject.CompareTag(Poubelle))
            {
                instruction.text = "Press E";
            }
            else
            {
                instruction.text = "";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //compte le nombre de pain
                int nbPain = 0;
                for (int i = 0; i < listPainDansPile.Count; i++)
                {
                    nbPain += 1;
                }

                //permet de définir le sens du pain
                if (nbPain % 2 == 0)
                {
                    sensPain = true;
                }
                else
                {
                    sensPain = false;
                }
                //ont regarde quelle ellement ont veut ajouter
                if (hit.transform.gameObject.CompareTag(Pain))
                {
                    if (spawnStock.pain.Count > 0)
                    {
                        GameObject ingredientsToDestroy = spawnStock.pain.Pop();
                        Destroy(ingredientsToDestroy);
                        pileString.Push(Pain);
                        listPainDansPile.Add(Pain);
                        if (sensPain)
                        {
                            spawnFood.Spawn(prefabPainBas, pileGameObject, plat);
                        }
                        else
                        {
                            spawnFood.Spawn(prefabPainHaut, pileGameObject, plat);
                        }
                    }
                    



                }
                if (hit.transform.gameObject.CompareTag(Viande))
                {
                    if (spawnStock.steack.Count > 0)
                    {
                        GameObject ingredientsToDestroy = spawnStock.steack.Pop();
                        Destroy(ingredientsToDestroy);
                        pileString.Push(Viande);
                        spawnFood.Spawn(prefabSteak, pileGameObject, plat);
                    }
                   
                }

                if (hit.transform.gameObject.CompareTag(Champignon))
                {
                    if (spawnStock.champi.Count > 0)
                    {
                        GameObject ingredientsToDestroy = spawnStock.champi.Pop();
                        Destroy(ingredientsToDestroy);
                        pileString.Push(Champignon);
                        spawnFood.Spawn(prefabChampi, pileGameObject, plat);
                    }
                   
                }


                if (hit.transform.gameObject.CompareTag(Salade))
                {
                    if (spawnStock.salade.Count > 0)
                    {
                        GameObject ingredientsToDestroy = spawnStock.salade.Pop();
                        Destroy(ingredientsToDestroy);
                        pileString.Push(Salade);
                        spawnFood.Spawn(prefabSalade, pileGameObject, plat);
                    }
                   
                }



                if (comptoir.ComptoirOccuper && hit.transform.CompareTag("borneComptoir"))
                {
                    
                    StartCoroutine(CommandePriseCoroutine());
                }

                if (hit.transform.gameObject == poubelleInterieur && !poubelleInInventory)
                {
                    poubelleImage.enabled = true;
                    poubelleInInventory = true;
                }
                if (hit.transform.gameObject == poubelleExterieur && poubelleInInventory)
                {
                    poubelleImage.enabled = false;
                    poubelleInInventory = false;
                    poubelle.resetPoubelle();
                    camion.RetirerPoubelle();
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !portePlat)
            {

                if (hit.transform.gameObject.CompareTag("Plat"))
                {
                    plat.GetComponent<Plat>().RecupererStack(pileGameObject);
                    plat.transform.SetParent(cam.transform);
                    plat.GetComponent<Rigidbody>().isKinematic = true;


                    portePlat = true;
                    platTransporter = hit.transform.gameObject;


                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && portePlat)
            {

                platTransporter.transform.SetParent(null);
                portePlat = false;
                platTransporter.GetComponent<Rigidbody>().isKinematic = false;
                StartCoroutine(PnjRecupDelay());
            }





        }
    }


    private void VerifGiveUp()
    {
        if(transform.position.y < -50)
        {
            GameManager.instance.GiveUp();
        }
    }


    IEnumerator PnjRecupDelay()
    {
        yield return new WaitForEndOfFrame();
        platTransporter.GetComponent<Plat>().PNJRecup();
    }

    private void retirer()
    {
        //retirer un element
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            try
            {
                listPainDansPile.Remove(Pain);
                pileString.Pop();
                GameObject temp = pileGameObject.Pop();
                Destroy(temp);
            }
            catch
            {

            }

        }
    }

    public void valider()
    {
        //valider la commande
        
        commande.CommandeTerminer(pileString);
        plat = Instantiate(platPrefab,posPlat,Quaternion.identity);
        plat.GetComponent<Rigidbody>().isKinematic = true;
    }
    
    public void ResetPile()
    {
        
        listPainDansPile.Clear();
        pileString.Clear();
        while (pileGameObject.Count > 0)
        {
            GameObject temp = pileGameObject.Pop();
           // Destroy(temp);
        }
    }

    private IEnumerator CommandePriseCoroutine()
    {

        commandePrise = true;
        yield return new WaitForSeconds(1);
        commandePrise = false;
    }
}
