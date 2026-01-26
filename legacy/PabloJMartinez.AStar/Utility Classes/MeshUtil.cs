// Copyright (c) 2013-2017 Pablo J. Martínez. All rights reserved.
// Licensed under the MIT License.
// Part of the legacy PabloJMartinez.AStar implementation.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PabloJMartinez.AStar.Legacy
{
    /// <summary>
    /// Utility Class with mesh related methods.
    /// </summary>
    public static class MeshUtil
    {
        // Source: ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        public static bool IsPointInPoly(Vector3[] poly, Vector3 pt)
        {
            bool c = false;
            int l = poly.Length;
            int j = l - 1;

            for(int i = -1; ++i < l; j = i)
            {
                if(((poly[i].z <= pt.z && pt.z < poly[j].z) || (poly[j].z <= pt.z && pt.z < poly[i].z))
                && (pt.x < (poly[j].x - poly[i].x) * (pt.z - poly[i].z) / (poly[j].z - poly[i].z) + poly[i].x))
                {
                    c = !c;
                }
            }
            return c;
        }

        public static Vector3[] FindMeshContainingPoint(IList<Vector3[]> navmeshMeshes, Vector3 point)
        {
            Vector3[] theContainedMesh = null;
            int navmeshMeshesCount = navmeshMeshes.Count;
            for(int i = 0; i < navmeshMeshesCount; i++)
            {
                if(IsPointInPoly(navmeshMeshes[i], point) == true)
                {
                    theContainedMesh = navmeshMeshes[i];
                    break;
                }
            }
            return theContainedMesh;
        }

        // Check if the mesh vertices order are clockwise
        public static bool IsMeshClockwise(Vector3[] meshNodes)
        {
            int meshNodesLength = meshNodes.Length;
            float resultSoFar = 0.0f;
            int nextEdge = 0;
            for(int i = 0; i < meshNodesLength; i++)
            {
                nextEdge = i + 1;
                if(nextEdge == meshNodesLength)
                {
                    nextEdge = 0;
                }
                resultSoFar = resultSoFar + ((meshNodes[nextEdge].x - meshNodes[i].x) * (meshNodes[nextEdge].z + meshNodes[i].z));
            }

            if(resultSoFar > 0) // Clockwise
            {
                return true;
            }
            else
            {
                return false;
            }
            // Check if the mesh vertices order are clockwise relative to camera forward direction
            /*
            float lastDotProductResult = 0.0f;
            //Vector3 triangleNormal = Vector3.Cross(meshNodes[1] - meshNodes[0], meshNodes[2] - meshNodes[0]);
            //Debug.Log("IsTriangleClockwise -> X: " + triangleNormal.x + " Y: " + triangleNormal.y + " Z: " + triangleNormal.z);
            //float dotProductResult = Vector3.Dot(Game.MainCamera.transform.TransformDirection(Vector3.forward), triangleNormal);

            if(dotProductResult < 0) // Clockwise
            {
                return true;
            }
            else
            {
                return false;
            }*/
        }

        // Swap clockwise order of the vertices, to the opposite ordering
        public static void ToggleMeshClockwiseOrder(Vector3[] triangleNodes)
        {
            /*Vector3 tempTriangleNode;
            int triangleNodesLength = triangleNodes.Length;
            for(int i = 1; i < triangleNodesLength; i += 2)
            {
                tempTriangleNode = triangleNodes[i];
                triangleNodes[i] = triangleNodes[i+1];
                triangleNodes[i+1] = tempTriangleNode;
            }*/
        }

        public static void ToggleMeshClockwiseOrder(Portal[] trianglePortals)
        {
            /*Portal tempTrianglePortal;
            int triangleNodesLength = trianglePortals.Length;
            for(int i = 1; i < triangleNodesLength; i += 2)
            {
                tempTrianglePortal = trianglePortals[i];
                trianglePortals[i] = trianglePortals[i+1];
                trianglePortals[i+1] = tempTrianglePortal;
            }*/
        }

        public static void ToggleMeshClockwiseOrder(int[] triangleNodes)
        {
            /*int tempTriangleNode;
            int triangleNodesLength = triangleNodes.Length;
            for(int i = 1; i < triangleNodesLength; i += 2)
            {
                tempTriangleNode = triangleNodes[i];
                triangleNodes[i] = triangleNodes[i+1];
                triangleNodes[i+1] = tempTriangleNode;
            }*/
        }

        public static Vector3[] SortMeshClockwise(Vector3[] meshNodes)
        {
            Vector3[] sorteredMesh = new Vector3[meshNodes.Length];
            Vector3 center = GetCenter(meshNodes);
            sorteredMesh[0] = meshNodes[0];
            int meshNodesLength = meshNodes.Length - 1;
            for(int i = 0; i < meshNodesLength; i++)
            {
                int nodeWithSmallerAngle = 0;
                float smallestNodeAnglee = float.PositiveInfinity;
                float currentAngle = 0.0f;
                for(int j = 1; j < meshNodesLength; j++)
                {
                    if(sorteredMesh[i] != meshNodes[j])
                    {
                        currentAngle = Vector3.Angle(sorteredMesh[i], meshNodes[j]);
                        if(currentAngle < smallestNodeAnglee && Vector3Util.TriArea2(center, sorteredMesh[i], meshNodes[j]) >= 0.0f)
                        {
                            nodeWithSmallerAngle = j;
                            smallestNodeAnglee = currentAngle;
                        }
                    }
                }
                int nextSortedNode = i + 1;
                if(nextSortedNode < meshNodesLength)
                {
                    sorteredMesh[i + 1] = meshNodes[nodeWithSmallerAngle];
                }
            }
            return sorteredMesh;
        }

        public static bool IsMeshConvex(Vector3[] meshVertices)
        {
            float crossProductResult = 0.0f;
            bool oneSide = false;
            bool otherSide = false;
            int firstVertice = 0;
            int secondVertice = 0;
            int thirdVertice = 0;
            int meshVerticesLength = meshVertices.Length;
            for(int i = 0; i < meshVerticesLength; i++)
            {
                firstVertice = i;
                secondVertice = (i + 1) % meshVerticesLength;
                thirdVertice = (i + 2) % meshVerticesLength;
                crossProductResult = Vector3Util.TriArea2(meshVertices[secondVertice], meshVertices[firstVertice], meshVertices[thirdVertice]);
                if(crossProductResult >= 0.0f)
                {
                    oneSide = true;
                }
                else
                {
                    otherSide = true;
                }
                if(oneSide == true && otherSide == true) return false;
            }
            return true;
        }

        public static Vector3 GetCenter(Vector3[] points)
        {
            Vector3 result = Vector3.zero;
            int pointsLength = points.Length;
            if(pointsLength > 1)
            {
                for(int i = 0; i < pointsLength; i++)
                {
                    result.x = result.x + points[i].x;
                    result.y = result.y + points[i].y;
                    result.z = result.z + points[i].z;
                }
                result.x = result.x / pointsLength;
                result.y = result.y / pointsLength;
                result.z = result.z / pointsLength;
            }
            else result = points[0];
            return result;
        }

        /// <summary>
        /// Simply adds triangles coming from the first vertice. It assumes a convex mesh with vertices in clockwise or counterclockwise order
        /// </summary>
        /// <param name="convexMesh"></param>
        /// <returns></returns>
        public static int[] GetTriangles(Vector3[] convexMesh)
        {
            if(convexMesh.Length == 3)
            {
                int[] triangles = new int[3];
                triangles[0] = 0;
                triangles[1] = 1;
                triangles[2] = 2;
                return triangles;
            }
            else
            {
                int convexMeshLengthMinusOne = convexMesh.Length - 1;
                int[] triangles = new int[convexMeshLengthMinusOne * 3];
                int trianglesLength = triangles.Length;
                int nextFirstTriangleIndex = 3;
                int nextSecondTriangleIndex = 4;
                int nextThirdTriangleIndex = 5;
                for(int i = 1; i < convexMeshLengthMinusOne; i++)
                {
                    /*trianglesLength = triangles.Length;
                    nextFirstTriangleIndex = (i - 1) * 3;
                    nextSecondTriangleIndex = (i - 1) * 3 + 1;
                    nextThirdTriangleIndex = (i - 1) * 3 + 2;
                    if(nextFirstTriangleIndex >= trianglesLength || nextSecondTriangleIndex >= trianglesLength || nextThirdTriangleIndex >= trianglesLength)
                    {
                        System.Array.Resize<int>(ref triangles, trianglesLength*3);
                    }*/
                    nextFirstTriangleIndex = (i - 1) * 3;
                    nextSecondTriangleIndex = (i - 1) * 3 + 1;
                    nextThirdTriangleIndex = (i - 1) * 3 + 2;
                    triangles[nextFirstTriangleIndex] = 0;
                    triangles[nextSecondTriangleIndex] = i;
                    triangles[nextThirdTriangleIndex] = i + 1;
                }
                return triangles;
            }
        }
    }
}