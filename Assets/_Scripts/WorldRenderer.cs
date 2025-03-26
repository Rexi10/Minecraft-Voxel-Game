using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Queue<ChunkRenderer> chunkPool = new Queue<ChunkRenderer>();
    public Transform chunksParent; // Add this field to hold the parent transform

    private void Awake()
    {
        // Find or create the "Chunks" parent GameObject
        GameObject chunksParentObject = GameObject.Find("Chunks");
        if (chunksParentObject == null)
        {
            chunksParentObject = new GameObject("Chunks");
        }
        chunksParent = chunksParentObject.transform;
    }

    public void Clear(WorldData worldData)
    {
        foreach (var item in worldData.chunkDictionary.Values)
        {
            Destroy(item.gameObject);
        }
        chunkPool.Clear();
    }

    internal ChunkRenderer RenderChunk(WorldData worldData, Vector3Int position, MeshData meshData)
    {
        ChunkRenderer newChunk = null;
        if (chunkPool.Count > 0)
        {
            newChunk = chunkPool.Dequeue();
            newChunk.transform.position = position;
        }
        else
        {
            GameObject chunkObject = Instantiate(chunkPrefab, position, Quaternion.identity, chunksParent); // Set the parent to chunksParent
            newChunk = chunkObject.GetComponent<ChunkRenderer>();
            newChunk.gameObject.name = "Chunk " + position;
        }

        newChunk.InitializeChunk(worldData.chunkDataDictionary[position]);
        newChunk.UpdateChunk(meshData);
        newChunk.gameObject.SetActive(true);
        return newChunk;
    }

    public void RemoveChunk(ChunkRenderer chunk)
    {
        chunk.gameObject.SetActive(false);
        chunkPool.Enqueue(chunk);
    }
}