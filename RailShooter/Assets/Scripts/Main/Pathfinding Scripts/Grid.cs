using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
    //
    public bool DisplayGridGizmos;
    public LayerMask unwalkableMask;
    LayerMask walkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    public PenaltyType[] regions;
    Node[,,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    //
    //
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY * gridSizeZ;
        }
    }
    //
    void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        //
        foreach (PenaltyType region in regions)
        {
            walkableMask.value = walkableMask |= region.penaltyMask.value;
            walkableRegionsDictionary.Add(Mathf.RoundToInt(Mathf.Log(region.penaltyMask.value, 2)), region.penaltyValue);
        }
        //
        CreateGrid();
    }
    //
    void CreateGrid()
    {
        //
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2 - Vector3.up * gridWorldSize.y / 2;
        //
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    //
                    int movementPenalty= 0;
                    //
                    if (walkable)
                    {
                        Ray ray = new Ray(worldPoint + Vector3.up * 1000, Vector3.down);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 2000, walkableMask))
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }
                    //
                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z, movementPenalty);
                }
            }
        }
    }
    //
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        //
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;
                    //
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }
        return neighbours;
    }
    //
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        float percentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);
        //
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        //
        return grid[x, y, z];
    }
    //
    void OnDrawGizmos () {
        //
        //if (Game_Manager.instance.debugging == Game_Manager.Debug.True && Game_Manager.instance.debugGrid == true)
        //{
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
            //
            if (grid != null && DisplayGridGizmos == true)
            {
                if (grid != null)
                {
                    foreach (Node n in grid)
                    {
                        Gizmos.color = (n.walkable) ? new Color(1, 1, 1, 0.05f) : Color.red;
                        Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                    }
                }
            }
        //}
    }
    [System.Serializable]
    public class PenaltyType
    {
        public LayerMask penaltyMask;
        public int penaltyValue;
    }
}
