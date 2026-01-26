// Copyright (c) 2013-2017 Pablo J. Martínez. All rights reserved.
// Licensed under the MIT License.
// Part of the legacy PabloJMartinez.AStar implementation.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PabloJMartinez.AStar.Legacy
{
    public static class Vector3Util
    {
        public static bool IsOneVector3EqualToTheOther(Vector3 a, Vector3 b)
        {
            return ((b - a).sqrMagnitude < 0.001f*0.001f);
        }

        public static float TriArea2(Vector3 a, Vector3 b, Vector3 c)
        {
            float ax = b.x - a.x;
            float az = b.z - a.z;
            float bx = c.x - a.x;
            float bz = c.z - a.z;
            return bx*az - ax*bz;
        }

        public static bool IsPointToTheLeftOfThisLine(Vector3 linePointA, Vector3 linePointB, Vector3 pointToCheck)
        {
            return ((linePointB.x - linePointA.x)*(pointToCheck.y - linePointA.y) - (linePointB.y - linePointA.y)*(pointToCheck.x - linePointA.x)) > 0.0f;
        }

        public static float SquaredDistance(Vector3 fromPosition, Vector3 toPosition)
        {
            return (toPosition - fromPosition).sqrMagnitude;
        }
    }
}