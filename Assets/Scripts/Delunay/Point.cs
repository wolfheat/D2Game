using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class PathPoint
    {
        public List<PathPoint> unvisitedNeighbors = new List<PathPoint>();
        public List<PathPoint> connectedNeighbors = new List<PathPoint>();
        public Vector2Int pos { get; set; }

        public PathPoint(Vector2Int p)
        {
            pos = p;    
        }

        internal float ManhattanDistance(PathPoint neighbor)
        {
            Debug.Log("Trying to Find Manhattan for "+neighbor+" and "+this);
            return Vector2.Distance(pos,neighbor.pos);
        }        

        public bool IsEqual(PathPoint other)
        {
            return pos.Equals(other.pos);
        }

        internal PathPoint ClosestUnvisitedNeigbor()
        {
            if (unvisitedNeighbors.Count == 0) { Debug.Log("unvisitedNeighbors.Count == 0"); return null; }
            PathPoint closest = null;
            float minDistance = 1000f;
            foreach (var neighbor in unvisitedNeighbors)
            {
				//Debug.Log("PosC neigbor:"+neighbor);

				if (neighbor.connectedNeighbors.Count==0 && ManhattanDistance(neighbor) < minDistance)
                {
                    //Debug.Log("PosA");
                    closest = neighbor;
                    minDistance = ManhattanDistance(neighbor);
				}
            }
            return closest;
        }

        internal static PathPoint FindPointWithClosestNeighbor(List<PathPoint> connectedPoints)
        {
            PathPoint closest = null;
            PathPoint closestNeighbor;
			float minDistance = 1000f;

            for (int i = 0; i < connectedPoints.Count; i++)
            {
                PathPoint pathPoint = connectedPoints[i];
                closestNeighbor = pathPoint.ClosestUnvisitedNeigbor();
                float distance = pathPoint.ManhattanDistance(closestNeighbor);
                if (distance < minDistance)
                {
				    Debug.Log("Point found in Connected Points, with closest legit neighbor at distance: "+distance);
                    closest = pathPoint;
                    minDistance = distance;
                }
            }
			Debug.Log("PosB END: "+closest);
            return closest;
		}

        internal void ConnectNeighbors(PathPoint other)
        {
            Debug.Log("Connecting this: "+this+" to other: "+other);
			connectedNeighbors.Add(other);
			unvisitedNeighbors.Remove(other);
			other.connectedNeighbors.Add(this);
			other.unvisitedNeighbors.Remove(this);
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