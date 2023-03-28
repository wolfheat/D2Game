using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class PathPoint : Point
    {
        public List<PathPoint> pathPoints = new List<PathPoint>();

        public PathPoint(Point p) : base(p.X, p.Y)
        {
        }
        public PathPoint(double x, double y) : base(x, y)
        {
        }

        internal float ManhattanDistance(PathPoint neighbor)
        {
            return Mathf.Abs((float)X-(float)neighbor.X)+Mathf.Abs((float)Y-(float)neighbor.Y);
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
            return $"{nameof(Point)} {_instanceId} {X:0.##}@{Y:0.##}";
        }
    }
}