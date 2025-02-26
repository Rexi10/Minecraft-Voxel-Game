using UnityEngine;
using System.Collections.Generic;

public class MeshData
{
    //usamos list para ser mais facil adicionar novos vertices porque nem todos os chunks terão a mesma quantidade de vertices
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    //colisão (p. ex colisão com a água)
    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();

    //a mesh da agua é diferente da mesh principal porque é transparente, logo teremos de usar submeshs
    public MeshData waterMesh;

    //helper boolean flag
    private bool isMainMesh = true;


    //constructor, para evitar um problema de recursão infinita, usamos um booleano para saber se é a mesh principal ou não.
    public MeshData(bool isMainMesh)
    {
        if(isMainMesh)
        {
            waterMesh = new MeshData(false);
        }
    }

    //adicionar os vertices à lista de vertices e um boolean para saber se é um vertice que gera colisão
    public void AddVertex(Vector3 vertex, bool vertextGeneratesCollider)
    {
        vertices.Add(vertex);
        if (vertextGeneratesCollider)
        {
            colVertices.Add(vertex);
        }
    }


    public void AddQuadTriangles(bool quadGeneratesCollider)
    {
        //logica retirada do forum da Unity https://ilkinulas.github.io/development/unity/2016/04/30/cube-mesh-in-unity3d.html

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (quadGeneratesCollider)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);

            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }
    
}