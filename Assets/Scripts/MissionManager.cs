using Unity.VisualScripting;
using UnityEngine;

public class MissionManager : MonoBehaviour
{

    
    [SerializeField] GameObject[] InPs, OutPs;
    public GameObject InP()
    {
        return RandomSelect(InPs);
    }
    public GameObject OutP()
    {
        return RandomSelect(OutPs);
    }
    
    GameObject RandomSelect(GameObject[] lis)
    {
        return lis[Random.Range(0,lis.Length-1)];
    }


    
}