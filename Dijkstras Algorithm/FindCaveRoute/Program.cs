//Developed by Arran Smedley //40406581

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace FindCaveRoute

{
    class Program
    {
      

        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                //Taking in input from the command line and using a text writer for the output
                using (TextWriter tw = new StreamWriter(string.Format("{0}.csn", args)))
                {
                    //Creating a cavobject path
                    List<CaveObject> path = new List<CaveObject>();
                    
                    //Applying Dijkstras Algorithm
                    DijkstraAlgo(args, path);

                    //Output
                    string caveroutefound = "Caveroute: ";

                    foreach (CaveObject cav in path)
                    {
                        if (path.Count() == 2)
                        {
                            //If the number of paths is equal to 2 no path found
                            caveroutefound = "No Valid Route Found";
                        }
                        else
                        {
                            //Output route if applicable
                            caveroutefound += string.Format("{0} ", cav.CaveNumber + 1);
                        }
                        
                    }

                 


                    tw.Write(caveroutefound.ToString());

                    tw.Close();
                }
            }
        }


      static void DijkstraAlgo(string[] args, List<CaveObject> path)
        {
            //if arguements are null then file isnt found 
            if (args == null || args.Length == 0)
            {
                throw new System.ArgumentException("File Not Found");
            }

            //File location set to arguements
            string Location = args[0];

            //Create cave object list
            List<CaveObject> list = new List<CaveObject>();
    
            //File path is file name.cav
            string filepath = string.Format("{0}.cav", Location);

            string phrase = File.ReadAllText(filepath);

            string[] nums = phrase.Split(',');

            //raw data is the file being read in
            int[] rawData = new int[nums.Length];

            for (int i = 0; i < nums.Length; i++)
            {
                //converting the raw file to numbers
                rawData[i] = Convert.ToInt32(nums[i]);
            }

            //Number of caves equals the first number within the file
            int cavcount = rawData[0];

            //Number of sets of co-ordinates for the vec value 
            int ve = 0;

            //Vector for x and y coords of caves
            Vector2[] vec = new Vector2[cavcount];


            //Go through the files in 2's 
            for (int i = 1; i < cavcount * 2; i += 2)
            {
                vec[ve] = new Vector2(rawData[i], rawData[i + 1]);

                ve++;
            }


            for (int i = 0; i < cavcount; i++)
            {
                //Add the coords of the files to there assigned X and Y values
                list.Add(new CaveObject((int)vec[i].X, (int)vec[i].Y));

                //Cave number is also equal to every pair of coords found
                list[i].CaveNumber = i;

            }

            //Data is equal to the total number of caves within the file
            int[] data = new int[cavcount * cavcount];

            for (int i = 0; i < data.Length; i++)
            {
                //Retrieving the connection data
                data[i] = rawData[i + (cavcount * 2) + 1];
            }

            //2D array for connection matrix and distance matrix, dimensions of number of caves by number of caves
            int[,] conMat = new int[cavcount, cavcount];
            int[,] distMat = new int[cavcount, cavcount];

            //for every x and y value equal the 2D array connection matrix to the Data
            for (int x = 0; x < cavcount; x++)
            {
                for (int y = 0; y < cavcount; y++)
                {
                    conMat[x, y] = data[y * cavcount + x];
                }
            }


            //For every connection add the connection to a list of connections
            for (int x = 0; x < cavcount; x++)
            {
                for (int y = 0; y < cavcount; y++)
                {
                    if (conMat[x, y] == 1)
                    {
                        list[x].Connections.Add(list[y]);
                    }
                }
            }

            //for every number of caves in the matrix if the connection matrix is 0 and distance is 0 do nothing else calculate distance
            for (int x = 0; x < cavcount; x++)
            {
                for (int y = 0; y < cavcount; y++)
                {
                    if (conMat[x, y] == 0)
                    {
                        distMat[x, y] = 0;
                    }
                    else
                    {
                        int dist;
                        dist = list[x].EuclideanDist(list[y]);
                        distMat[x, y] = dist;
                    }
                }
            }

            #region Dijkstras Algroithm


            //This algorithm helps to find the shortest path from a point in a graph (the source) to a destination.

            //Refernce https://www.csharpstar.com/dijkstra-algorithm-csharp/

            //node distance
            int[] distance = new int[cavcount];
            //node itself
            int[] node = new int[cavcount];
            //shortest path
            bool[] shortestPathTreeSet = new bool[cavcount];

            //for every value in the list set the distance to max value and shortest path to false
            for (int i = 0; i < list.Count; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            //Distence is equal to source
            distance[0] = 0;
            //node is equal to source
            node[0] = 0;

            //for every value in the list count - 1
            for (int count = 0; count < list.Count - 1; ++count)
            {
                //Calculate the minimum distance between nodes
                int u = list[0].MinDist(distance, shortestPathTreeSet, list.Count);
                //set the shortest path to true
                shortestPathTreeSet[u] = true;

                //for every value in list 
                for (int v = 0; v < list.Count; ++v)
                {
                    //if the shortest set is false and distance mat is true and distance is not equal to the maximum valye and distance + distance matrix is less than distance 
                    //then distance is equal to the distance + the distance matrix and the node is equal to the cave number
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(distMat[u, v]) && distance[u] != int.MaxValue && distance[u] + distMat[u, v] < distance[v])
                    {
                        distance[v] = distance[u] + distMat[u, v];
                        node[v] = list[u].CaveNumber;
                    }
                }
            }
            //Add the list to path
            path.Add(list[list.Count - 1]);

            //for every value in the list -1 add the node to path and value equals the number of node
            for (int i = list.Count - 1; i > 0;)
            {
                path.Add(list[node[i]]);
                i = node[i];
            }

            //reverse the list as it comes out with the largest value first
            path.Reverse();

            #endregion

        }
    }

}


