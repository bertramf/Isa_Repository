using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudio : MonoBehaviour {
    
	private void Start () {
        //Wwise stukje
        AkSoundEngine.PostEvent("Wind", gameObject);
        AkSoundEngine.SetRTPCValue("Wind_RTCP", 100f);
        AkSoundEngine.SetRTPCValue("Wind_Kracht", 15);
    }

    public void ChangeRTCP(float windRTCP, float windKracht) {
        //Wwise stukje
        AkSoundEngine.SetRTPCValue("Wind_RTCP", windRTCP);
        AkSoundEngine.SetRTPCValue("Wind_Kracht", windKracht);
    }
	
}
