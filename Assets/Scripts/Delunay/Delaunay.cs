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

        public static IEnumerable<Triangle> GenerateBowyerWatsonFromList(List<Vector2> points)
        {
            DelaunayTriangulator triangulator = new DelaunayTriangulator();
            return triangulator.BowyerWatson(points);
        }
        public IEnumerable<Triangle> BowyerWatson(List<Vector2> points)
        {
            var MinX = points[0].x;
            var MinY = points[0].y;
            MaxX = points[0].x;
            MaxY = points[0].y;

            foreach (Vector2 p in points)
            {
                if(p.x<MinX) MinX = p.x;
                if(p.x>MaxX) MaxX = p.x;
                if(p.y<MinY) MinY = p.y;
                if(p.y>MaxY) MaxY = p.y;
            }
            int margin = 100;
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

        private ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => o.IsPointInsideCircumcircle(point));
            return new HashSet<Triangle>(badTriangles);
        }

		internal static Dictionary<Point, List<Point>> FindMinimumPath(IEnumerable<Triangle> triangles)
		{
            // Have triangles
            // Make Dictionary with a list

            Dictionary<Point, List<Point>> pointDictionary = new Dictionary<Point, List<Point>>(new PointEqualityComparer());
            Dictionary<Point, List<Point>> connectedDictionary = new Dictionary<Point, List<Point>>(new PointEqualityComparer());

            foreach (var triangle in triangles)
            {
                for (int i = 0; i < triangle.Vertices.Length; i++)
                {
                    Point key = triangle.Vertices[i];

					if (!pointDictionary.ContainsKey(key)) pointDictionary.Add(key, new List<Point>());
					if(!((List<Point>)pointDictionary[key]).Contains(triangle.Vertices[(i + 1) % 3], new PointEqualityComparer())) pointDictionary[key].Add(triangle.Vertices[(i+1)%3]);
					if(!((List<Point>)pointDictionary[key]).Contains(triangle.Vertices[(i + 2) % 3], new PointEqualityComparer())) pointDictionary[key].Add(triangle.Vertices[(i+2)%3]);                    
                }
            }

			//PrintDictionary(pointDictionary);


			// Get random Point And Add it as connected
			Point start = pointDictionary.FirstOrDefault().Key;
            connectedDictionary.Add(start, new List<Point>());

            Debug.Log("STARTING COMMECTION FROM "+start);

            //pointDictionary.Remove(start);

            /*
            // Remove start Position for all connected Points
            foreach (var connection in pointDictionary[start])
            {
                pointDictionary[connection].Remove(start);
            }
            // Remove Start Position from Dictionary
            pointDictionary.Remove(start);
            */

            int emergencyCounter = 0;

			while (connectedDictionary.Count < pointDictionary.Count && emergencyCounter < 100)
            {


                Point closestChecked = null;
                Point closestNeighbor = null;
                float distance = 10000f;

                // Find Closest Points
                foreach (Point connectedPoint in connectedDictionary.Keys)
                {
                    foreach (var neighbor in pointDictionary[connectedPoint])
                    {
                        if (connectedDictionary.ContainsKey(neighbor)) continue;

                        float newDistance = connectedPoint.ManHattanDistance(neighbor);
                        if(newDistance < distance)
                        {
                            closestNeighbor = neighbor;
                            closestChecked = connectedPoint;
                            distance = newDistance;
                        }
                    }

                }
                if (closestNeighbor == null) Debug.LogWarning("No Closest Neighbor found");
                else
                {
                    connectedDictionary[closestChecked].Add(closestNeighbor);
                    connectedDictionary.Add(closestNeighbor, new List<Point>());
                    connectedDictionary[closestNeighbor].Add(closestChecked);
                }

                emergencyCounter++;
                if (emergencyCounter == 500) Debug.LogError("Emergency Counter 100");
            }

            Debug.Log("Dictionary Created, size: "+connectedDictionary.Count);
            //PrintDictionary(connectedDictionary);

            return connectedDictionary;


		}

        internal static Dictionary<Point, List<Point>> DelunayDictionaryToCartesianPaths(Dictionary<Point, List<Point>> delaunayDictionary)
        {
			// Have A Dictionary
			Dictionary<Point, List<Point>> wayDictionary = new Dictionary<Point, List<Point>>();

            foreach (var point in delaunayDictionary.Keys)
            {
                    foreach(var target in delaunayDictionary[point])
                    {
                        // Create new Point inbetween
                        Point newMiddlePoint = point.GetCorner(target);

                        if (!wayDictionary.ContainsKey(point))
                        {
                            wayDictionary.Add(point, new List<Point>());
                        }

                        wayDictionary[point].Add(newMiddlePoint);

                        if (!wayDictionary.ContainsKey(target))
                        {
                            wayDictionary.Add(target, new List<Point>());
                        }

                        wayDictionary[target].Add(newMiddlePoint);

                        delaunayDictionary[target].Remove(point);
                    }               
            }
            return wayDictionary;
		}
    }
}