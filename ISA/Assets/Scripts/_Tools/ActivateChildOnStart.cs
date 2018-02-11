using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChildOnStart : MonoBehaviour {

    private GameObject childObj;

    private void Start(){
        childObj = transform.GetChild(0).gameObject;
        childObj.SetActive(true);
    }

}
