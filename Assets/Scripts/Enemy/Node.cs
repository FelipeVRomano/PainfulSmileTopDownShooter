using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int iGridX;
    public int iGridY;

    public bool isCollision;
    public Vector3 vPosition;
    public Node parentNode;
    public int igCost;
    public int ihCost;

    public int FCost
    {
        get
        {
            return igCost + ihCost;
        }
    }

    public Node(bool collision, Vector3 vPos, int gridX, int gridY)
    {
        isCollision = collision;
        vPosition = vPos;
        iGridX = gridX;
        iGridY = gridY;
    }
}
