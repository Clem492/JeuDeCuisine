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

    private Vector3 startPosition;


   

    public int indicePnj;

    private void Awake()
    {
        pnjNavMeshAgent = GetComponent<NavMeshAgent>();
        borne = GameObject.FindGameObjectsWithTag("borne");
        comptoirPosition = GameObject.FindGameObjectsWithTag("Comptoir");
        chaisePosition = GameObject.FindGameObjectsWithTag("chaise");
        cuisine = GameObject.FindGameObjectWithTag("Player").GetComponent<Cuisine>();
        bin = GameObject.FindWithTag("bin");
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



        yield return new WaitUntil(() => !comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper || !comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper && (indicePnj == PNJManager.instance.PNJFileAttenteComptoir.Peek() || indicePnj == PNJManager.instance.PNJFileAttenteComptoir.ElementAt(1)));

        if (!comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper)
        {
            PNJManager.instance.PNJFileAttenteComptoir.Dequeue();
            comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            if (borneOccuper0) borne[0].GetComponent<borne>().borneOccuper = false;
            else if (borneOccuper1) borne[1].GetComponent<borne>().borneOccuper = false;
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);

        }
        else if (!comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper)
        {
            PNJManager.instance.PNJFileAttenteComptoir.Dequeue();
            if (borneOccuper0) borne[0].GetComponent<borne>().borneOccuper = false;
            else if (borneOccuper1) borne[1].GetComponent<borne>().borneOccuper = false;
            comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            pnjNavMeshAgent.SetDestination(comptoirPosition[1].transform.position);
            yield return new WaitUntil(() => !comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper);
            comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper = false;
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);
        }
        /*else if (comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper && comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper && indicePnj == PNJManager.instance.PNJFileAttenteComptoir.Peek())
        {

            yield return new WaitUntil(() => !comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper);
            if (borneOccuper0) borne[0].GetComponent<borne>().borneOccuper = false;
            else if (borneOccuper1) borne[1].GetComponent<borne>().borneOccuper = false;
            comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            pnjNavMeshAgent.SetDestination(comptoirPosition[1].transform.position);
            yield return new WaitUntil(() => !comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper);
            comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper = false;
            comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            pnjNavMeshAgent.SetDestination(comptoirPosition[1].transform.position);
        }*/


        //attend que le joueur prend la commande du pnj
        yield return new WaitUntil(() => cuisine.commandePrise && Vector3.Distance(gameObject.transform.position, comptoirPosition[0].transform.position) <= 1);
        comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = false;

        for (int i = 0; i < chaisePosition.Length; i++)
        {
            try
            {
                if (chaisePosition[i] == null)
                {
                    Debug.Log("pas de chaise");
                }
            }
            catch
            {
                Debug.Log("djfk");
            }
            if (!chaisePosition[i].GetComponent<chaisePosition>().chaiseOccuper)
            {
                comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = false;

                Debug.Log("les chaise ne sont pas occuper je vais m'assoir");
                pnjNavMeshAgent.SetDestination(chaisePosition[i].transform.position);
                yield return new WaitUntil(() => Vector3.Distance(transform.position, pnjNavMeshAgent.destination) <= 1);
                pnjNavMeshAgent.enabled = false;
                GameObject go = chaisePosition[i].transform.GetChild(0).gameObject;
                transform.SetParent(go.transform, true);
                
                yield return new WaitForEndOfFrame();
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0,0,0);
                //attendre pour manger
                //faut décrémenter le burger
                yield return new WaitForSeconds(TimeToEat);
                transform.SetParent(null);
                transform.position = chaisePosition[i].transform.position;
                pnjNavMeshAgent.enabled = true;

                break;
            }
        }
       

        

        pnjNavMeshAgent.SetDestination(bin.transform.position);
        yield return new WaitForSeconds(TimeDropTrash);
        pnjNavMeshAgent.SetDestination(startPosition);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, startPosition) <=1);
        gameObject.SetActive(false);
    }
    


}
