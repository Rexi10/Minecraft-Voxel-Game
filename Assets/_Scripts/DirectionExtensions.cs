using UnityEngine;

public static class DirectionExtensions
{
    public static Vector3Int GetVector(this Direction direction)
    {
        return direction switch
        {
            Direction.up => Vector3Int.up,
            Direction.down => Vector3Int.down,
            Direction.left => Vector3Int.left,
            Direction.right => Vector3Int.right,
            Direction.foreward => Vector3Int.forward,
            Direction.backwards => Vector3Int.back,
            _ => throw new System.Exception("Invalid input direction")
        };
    }
}
