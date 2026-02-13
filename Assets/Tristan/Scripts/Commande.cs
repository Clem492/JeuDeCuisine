using NootColis.Logic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Commande : MonoBehaviour
{
    //file
    Queue<string> fileCommande;
    public Queue<GameObject> fileClients = new Queue<GameObject>();
    //public int argent = 0;
    string[] tabCommande;
    //dictionaire
    Dictionary<string, Stack<string>> dicoCommandePossible;
    Dictionary<string, Texture> dicoImageCommande;

    [SerializeField] Texture imageSimple, imageChampi, imageMax;
    [SerializeField] RawImage commande1, commande2, commande3;

   

    private List<Stack<string>> stackRecette;


    [SerializeField] Cuisine cuisine;
    //food
    private string Pain = "Pain";
    private string Viande = "Viande";
    private string Champignon = "Champignon";
    private string Salade = "Salade";

    private bool waitCommande =false;

    [SerializeField] RawImage cocheVerte, croixRouge;

    [SerializeField] ClientAgent[] tabClient;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NootColisAPI.GetStreamOfColis("Resto");

        croixRouge.enabled = false;
        cocheVerte.enabled = false;

        //structure donnée
        dicoImageCommande = new Dictionary<string, Texture>();
        dicoCommandePossible = new Dictionary<string, Stack<string>>();
        fileCommande = new Queue<string>();
        stackRecette = new List<Stack<string>>();

        //pile de référence du 1er burger
        Stack<string> burgerChampi = new Stack<string>();
        burgerChampi.Push(Pain);
        burgerChampi.Push(Viande);
        burgerChampi.Push(Champignon);
        burgerChampi.Push(Pain);

        //pile de référence du 2eme burger
        Stack<string> burgerSimple = new Stack<string>();
        burgerSimple.Push(Pain);
        burgerSimple.Push(Viande);
        burgerSimple.Push(Salade);
        burgerSimple.Push(Pain);

        //pile de référence du 3eme burger
        Stack<string> burgerMax = new Stack<string>();
        burgerMax.Push(Pain);
        burgerMax.Push(Viande);
        burgerMax.Push(Salade);
        burgerMax.Push(Viande);
        burgerMax.Push(Pain);

        //liste de toutes les piles 
        stackRecette.Add(burgerChampi);
        stackRecette.Add(burgerSimple);
        stackRecette.Add(burgerMax);

        //Ajout des commandes possibles
        dicoCommandePossible.Add("BurgerChampi", stackRecette[0]);
        dicoCommandePossible.Add("BurgerSimple", stackRecette[1] );
        dicoCommandePossible.Add("BurgerMax", stackRecette[2]);

        

        //tableau avec les plats possibles
        tabCommande = new string[dicoCommandePossible.Count];
        int i = 0;
        foreach (string key in dicoCommandePossible.Keys)
        {
            tabCommande[i] = key;
            i++;
        }

        //dictionnaire d'image 
        dicoImageCommande.Add("BurgerChampi", imageChampi);
        dicoImageCommande.Add("BurgerSimple",imageSimple);
        dicoImageCommande.Add("BurgerMax", imageMax);

    }

    // Update is called once per frame
    void Update()
    {
        //limite le nombre de commande
       
        ChangeImage();

        
        
        
        
    }

    private void ChangeImage()
    {
        //image des commandes
        if (fileCommande.Count == 0)
        {
            commande1.texture = null;
        } 
        if (fileCommande.Count > 0)
        {
            commande1.texture = dicoImageCommande[fileCommande.Peek()];
        }
        if (fileCommande.Count > 1)
        {
            commande2.texture = dicoImageCommande[fileCommande.ElementAt(1)];
        }
        if (fileCommande.Count > 2)
        {
            commande3.texture = dicoImageCommande[fileCommande.ElementAt(2)];
        }
        if (fileCommande.Count == 1)
        {
            commande2.texture = null;
        }
        if (fileCommande.Count == 2)
        {
            commande3.texture = null;
        }
    }

    //wait avant de refaire une commande
    IEnumerator WaitAjoutCommande()
    {
        yield return new WaitForSeconds(5);
        waitCommande = false;
    }

    public void AjoutCommande(GameObject client)
    {
        fileClients.Enqueue(client);
        //chiffre aléatoire
        int rand = Random.Range(0, dicoCommandePossible.Count);

        //ajoute a la file la comamnde;
        fileCommande.Enqueue(tabCommande[rand]);
        StartCoroutine(WaitAjoutCommande());
        

    }

    public void CommandeTerminer(Stack<string> platPreparer)
    {

        if(platPreparer.Count > 0)
        {
            fileClients.Peek().GetComponent<PNJScript>().commandeRecu = true;
            int i;

            //vérifie que c'est exactement la meme pile
            for (i = 0; i < dicoCommandePossible[fileCommande.Peek()].Count; i++)
            {
                if (platPreparer.ElementAt(i) != dicoCommandePossible[fileCommande.Peek()].ElementAt(i))
                {
                    break;
                }
            }
            //si bonne commande
            if (i == dicoCommandePossible[fileCommande.Peek()].Count)
            {
                Debug.Log("bonne commande");
                GameManager.instance.argent += 25;
                EnvoieArgent(25);
                fileCommande.Dequeue();
                fileClients.Dequeue();
                

                //reset de la pile
                cuisine.ResetPile();
                StartCoroutine(AfficheIconeReussite());

                for (int y = 0; y < tabClient.Length; y++)
                {
                    tabClient[y].Mouv();
                }
            }
            //sinon
            else
            {
                GameManager.instance.argent -= 25;
                
                fileCommande.Dequeue();

                //reset de la pile
                cuisine.ResetPile();
                fileClients.Dequeue();
                StartCoroutine(AfficheIconeEchec());
                for (int y = 0; y < tabClient.Length; y++)
                {
                    tabClient[y].Mouv();
                }
            }
        }
        

        

        
    }
    //coroutine qui affiche un certain tempp coche verte et croix rouge
    IEnumerator AfficheIconeReussite()
    {
        cocheVerte.enabled = true;
        yield return new WaitForSeconds(3);
        cocheVerte.enabled = false;
    }
    //coroutine qui affiche un certain tempp croix rouge et croix rouge
    IEnumerator AfficheIconeEchec()
    {
        croixRouge.enabled = true;
        yield return new WaitForSeconds(3);
        croixRouge.enabled = false;
    }


    async void EnvoieArgent(int argent)
    {
        await NootColisAPI.SendColis("Resto", "Nicolas", argent.ToString());
    }
}
