using UnityEngine;

[CreateAssetMenu(fileName = "NewCarData", menuName = "Car/Car Data")]
public class CarData : ScriptableObject
{
    public string carName;
    public float Speed;
    public float FuilCapa;
    public float gear;
    public float backGear;
    public float turnTime;
    public AudioClip engineSound;

    [Header("Animations")]
    public AnimationClip idleNorth;
    public AnimationClip idleNorthEast;
    public AnimationClip idleEast;
    public AnimationClip idleSouthEast;
    public AnimationClip idleSouth;
    public AnimationClip idleSouthWest;
    public AnimationClip idleWest;
    public AnimationClip idleNorthWest;

    public AnimationClip idleUpperNorthEast;
    public AnimationClip idleUpperSouthEast;
    public AnimationClip idleUpperSouthWest;
    public AnimationClip idleUpperNorthWest;

    public AnimationClip idleLowerNorthEast;
    public AnimationClip idleLowerSouthEast;
    public AnimationClip idleLowerSouthWest;
    public AnimationClip idleLowerNorthWest;



}
