using System.Collections.Generic;
using UnityEngine;

public class TreeLayerHandler : BlockLayerHandler
{
    public float terrainHeightLimit = 25;


    public static List<Vector3Int> treeLeafesStaticLayout = new List<Vector3Int>
{
        // Layer 1 (top layer of leaves)
        new Vector3Int(0, 4, 0),  // Center

        // Layer 2
        new Vector3Int(0, 3, 0),  // Center
        new Vector3Int(1, 3, 0),  // E (East)
        new Vector3Int(-1, 3, 0), // W (West)
        new Vector3Int(0, 3, 1),  // N (North)
        new Vector3Int(0, 3, -1), // S (South)
        new Vector3Int(1, 3, 1),  // NE (Northeast)
        new Vector3Int(1, 3, -1), // SE (Southeast)
        new Vector3Int(-1, 3, 1), // NW (Northwest)
        new Vector3Int(-1, 3, -1),// SW (Southwest)

        // Layer 3
        new Vector3Int(0, 2, 0),  // Center
        new Vector3Int(1, 2, 0),  // E (East)
        new Vector3Int(-1, 2, 0), // W (West)
        new Vector3Int(0, 2, 1),  // N (North)
        new Vector3Int(0, 2, -1), // S (South)
        new Vector3Int(1, 2, 1),  // NE (Northeast)
        new Vector3Int(1, 2, -1), // SE (Southeast)
        new Vector3Int(-1, 2, 1), // NW (Northwest)
        new Vector3Int(-1, 2, -1),// SW (Southwest)
        new Vector3Int(2, 2, 0),  // Far E (Far East)
        new Vector3Int(-2, 2, 0), // Far W (Far West)
        new Vector3Int(0, 2, 2),  // Far N (Far North)
        new Vector3Int(0, 2, -2), // Far S (Far South)

        // Layer 4 (bottom layer of leaves)
        new Vector3Int(0, 1, 0),  // Center
        new Vector3Int(1, 1, 0),  // E (East)
        new Vector3Int(-1, 1, 0), // W (West)
        new Vector3Int(0, 1, 1),  // N (North)
        new Vector3Int(0, 1, -1), // S (South)
        new Vector3Int(1, 1, 1),  // NE (Northeast)
        new Vector3Int(1, 1, -1), // SE (Southeast)
        new Vector3Int(-1, 1, 1), // NW (Northwest)
        new Vector3Int(-1, 1, -1),// SW (Southwest)
        new Vector3Int(2, 1, 0),  // Far E (Far East)
        new Vector3Int(-2, 1, 0), // Far W (Far West)
        new Vector3Int(0, 1, 2),  // Far N (Far North)
        new Vector3Int(0, 1, -2), // Far S (Far South)
        new Vector3Int(2, 1, 1),  // Far NE (Far Northeast)
        new Vector3Int(2, 1, -1), // Far SE (Far Southeast)
        new Vector3Int(-2, 1, 1), // Far NW (Far Northwest)
        new Vector3Int(-2, 1, -1) // Far SW (Far Southwest)
};


    protected override bool TryHandling(ChunkData chunkData, int x, int y, int z, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (chunkData.worldPosition.y < 0)
        {
            return false;
        }
        if (surfaceHeightNoise < terrainHeightLimit 
            && chunkData.treeData.treePositions.Contains(new Vector2Int(chunkData.worldPosition.x + x,chunkData.worldPosition.z + z)))
        {
            Vector3Int chunkCoordinates = new Vector3Int(x, surfaceHeightNoise, z);
            BlockType type = Chunk.GetBlockFromChunkCoordinates(chunkData, chunkCoordinates);
            if(type == BlockType.Grass_Dirt)
            {
                
                Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.Dirt);
                for ( int i = 1; i < 5 ; i++)
                {
                    chunkCoordinates.y = surfaceHeightNoise + i;
                    Chunk.SetBlock(chunkData, chunkCoordinates, BlockType.TreeTrunk);
                }
                foreach (Vector3Int leafPosition in treeLeafesStaticLayout)
                {
                    chunkData.treeData.treeLeafes.Add(new Vector3Int(x + leafPosition.x, surfaceHeightNoise + 4 + leafPosition.y, z + leafPosition.z));
                }
            }
        }
        return false;
    }
}
