// Copyright (c) 2013-2017 Pablo J. Martínez. All rights reserved.
// Licensed under the MIT License.
// Part of the legacy PabloJMartinez.AStar implementation.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PabloJMartinez.AStar.Legacy
{
    /// <summary>
    /// Utility Class with "Collections" related methods.
    /// </summary>
    public static class CollUtil 
    {
        public static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        public static object InitializeJaggedArray(Type type, int index, int[] lengths)
        {
            if(lengths[index] > 0)
            {
                Array array = Array.CreateInstance(type, lengths[index]);
                Type elementType = type.GetElementType();

                if(elementType != null)
                {
                    for(int i = 0; i < lengths[index]; i++)
                    {
                        array.SetValue(
                            InitializeJaggedArray(elementType, index + 1, lengths), i);
                    }
                }
                return array;
            }
            else return null;
        }
    }
}