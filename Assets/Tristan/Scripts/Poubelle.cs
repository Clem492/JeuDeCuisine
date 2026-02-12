using System.Collections;
using TMPro;
using UnityEngine;

public class Poubelle : MonoBehaviour
{
    public static Poubelle instance;
    [SerializeField] private TextMeshProUGUI textPourcentage;
    public int garbagePourcentage;
    private bool antiSpamCoroutine;

    private int looseMoneyWithTime = 1; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

            antiSpamCoroutine = true;
        garbagePourcentage = 0;
        textPourcentage.text = garbagePourcentage + "% ";
        ChangeColor();
    }

    public void AddGarbage()
    {
        textPourcentage.text = garbagePourcentage + "% ";
        garbagePourcentage += 5;
        ChangeColor();
        if (garbagePourcentage >= 100 && antiSpamCoroutine)
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
        ChangeColor();
    }

    private void ChangeColor()
    {
        if(garbagePourcentage <= 24)
        {
            textPourcentage.color = Color.gray;
        }
        else if (garbagePourcentage <= 49)
        {
            textPourcentage.color = Color.green;
        }
        else if (garbagePourcentage <= 74)
        {
            textPourcentage.color = Color.yellow;
        }
        else if (garbagePourcentage <= 99)
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
       
        if (garbagePourcentage >= 100 && antiSpamCoroutine)
        {
            antiSpamCoroutine = false;
            StartCoroutine(LooseMoneyGarbage());
        }
    }

    public void resetPoubelle()
    {
        garbagePourcentage = 0;
        textPourcentage.text = garbagePourcentage + "% / 100%";
        ChangeColor();
    }
}
