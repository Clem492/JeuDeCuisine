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
    private GameObject[] chaisePosition;
    private GameObject bin;
    private Cuisine cuisine;

    private Vector3 startPosition;


    private void Awake()
    {
        pnjNavMeshAgent = GetComponent<NavMeshAgent>();
        borne = GameObject.FindGameObjectsWithTag("borne");
        comptoirPosition = GameObject.FindGameObjectsWithTag("Comptoir");
        chaisePosition = GameObject.FindGameObjectsWithTag("chaise");
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
        for (int i = 0; i < borne.Length; i++)
        {
            if (!borne[i].GetComponent<borne>().borneOccuper)
            {
                pnjNavMeshAgent.SetDestination(borne[i].transform.position);
            }
            else
            {
                yield return new WaitUntil(() => !borne[i].GetComponent<borne>().borneOccuper);
                pnjNavMeshAgent.SetDestination(borne[i].transform.position);
            }

        }
        //attend 5 second avant d'aller au comptoir 
        yield return new WaitForSeconds(TimeChooseCommand);
        

        if (!comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper)
        {
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);
        }
        else
        {
            pnjNavMeshAgent.SetDestination(comptoirPosition[1].transform.position);
            yield return new WaitUntil(() => !comptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper);
            pnjNavMeshAgent.SetDestination(comptoirPosition[0].transform.position);
        }

        //attend que le joueur prend la commande du pnj
        yield return new WaitUntil(() => cuisine.commandePrise && gameObject.transform.position == comptoirPosition[0].transform.position);
        
        for (int i = 0; i < chaisePosition.Length; i++)
        {
            if (!chaisePosition[i].GetComponent<chaisePosition>().chaiseOccuper) pnjNavMeshAgent.SetDestination(chaisePosition[i].transform.position);
        }

        //attendre pour manger
        yield return new WaitForSeconds(TimeToEat);
        //faut décrémenter le burger

        pnjNavMeshAgent.SetDestination(bin.transform.position);
        yield return new WaitForSeconds(TimeDropTrash);
        pnjNavMeshAgent.SetDestination(startPosition);
        yield return new WaitUntil(() => transform.position == startPosition);
        Destroy(gameObject);
    }


}
