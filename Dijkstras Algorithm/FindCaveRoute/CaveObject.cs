using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace FindCaveRoute
{
    class CaveObject
    {
        int cavenumber;
        Vector2 pos;
        float enddistance;
        float minindex;
        double? _cost = 0;
        List<CaveObject> con = new List<CaveObject>();
        bool nodesvisited;

        //Cave Number
        public int CaveNumber
        {
            get { return cavenumber; }
            set { cavenumber = value; }
        }

        //Caves Position (x and y coords)
        public Vector2 CavPos
        {
            get { return pos; }
            set { pos = value; }
        }

        //Visited Nodes
        public bool NodesVisited
        {
            get { return nodesvisited; }
            set { nodesvisited = value; }
        }

        //Cost
        public double? Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        //Conections
        public List<CaveObject> Connections
        {
            get { return con; }
            set { con = value; }
        }

        //Distance to end
        public float EndDistance
        {
            get { return enddistance; }
            set { enddistance = value; }
        }

        //Min Distance
        public float MinIndex
        {
            get { return minindex; }
            set { minindex = value; }
        }

        
        //CaveObject 
        public CaveObject(int x, int y)
        {
            nodesvisited = false;
            pos = new Vector2(x, y);
        }

        //The distance between any two caverns is the Euclidean distance between the two coordinates: 

        public int EuclideanDist(CaveObject dist)
        {
            float x = (float)Math.Pow(dist.CavPos.X - pos.X, 2);
            float y = (float)Math.Pow(dist.CavPos.Y - pos.Y, 2);
            return (int)Math.Round(Math.Sqrt(x + y));
        }



        // A utility function to find the 
        // vertex with minimum distance 
        // value, from the set of vertices 
    

        public int MinDist(int[] dist, bool[] shortestpathtreeset, int cavecount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < cavecount; ++v)
            {
                if (shortestpathtreeset[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    minIndex = v;
                    minindex = minIndex;
                }
            }

            return minIndex;
        }

    }
}

