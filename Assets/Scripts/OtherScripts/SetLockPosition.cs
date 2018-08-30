using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLockPosition : MonoBehaviour {

	public Vector3 posCameraLock;

	public void SetPosition(Vector3 otherPosition){
		posCameraLock = otherPosition;
	}
}
