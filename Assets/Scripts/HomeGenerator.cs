using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeGenerator
{
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int homeWidth;
    private int homeLength;

    public HomeGenerator(int homeWidth, int homeLength)
    {
        this.homeWidth = homeWidth;
        this.homeLength = homeLength;
    }

    //used to exact children that represent rooms in tree (lowest)
    public List<Node> CalculateHome(int maxIterations, int roomWidthMin, int roomLengthMin, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(homeWidth, homeLength);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GemerateRoomsInGivenSpaces(roomSpaces);

        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCoridoor(allNodesCollection, corridorWidth);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}