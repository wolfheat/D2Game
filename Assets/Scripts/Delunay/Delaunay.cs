using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        public static IEnumerable<PathPoint> FindMinimumPath(IEnumerable<Triangle> triangles)
        {
            
            List<PathPoint> points = ConvertToPathPoints(triangles);

            string print = "Points: ";

            foreach (var point in points)
            {
                print += "(";
                foreach (var neighbor in point.pathPoints)
                {
                    float distance = point.ManhattanDistance(neighbor);
                    print += distance + ", ";

                }
                print += ")";
            }

            Debug.Log(print);

            return points;
		}
        
        public static List<PathPoint> ConvertToPathPoints(IEnumerable<Triangle> triangles)
        {
			/*
            List<PathPoint> points = new List<PathPoint>();
			HashSet<Point> uniquePoints = new HashSet<Point>();

			foreach (var tri in triangles)
            {
                List<PathPoint> pathPoints = new List<PathPoint>();
				for (int i = 0; i < 3; i++)
                {
                    PathPoint pathPoint = new PathPoint(tri.Vertices[i]); 
                    pathPoints.Add(pathPoint);

                    if(!points.Contains(pathPoint)) points.Add(pathPoint);
				}
                for (int i = 0; i < 3; i++)
                {
                    pathPoints[i].pathPoints.Add(pathPoints[(i+1)%3]);
                    pathPoints[i].pathPoints.Add(pathPoints[(i+4)%3]);
                }
            }
            return points;
            */
			List<PathPoint> pathPoints = new List<PathPoint>();
			HashSet<Point> uniquePoints = new HashSet<Point>();

			foreach (Triangle triangle in triangles)
			{
				PathPoint[] points = new PathPoint[3];

				for (int i = 0; i < 3; i++)
				{
					Point vertex = triangle.Vertices[i];
					PathPoint point;
                    if(vertex == null) Debug.Log("Vertex is NUll");

					if (uniquePoints.Contains(vertex))
					{
						Debug.Log($"vertex: {vertex}, pathPoints: {string.Join(", ", pathPoints)}");
						point = pathPoints.Find(p => p.Equals(vertex));
						Debug.Log($"point: {point}");

						Debug.Log("Point: "+i+" This vertex already exists: "+point);
					}
					else
					{
						point = new PathPoint(vertex);
						uniquePoints.Add(vertex);
						pathPoints.Add(point);
                        Debug.Log("vertex("+i+"):"+vertex);
					}
					points[i] = point;
				}
				for (int i = 0; i < 3; i++)
				{
					PathPoint point1 = points[i];
					PathPoint point2 = points[(i + 1) % 3];

                    Debug.Log("point1: "+point1+ " point2: "+point2);
					if (!point1.pathPoints.Contains(point2))
					{
						point1.pathPoints.Add(point2);
						point2.pathPoints.Add(point1);
					}
				}
			}

			return pathPoints;
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