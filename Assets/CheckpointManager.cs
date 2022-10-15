using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

public class CheckpointManager : Singleton<CheckpointManager>
{
    // (Optional) Prevent non-singleton constructor use.
    protected CheckpointManager() { }

    [SerializeField]
    Text checkpointDebugText;
    Dictionary<int, CheckpointScript> checkpoints;
    CheckpointScript activeCheckpoint;
    int activeIndex = 0;

    [SerializeField]
    GameObject[] debugObjects;

    GameObject player;
	// Start is called before the first frame update

	private void Awake()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        //GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

        //if (objs.Length > 1)
        //{
        //    Destroy(this.gameObject);
        //}

        //DontDestroyOnLoad(this.gameObject);
    }

	void Start()
    {

        foreach (GameObject child in debugObjects)
        {
            child.SetActive(false);
        }
        CheckpointScript[] temparr = FindObjectsOfType(typeof(CheckpointScript)) as CheckpointScript[];
        checkpoints = new Dictionary<int, CheckpointScript>();
        foreach (var checkpoint in temparr)
		{
            if (checkpoints.ContainsKey(checkpoint.index))
			{
                throw new System.Exception("CHECKPOINT WITH INDEX ALREADY EXISTS, conflicting objects: " + 
                    checkpoint.gameObject.name + ", " + checkpoints[checkpoint.index].gameObject.name);
			}
            checkpoints[checkpoint.index] = checkpoint;
            checkpoints[checkpoint.index].interactionEvent.AddListener(ActivateCheckpoint);

        }
        activeCheckpoint = checkpoints[0];

        
		//checkpoints = new List<CheckpointScript>(FindObjectsOfType(typeof(CheckpointScript)) as CheckpointScript[]);
  //      foreach (var checkpoint in checkpoints)
		//{
  //          //Debug.Log(checkpoint.name);
  //          checkpoint.interactionEvent.AddListener(ActivateCheckpoint);
		//}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) //toggle debug ui
		{
            foreach (GameObject child in debugObjects)
            {
                child.SetActive(!child.activeInHierarchy);
			}
		}
        if (Input.GetKeyDown(KeyCode.F2))
        {
            activeIndex--;
            activeIndex = Mathf.Clamp(activeIndex, checkpoints.Keys.Min(), checkpoints.Keys.Max());
            if (checkpoints.ContainsKey(activeIndex))
            {
                activeCheckpoint = checkpoints[activeIndex];
                LoadGameFromCheckpoint();
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
		{
            activeIndex++;
            activeIndex = Mathf.Clamp(activeIndex, checkpoints.Keys.Min(), checkpoints.Keys.Max());
            if (checkpoints.ContainsKey(activeIndex))
            {
                activeCheckpoint = checkpoints[activeIndex];
                LoadGameFromCheckpoint();
            }
        }
    }

    void ActivateCheckpoint(CheckpointScript _checkpoint)
	{
        activeCheckpoint = _checkpoint;
        activeIndex = _checkpoint.index;
        //Debug.Log("ACTIVE CHECKPOINT : " + activeCheckpoint);
        checkpointDebugText.text = activeIndex.ToString();

    }

    public void LoadGameFromCheckpoint()
	{
        player.transform.position = new Vector3(activeCheckpoint.transform.position.x + 0.5f, activeCheckpoint.transform.position.y + 0.5f);
	}
}
