using System;

namespace Architecture.Data
{
    [Serializable]
    public struct TransformData
    {
        public Vector position;
        public Quaternion rotation;

        public TransformData(TransformData transformData)
        {
            position = transformData.position;
            rotation = transformData.rotation;
        }

        public TransformData(Vector position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}