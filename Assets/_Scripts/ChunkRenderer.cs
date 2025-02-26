using System.Linq;
using UnityEditor;
using UnityEngine;



// https://docs.unity3d.com/Manual/class-MeshFilter.html
// https://docs.unity3d.com/Manual/class-MeshRenderer.html
// https://docs.unity3d.com/Manual/class-MeshCollider.html

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

// Vai adicionar automaticamente os componentes MeshFilter, MeshRenderer e MeshCollider ao GameObject que tiver este script
public class ChunkRenderer : MonoBehaviour 
{

    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    //Gizmo para observar o tamanho do chunk para apoio visual
    public bool showGizmo = false;

    //propriedade para guardar o chunkData
    public ChunkData ChunkData { get; private set; }

    public bool ModifiedByPlayer
    {
        get
        {
            return ChunkData.modifiedByThePlayer;
        }
        set
        {
            ChunkData.modifiedByThePlayer = value;
        }
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void InitializeChunk(ChunkData data)
    {
        this.ChunkData = data;
    }

    private void RenderMesh(MeshData meshData)
    {
        // limpamos a mesh, usamos submeshs para defenir um diferente material para a agua e para a objetos solidos
        mesh.Clear();

        mesh.subMeshCount = 2;
        mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();

        // cada submesh precisa dos seus triangulos defenidos separadamente
        mesh.SetTriangles(meshData.triangles.ToArray(), 0);
        mesh.SetTriangles(meshData.waterMesh.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1); // off seat dos indices

        mesh.uv = meshData.uv.Concat(meshData.waterMesh.uv).ToArray();
        mesh.RecalculateNormals();

        //separamos as meshs porque agora podemos criar uma nova mesh , aceder a mesh de colisao, acedar aos vertices, associalos a colVertices e fazer o mesmo aos triangulos
        meshCollider.sharedMesh = null;
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = meshData.colVertices.ToArray();
        collisionMesh.triangles = meshData.colTriangles.ToArray();
        
        //não tenho a certeza que isto é necessario
        collisionMesh.RecalculateNormals();

        meshCollider.sharedMesh = collisionMesh;
    }
    
    public void UpdateChunk()
    {
        RenderMesh(Chunk.GetChunkMeshData(ChunkData));
    }

    public void UpdateChunk(MeshData data)
    {
        RenderMesh(data);
    }


// gizmo para apoio visual retirado da internet
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            if (Application.isPlaying && ChunkData != null)
            {
                if (Selection.activeObject == gameObject)
                    Gizmos.color = new Color(0, 1, 0, 0.5f);
                else
                    Gizmos.color = new Color(1, 0, 1, 0.5f);

                Gizmos.DrawCube(transform.position + new UnityEngine.Vector3(ChunkData.chunkSize / 2f, ChunkData.chunkHeight / 2f, ChunkData.chunkSize / 2f), new UnityEngine.Vector3(ChunkData.chunkSize, ChunkData.chunkHeight, ChunkData.chunkSize));
            }
        }
    }
#endif



}
