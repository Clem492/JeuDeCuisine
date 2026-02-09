using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public int argent;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        argent = 0;
    }
   
}
