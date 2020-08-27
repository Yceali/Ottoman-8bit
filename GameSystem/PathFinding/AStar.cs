using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { START, GOAL , WATER, GRASS, PATH}
public class AStar : MonoBehaviour
{

    [SerializeField]
    private LayerMask layerMask;


    [SerializeField]
    private Tilemap tilemap;

    private Vector3Int startPos, goalPos;
    private Node current;
    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Stack<Vector3> path;

    private List<Vector3Int> waterTiles = new List<Vector3Int>();

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private static HashSet<Vector3Int> noDiagonalTiles;


    public Tilemap MyTilemap { get => tilemap; set => tilemap = value; }
    public static HashSet<Vector3Int> MyNoDiagonalTiles { get => noDiagonalTiles; }

    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = tilemap.WorldToCell(start);
        goalPos = tilemap.WorldToCell(goal);

        current = GetNode(startPos);

        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }
        allNodes.Clear();

        openList.Add(current);
        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);            
        }      

        if(path != null)
        {
            return path;
        }

        return null;

        //AstarDebugger.MyInstance.CreateTiles(openList,closedList,allNodes,startPos, goalPos, path);
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];

            if (!ConnectedDiagonaly(current,neighbor))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbors[i].Position, current.Position);

            //if (gScore == 14 && MyNoDiagonalTiles.Contains(neighbor.Position) && MyNoDiagonalTiles.Contains(current.Position))
            //{
            //    continue;
            //}

            if (openList.Contains(neighbor))
            {
                if (current.G + gScore < neighbor.G)
                {
                    CalcValues(current, neighbor, gScore);
                }
            }
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);
                openList.Add(neighbor);
            }

        }
    }

    private List<Node> FindNeighbors(Vector3Int parentPos)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPos.x - x, parentPos.y - y, parentPos.z);

                if (y !=0 || x!=0)
                {
                    if (neighborPos != startPos && !GameManager.MyInstance.Blocked.Contains(neighborPos))
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                    
                }
            }
        }

        return neighbors;
    }

    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = (Math.Abs(neighbor.Position.x - goalPos.x) + Math.Abs(neighbor.Position.y - goalPos.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current) 
    {
        int gScore = 0;
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Math.Abs(x-y)% 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile( ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
   

    private bool ConnectedDiagonaly(Node currentNode, Node neighbor)
    {
        Vector3Int direction = currentNode.Position - neighbor.Position;
        Vector3Int first = new Vector3Int(current.Position.x + (direction.x * -1), currentNode.Position.y, currentNode.Position.z);
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direction.y * -1), current.Position.z);

        if (waterTiles.Contains(first) || waterTiles.Contains(second))
        {
            return false;
        }

        return true;
    }

    private Stack<Vector3> GeneratePath(Node current)
    {
        if (current.Position == goalPos)
        {
            Stack<Vector3> finalPath = new Stack<Vector3>();

            while (current != null)
            {
                finalPath.Push(current.Position);
                current = current.Parent;
            }

            return finalPath;
        }

        return null;
    }
}
