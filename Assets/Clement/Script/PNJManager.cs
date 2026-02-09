using System.Collections;
using UnityEngine;

public class PNJManager : MonoBehaviour
{
    [SerializeField] Transform PNJSpawnPoint;

    [SerializeField] private GameObject[] pnj;
    
    //système de pool pour les pnj 
    public int poolSize;
    private GameObject[] pnjPool;
    private int poolIndex;

    private void Start()
    {
        pnjPool = new GameObject[poolSize];
        for (int i = 0; i< poolSize; i++)
        {
            pnjPool[i] = Instantiate(pnj[i]);
            pnjPool[i].SetActive(false);
        }

        StartCoroutine(SpawnPNJ());
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
                    poolIndex = (index + 1) % poolSize;
                    break; //sortir après avoir trouvé un missile 
                }
            }
        }
        
    }

}
