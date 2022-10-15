using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //[SerializeField] Text stateDisplay;
    [SerializeField] TextMesh stateDisplay2;
    //[SerializeField] Transform textPosition;
    //private Transform player;
	private void Start()
	{
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //textPosition.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, player.transform.position.z);
	}
	public void Display(State state)
    {
        var name = state.ToString();
        if (stateDisplay2.text == name)
            return;
        stateDisplay2.text = name;
    }
}
