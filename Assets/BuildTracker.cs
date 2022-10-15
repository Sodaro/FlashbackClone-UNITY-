using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTracker : MonoBehaviour
{
    [SerializeField] Text buildNumberTracker;
    // Start is called before the first frame update
    void Start()
    {
        buildNumberTracker.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
