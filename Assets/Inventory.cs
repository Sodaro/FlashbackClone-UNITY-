using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
	public void AddItem(string keyvalue, int itemvalue)
	{
		if (!inventoryItems.ContainsKey(keyvalue))
			inventoryItems[keyvalue] = 0;
		inventoryItems[keyvalue] += itemvalue;
	}
	public void RemoveItem(string keyvalue, int itemvalue)
	{
		if (inventoryItems.ContainsKey(keyvalue))
			inventoryItems[keyvalue] -= itemvalue;
			
	}
	public bool HasItem(string keyvalue)
	{
		if (inventoryItems.ContainsKey(keyvalue))
			return inventoryItems[keyvalue] > 0;
		return false;
	}
}
