using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    [RequireComponent(typeof(CanvasRenderer))]
    [ExecuteInEditMode]
    public class ConcavePolygonRenderer : MaskableGraphic
    {

	    private class Vertex
	    {
		    public UIVertex vertex;
		    
		    public Vertex previous;
		    public Vertex next;

		    public bool isReflex;

		    public Vertex(Vector2 position, Color color)
		    {
			    vertex = UIVertex.simpleVert;
			    vertex.color = color;
			    vertex.position = position;
		    }
		    
	    }

	    [SerializeField]
	    private List<Vector2> positions = new(3);

	    public void SetPositions(List<Vector2> value)
	    {
		    positions = value;
		    gameObject.SetActive(false);
		    gameObject.SetActive(true);
	    }

	    protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();
            TriangulateConcavePolygon(helper);
        }
        
        
        private void TriangulateConcavePolygon(VertexHelper helper) 
        {
	        if (positions.Count == 3)
			{
				helper.AddUIVertexTriangleStream(positions.Select(pos =>
				{
					UIVertex vertex = UIVertex.simpleVert;
					vertex.color = color;
					vertex.position = pos;
					return vertex;
				}).ToList());

				return;
			}
	        
			List<Vertex> vertices = new List<Vertex>(positions.Count);

			foreach (var pos in positions)
			{
				vertices.Add(new Vertex(pos, color));
			}
			
			for (int i = 0; i < vertices.Count; i++)
			{
				int nextPos = Mathf.Clamp(i + 1, 0, vertices.Count - 1);
				int prevPos = (i - 1 % vertices.Count + vertices.Count) % vertices.Count;
				vertices[i].previous = vertices[prevPos];
				vertices[i].next = vertices[nextPos];
			}
			
			foreach (var vert in vertices)
			{
				CheckIfReflexOrConvex(vert);
			}
			
			List<Vertex> earVertices = new List<Vertex>();
			
			foreach (var vert in vertices)
			{
				IsVertexEar(vert, vertices, earVertices);
			}
			
			while (true)
			{
				if (vertices.Count == 3)
				{
					helper.AddVert(vertices[0].vertex);
					helper.AddVert(vertices[0].previous.vertex);
					helper.AddVert(vertices[0].next.vertex);
					helper.AddTriangle(helper.currentVertCount - 3, helper.currentVertCount - 2, helper.currentVertCount - 1);
					return;
				}
				
				Vertex earVertex = earVertices.Count > 0 ? earVertices[0] : null;

				if (earVertex == null)
				{
					break;
				}
				
				Vertex earVertexPrev = earVertex.previous;
				Vertex earVertexNext = earVertex.next;

				helper.AddVert(earVertex.vertex);
				helper.AddVert(earVertexPrev.vertex);
				helper.AddVert(earVertexNext.vertex);
				helper.AddTriangle(helper.currentVertCount - 3, helper.currentVertCount - 2, helper.currentVertCount - 1);
				
				earVertices.Remove(earVertex);

				vertices.Remove(earVertex);
				
				earVertexPrev.next = earVertexNext;
				earVertexNext.previous = earVertexPrev;
				
				CheckIfReflexOrConvex(earVertexPrev);
				CheckIfReflexOrConvex(earVertexNext);

				earVertices.Remove(earVertexPrev);
				earVertices.Remove(earVertexNext);

				IsVertexEar(earVertexPrev, vertices, earVertices);
				IsVertexEar(earVertexNext, vertices, earVertices);
			}
			
        }
        
		private static void CheckIfReflexOrConvex(Vertex v)
		{
			v.isReflex = false;
			Vector2 a = v.previous.vertex.position;
			Vector2 b = v.vertex.position;
			Vector2 c = v.next.vertex.position;

			if (IsTriangleOrientedClockwise(a, b, c))
			{
				v.isReflex = true;
			}
		}
		
		private static void IsVertexEar(Vertex v, List<Vertex> vertices, List<Vertex> earVertices)
		{
			if (v.isReflex)
			{
				return;
			}
			
			Vector2 a = v.previous.vertex.position;
			Vector2 b = v.vertex.position;
			Vector2 c = v.next.vertex.position;

			bool hasPointInside = false;

			for (int i = 0; i < vertices.Count; i++)
			{
				if (vertices[i].isReflex)
				{
					Vector2 p = vertices[i].vertex.position;
					
					if (IsPointInTriangle(a, b, c, p))
					{
						hasPointInside = true;

						break;
					}
				}
			}

			if (!hasPointInside)
			{
				earVertices.Add(v);
			}
		}
		
		private static bool IsTriangleOrientedClockwise(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			
			bool isClockWise = true;

			float determinant = p1.x * p2.y + p3.x * p1.y + p2.x * p3.y - p1.x * p3.y - p3.x * p2.y - p2.x * p1.y;

			if (determinant > 0f)
			{
				isClockWise = false;
			}

			return isClockWise;
		}
		
		private static bool IsPointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p)
		{
			bool isWithinTriangle = false;
			
			float denominator = ((p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y));

			float a = ((p2.y - p3.y) * (p.x - p3.x) + (p3.x - p2.x) * (p.y - p3.y)) / denominator;
			float b = ((p3.y - p1.y) * (p.x - p3.x) + (p1.x - p3.x) * (p.y - p3.y)) / denominator;
			float c = 1 - a - b;
			
			if (a > 0f && a < 1f && b > 0f && b < 1f && c > 0f && c < 1f)
			{
				isWithinTriangle = true;
			}

			return isWithinTriangle;
		}

    }
}
