using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject boutonContinuer, boutonMenu;

    private void Start()
    {
        boutonContinuer.SetActive(false);
        boutonMenu.SetActive(false);
    }
    private void Update()
    {
        AfficherPause();
    }
    public void AfficherPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.isPause)
            {
                boutonContinuer.SetActive(false);
                boutonMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                GameManager.instance.isPause = false;
            }
            else
            {
                boutonContinuer.SetActive(true);
                boutonMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                GameManager.instance.isPause = true;
            }
        } 
    }

    public void Continue()
    {
        boutonContinuer.SetActive(false);
        boutonMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.instance.isPause = false;
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
