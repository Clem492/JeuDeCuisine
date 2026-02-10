using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetourMenu : MonoBehaviour
{
    private string Menu = "Menu";
    [SerializeField] float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Retour());
    }

    IEnumerator Retour()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(Menu);
    }
    
}
