using System.Collections;
using TMPro;
using UnityEngine;

public class Poubelle : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textPourcentage;
    public int garbagePourcentage;
    private bool antiSpamCoroutine;

    private int looseMoneyWithTime = 1; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        antiSpamCoroutine = true;
        garbagePourcentage = 0;
        textPourcentage.text = garbagePourcentage + "% ";
    }

    public void AddGarbage()
    {
        textPourcentage.text = garbagePourcentage + "% ";
        garbagePourcentage += 5;
        if(garbagePourcentage >= 100 && antiSpamCoroutine)
        {
            antiSpamCoroutine = false;
            StartCoroutine(LooseMoneyGarbage());
        }
    }
    IEnumerator LooseMoneyGarbage()
    {
        while(garbagePourcentage >= 100)
        {
            GameManager.instance.argent -= looseMoneyWithTime;
            looseMoneyWithTime += 1;
            yield return new WaitForSeconds(5);
        }
        textPourcentage.text = garbagePourcentage + "% / 100%";
        looseMoneyWithTime = 1;
        antiSpamCoroutine = true;
    }

    private void ChangeColor()
    {
        if(garbagePourcentage <= 25)
        {
            textPourcentage.color = Color.gray;
        }
        else if (garbagePourcentage <= 50)
        {
            textPourcentage.color = Color.green;
        }
        else if (garbagePourcentage <= 75)
        {
            textPourcentage.color = Color.yellow;
        }
        else if (garbagePourcentage <= 100)
        {
            textPourcentage.color = Color.red;
        }
        else if (garbagePourcentage >= 100)
        {
            textPourcentage.color = Color.magenta;
        }

    }
    private void Update()
    {
        Input.GetKeyDown(KeyCode.Space);
    }
}
