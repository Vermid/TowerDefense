using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text livesText;
	// Update is called once per frame
	void Update () {
        //change this to a coroutine or to the player sats for android
	    livesText.text = PlayerStarts.lives.ToString() + " Lives";
	}
}
