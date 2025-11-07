using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Game/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public int resourceAmount;  
    public int maxCapacity = 9999; 
}