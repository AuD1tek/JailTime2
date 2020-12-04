using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JailTime2.Core
{
    [Serializable]
    public class Vector3SE
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3SE(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3SE()
        {

        }


        public static implicit operator Vector3(Vector3SE vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
        public static implicit operator Vector3SE(Vector3 vector)
        {
            return new Vector3SE(vector.x, vector.y, vector.z);
        }
    }
}
