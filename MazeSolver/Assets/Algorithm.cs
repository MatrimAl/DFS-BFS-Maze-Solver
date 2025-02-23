using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build;
using UnityEngine;
using Debug = UnityEngine.Debug;

public  class Algorithm 
        {
          


                    static void AddEdge(List<List<int>> adj, int s, int t)
                    {
                        Debug.Log(s+ " "+  t);
                        adj[s].Add(t);
                        adj[t].Add(s);
                    }

                    public static (int row, int col) start, end;
                    public static int[,] matrix;
                    public static int source;
                    public static int target;
                    public static List<List<int>> adj;
                    public static int num;
                     public static int[,] generateMatrix()
                    {
                            string fileName = "maze.txt";  // you can chane here for the try another maze. For example you can write here maze2 by default

                            string[] lines = File.ReadAllLines(fileName);

                            int rows = (lines.Length - 2) / 2; // We find how many lines the labyrinth has.
                            int cols = (lines[0].Length + 1) / 2; // We find how many columns the labyrinth has.

                            //   maze = new char[rows * 2 + 1, cols * 2 + 1];


                             matrix = new int[rows * 2 + 1, (cols * 2) - 1];

                            // Initially fill the matrix with zeros

                            num = 1;

                            for (int i = 1; i < rows * 2; i++)
                            {
                                string line = lines[i].PadRight(cols * 2);
                                bool isBlockRow = lines[i][0] == '|';
                                for (int j = 0; j < (cols * 2) - 1; j++)
                                {
                                    bool isWall = line[j] == '|' ||
                                                  (i > 0 && lines[i - 1][j] == '|') ||
                                                  (i + 1 < line.Length && lines[i + 1][j] == '|') ||
                                                  (j + 1 < line.Length && line[j + 1] == '-') ||
                                                  (j - 1 >= 0 && line[j - 1] == '-') ||
                                                  (line[j] == '-');

                                    if (isWall)
                                    {
                                        matrix[i, j] = 0;
                                        Console.Write(matrix[i, j]);
                                    }
                                    else if (j % 2 == 1 && isBlockRow)
                                    {
                                        matrix[i, j] = num;
                                        Console.Write(matrix[i, j]);
                                        num++;
                                    }
                                    else if (j % 2 == 0 || (j % 2 == 1 && !isBlockRow))
                                    {
                                        matrix[i, j] = -1;
                                        Console.Write(matrix[i, j]);
                                    }

                                }
                                Console.WriteLine("");
                                ;
                              }

                                PrintMatrix(matrix);



                                string[] startCoords = lines[^2].Split(',');
                                start = (int.Parse(startCoords[0]) * 2 + 1, int.Parse(startCoords[1]) * 2 + 1);

                                string[] endCoords = lines[^1].Split(',');
                                end = (int.Parse(endCoords[0]) * 2 + 1, int.Parse(endCoords[1]) * 2 + 1);


                                source = matrix[start.row, start.col]; // Starting vertex for DFS
                                target = matrix[end.row, end.col];
                                MatrixToGraph();
                                return matrix;
                       }

                    public static void MatrixToGraph()
                    {

                        adj = new List<List<int>>(num);
                        for (int i = 0; i < num; i++)
                        {
                            adj.Add(new List<int>());
                        }

                        for (int i = 0; i < matrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                if (matrix[i, j] != 0 && matrix[i, j] != -2 && matrix[i, j] != -1 )
                                {

                                    if (matrix[i, j + 1] != 0 && matrix[i, j + 1] != -2 ) // right  
                                    {
                                        matrix[i, j + 1] = -2;
                                        AddEdge(adj, matrix[i, j], matrix[i, j + 2]);
                                        //recusive
                                    }
                                    if (matrix[i, j - 1] != 0 && matrix[i, j - 1] != -2) // left  
                                    {
                                        matrix[i, j - 1] = -2;
                                        AddEdge(adj, matrix[i, j], matrix[i, j - 2]);

                                        //recusive
                                    }
                                    if (matrix[i + 1, j] != 0 && matrix[i + 1, j] != -2) // up  
                                    {
                                        matrix[i + 1, j] = -2;
                                        AddEdge(adj, matrix[i, j], matrix[i + 2, j]);
                                        //recusive
                                    }
                                    if (matrix[i - 1, j] != 0 && matrix[i - 1, j] != -2) // down
                                    {
                                        matrix[i - 1, j] = -2;
                                        AddEdge(adj, matrix[i, j], matrix[i - 2, j]);

                                        //recusive
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                        PrintAdjacencyList(adj);


                      
                    }
          
                    static void PrintMatrix(int[,] matrix)
                    {
                        for (int i = 0; i < matrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                Console.Write(matrix[i, j] + " ");
                            }
                            Console.WriteLine();
                        }
                    }

            

            static void PrintAdjacencyList(List<List<int>> adj)
                    {
                        for (int i = 1; i < adj.Count; i++)
                        {
                            Console.Write($"Node {i}: ");
                            Console.WriteLine(string.Join(", ", adj[i]));
                        }
                    }

                }


