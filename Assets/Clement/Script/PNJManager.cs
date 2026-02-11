using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJManager : MonoBehaviour
{
    public static PNJManager instance;

    

    [SerializeField] Transform PNJSpawnPoint;

    [SerializeField] private GameObject[] pnj;

    public int indicePnj;

    public Queue<int> PNJFileAttenteBorne = new Queue<int>();
    public Queue<int> PNJFileAttenteComptoir = new Queue<int>();
    

    //système de pool pour les pnj 
    public int poolSize;
    private GameObject[] pnjPool;
    private int poolIndex;
    private GameObject pnjPrefab;


    private void Awake()
    {
        pnjPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pnjPool[i] = Instantiate(pnj[i]);
            pnjPool[i].SetActive(false);

        }
    }

    private void Start()
    {
        if (instance  == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(SpawnPNJ());
        
    }

    private void Update()
    {
        Debug.Log(PNJFileAttenteBorne.Count);
    }

    private IEnumerator SpawnPNJ()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            for (int i = 0; i < poolSize; i++)
            {
                int index = (poolIndex + i) % poolSize;

                if (!pnjPool[index].activeSelf)
                {
                    
                    pnjPool[index].transform.position = PNJSpawnPoint.position;
                    pnjPool[index].transform.rotation = Quaternion.identity;
                    pnjPool[index].SetActive(true);
                    if (pnjPool[index].GetComponent<PNJScript>() == null)
                    {
                        Debug.LogWarning("impossible d'avoir accès au PNJScript");
                    }else
                    {
                        PNJFileAttenteBorne.Enqueue(pnjPool[index].GetComponent<PNJScript>().indicePnj);
                        PNJFileAttenteComptoir.Enqueue(pnjPool[index].GetComponent<PNJScript>().indicePnj);
                    }

                    poolIndex = (index + 1) % poolSize;
                    break; //sortir après avoir trouvé un missile 
                }
            }
        }
        
    }

   

}
