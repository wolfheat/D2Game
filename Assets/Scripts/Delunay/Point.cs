using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class PathPoint
    {
        public List<PathPoint> neighbors = new List<PathPoint>();
        public Vector2Int pos { get; set; }

        public PathPoint(Vector2Int p)
        {
            pos = p;    
        }

        internal float ManhattanDistance(PathPoint neighbor)
        {
            return Vector2.Distance(pos,neighbor.pos);
        }        

        public bool IsEqual(PathPoint other)
        {
            Debug.Log("Check if equal: "+pos.Equals(other.pos));
            return pos.Equals(other.pos);
        }

        internal PathPoint ClosestNeigbor()
        {
            PathPoint closest = neighbors[0];
            float minDistance = ManhattanDistance(closest);
            foreach (var neighbor in neighbors)
            {
                if(ManhattanDistance(neighbor) < minDistance)
                {
                    closest = neighbor;
                    minDistance = ManhattanDistance(neighbor);
				}
            }
            return closest;
        }
    }

    public class Point
    {
        /// <summary>
        /// Used only for generating a unique ID for each instance of this class that gets generated
        /// </summary>
        private static int _counter;

        /// <summary>
        /// Used for identifying an instance of a class; can be useful in troubleshooting when geometry goes weird
        /// (e.g. when trying to identify when Triangle objects are being created with the same Point object twice)
        /// </summary>
        private readonly int _instanceId = _counter++;

        public double X { get; }
        public double Y { get; }
        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            // Simple way of seeing what's going on in the debugger when investigating weirdness
            //return $"{nameof(Point)} {_instanceId} {X:0.##}@{Y:0.##}";
            return "("+X+","+Y+")";
        }
    }
}