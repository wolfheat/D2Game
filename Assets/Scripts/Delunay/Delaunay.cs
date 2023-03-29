using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace DelaunayVoronoi
{
    public class DelaunayTriangulator
    {
        private double MaxX { get; set; }
        private double MaxY { get; set; }
        private IEnumerable<Triangle> border;
        private HashSet<Point> startPoints;

        public IEnumerable<Point> GeneratePoints(int amount, double maxX, double maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            // TODO make more beautiful
            var point0 = new Point(0, 0);
            var point1 = new Point(0, MaxY);
            var point2 = new Point(MaxX, MaxY);
            var point3 = new Point(MaxX, 0);
            var points = new List<Point>() { point0, point1, point2, point3 };
            var tri1 = new Triangle(point0, point1, point2);
            var tri2 = new Triangle(point0, point2, point3);
            border = new List<Triangle>() { tri1, tri2 };

            var random = new Random();
            for (int i = 0; i < amount - 4; i++)
            {
                var pointX = random.NextDouble() * MaxX;
                var pointY = random.NextDouble() * MaxY;
                points.Add(new Point(pointX, pointY));
            }

            return points;
        }

        public IEnumerable<Triangle> BowyerWatson(List<Vector2Int> points)
        {
            var MinX = points[0].x;
            var MinY = points[0].y;
            MaxX = points[0].x;
            MaxY = points[0].y;

            foreach (Vector2Int p in points)
            {
                if(p.x<MinX) MinX = p.x;
                if(p.x>MaxX) MaxX = p.x;
                if(p.y<MinY) MinY = p.y;
                if(p.y>MaxY) MaxY = p.y;
            }
            int margin = 10;
            MinX -= margin;
            MinY -= margin;
            MaxX += margin;
            MaxY += margin;

			var point0 = new Point(MinX, MinY);
			var point1 = new Point(MinX, MaxY);
			var point2 = new Point(MaxX, MaxY);
			var point3 = new Point(MaxX, MinY);
			var tri1 = new Triangle(point0, point1, point2);
			var tri2 = new Triangle(point0, point2, point3);
			border = new List<Triangle>() { tri1, tri2 };
			startPoints = new HashSet<Point>() { point0, point1, point2, point3 };

			IEnumerable<Point> pointEnumerable = points.Select(p => new Point(p.x, p.y)).ToList();
            return BowyerWatson(pointEnumerable);
        }

        public static List<PathPoint> FindMinimumPath(IEnumerable<Triangle> triangles)
        {
            
            List<PathPoint> points = ConvertToPathPoints(triangles);


            List<PathPoint> unConnectedPoints = new List<PathPoint>(points);
            List<PathPoint> connectedPoints = new List<PathPoint>();
            
            PathPoint point = unConnectedPoints.First();
			connectedPoints.Add(point);
            unConnectedPoints.Remove(point);
            int countTimer = 0;
			while (unConnectedPoints.Count > 0 && countTimer < 10)
            {
                Debug.Log("unConnectedPoints.Count: "+ unConnectedPoints.Count);
                countTimer++;
                //

                PathPoint closest = point.ClosestUnvisitedNeigbor();

                point.connectedNeighbors.Add(closest);
                point.unvisitedNeighbors.Remove(closest);
                closest.connectedNeighbors.Add(point);
                closest.unvisitedNeighbors.Remove(point);
                unConnectedPoints.Remove(closest);
                connectedPoints.Add(closest);

                if(unConnectedPoints.Count>0) point = PathPoint.FindPointWithClosestNeighbor(connectedPoints);
			}
            if (countTimer == 10) Debug.LogError("Stuck in While loop, exiting.");
            return points;
		}
        
        public static List<PathPoint> ConvertToPathPoints(IEnumerable<Triangle> triangles)
        {
            List<VectorTriangle> vectorTriangles = new List<VectorTriangle>();
            foreach (var triangle in triangles)
            {
                VectorTriangle vectorTriangle = new VectorTriangle(triangle.ReturnAsVector2Int());
                vectorTriangles.Add(vectorTriangle);
            }

			List<PathPoint> pathPoints = new List<PathPoint>();

			foreach (VectorTriangle triangle in vectorTriangles)
			{
				PathPoint[] points = new PathPoint[3];

				for (int i = 0; i < 3; i++)
				{
					Vector2Int vertex = triangle.Vertices[i];
					points[i] = new PathPoint(vertex);
				}
				for (int i = 0; i < 3; i++)
				{
                    points[i].unvisitedNeighbors.Add(points[(i + 1) % 3]);
                    points[i].unvisitedNeighbors.Add(points[(i + 2) % 3]);
				}
                pathPoints = AddOrCombineRange(pathPoints, points);
			}
			return pathPoints;
		}

        private static List<PathPoint> AddOrCombineRange(List<PathPoint> storedPoints, PathPoint[] newPoints)
        {
            foreach (var newPoint in newPoints)
            {
                bool exists = false;
                foreach(var storedPoint in storedPoints)
                {
                	if (storedPoint.IsEqual(newPoint))
                    {
                        foreach (var newNeighbor in newPoint.unvisitedNeighbors)
                        {
                            bool neighborexist = false;
                            foreach (var storedNeighbor in storedPoint.unvisitedNeighbors)
                            {
                                if (storedNeighbor.IsEqual(newNeighbor))
                                {
                                    neighborexist = true;
                                }
                            }
                            if(!neighborexist) storedPoint.unvisitedNeighbors.Add(newNeighbor);
                        }
                        exists = true;
                    }
                }
                if(!exists) storedPoints.Add(newPoint);
            }
            return storedPoints;
        }

        public IEnumerable<Triangle> BowyerWatson(IEnumerable<Point> points)
        {
            //var supraTriangle = GenerateSupraTriangle();
            var triangulation = new HashSet<Triangle>(border);

            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles);

                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }
                triangulation.RemoveWhere(o => badTriangles.Contains(o));

                foreach (var edge in polygon.Where(possibleEdge => possibleEdge.Point1 != point && possibleEdge.Point2 != point))
                {
                    var triangle = new Triangle(point, edge.Point1, edge.Point2);
                    triangulation.Add(triangle);
                }
            }

            List < Triangle > startTriangles = new List<Triangle>();
			startTriangles = triangulation.Where(triangle => triangle.Vertices.Any(point => startPoints.Contains(point))).ToList();            
			triangulation.ExceptWith(startTriangles);

            return triangulation;
        }

        private List<Edge> FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = new List<Edge>();
            foreach (var triangle in badTriangles)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }

        private Triangle GenerateSupraTriangle()
        {
            //   1  -> maxX
            //  / \
            // 2---3
            // |
            // v maxY
            var margin = 500;
            var point1 = new Point(0.5 * MaxX, -2 * MaxX - margin);
            var point2 = new Point(-2 * MaxY - margin, 2 * MaxY + margin);
            var point3 = new Point(2 * MaxX + MaxY + margin, 2 * MaxY + margin);
            return new Triangle(point1, point2, point3);
        }

        private ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => o.IsPointInsideCircumcircle(point));
            return new HashSet<Triangle>(badTriangles);
        }
    }
}