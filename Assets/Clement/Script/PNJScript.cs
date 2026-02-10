using System.Collections;
using System.Collections.Generic;
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

        StartCoroutine(Attente());
    }




    //Une fois que le pnj est activer il va a la borne
    //ensuite il va a la caisse le joueur prend la commande
    //il va s'assoir et attend ça commande et mange
    //va deposer son plateau et resort 

    private IEnumerator Attente()
    {
        yield return new WaitForSeconds(indicePnj);
        StartCoroutine(Commander());
    }

    private IEnumerator Commander()
    {

        for (int i = 0; i < indicePnj *100; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        
        if (!borne[0].GetComponent<borne>().borneOccuper)
        {
            borne[0].GetComponent<borne>().borneOccuper = true;
            pnjNavMeshAgent.SetDestination(borne[0].transform.position);
            yield break;
        }
        else if (!borne[1].GetComponent<borne>().borneOccuper)
        {
            borne[1].GetComponent<borne>().borneOccuper = true;
            pnjNavMeshAgent.SetDestination(borne[1].transform.position);
            yield break;
        }
        else if (borne[0].GetComponent<borne>().borneOccuper && borne[1].GetComponent<borne>().borneOccuper)
        {
            pnjNavMeshAgent.SetDestination(gameObject.transform.position);
            yield return new WaitUntil(() => !borne[0].GetComponent<borne>().borneOccuper || !borne[1].GetComponent<borne>().borneOccuper);
            if (!borne[0].GetComponent<borne>().borneOccuper)
            {
                borne[0].GetComponent<borne>().borneOccuper = true;
                pnjNavMeshAgent.SetDestination(borne[0].transform.position);
            }
            if (!borne[1].GetComponent<borne>().borneOccuper)
            {
                borne[1].GetComponent<borne>().borneOccuper = true;
                pnjNavMeshAgent.SetDestination(borne[1].transform.position);
            }

        }

        yield return new WaitUntil(() => Vector3.Distance(transform.position, pnjNavMeshAgent.destination) < 1);
        //attend 5 second avant d'aller au comptoir 
        yield return new WaitForSeconds(TimeChooseCommand);
        

        if (!comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper)
        {
            comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);

        }
        else
        {
            comptoirPosition[1].GetComponent<ComptoirPosition>().ComptoirOccuper = true;
            pnjNavMeshAgent.SetDestination(comptoirPosition[1].transform.position);
            yield return new WaitUntil(() => !comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper);
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);
        }

        //attend que le joueur prend la commande du pnj
        yield return new WaitUntil(() => cuisine.commandePrise && Vector3.Distance(gameObject.transform.position, comptoirPosition[0].transform.position) <=1);
        
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
        Destroy(gameObject);
    }


}
