using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject boutonContinuer, boutonMenu;

    private bool isPause;

    private void Start()
    {
        
        isPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        boutonContinuer.SetActive(false);
        boutonMenu.SetActive(false);
        
    }
    private void Update()
    {
        AfficherPause();
    }
    public void AfficherPause()
    {
        Debug.Log(isPause);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                
                boutonContinuer.SetActive(false);
                boutonMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                isPause = false;
            }
            else if(!isPause)
            {
                
                boutonContinuer.SetActive(true);
                boutonMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                isPause = true;
            }
        } 
    }

    public void Continue()
    {
        
        boutonContinuer.SetActive(false);
        boutonMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isPause = false;
    }
    public void Menu()
    {
        Cursor.lockState = CursorLockMode.None;
        isPause = false;
        SceneManager.LoadScene("Menu");
    }
}
