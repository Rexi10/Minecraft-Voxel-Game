using System;
using System.IO.Compression;
using UnityEngine;

public static class Chunk 
{
    public static void LoopThroughTheBlocks(ChunkData chunkData, Action<int, int, int> actiontoPerform)
    {
        for (int index = 0; index < chunkData.blocks.Length; index++)
        {
            var position = GetPositionFromIndex(chunkData, index);
            actiontoPerform(position.x, position.y, position.z);
        }
    }

    private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int index)
    {

        //retirado do stackoverflow convert 1d array index to 3d array index
        int x = index % chunkData.chunkSize;
        int y = (index / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = index / (chunkData.chunkSize * chunkData.chunkHeight);

        return new Vector3Int(x, y, z);
    }

    //Sistema de coordenadas dentro do chunk
    private static bool InRange(ChunkData chunkData, int axisCoordinate)
    {
        if(axisCoordinate < 0 || axisCoordinate >= chunkData.chunkSize)
        {
            return false;
        }
        return true;
    }

    private static bool InRangeHeight(ChunkData chunkData, int yCoordinate)
    {
        if(yCoordinate < 0 || yCoordinate >= chunkData.chunkHeight)
        {
            return false;
        }
        return true;
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int chunkCoordinates)
    {
        return GetBlockFromChunkCoordinates(chunkData, chunkCoordinates.x, chunkCoordinates.y, chunkCoordinates.z);
    }
    


    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        if(InRange(chunkData, x) && InRangeHeight(chunkData, y) && InRange(chunkData, z))
        {
            int index = GetIndexFromPosition(chunkData, x, y, z);
            return chunkData.blocks[index];
        }
        else
        {
            return chunkData.worldReference.GetBlockFromChunkCoordinates(chunkData, chunkData.worldPosition.x + x, chunkData.worldPosition.y + y, chunkData.worldPosition.z + z);
        }
    }


    public static void SetBlock(ChunkData chunkData,Vector3Int localPosition, BlockType blockType)
    {
        if(InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) && InRange(chunkData, localPosition.z))
        {
            int index = GetIndexFromPosition(chunkData, localPosition.x, localPosition.y, localPosition.z);
            chunkData.blocks[index] = blockType;
        }
        else
        {
            Debug.LogError("block está fora do range do chunk");
        }
    }

    private static int GetIndexFromPosition(ChunkData chunkData, int x, int y, int z)
    {

        // calculo inverso do getpositionfromindex do stackoverflow
        return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
    }



    public static Vector3Int GetBlockInChunkCoordinates(ChunkData chunkData, Vector3Int pos)
    {
        return new Vector3Int
        {
            //coordenadas do chunk sao diferentes das coordenados do mundo
            // EXEMPLO
            // se a world position for 10 e o chunk estiver em na origem, posicao do chunk é 10
            // mas se a world position for 30 e o chunk estíver na posicao 16 , sera 30-16 = 14 que sera a posicao dentro do chunk

            x = pos.x - chunkData.worldPosition.x,
            y = pos.y - chunkData.worldPosition.y,
            z = pos.z - chunkData.worldPosition.z
        };



    }



    public static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new MeshData(true);

        LoopThroughTheBlocks(chunkData, (x, y, z) => meshData = BlockHelper.GetMeshData(chunkData, x, y, z, meshData,
                                                     chunkData.blocks[GetIndexFromPosition(chunkData, x, y, z)]));
       
        return meshData;
    }

    internal static Vector3Int ChunkPositionFromBlockCoords(World world, int x, int y, int z)
    {
        Vector3Int pos = new Vector3Int
        {
            x = Mathf.FloorToInt(x / (float)world.chunkSize) * world.chunkSize,
            y = Mathf.FloorToInt(y / (float)world.chunkHeight) * world.chunkHeight,
            z = Mathf.FloorToInt(z / (float)world.chunkSize) * world.chunkSize
        };
        return pos;
    }
}
