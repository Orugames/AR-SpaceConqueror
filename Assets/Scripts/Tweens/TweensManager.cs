using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweensManager : MonoBehaviour
{
    public static TweensManager instance = null;

    public TableTween tableTween;
    public DoorsOpeningTween doorsOpeningTween;
    public PlanetsTween planetsTween;
    public LevelSelectorTween levelSelectorTween;
    public LevelBriefingTween levelBriefingTween;
    public LevelCompletedTween levelCompletedTween;
    public FloorShaderTween floorShaderTween;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }
    
}
