using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;
    int[,] matrix = Algorithm.generateMatrix();
    static Stopwatch stopwatch = new Stopwatch();



    void Start()
    {
        _mazeWidth =  matrix.GetLength(0); 
        _mazeDepth = matrix.GetLength(1);
        int tempNum_Z = 0;
        int tempNum_X = 0;
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 1; x < _mazeWidth; x+=2)
        {
            for (int z = 1; z < _mazeDepth; z+=2)
            {
                    if (matrix[x,z] != 0){
                        Debug.Log(matrix[x,z]);
                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(tempNum_X, 0, tempNum_Z), Quaternion.identity);
                    _mazeGrid[x, z].CellText = matrix[x,z].ToString();
                    tempNum_Z++;
                }   
            }
            tempNum_Z = 0;
            tempNum_X++;
        }

        for (int x = 1; x < _mazeWidth+2; x+=2)
        {
            for (int z = 1; z < _mazeDepth+2; z+=2)
            {
                        // Bound check
                    if (z  >= matrix.GetLength(1) || x  >= matrix.GetLength(0))
                     {
                        Debug.Log($"bound problm! x={x}, z={z}");
                        continue;
                    }

                    

        ///// Horizontal wall cleaning
                    if (matrix[x, z + 1] == -2)
                        {
                        matrix[x, z + 1] = -1;
                        Debug.Log("horizontal worked!!");

                        _mazeGrid[x, z].Block();
                        _mazeGrid[x, z+2].Block();

                        _mazeGrid[x, z].ClearFrontWall();
                        _mazeGrid[x, z+2].ClearBackWall();
                        }
            

        ///// Vertical wall cleaning
                        if (matrix[x + 1, z] == -2 )
                       {
                              if (_mazeGrid[x, z] == null){
                                 Debug.Log("NULL!" + matrix[x,z]);
                           }
                                 matrix[x + 1, z] = -1;

                                _mazeGrid[x, z].Block();
                                _mazeGrid[x+2, z].Block();

                            
                                _mazeGrid[x, z].ClearRightWall();

                                _mazeGrid[x+2 , z].ClearLeftWall();
                         }

            }
        }





        StartCoroutine(DFSWithVisualization(Algorithm.source, Algorithm.target));
    }
    public IEnumerator DFSWithVisualization(int source, int target)
    {
        bool[] visited = new bool[Algorithm.num];
        Stack<int> stack = new Stack<int>();
        Dictionary<int, int> parent = new Dictionary<int, int>();

        stack.Push(source);
        visited[source] = true;

        while (stack.Count > 0)
        {
            int current = stack.Pop();

            
            (int x, int z) = GetCoordinates(current);
            _mazeGrid[x, z].SetActiveBottom(); 
            _mazeGrid[x, z].CellText = "Visited"; 
            yield return new WaitForSeconds(0.5f); 

            if (current == target)
            {
                Debug.Log("Target found!");
                yield break; 
            }

            foreach (int neighbor in Algorithm.adj[current])
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    stack.Push(neighbor);
                    parent[neighbor] = current;
                }
            }
        }

        Debug.Log("No path found."); 
    }

    public IEnumerator BFSWithVisualization(int source, int target)
    {
        Queue<int> queue = new Queue<int>();
        bool[] visited = new bool[Algorithm.adj.Count];
        Dictionary<int, int> parent = new Dictionary<int, int>();

        queue.Enqueue(source);
        visited[source] = true;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            
            (int x, int z) = GetCoordinates(current);
            _mazeGrid[x, z].SetActiveBottom(); 
            _mazeGrid[x, z].CellText = "Visited"; 
            yield return new WaitForSeconds(0.5f); 

            if (current == target)
            {
                Debug.Log("Target found!");
                yield break; 
            }

            foreach (int neighbor in Algorithm.adj[current])
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                    parent[neighbor] = current;
                }
            }
        }

        Debug.Log("No path found."); 
    } 
    private (int x, int z) GetCoordinates(int node)
    {
        for (int i = 0; i < Algorithm.matrix.GetLength(0); i++)
        {
            for (int j = 0; j < Algorithm.matrix.GetLength(1); j++)
            {
                if (Algorithm.matrix[i, j] == node)
                    return (i, j);
            }
        }
        throw new Exception("Node not found in matrix.");
    }

    public void PrintPath(List<int> path)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int k = 0; k < path.Count; k++)
                {
                    if (matrix[i, j] == path[k])
                    {
                        _mazeGrid[i, j].SetActiveBottom();                       
                    }
                }
            }
            Console.WriteLine();
        }
    }




}
