
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimBouton : MonoBehaviour
{
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnMouseEnter()
    {
        
        rectTransform.localScale = new Vector3(1.25f, 1.25f, 1.25f);                
    }
    public void OnMouseExit()
    {
        rectTransform.localScale = new Vector3( 1f, 1f, 1f);
    }
    
}
