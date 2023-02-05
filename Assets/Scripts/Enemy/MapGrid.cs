using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGrid : MonoBehaviour
{
    [Header("GRID INFO")]
    [SerializeField] private LayerMask _collisionMask;
    [Range(0, 1)]
    [SerializeField] private float _nodeRadius;
    [SerializeField] Vector2 _gridSize;


    Node[,] _nodeMat;
    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;

    public List<Node> EnemyPath;

    private void Start()
    {
        _nodeDiameter = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridSize.y / _nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        _nodeMat = new Node[_gridSizeX, _gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * _gridSize.x / 2 - Vector3.up * _gridSize.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius);
                bool collision = true;

                if (Physics2D.OverlapCircle(worldPoint, _nodeRadius, _collisionMask))
                {
                    collision = false;
                }

                _nodeMat[x, y] = new Node(collision, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node neighborNode)
    {
        List<Node> neighborNodeList = new List<Node>();
        int checkX;
        int checkY;

        checkX = neighborNode.iGridX + 1;
        checkY = neighborNode.iGridY;
        if ((checkX >= 0) && (checkX < _gridSizeX))
        {
            if ((checkY >= 0) && (checkY < _gridSizeY))
            {
                neighborNodeList.Add(_nodeMat[checkX, checkY]);
            }
        }

        checkX = neighborNode.iGridX - 1;
        checkY = neighborNode.iGridY;
        if ((checkX >= 0) && (checkX < _gridSizeX))
        {
            if ((checkY >= 0) && (checkY < _gridSizeY))
            {
                neighborNodeList.Add(_nodeMat[checkX, checkY]);
            }
        }

        checkX = neighborNode.iGridX;
        checkY = neighborNode.iGridY + 1;
        if ((checkX >= 0) && (checkX < _gridSizeX))
        {
            if ((checkY >= 0) && (checkY < _gridSizeY))
            {
                neighborNodeList.Add(_nodeMat[checkX, checkY]);
            }
        }

        checkX = neighborNode.iGridX;
        checkY = neighborNode.iGridY - 1;
        if ((checkX >= 0) && (checkX < _gridSizeX))
        {
            if ((checkY >= 0) && (checkY < _gridSizeY))
            {
                neighborNodeList.Add(_nodeMat[checkX, checkY]);
            }
        }

        return neighborNodeList;
    }

    public Node FromWorldPoint(Vector3 worldPos)
    {
        float xPos = ((worldPos.x + _gridSize.x / 2) / _gridSize.x);
        float yPos = ((worldPos.y + _gridSize.y / 2) / _gridSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * yPos);

        return _nodeMat[x, y];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridSize.x, _gridSize.y, 1));

        if (_nodeMat != null)
        {
            foreach (Node n in _nodeMat)
            {
                if (n.isCollision)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                if (EnemyPath != null)
                {
                    if (EnemyPath.Contains(n))
                    {
                        Gizmos.color = Color.green;
                    }
                }

                Gizmos.DrawWireCube(n.vPosition, Vector3.one * (_nodeDiameter));
            }
        }
    }
#endif
 }
