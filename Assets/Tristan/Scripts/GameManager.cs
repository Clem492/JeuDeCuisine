using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] TextMeshProUGUI argentText;

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
    private void Update()
    {
        argentText.text = argent.ToString()+" $";
    }
}
