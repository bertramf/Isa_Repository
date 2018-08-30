using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudio : MonoBehaviour {
    
	public void PlayMusicState(string musicState) {
        if(musicState == "StartEvent") {
            //Wwise stukje
            AkSoundEngine.PostEvent("Music", gameObject);
        }
        else if (musicState == "Start") {
            //Wwise stukje
            AkSoundEngine.SetState("Music_Progress", "Start");
        }
        else if (musicState == "Middle") {
            //Wwise stukje
            AkSoundEngine.SetState("Music_Progress", "Middle");
        }
        else if (musicState == "End") {
            //Wwise stukje
            AkSoundEngine.SetState("Music_Progress", "End");
        }
    }

}
