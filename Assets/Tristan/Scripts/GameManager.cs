using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        verifVictoireDefaite();
    }

    public void verifVictoireDefaite()
    {
        if(argent < 0)
        {
            SceneManager.LoadScene("Defaite");
        }
        if(argent >= 1000)
        {
            SceneManager.LoadScene("Victoire");
        }
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("GiveUp");
    }
}
