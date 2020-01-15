using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CreateParth : MonoBehaviour
{
    private VertexPath vertexPath;
    private GlobalDisplaySettings globalEditorDisplaySettings;
    [SerializeField] private bool closePath;
    private void Update()
    {
        List<Vector3> points = new List<Vector3>();
        foreach (Transform i in transform)
        {
            points.Add(i.position);
        }
        vertexPath = GeneratePath(points.ToArray(), closePath);
    }

    private VertexPath GeneratePath(Vector3[] points, bool closedPath)
    {
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xyz);
        return new VertexPath(bezierPath, transform);
    }

    private void OnDrawGizmos()
    {
        vertexPath.UpdateTransform(transform);
        if (globalEditorDisplaySettings == null)
        {
            globalEditorDisplaySettings = GlobalDisplaySettings.Load();
        }
        Gizmos.color = globalEditorDisplaySettings.bezierPath;

        for (int i = 0; i < vertexPath.NumPoints; i++)
        {
            int nextI = i + 1;
            if (nextI >= vertexPath.NumPoints)
            {
                if (vertexPath.isClosedLoop)
                {
                    nextI %= vertexPath.NumPoints;
                }
                else
                {
                    break;
                }
            }
            Gizmos.DrawLine(vertexPath.GetPoint(i), vertexPath.GetPoint(nextI));
        }
    }
}
