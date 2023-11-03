using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    //Allows us to extract all children as child from the parent node provided.
    public static List<Node> TraverseGraphToExtractLowestLeafes(Node parentNode)
    {
        Queue<Node> nodesTocheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();
        if (parentNode.ChildrenNodeList.Count == 0)
        {
            return new List<Node>()
            {
                parentNode
            };
        }
        foreach (var child in parentNode.ChildrenNodeList)
        {
            nodesTocheck.Enqueue(child);
        }
        while (nodesTocheck.Count > 0)
        {
            var currentNode = nodesTocheck.Dequeue();
            if (currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesTocheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier)
    {
        int minX = boundaryLeftPoint.x;
        int maxX = boundaryRightPoint.x;
        int minY = boundaryLeftPoint.y;
        int maxY = boundaryRightPoint.y;

        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier)));

    }

    public static Vector2Int GenerateTopRightCornerBetween(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier)
    {
        int minX = boundaryLeftPoint.x;
        int maxX = boundaryRightPoint.x;
        int minY = boundaryLeftPoint.y;
        int maxY = boundaryRightPoint.y;

        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * pointModifier),maxX),
            Random.Range((int)(minY + (maxY - minY) * pointModifier),maxY)
            );

    }

    public static Vector2Int CalculatemiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v1;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}

public enum RelativePosition
    {
        Up,
        Down,
        Right,
        Left,
    }