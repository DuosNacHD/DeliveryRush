using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DeliveryPanel : MonoBehaviour
{
    int Stage;
    int Goal = 25;

    public TMP_Text Text;
    public Slider slider;
    
    void Start()
    {
        slider = transform.Find("SliderAndFlourIcon").Find("Progress Bar Green").GetComponent<Slider>();
        Text = transform.Find("Goal").Find("InnerPanel").Find("2K").GetComponent<TMP_Text>();
        Text.text = Goal.ToString();
    }
}
