using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Camion : MonoBehaviour
{
    [SerializeField] private Transform pos1, pos2, pos3;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    public void RetirerPoubelle()
    {
        StartCoroutine(MouvCamion());
    }

    IEnumerator MouvCamion()
    {
        
        agent.SetDestination(pos1.position);
        
        yield return new WaitUntil(() => Vector3.Distance(transform.position, pos1.position) < 2);
        agent.SetDestination(pos2.position);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, pos2.position) < 2);
        agent.enabled = false;
        //prendre poubelle
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(5);
        agent.enabled = true;
        agent.SetDestination(pos1.position);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, pos1.position) < 2);
        agent.SetDestination(pos3.position);

    }
}
