using UnityEngine;

public class Deplacementcam : MonoBehaviour
{
    public float zMin, zMax;
    bool Moove = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y,zMin);
    }

    // Update is called once per frame
    
    private void FixedUpdate()
    {
        ChangeDirect();
        Debug.Log(Moove);
        //deplacer la cam en avant et en arrière
        if (!Moove)
        {
            transform.Translate(new Vector3(0, 0, 0.1f));
        }
        else
        {
            transform.Translate(new Vector3(0, 0, -0.1f));
        }

        
    }
    private void ChangeDirect()
    {
        if (transform.position.z > zMax)
        {
            Moove = true;
        }
        else if (transform.position.z < zMin)
        {

            Moove = false;
        }
    }
}
