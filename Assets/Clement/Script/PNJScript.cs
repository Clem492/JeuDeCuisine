using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PNJScript : MonoBehaviour
{
    private GameObject[] borne;
    private GameObject[] ComptoirPosition;
    private NavMeshAgent pnjNavMeshAgent;

    private void Awake()
    {
        borne = GameObject.FindGameObjectsWithTag("borne");
        ComptoirPosition = GameObject.FindGameObjectsWithTag("Comptoir");
        pnjNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
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

        yield return new WaitForSeconds(5);

        for (int i = 0; i < ComptoirPosition.Length; i++)
        {
            if (!ComptoirPosition[i].GetComponent<ComptoirPosition>().ComptoirOccuper)
            {
                pnjNavMeshAgent.SetDestination(ComptoirPosition[i].transform.position);
            }
            else
            {
                yield return new WaitUntil(() => (!ComptoirPosition[i].GetComponent<ComptoirPosition>().ComptoirOccuper));
                pnjNavMeshAgent.SetDestination(ComptoirPosition[i].transform.position);
            }

        }



    }

}
