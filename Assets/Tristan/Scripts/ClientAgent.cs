using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class ClientAgent : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] GameObject pos1, pos2, pos3, posDepart;
    public int numeroPos;
    int temp_attente = 0;

    bool sortie = false;
    
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        //temp d'attene au depart pour chaque clients
           agent = GetComponent<NavMeshAgent>();
        if (numeroPos == 1)
            temp_attente = 0;
        if (numeroPos == 2)
            temp_attente = 5;
        if (numeroPos == 3)
            temp_attente = 10;
        StartCoroutine(Depart());
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped)
        {
            animator.ResetTrigger("marche");
        }
        else
        {
            animator.SetTrigger("marche");
        }
    }

    //change de position quand une commande terminé
    public void Mouv()
    {
        //if (sortie)return;

        if (numeroPos == 1)
        {
            StartCoroutine(Sortie());
            
        }
        else if (numeroPos == 2)
        {
            agent.SetDestination(pos1.transform.position);
            numeroPos = 1;
        }
        else if (numeroPos == 3)
        {
            agent.SetDestination(pos2.transform.position);
            numeroPos = 2;
        }
    }

    IEnumerator Depart()
    {
        yield return new WaitForSeconds(temp_attente);
        if (numeroPos == 1)
            agent.SetDestination(pos1.transform.position);
        else if (numeroPos == 2)
            agent.SetDestination(pos2.transform.position);
        else if (numeroPos == 3)
            agent.SetDestination(pos3.transform.position);
    }

    IEnumerator Sortie()
    {
        sortie = true;
        agent.SetDestination(posDepart.transform.position);
        yield return new WaitForSeconds(5);
        agent.SetDestination(pos3.transform.position);
        numeroPos = 3;
        sortie = false;
    }
    
}
