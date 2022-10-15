using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public static class Utilities
{
	public static float tileSize = 8f;
    public static float playerSize = 16f;
    public static float unitSize = 1f;
    static int amountOfTiles = 0;
    public static float playerMovementOffset = 0.5f; //player occupies 2 tiles, 0.5f = 1 tile
	public enum Direction { LEFT, RIGHT, UP, DOWN };
	public static KeyCode ActionKey = KeyCode.LeftShift;
	public static KeyCode JumpKey = KeyCode.UpArrow;
	//public static KeyCode InteractKey = KeyCode.Return;
	public static KeyCode DownKey = KeyCode.DownArrow;

    [System.Serializable]
    public class InteractionEvent : UnityEvent<CheckpointScript> { };


    public static Vector3 GetTileCenterPos(this Tilemap tilemap, Vector3 position) //extension method
	{
		Vector3Int cellPos = tilemap.WorldToCell(position);
		return tilemap.GetCellCenterWorld(cellPos);
	}

    public static Vector3 GetGridCenterPos(this GridLayout gridLayout, Vector3 position) //extension
	{
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        return gridLayout.CellToWorld(cellPos);
	}

    //   public static bool HasTile16x16(this Tilemap tilemap, Vector3 position)
    //{

    //       return false;
    //}

    public static bool HasWallInDirection(this Tilemap tilemap, Vector3 position, Direction direction, TileBase highlightTile, Tilemap highlightMap) //check nearest tile (might need to change to check 2x1 shape)?
    {
        Vector3 pos2;
        Vector3Int cellPos, cellPos2;
        amountOfTiles += 2;
        if (amountOfTiles > 12)
		{
            highlightMap.ClearAllTiles();
            amountOfTiles = 0;
		}
        switch (direction)
        {
            case Direction.LEFT:
                //cellPos = tilemap.WorldToCell(position);
                //cellPos2 = tilemap.WorldToCell(pos2);
                position = new Vector3(position.x - 1f, position.y, position.z);
                pos2 = new Vector3(position.x, position.y - 0.5f, position.z);
                cellPos = tilemap.WorldToCell(position);
                cellPos2 = tilemap.WorldToCell(pos2);
                //Debug.Log($"checking wall at: {cellPos}, {cellPos2}");
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) && tilemap.HasTile(cellPos2);
            case Direction.RIGHT:
                position = new Vector3(position.x + 0.5f, position.y, position.z);
                pos2 = new Vector3(position.x, position.y - 0.5f, position.z);
                cellPos = tilemap.WorldToCell(position);
                cellPos2 = tilemap.WorldToCell(pos2);
                //Debug.Log($"checking wall at: {cellPos}, {cellPos2}");
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                return tilemap.HasTile(cellPos) && tilemap.HasTile(cellPos2);
            case Direction.UP:
                position = new Vector3(position.x, position.y, position.z);
                pos2 = new Vector3(position.x - 0.5f, position.y, position.z);
                cellPos = tilemap.WorldToCell(position);
                cellPos2 = tilemap.WorldToCell(pos2);
                //Debug.Log($"checking wall at: {cellPos}, {cellPos2}");
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                //Debug.Log($"checking ledge pos at: {cellPos}, {cellPos2}");
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) && tilemap.HasTile(cellPos2);
            case Direction.DOWN:
                position = new Vector3(position.x, position.y-0.5f, position.z);
                pos2 = new Vector3(position.x + 0.5f, position.y, position.z);
                cellPos = tilemap.WorldToCell(position);
                cellPos2 = tilemap.WorldToCell(pos2);
                //Debug.Log($"checking wall pos at: {cellPos}, {cellPos2}");
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) && tilemap.HasTile(cellPos2);
        }
        return false;
    }

    public static bool HasHalfTileInDirection(this Tilemap tilemap, Vector3 position, Direction direction, TileBase highlightTile, Tilemap highlightMap) //check nearest tile (might need to change to check 2x1 shape)?
	{
        Vector3 pos2;
        Vector3Int cellPos, cellPos2;
        amountOfTiles += 2;
        if (amountOfTiles > 12)
        {
            highlightMap.ClearAllTiles();
            amountOfTiles = 0;
        }
        switch (direction)
        {
            case Direction.LEFT:
                position = new Vector3(position.x - 1f, position.y, position.z);
                pos2 = new Vector3(position.x - 0.5f, position.y, position.z);
                cellPos = tilemap.WorldToCell(position);
                cellPos2 = tilemap.WorldToCell(pos2);
                //Debug.Log($"checking ledge pos at: {cellPos}, {cellPos2}");
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) || tilemap.HasTile(cellPos2);
            case Direction.RIGHT:
                position = new Vector3(position.x + 0.5f, position.y, position.z);
                pos2 = new Vector3(position.x + 0.5f, position.y, position.z);
                cellPos = tilemap.WorldToCell(position);
                cellPos2 = tilemap.WorldToCell(pos2);
                //Debug.Log($"checking ledge pos at: {cellPos}, {cellPos2}");
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) || tilemap.HasTile(cellPos2);
        }
        return false;
    }

    public static bool HasTileInDirection(this Tilemap tilemap, Vector3 position, Direction direction, float offset, TileBase highlightTile, Tilemap highlightMap) //extension method
    { 
        Vector3 position2, position3, position4;
        Vector3Int cellPos, cellPos2, cellPos3, cellPos4;
        
        switch (direction)
        {
            case Direction.LEFT:
                position = new Vector3(position.x - offset, position.y, position.z);
                position2 = new Vector3(position.x, position.y - 0.5f, position.z);
                position3 = new Vector3(position.x - offset, position.y, position.z);
                position4 = new Vector3(position.x - offset, position.y - 0.5f, position.z);
                cellPos = tilemap.WorldToCell(position); cellPos2 = tilemap.WorldToCell(position2);
                cellPos3 = tilemap.WorldToCell(position3); cellPos4 = tilemap.WorldToCell(position4);
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) || tilemap.HasTile(cellPos2) || tilemap.HasTile(cellPos3) || tilemap.HasTile(cellPos4);
            case Direction.RIGHT:
                position = new Vector3(position.x + 0.5f, position.y, position.z);
                position2 = new Vector3(position.x, position.y - 0.5f, position.z);
                position3 = new Vector3(position.x + offset, position.y, position.z);
                position4 = new Vector3(position.x + offset, position.y - 0.5f, position.z);
                cellPos = tilemap.WorldToCell(position); cellPos2 = tilemap.WorldToCell(position2);
                cellPos3 = tilemap.WorldToCell(position3); cellPos4 = tilemap.WorldToCell(position4);
               // Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}, pos3: {cellPos3}, pos4: {cellPos4}");
                return tilemap.HasTile(cellPos) || tilemap.HasTile(cellPos2) || tilemap.HasTile(cellPos3) || tilemap.HasTile(cellPos4);
            case Direction.DOWN:
                position = new Vector3(position.x, position.y -1, position.z);
                position2 = new Vector3(position.x - offset, position.y, position.z);
                //Debug.Log($"pos1 {position}, pos2 {position2}");
                cellPos = tilemap.WorldToCell(position); cellPos2 = tilemap.WorldToCell(position2);
                return tilemap.HasTile(cellPos) && tilemap.HasTile(cellPos2);
            case Direction.UP:
                position = new Vector3(position.x, position.y + 0.5f, position.z);
                position2 = new Vector3(position.x - offset, position.y, position.z);
                cellPos = tilemap.WorldToCell(position); cellPos2 = tilemap.WorldToCell(position2);
                highlightMap.SetTile(cellPos, highlightTile);
                highlightMap.SetTile(cellPos2, highlightTile);
                //Debug.Log($"pos1: {cellPos}, pos2: {cellPos2}");
                return tilemap.HasTile(cellPos) || tilemap.HasTile(cellPos2);
        }
        return false;
    }
    public static TileBase GetTile(this Tilemap tilemap, Vector3 position)
	{
        return tilemap.GetTile(tilemap.WorldToCell(position));
	}
    public static Vector3 SmoothStep(Vector3 start, Vector3 end, float time, float duration) //extension method for slow start, fast mid, slow end curve
    {
        float t = time / duration;
        t = t * t * (3f - 2f * t);
        return Vector3.Lerp(start, end, t);
    }
}
