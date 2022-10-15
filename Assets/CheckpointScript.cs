using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointScript : InteractibleBase
{


    [Range(0, 100)]
    public int index = 0;

	public override void Interact(GameObject caller)
	{
		interactionEvent.Invoke(this);
	}
}
