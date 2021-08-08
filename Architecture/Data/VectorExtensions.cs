using UnityEngine;

namespace Architecture.Data
{
    public static class VectorExtensions
    {
        public static float GetMagnitude(this Vector vector)
        {
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public static Vector GetNormalized(this Vector vector)
        {
            return vector / vector.GetMagnitude();
        }
    }
}