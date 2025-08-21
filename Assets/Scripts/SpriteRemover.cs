using UnityEngine;

public class SpriteRemover : MonoBehaviour
{
 
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

 
}
