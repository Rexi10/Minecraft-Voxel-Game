using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Queue<ChunkRenderer> chunkPool = new Queue<ChunkRenderer>();

    public void Clear (WorldData worldData)
    {
        foreach (var item in worldData.chunkDictionary.Values)
        {
            Destroy(item.gameObject);
        }
        chunkPool.Clear();
    }
    internal ChunkRenderer RenderChunk(WorldData worldData, Vector3Int pos, MeshData meshData)
    {

        ChunkRenderer newChunk = null;
        if(chunkPool.Count > 0)
        {
            newChunk = chunkPool.Dequeue();
            newChunk.transform.position = pos;
        }
        else
        {
            GameObject chunkObject = Instantiate(chunkPrefab, pos, Quaternion.identity);
            chunkObject.transform.parent = this.transform;
            newChunk = chunkObject.GetComponent<ChunkRenderer>();
            newChunk.gameObject.name = "Chunk " + pos;
        }

        newChunk.InitializeChunk(worldData.chunkDataDictionary[pos]);
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
