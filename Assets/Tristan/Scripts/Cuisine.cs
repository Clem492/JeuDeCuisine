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
    [SerializeField] Plat plat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poubelleImage.enabled = false;
        poubelleInInventory = false;
        instruction.text = "";
        listPainDansPile = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        



        //recupéré la nourriture
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * rayRange, Color.blue);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayRange))
        {
            //aficher text quand ont vise un ingredients
            if(hit.transform.gameObject.CompareTag(Pain) || hit.transform.gameObject.CompareTag(Viande) || hit.transform.gameObject.CompareTag(Champignon) || hit.transform.gameObject.CompareTag(Salade) || hit.transform.gameObject.CompareTag(Poubelle))
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

                    pileString.Push(Pain);
                    listPainDansPile.Add(Pain);
                    if (sensPain)
                    {
                        spawnFood.Spawn(prefabPainBas, pileGameObject);
                    }
                    else
                    {
                        spawnFood.Spawn(prefabPainHaut, pileGameObject);
                    }
                    


                }
                if (hit.transform.gameObject.CompareTag(Viande))
                {
                    pileString.Push(Viande);
                    spawnFood.Spawn(prefabSteak, pileGameObject);
                }
                if (hit.transform.gameObject.CompareTag(Champignon))
                {
                    pileString.Push(Champignon);
                    spawnFood.Spawn(prefabChampi, pileGameObject);
                }
                if (hit.transform.gameObject.CompareTag(Salade))
                {
                    pileString.Push(Salade);
                    spawnFood.Spawn(prefabSalade, pileGameObject);
                }
                if (comptoir.ComptoirOccuper)
                {
                    PNJManager.instance.PNJFileAttenteComptoir.Dequeue();
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


            

            
        }

        retirer();
        valider();

        
        

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

    private void valider()
    {
        //valider la commande
        if (Input.GetKeyDown(KeyCode.R))
        {
            commande.CommandeTerminer(pileString);
        }
    }
    
    public void ResetPile()
    {
        plat.RecupererStack(pileGameObject);
        listPainDansPile.Clear();
        pileString.Clear();
        //while(pileGameObject.Count>0)
        //{
        //    GameObject temp =pileGameObject.Pop();
        //    Destroy(temp);
        //}
    }

    private IEnumerator CommandePriseCoroutine()
    {
        Debug.Log("commande prise");
        commandePrise = true;
        yield return new WaitForSeconds(1);
        commandePrise = false;
    }
}
