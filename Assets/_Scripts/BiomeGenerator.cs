using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    public int waterThreshold = 50;

    public NoiseSettings biomeNoiseSettings;

    public DomainWarping domainWarping;

    public bool useDomainWarping = true;

    public BlockLayerHandler startLayerHandler;

    public List<BlockLayerHandler> additionalLayerHandlers;
    
    public TreeGenerator treeGenerator;

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset)
    {
        biomeNoiseSettings.worldOffset = mapSeedOffset;
        int groundPosition = GetSurfaceHeightNoise(data.worldPosition.x + x, data.worldPosition.z + z, data.chunkHeight);

        for (int y = data.worldPosition.y; y < data.worldPosition.y + data.chunkHeight; y++)
        {
            startLayerHandler.Handle(data, x, y, z, groundPosition, mapSeedOffset);
        }

        foreach (var layer in additionalLayerHandlers)
        {
            layer.Handle(data, x, data.worldPosition.y, z, groundPosition, mapSeedOffset);
        }
        return data;
    }

    internal TreeData GetTreeData(ChunkData data, Vector2Int mapSeedOffset)
    {
        if (treeGenerator == null)
        {
            return new TreeData();
        }
        return treeGenerator.GenerateTreeData(data, mapSeedOffset);
    }

    private int GetSurfaceHeightNoise(int x, int z, int chunkHeight)
    {
        float terrainHeight;
        if(useDomainWarping == false)
        {
            terrainHeight = MyNoise.OctavePerlin(x, z, biomeNoiseSettings);
        }
        else
        {
            terrainHeight = domainWarping.GenerateDomainNoise(x, z, biomeNoiseSettings);
        }

        terrainHeight = MyNoise.Redistribution(terrainHeight, biomeNoiseSettings);
        int surfaceHeight = (int)MyNoise.RemapValue01ToInt(terrainHeight, 0, chunkHeight);
        return surfaceHeight;
    }
}
