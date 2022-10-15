using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupScript : InteractibleBase
{
	//[SerializeField]
	//GameObject targetToDestroy;

	[SerializeField]
	Tilemap targetTilemap;

	[SerializeField]
	Vector3Int[] tileCoordinates;

	[SerializeField]
	string key;


	private void Start()
	{
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	public override void Interact(GameObject caller)
	{
		caller.GetComponent<Inventory>().AddItem(key, 1);
		foreach (var coordinate in tileCoordinates)
		{
			targetTilemap.SetTile(coordinate, null);
		}
		//Destroy(targetToDestroy);
		Destroy(this.gameObject);
	}

}
