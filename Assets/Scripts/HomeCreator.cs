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

    public GameObject wallV;
    public GameObject wallH;
    List<Vector3Int> possDoorVPos;
    List<Vector3Int> possDoorHPos;
    List<Vector3Int> possWallVPos;
    List<Vector3Int> possWallHPos;



    // Start is called before the first frame update
    void Start()
    {
        CreateHome();
    }

    private void CreateHome()
    {
        HomeGenerator generator = new HomeGenerator(homeWidth, homeLength);
        var listOfRooms = generator.CalculateHome(maxIterations, roomWidthMin, roomLengthMin, corridorWidth);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;

        possDoorVPos = new List<Vector3Int>();
        possDoorHPos = new List<Vector3Int>();
        possWallVPos = new List<Vector3Int>();
        possWallHPos = new List<Vector3Int>();

        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }

        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach(var wallPosition in possWallHPos)
        {
            CreateWall(wallParent, wallPosition, wallH);
        }

        foreach(var wallPosition in possWallVPos)
        {
            CreateWall(wallParent, wallPosition, wallV);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab,wallPosition,Quaternion.identity, wallParent.transform);
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
        groundFloor.transform.parent = transform;

        for (int row = (int)bottomLeftv.x; row < (int)bottomRightv.x; row++) 
        {
            var wallPosition = new Vector3(row, 0, bottomLeftv.z);
            AddWallPossToList(wallPosition, possWallHPos, possDoorHPos);
        }
        
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPossToList(wallPosition, possWallHPos, possDoorHPos);
        }
        for (int col = (int)bottomLeftv.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftv.x, 0, col);
            AddWallPossToList(wallPosition, possWallVPos, possDoorVPos);
        }
        for (int col = (int)bottomRightv.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightv.x, 0, col);
            AddWallPossToList(wallPosition, possWallVPos, possDoorVPos);
        }

    }

    private void AddWallPossToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);

        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }
}
