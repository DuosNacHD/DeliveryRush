using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour
{
    
    [SerializeField] CarData car;

    enum hill { NorthWest, NorthEast, SouthEast, SouthWest }
    List<hill> hills = new List<hill>();
    
    DeliveryPoint StayedDeliveryPoint;
    float StayedTime;
    
    int turning;
    float dTime;
    float movement;
    float direction;



    [Header("UserInterface")]
    [SerializeField]Scrollbar TargetGear;
    [SerializeField]Button Break;
    [SerializeField]Slider GearSwap;
    [SerializeField]GameObject leftB, rightB, resetB;
    [SerializeField]Sprite Forward;
    [SerializeField]TMP_Text cashText;
    [SerializeField]DeliveryPanel deliveryPanel;


    ParticleSystem Ps;
    MissionManager missionManager;
    DotPathRenderer dotPathRenderer;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    MenuController menuController;
    
    void Start()
    {
        SetReference();
        SetResulations();
        OverrideAnimations(car);

        targetController(missionManager.InP());
    }
    
    private void Update()
    {
        DeliveryControl();

        RotationAndMotion();
        
        turner();
    }

    void FixedUpdate()
    {
        CharacterMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "hillNW":
                hills.Add(hill.NorthWest);
                break;

            case "hillNE":
                hills.Add(hill.NorthEast);
                break;

            case "hillSE":
                hills.Add(hill.SouthEast);
                break;

            case "hillSW":
                hills.Add(hill.SouthWest);
                break;



            // Delivery Points

            case "DeliveryPoint":
                GameObject State = collision.gameObject;
                if (State.GetComponent<DeliveryPoint>() != null)
                {
                    if (State.GetComponent<DeliveryPoint>().Active)
                    {
                        StayedTime = Time.time;
                        StayedDeliveryPoint = State.GetComponent<DeliveryPoint>();
                    }
                }
                break;
        }
        switch (collision.name)
        {

            case "pLayer1":
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 1;
                spriteRenderer.sortingOrder = 1;
                break;

            case "pLayer2":
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 2;
                spriteRenderer.sortingOrder = 2;
                break;

            case "pLayer3":
                spriteRenderer.sortingOrder = 3;
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 3;
                break;

            case "pLayer4":
                spriteRenderer.sortingOrder = 4;
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 4;
                break;

            case "pLayer5":
                spriteRenderer.sortingOrder = 5;
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 5;
                break;

            case "pLayer6":
                spriteRenderer.sortingOrder = 6;
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 6;
                break;

            case "pLayer7":
                spriteRenderer.sortingOrder = 7;
                Ps.GetComponent<ParticleSystemRenderer>().sortingOrder = 7;
                break;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "hillNW":
                hills.Remove(hill.NorthWest);
                break;

            case "hillNE":
                hills.Remove(hill.NorthEast);
                break;

            case "hillSE":
                hills.Remove(hill.SouthEast);
                break;

            case "hillSW":
                hills.Remove(hill.SouthWest);
                break;


            case "DeliveryPoint":
                StayedDeliveryPoint = null;
                break;

        }
    }

    void SetResulations()
    {
        Screen.SetResolution(Screen.currentResolution.width / 3, Screen.currentResolution.height / 3, true);
    }
    void SetReference()
    {
        Ps = GetComponent<ParticleSystem>();
        dotPathRenderer = GetComponent<DotPathRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        missionManager = GameObject.FindGameObjectWithTag("Mission").GetComponent<MissionManager>();
        menuController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        cashText.text = PlayerPrefs.GetFloat("Cash").ToString();
        
    }
    void targetController(GameObject TPoint)
    {
        dotPathRenderer.endPoint = TPoint.transform;
        TPoint.GetComponent<DeliveryPoint>().Active = true;
    }

    void DeliveryControl()
    {
        if (StayedDeliveryPoint != null)
        {
            if (StayedTime + 1f <= Time.time)
            {
                StayedDeliveryPoint.Active = false;
                if (StayedDeliveryPoint.isOut)
                {
                    deliveryPanel.slider.value = 2;
                    PlayerPrefs.SetFloat("Cash", PlayerPrefs.GetFloat("Cash") + 25);
                    cashText.text = PlayerPrefs.GetFloat("Cash").ToString();
                    targetController(missionManager.InP());
                    menuController.OpenMissionTab();
                }
                else
                {
                    deliveryPanel.slider.value = 1;
                    targetController(missionManager.OutP());
                    menuController.OpenMissionTab();
                }
                StayedDeliveryPoint = null;
            }
        }
    }
    
    void RotationAndMotion()
    {
        if (GearSwap.value == 0)
        {
            TargetGear.numberOfSteps = (int)car.gear + 1;
            TargetGear.transform.localScale = new Vector3(1, 1, 1);
            TargetGear.transform.Find("Sliding Area").Find("Handle").GetComponent<Image>().sprite = Forward;
            if (TargetGear.value < 0)
            {
                movement = Mathf.Lerp(movement, TargetGear.value * car.gear, Time.deltaTime * 2);
            }
            else
            {
                movement = Mathf.Lerp(movement, TargetGear.value * car.gear, Time.deltaTime * 4);
            }
        }
        else if (GearSwap.value == 1) 
        {
            TargetGear.numberOfSteps = (int)car.backGear + 1;
            TargetGear.transform.localScale = new Vector3(1, -1, 1);
            TargetGear.transform.Find("Sliding Area").Find("Handle").GetComponent<Image>().sprite = Forward;
            if (TargetGear.value < 0)
            {
                movement = Mathf.Lerp(movement, -TargetGear.value * car.backGear, Time.deltaTime * 2);
            }
            else
            {
                movement = Mathf.Lerp(movement, -TargetGear.value * car.backGear, Time.deltaTime * 4);
            }
        }
        if (MathF.Abs(movement) >= 0.05f)
        {
            GearSwap.gameObject.SetActive(false);
        }
        else
        {
            GearSwap.gameObject.SetActive(true);
        }

        if (turning >= 2) 
        {
            TurnButtonDeactivator(leftB, true);
        }
        else 
        {
            TurnButtonDeactivator(leftB, false);
        }
        if (turning <= -2)
        {
            TurnButtonDeactivator(rightB, true);
        }
        else 
        {
            TurnButtonDeactivator(rightB, false);
        }
        if (turning == 0)
        {
            TurnButtonDeactivator(resetB, true);
        }
        else
        {
            TurnButtonDeactivator(resetB, false);
        }


        // Test Code
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (movement < car.gear)
            {
                movement++;
            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (movement == car.gear)
            {
                movement--;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (movement > -car.backGear)
            {
                movement--;
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (movement == -car.backGear)
            {
                movement++;
            }
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            if (turning > -2)
            {
                turning--;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (turning < 2)
            {
                turning++;
            }
        }
    }
    void TurnButtonDeactivator(GameObject Object , bool Deact)
    {
        if (Deact)
        {
            Object.GetComponent<EventTrigger>().enabled = false;
            Object.GetComponent<Button>().interactable = false;
        }
        else 
        {
            Object.GetComponent<EventTrigger>().enabled = true;
            Object.GetComponent<Button>().interactable = true;
        }
    }
    public void Turned(int Tr)
    {
        if (Tr != 0)
        {
            turning += Tr;
        }
        else
        {
            turning = 0;
        }

    }
    public void BreakPress()
    {
        TargetGear.value = 0;
    }
    void turner()
    {
          
        if (turning != 0)
        {
            if (rb.linearVelocity.sqrMagnitude != 0)
            {
                dTime += Time.deltaTime * MathF.Abs(MathF.Abs(rb.linearVelocity.sqrMagnitude)) *4;
            }
            
            if (car.turnTime < dTime) 
            {
                if (turning < 0)
                {
                    turning++;
                    direction -= 45f;
                }
                else 
                {
                    direction += 45f;
                    turning--;
                }
                
                dTime = 0;
            }
        }
    }

    void CharacterMove()
    { 
        Vector2 inputVector;
        inputVector = new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad));
        
        float NorthUp = 1, EastUp = 1;
        Vector2 velocity;

        if (hills.Count > 0)
        {
            switch (hills[0])
            {
                case hill.NorthWest:
                    NorthUp = 2; EastUp = 0;
                    break;

                case hill.NorthEast:
                    NorthUp = 2; EastUp = 2;
                    break;

                case hill.SouthEast:
                    NorthUp = 0; EastUp = 2;
                    break;

                case hill.SouthWest:
                    NorthUp = 0; EastUp = 0;
                    break;

            }
            
            velocity = movementVector(inputVector, hills[0], true);
            
            
            animator.SetFloat("North", NorthUp);
            animator.SetFloat("East", EastUp);
        }
        else
        {
            NorthUp = 1; EastUp = 1;
            velocity = movementVector(inputVector, hill.NorthEast, false);
            animator.SetFloat("North", NorthUp);
            animator.SetFloat("East", EastUp);
        }


        rb.linearVelocity = velocity * car.Speed * Time.fixedDeltaTime;
        if (inputVector.sqrMagnitude > 0.1)
        {
            animator.SetFloat("Xvelocity", inputVector.x);
            animator.SetFloat("Yvelocity", inputVector.y);
        }
    }

    Vector2 movementVector(Vector2 realVector, hill activehill,bool hilling)
    {
        Vector2 Velocity = new Vector2(),Slope = new Vector2(1,1);
        if (!hilling)
        {
            Velocity = new Vector2((realVector.x * 2) / Mathf.Sqrt(5), realVector.y / Mathf.Sqrt(5));
        }
        else
        {
            switch (activehill)
            {
                case hill.NorthWest:Velocity = (new Vector2(realVector.x * 5, realVector.y * 4))/Mathf.Sqrt(41);
                    if(Velocity.x < 0)
                    {
                        Slope.x = 0.68f;
                    }
                    else
                    {
                        Slope.x = 1.3f;
                    }
                    if (Velocity.y < 0)
                    {
                        Slope.x = 1.3f;
                    }
                    else
                    {
                        Slope.y = 0.68f;
                    }
                    break;

                case hill.NorthEast:Velocity = (new Vector2(realVector.x * 50, realVector.y * 40)) / Mathf.Sqrt(4100);
                    if (Velocity.x < 0)
                    {
                        Slope.x = 1.3f;
                    }
                    else
                    {
                        Slope.x = 0.68f;
                    }
                    if (Velocity.y < 0)
                    {
                        Slope.x = 1.3f;
                    }
                    else
                    {
                        Slope.y = 0.68f;
                    }
                    break;

                case hill.SouthEast:Velocity = (new Vector2(realVector.x * 48, realVector.y * 10)) / Mathf.Sqrt(2404);
                    if (Velocity.x < 0)
                    {
                        Slope.x = 1.3f;
                    }
                    else
                    {
                        Slope.x = 0.68f;
                    }
                    if (Velocity.y < 0)
                    {
                        Slope.x = 0.68f;
                    }
                    else
                    {
                        Slope.y = 1.3f;
                    }
                    break;

                case hill.SouthWest:Velocity = (new Vector2(realVector.x * 48, realVector.y * 10)) / Mathf.Sqrt(2404);
                    if (Velocity.x < 0)
                    {
                        Slope.x = 0.68f;
                    }
                    else
                    {
                        Slope.x = 1.3f;
                    }
                    if (Velocity.y < 0)
                    {
                        Slope.x = 0.68f;
                    }
                    else
                    {
                        Slope.y = 1.3f;
                    }
                    break;
            }
        }
        

        
        if (movement < 0)
        {
            return movement * Slope * Velocity.normalized * 0.75f;
        }else
        {
            return movement * Slope * Velocity.normalized;
        }
        
    }

    void OverrideAnimations(CarData data)
    {
        if (data == null)
        {
            Debug.LogError("CarData null geldi.");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator null.");
            return;
        }

        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

         
        overrideController["NDrive"] = data.idleNorth;
        overrideController["NEDrive"] = data.idleNorthEast;
        overrideController["EDrive"] = data.idleEast;
        overrideController["SEDrive"] = data.idleSouthEast;
        overrideController["SDrive"] = data.idleSouth;
        overrideController["SWDrive"] = data.idleSouthWest;
        overrideController["WDrive"] = data.idleWest;
        overrideController["NWDrive"] = data.idleNorthWest;

        overrideController["NEUDrive"] = data.idleUpperNorthEast;
        overrideController["SEUDrive"] = data.idleUpperSouthEast;
        overrideController["SWUDrive"] = data.idleUpperSouthWest;
        overrideController["NWUDrive"] = data.idleUpperNorthWest;

        overrideController["NEDDrive"] = data.idleLowerNorthEast;
        overrideController["SEDDrive"] = data.idleLowerSouthEast;
        overrideController["SWDDrive"] = data.idleLowerSouthWest;
        overrideController["NWDDrive"] = data.idleLowerNorthWest;

        animator.runtimeAnimatorController = overrideController;
    }

}