using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointsBase : MonoBehaviour {

    private Transform[] checkpoints;

    private Transform checkpointsParent;
    public Vector2[] checkpointsPosition;

    [Header("Important Start Value")]
    public Vector2 startPosition;

    void Start () {
        checkpointsParent = GameObject.Find("Triggers_Checkpoints").transform;
        //Set lengths of the array
        checkpoints = new Transform[checkpointsParent.childCount + 1];
        checkpointsPosition = new Vector2[checkpointsParent.childCount + 1];

        //Fill transform array
        for (int i = 0; i < (checkpoints.Length - 1); i++){
            checkpoints[i + 1] = checkpointsParent.GetChild(i);
        }

        //Fill Vector2 array
        for(int i = 0; i < checkpoints.Length; i++){
            if (i == 0){
                checkpointsPosition[i] = startPosition;
            }
            else{
                checkpointsPosition[i] = new Vector2(checkpoints[i].position.x, checkpoints[i].position.y);
            }
        }

	}
	
}
