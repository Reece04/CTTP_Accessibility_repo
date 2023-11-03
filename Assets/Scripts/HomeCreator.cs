using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCreator : MonoBehaviour
{

    public int homeWidth;
    public int homeLength;
    public int roomWidthMin;
    public int roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        CreateHome();
    }

    private void CreateHome()
    {
        HomeGenerator generator = new HomeGenerator(homeWidth, homeLength);
        var listOfRooms = generator.CalculateHome(maxIterations, roomWidthMin, roomLengthMin, corridorWidth);
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftv = new Vector3(bottomLeftCorner.x,0,bottomLeftCorner.y);
        Vector3 bottomRightv = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x,0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftv,
            bottomRightv,
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3,
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject groundFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        groundFloor.transform.position = Vector3.zero;
        groundFloor.transform.localScale = Vector3.one;
        groundFloor.GetComponent<MeshFilter>().mesh = mesh;
        groundFloor.GetComponent<MeshRenderer>().material = material;


        

    }

    
}
