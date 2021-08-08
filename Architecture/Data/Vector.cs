using System;
using UnityEngine;

namespace Architecture.Data
{
    [Serializable]
    public partial struct Vector
    {
        public float x;
        public float y;
        public float z;

        public Vector(Vector vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public static Vector operator-(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        
        public static Vector operator+(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector operator /(Vector vector, float value)
        {
            return new Vector(vector.x / value, vector.y / value, vector.z / value);
        }
        
        public static Vector operator *(Vector vector, float value)
        {
            return new Vector(vector.x * value, vector.y * value, vector.z * value);
        }
        
#if UNITY_5_3_OR_NEWER
        public static implicit operator Vector(UnityEngine.Vector3 vector3)
        {
            return new Vector(vector3.x, vector3.y, vector3.z);
        }

        public static implicit operator UnityEngine.Vector3(Vector vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
#endif
    }
}