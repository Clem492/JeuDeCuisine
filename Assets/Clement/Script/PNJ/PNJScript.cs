using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PNJScript : MonoBehaviour
{
    [SerializeField] private int TimeToEat;
    [SerializeField] private int TimeChooseCommand;
    [SerializeField] private int TimeDropTrash;
    

    private NavMeshAgent pnjNavMeshAgent;

    private GameObject[] borne;
    private GameObject[] comptoirPosition;
    public GameObject[] chaisePosition;
    private GameObject bin;
    private Cuisine cuisine;
    private Commande commande;
    private Vector3 startPosition;
    public bool commandeRecu = false;

   

    public int indicePnj;

    private void Awake()
    {
        pnjNavMeshAgent = GetComponent<NavMeshAgent>();
        borne = GameObject.FindGameObjectsWithTag("borne");
        comptoirPosition = GameObject.FindGameObjectsWithTag("Comptoir");
        chaisePosition = GameObject.FindGameObjectsWithTag("chaise");
        cuisine = GameObject.FindGameObjectWithTag("Player").GetComponent<Cuisine>();
        bin = GameObject.FindWithTag("bin");
        commande = GameObject.FindWithTag("commande").GetComponent<Commande>();
        startPosition = transform.position;

        

    }

    void Start()
    {

        StartCoroutine(Commander());
    }




    //Une fois que le pnj est activer il va a la borne
    //ensuite il va a la caisse le joueur prend la commande
    //il va s'assoir et attend ça commande et mange
    //va deposer son plateau et resort 



    private IEnumerator Commander()
    {
        
        bool borneOccuper0 = false;
        bool borneOccuper1 = false;
        yield return new WaitUntil(() => !borne[0].GetComponent<borne>().borneOccuper || !borne[1].GetComponent<borne>().borneOccuper && indicePnj == PNJManager.instance.PNJFileAttenteBorne.Peek());
        
        if (!borne[0].GetComponent<borne>().borneOccuper)
       {
            PNJManager.instance.PNJFileAttenteBorne.Dequeue();
            borne[0].GetComponent<borne>().borneOccuper = true;
            borneOccuper0 = true;
            pnjNavMeshAgent.SetDestination(borne[0].transform.position);
       }
       else if (!borne[1].GetComponent<borne>().borneOccuper)
       {
            PNJManager.instance.PNJFileAttenteBorne.Dequeue();
            borne[1].GetComponent<borne>().borneOccuper = true;
            borneOccuper1 = true;
            pnjNavMeshAgent.SetDestination(borne[1].transform.position);
       }
       else if (borne[0].GetComponent<borne>().borneOccuper && borne[1].GetComponent<borne>().borneOccuper)
       {
            pnjNavMeshAgent.SetDestination(transform.position);
            
            yield return new WaitUntil(() => !borne[0].GetComponent<borne>().borneOccuper || !borne[1].GetComponent<borne>().borneOccuper && indicePnj == PNJManager.instance.PNJFileAttenteBorne.Peek());
            if (!borne[0].GetComponent<borne>().borneOccuper) 
            {
                Debug.LogWarning(PNJManager.instance.PNJFileAttenteBorne.Count);
                PNJManager.instance.PNJFileAttenteBorne.Dequeue();
                borne[0].GetComponent<borne>().borneOccuper = true;
                borneOccuper0 = true;
                pnjNavMeshAgent.SetDestination(borne[0].transform.position);
            }
            
            else if (!borne[1].GetComponent<borne>().borneOccuper)
            {
                PNJManager.instance.PNJFileAttenteBorne.Dequeue();
                PNJManager.instance.PNJFileAttenteBorne.Dequeue();
                borne[1].GetComponent<borne>().borneOccuper = true;
                borneOccuper1 = true;
                pnjNavMeshAgent.SetDestination(borne[1].transform.position);
            }
       }

        yield return new WaitUntil(() => Vector3.Distance(transform.position, pnjNavMeshAgent.destination) < 1);
        //attend 5 second avant d'aller au comptoir 
        yield return new WaitForSeconds(TimeChooseCommand);
        PNJManager.instance.PNJFileAttenteComptoir.Enqueue(indicePnj);


        yield return new WaitUntil(() => !comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper && indicePnj == PNJManager.instance.PNJFileAttenteComptoir.Peek());
        if (!comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper)
        {
            comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            if (borneOccuper0) borne[0].GetComponent<borne>().borneOccuper = false;
            else if (borneOccuper1) borne[1].GetComponent<borne>().borneOccuper = false;
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);

        }
        


        //attend que le joueur prend la commande du pnj
        yield return new WaitUntil(() => cuisine.commandePrise && Vector3.Distance(gameObject.transform.position, comptoirPosition[0].transform.position) <= 1 && indicePnj == PNJManager.instance.PNJFileAttenteComptoir.Peek());
        commande.AjoutCommande(gameObject);
        comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = false;
        PNJManager.instance.PNJFileAttenteComptoir.Dequeue();
        for (int i = 0; i < chaisePosition.Length; i++)
        {
            int random = Random.Range(0, chaisePosition.Length);
            try
            {
                if (chaisePosition[random] == null)
                {
                }
            }
            catch
            {
            }
            
            if (!chaisePosition[random].GetComponent<chaisePosition>().chaiseOccuper)
            {
                chaisePosition[random].GetComponent<chaisePosition>().chaiseOccuper = true;
                comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = false;

                pnjNavMeshAgent.SetDestination(chaisePosition[random].transform.position);
                yield return new WaitUntil(() => Vector3.Distance(transform.position, pnjNavMeshAgent.destination) <= 1);
                pnjNavMeshAgent.enabled = false;
                GameObject go = chaisePosition[random].transform.GetChild(0).gameObject;
                transform.SetParent(go.transform, true);
                
                yield return new WaitForEndOfFrame();
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0,0,0);
                yield return new WaitUntil(() => commandeRecu );
                //attendre pour manger
                //faut décrémenter le burger
                yield return new WaitForSeconds(TimeToEat);
                transform.SetParent(null);
                transform.position = chaisePosition[random].transform.position;
                pnjNavMeshAgent.enabled = true;
                chaisePosition[random].GetComponent<chaisePosition>().chaiseOccuper = false;
                break;
            }
        }
        PNJManager.instance.PNJFileAttentePoubelle.Enqueue(indicePnj);

        
        yield return new WaitUntil(() => !bin.GetComponent<binPosition>().binOccuper && indicePnj == PNJManager.instance.PNJFileAttentePoubelle.Peek());
        bin.GetComponent<binPosition>().binOccuper = true;
        //=====================
        //      Poubelle
        //=====================
        pnjNavMeshAgent.SetDestination(bin.transform.position);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, pnjNavMeshAgent.destination) <= 1);
        yield return new WaitForSeconds(TimeDropTrash);
        Poubelle.instance.AddGarbage();
        bin.GetComponent<binPosition>().binOccuper = false;
        PNJManager.instance.PNJFileAttentePoubelle.Dequeue();
        pnjNavMeshAgent.SetDestination(startPosition);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, startPosition) <=1);
        PNJManager.instance.totalPnjSpawn--;
        Destroy(gameObject);
    }
    


}
