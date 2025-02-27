using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataProcessing
{
    public static List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int( 0, 1),  // N (North)
        new Vector2Int( 1, 1),  // NE (Northeast)
        new Vector2Int( 1, 0),  // E (East)
        new Vector2Int( -1, 1), // SE (Southeast)
        new Vector2Int( -1, 0), // S (South)
        new Vector2Int( -1, -1),// SW (Southwest)
        new Vector2Int( 0, -1), // W (West)
        new Vector2Int( 1, -1)  // NW (Northwest)
    };

    public static List<Vector2Int> FindLocalMaxima(float[,] dataMatrix, int xCoord, int zCoord)
    {
        List<Vector2Int> maximas = new List<Vector2Int>();
        for(int x = 0; x < dataMatrix.GetLength(0); x++)
        {
            for(int y = 0; y < dataMatrix.GetLength(1); y++)
            {
                float noiseVal = dataMatrix[x, y];
                if(CheckNeighbours(dataMatrix, x, y, (neighbourNoise) => neighbourNoise < noiseVal))
                {
                    maximas.Add(new Vector2Int(xCoord + x, zCoord + y));
                }
            }
        }
        return maximas;
    }


    private static bool CheckNeighbours(float[,] dataMatrix, int x, int y, Func<float, bool> sucessCondition)
    {
        foreach(var dir in directions)
        {
            var newPost = new Vector2Int(x + dir.x, y + dir.y);

            if(newPost.x < 0 || newPost.x >= dataMatrix.GetLength(0) || newPost.y < 0 || newPost.y >= dataMatrix.GetLength(1) )
            {
                continue;
            }

            if (sucessCondition(dataMatrix[x + dir.x, y + dir.y]) == false)
            {
                return false;
            }
        }
        return true;
    }
}
