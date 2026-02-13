using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJManager : MonoBehaviour
{
    public static PNJManager instance;

    

    [SerializeField] Transform PNJSpawnPoint1;
    [SerializeField] Transform PNJSpawnPoint2;

    [SerializeField] private GameObject[] tabPnj;
    private GameObject[] tabChaise;
    public int totalPnjSpawn;
    

    public Queue<int> PNJFileAttenteBorne = new Queue<int>();
    public Queue<int> PNJFileAttenteComptoir = new Queue<int>();
    public Queue<int > PNJFileAttentePoubelle = new Queue<int>();


    //système de pool pour les pnj 

    public int spawningTime = 2;


    private void Awake()
    {
        tabChaise = GameObject.FindGameObjectsWithTag("chaise");
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
        try
        {
            Debug.Log(PNJFileAttenteComptoir.Peek());
        }
        catch
        {
        }
        
    }

    private IEnumerator SpawnPNJ()
    {
        while (true)
        {
            if (totalPnjSpawn <= tabChaise.Length)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    int random = Random.Range(0, tabPnj.Length);
                    GameObject pnj = Instantiate(tabPnj[random], PNJSpawnPoint1.transform.position, Quaternion.identity);
                    PNJFileAttenteBorne.Enqueue(pnj.GetComponent<PNJScript>().indicePnj);
                    totalPnjSpawn++;
                    yield return new WaitForSeconds(spawningTime);
                }
                else
                {
                    int random = Random.Range(0, tabPnj.Length);
                    GameObject pnj = Instantiate(tabPnj[random], PNJSpawnPoint2.transform.position, Quaternion.identity);
                    PNJFileAttenteBorne.Enqueue(pnj.GetComponent<PNJScript>().indicePnj);
                    totalPnjSpawn++;
                    yield return new WaitForSeconds(spawningTime);
                }
               
            }
            yield return new WaitForEndOfFrame();

        }
        
    }
}
