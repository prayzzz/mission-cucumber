using System.Collections.Generic;

using ProtoBuf;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.Protobuf
{
    [ProtoContract, SurrogateFor(typeof(Vector4), typeof(ProtobufSerializer))]
    public class Vector4Surrogate
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;
        [ProtoMember(4)] public float w;

        public static implicit operator Vector4(Vector4Surrogate s)
        {
            return new Vector4(s.x, s.y, s.z, s.w);
        }

        public static implicit operator Vector4Surrogate(Vector4 v)
        {
            return new Vector4Surrogate { x = v.x, y = v.y, z = v.z, w = v.w };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Vector3), typeof(ProtobufSerializer))]
    public class Vector3Surrogate
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;

        public static implicit operator Vector3(Vector3Surrogate s)
        {
            return new Vector3(s.x, s.y, s.z);
        }

        public static implicit operator Vector3Surrogate(Vector3 v)
        {
            return new Vector3Surrogate { x = v.x, y = v.y, z = v.z };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Vector2), typeof(ProtobufSerializer))]
    public class Vector2Surrogate
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;

        public static implicit operator Vector2(Vector2Surrogate s)
        {
            return new Vector2(s.x, s.y);
        }

        public static implicit operator Vector2Surrogate(Vector2 v)
        {
            return new Vector2Surrogate { x = v.x, y = v.y };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Color), typeof(ProtobufSerializer))]
    public class ColorSurrogate
    {
        [ProtoMember(1)] public float r;
        [ProtoMember(2)] public float g;
        [ProtoMember(3)] public float b;
        [ProtoMember(4)] public float a;

        public static implicit operator Color(ColorSurrogate s)
        {
            return new Color(s.r, s.g, s.b, s.a);
        }

        public static implicit operator ColorSurrogate(Color c)
        {
            return new ColorSurrogate { r = c.r, g = c.g, b = c.b, a = c.a };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Color32), typeof(ProtobufSerializer))]
    public class Color32Surrogate
    {
        [ProtoMember(1)] public byte r;
        [ProtoMember(2)] public byte g;
        [ProtoMember(3)] public byte b;
        [ProtoMember(4)] public byte a;

        public static implicit operator Color32(Color32Surrogate s)
        {
            return new Color(s.r, s.g, s.b, s.a);
        }

        public static implicit operator Color32Surrogate(Color32 c)
        {
            return new Color32Surrogate { r = c.r, g = c.g, b = c.b, a = c.a };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Rect), typeof(ProtobufSerializer))]
    public class RectSurrogate
    {
        [ProtoMember(1)] public float xMin;
        [ProtoMember(2)] public float xMax;
        [ProtoMember(3)] public float yMin;
        [ProtoMember(4)] public float yMax;

        public static implicit operator Rect(RectSurrogate s)
        {
            return new Rect(s.xMin, s.yMin, s.xMax - s.xMin, s.yMax - s.yMin);
        }

        public static implicit operator RectSurrogate(Rect r)
        {
            return new RectSurrogate { xMin = r.xMin, xMax = r.xMax, yMin = r.yMin, yMax = r.yMax };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Quaternion), typeof(ProtobufSerializer))]
    public class QuaternionSurrogate
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;
        [ProtoMember(4)] public float w;

        public static implicit operator Quaternion(QuaternionSurrogate s)
        {
            return new Quaternion(s.x, s.y, s.z, s.w);
        }

        public static implicit operator QuaternionSurrogate(Quaternion q)
        {
            return new QuaternionSurrogate { x = q.x, y = q.y, z = q.z, w = q.w };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Keyframe), typeof(ProtobufSerializer))]
    public class KeyframeSurrogate
    {
        [ProtoMember(1)] public int tanMode;
        [ProtoMember(2)] public float inTan;
        [ProtoMember(3)] public float outTan;
        [ProtoMember(4)] public float value;
        [ProtoMember(5)] public float time;

        public static implicit operator Keyframe(KeyframeSurrogate s)
        {
            return new Keyframe(s.inTan, s.outTan, s.time, s.value)
            {
                tangentMode = s.tanMode
            };
        }
        public static implicit operator KeyframeSurrogate(Keyframe f)
        {
            return new KeyframeSurrogate
            {
                tanMode = f.tangentMode,
                inTan   = f.inTangent,
                outTan  = f.outTangent,
                value   = f.value,
                time    = f.time
            };
        }
    }

    [ProtoContract, SurrogateFor(typeof(LayerMask), typeof(ProtobufSerializer))]
    public class LayerMaskSurrogate
    {
        [ProtoMember(1)] public int value;

        public static implicit operator LayerMask(LayerMaskSurrogate s)
        {
            return new LayerMask { value = s.value };
        }

        public static implicit operator LayerMaskSurrogate(LayerMask m)
        {
            return new LayerMaskSurrogate { value = m.value };
        }
    }

    [ProtoContract, SurrogateFor(typeof(Bounds), typeof(ProtobufSerializer))]
    public class BoundsSurrogate
    {
        [ProtoMember(1)] public Vector3 size;
        [ProtoMember(2)] public Vector3 center;

        public static implicit operator Bounds(BoundsSurrogate s)
        {
            return new Bounds(s.center, s.size);
        }

        public static implicit operator BoundsSurrogate(Bounds b)
        {
            return new BoundsSurrogate { center = b.center, size = b.size };
        }
    }

    [ProtoContract, SurrogateFor(typeof(AnimationCurve), typeof(ProtobufSerializer))]
    public class AnimationCurveSurrogate
    {
        [ProtoMember(1)] public WrapMode preWrap;
        [ProtoMember(2)] public WrapMode postWrap;
        [ProtoMember(3)] public Keyframe[] keys;

        public static implicit operator AnimationCurve(AnimationCurveSurrogate s)
        {
            return new AnimationCurve(s.keys)
            {
                preWrapMode  = s.preWrap,
                postWrapMode = s.postWrap
            };
        }

        public static implicit operator AnimationCurveSurrogate(AnimationCurve c)
        {
            return new AnimationCurveSurrogate
            {
                keys     = c.keys,
                postWrap = c.postWrapMode,
                preWrap  = c.preWrapMode
            };
        }
    }

    [ProtoContract]
    public class UnityObjectSurrogate<T> where T : Object
    {
        [ProtoMember(1)]
        public int index;
        public T obj;

        [ProtoBeforeSerialization]
        public void Serialization(SerializationContext info)
        {
            var database = (info.Context as List<Object>);
            this.index = database.Count;
            database.Add(this.obj as Object);
        }

        [ProtoAfterDeserialization]
        public void Deserialization(SerializationContext info)
        {
            var database = (info.Context as List<Object>);
            this.obj = database[this.index] as T;
        }

        public static implicit operator T(UnityObjectSurrogate<T> surrogate)
        {
            return surrogate.obj;
        }

        public static implicit operator UnityObjectSurrogate<T>(T obj)
        {
            return new UnityObjectSurrogate<T> { obj = obj };
        }

    }
}
