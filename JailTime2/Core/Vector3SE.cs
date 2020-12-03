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

        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
