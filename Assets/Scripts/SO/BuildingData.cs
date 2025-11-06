using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Game/Building Data")]
public class BuildingData : ScriptableObject
{
    [Header("Основные данные здания")]
    public string buildingName;

    [Header("Сохраняемое значение ресурса")]
    public int collectPoint; // количество ресурса, которое нужно сохранять
}