using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractibleBase : MonoBehaviour
{
    [HideInInspector]
    public Utilities.InteractionEvent interactionEvent = new Utilities.InteractionEvent();
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public abstract void Interact(GameObject caller);
}
