using UnityEngine;

public class audioManager : MonoBehaviour
{
    public static audioManager Instance;
    [SerializeField] private AudioSource musique;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
