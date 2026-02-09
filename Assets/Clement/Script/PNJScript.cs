using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJScript : MonoBehaviour
{
   
    private NavMeshAgent pnjNavMeshAgent;

    private GameObject[] borne;
    private GameObject[] ComptoirPosition;
    private Cuisine cuisine;

    private void Awake()
    {
        pnjNavMeshAgent = GetComponent<NavMeshAgent>();
        borne = GameObject.FindGameObjectsWithTag("borne");
        ComptoirPosition = GameObject.FindGameObjectsWithTag("Comptoir");
        
    }

    void Start()
    {
        
    }




    //Une fois que le pnj est activer il va a la borne
    //ensuite il va a la caisse le joueur prend la commande
    //il va s'assoir et attend ça commande et mange
    //va deposer son plateau et resort 
    private IEnumerator Commander(NavMeshAgent pnjNavMeshAgent)
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
        yield return new WaitForSeconds(5);
        PNJManager.instance.PNJFileAttenteComptoir.Enqueue(gameObject);

        if (!ComptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper)
        {
            pnjNavMeshAgent.SetDestination(ComptoirPosition[0].transform.position);
        }
        else
        {
            pnjNavMeshAgent.SetDestination(ComptoirPosition[1].transform.position);
            yield return new WaitUntil(() => !ComptoirPosition[0].GetComponent<ComptoirPosition>().ComptoirOccuper);
            pnjNavMeshAgent.SetDestination(ComptoirPosition[0].transform.position);
        }

        //attend que le joueur prend la commande du pnj
        yield return new WaitUntil(() => cuisine.commandePrise && PNJManager.instance.PNJFileAttenteComptoir.Peek() == gameObject);
        PNJManager.instance.PNJFileAttenteComptoir.Dequeue();


    }


}
