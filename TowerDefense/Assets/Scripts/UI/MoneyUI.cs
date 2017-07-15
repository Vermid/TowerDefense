using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;
	// Update is called once per frame
	void Update ()
	{
        //if you add a string to int in UNITY unity will make a int to string no need to use the method ToString()
	    moneyText.text = "$" + PlayerStarts.money.ToString();
	}
}
