using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum PlanetsAlliance {enemyControlled, playerControlled, neutral };

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int numberOfPlanets;

    public List<int> planetsScore = new List<int>();
    public List<Vector3> planetsPosition = new List<Vector3>();

    public List<string> planetsName = new List<string>();
    public List<ColourSettings> planetsColourSettings = new List<ColourSettings>();
    public List<ShapeSettings> planetsShapeSettings = new List<ShapeSettings>();

    public List<PlanetsAlliance> planetsAlliance = new List<PlanetsAlliance>();
    // Start is called before the first frame update
    void Start()
    {
        
    }


}
