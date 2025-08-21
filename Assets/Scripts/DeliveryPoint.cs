using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    public bool Active;
    public bool isOut;
    private void Start()
    {
        if( gameObject.transform.parent.name == "OutS")
        {
            isOut = true;
        }
        else if(gameObject.transform.parent.name == "InS")
        {
            isOut = false;
        }

    }

}
