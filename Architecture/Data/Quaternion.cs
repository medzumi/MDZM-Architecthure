using System;

namespace Architecture.Data
{
    [Serializable]
    public partial struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;
        
        public Quaternion(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

#if UNITY_5_3_OR_NEWER
        public static implicit operator Quaternion(UnityEngine.Quaternion quaternion)
        {
            return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        public static implicit operator UnityEngine.Quaternion(Quaternion quaternion)
        {
            return new UnityEngine.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
#endif
    }
}