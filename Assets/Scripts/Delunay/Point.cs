﻿using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class PathPoint
    {
        public List<PathPoint> unvisitedNeighbors = new List<PathPoint>();
        public PathPoint closest = null;
        public float closestDistance;
        public List<PathPoint> connectedNeighbors = new List<PathPoint>();
        public Vector2Int pos { get; set; }

        public PathPoint(Vector2Int p)
        {
            pos = p;    
        }

        internal float ManhattanDistance(PathPoint neighbor)
        {
            if(neighbor==null) Debug.Log("Trying to Find Manhattan for null neighbor");
            return Vector2.Distance(pos,neighbor.pos);
        }        

        public bool IsVisited()
        {
            return (connectedNeighbors.Count>0);
        }
        
        public bool HasUnvisitedNeighbors()
        {
            return (unvisitedNeighbors.Count>0);
        }
        
        public bool IsEqual(PathPoint other)
        {
            return pos.Equals(other.pos);
        }

            /*
        internal PathPoint ClosestUnvisitedNeigbor()
        {
            Debug.Log("Unvisited neighbors: " + HasUnvisitedNeighbors());




			if (!HasUnvisitedNeighbors()) { Debug.Log("No Available unvisited neighbors"); return null; }
            PathPoint closest = null;
            float minDistance = 1000f;
            foreach (var neighbor in unvisitedNeighbors)
            {
				//Debug.Log("PosC neigbor:"+neighbor);

				if (!neighbor.IsVisited() && ManhattanDistance(neighbor) < minDistance)
                {
                    //Debug.Log("PosA");
                    closest = neighbor;
                    minDistance = ManhattanDistance(neighbor);
				}
            }
            return closest;
        }
            */

        internal static PathPoint FindPointWithClosestNeighbor(List<PathPoint> connectedPoints)
        {
            PathPoint closest = null;
            //PathPoint closestNeighbor;
			float minDistance = 1000f;

			Debug.Log("Finding Point With Closest Neighbor START: ");

            foreach(PathPoint point in connectedPoints)
            {
                if(point.closest != null && point.closestDistance<minDistance) closest = point;
            }
            return closest;


				/*
				for (int i = 0; i < connectedPoints.Count; i++)
				{
					PathPoint pathPoint = connectedPoints[i];
					Debug.Log("Testing New PathPoint: " + i + "/" + connectedPoints.Count);

					closestNeighbor = pathPoint.ClosestUnvisitedNeigbor();
					if (closestNeighbor == null) { Debug.Log("Closest neighbor: NONE Stored:"+closest); continue; }
					else Debug.Log("Closest Neighbor: " + closestNeighbor + "Stored:" + closest);
					float distance = pathPoint.ManhattanDistance(closestNeighbor);
					if (distance < minDistance)
					{
						Debug.Log("Point found in Connected Points, with closest legit neighbor at distance: "+distance);
						closest = pathPoint;
						minDistance = distance;
					}
				}
				Debug.Log("Finding Point With Closest Neighbor COMPLETE: "+closest);
				return closest;
				*/
			}

        public void SetClosest()
        {
            PathPoint closeNeighbor = null;
            float distance = 1000f;
            foreach (var unvisited in unvisitedNeighbors)
            {
                float newDistance = ManhattanDistance(unvisited);
				if (newDistance < distance && !unvisited.IsVisited())
                {
                    closeNeighbor = unvisited;
                    distance = newDistance;
                }
            }
            if (closeNeighbor != null)
            {
                closest = closeNeighbor;
                closestDistance = distance;
            }
        }

        internal void ConnectNeighbors(PathPoint other)
        {
            Debug.Log("Connecting point: ("+this.pos+") to other: ("+other.pos+")");
			connectedNeighbors.Add(other);
			unvisitedNeighbors.Remove(other);
			other.connectedNeighbors.Add(this);
			other.unvisitedNeighbors.Remove(this);
            if (other.unvisitedNeighbors.Contains(this)) Debug.LogWarning("Other was not able to remove this PathPoint");
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