using System.Collections.Generic;
using System.Runtime.Serialization;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Serialization.Serializers.BinaryFormatter
{
    [SurrogateFor(typeof(Vector4), typeof(BinaryFormatterSerializer))]
    public class Vector4Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var v4 = (Vector4)obj;
            info.AddValue("x", v4.x);
            info.AddValue("y", v4.y);
            info.AddValue("z", v4.z);
            info.AddValue("w", v4.w);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Vector4(info.GetSingle("x"), info.GetSingle("y"), info.GetSingle("z"), info.GetSingle("w"));
        }
    } 

    [SurrogateFor(typeof(Vector3), typeof(BinaryFormatterSerializer))]
    public class Vector3Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var v3 = (Vector3)obj;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Vector3(info.GetSingle("x"), info.GetSingle("y"), info.GetSingle("z"));
        }
    }

    [SurrogateFor(typeof(Vector2), typeof(BinaryFormatterSerializer))]
    public class Vector2Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var v2 = (Vector2)obj;
            info.AddValue("x", v2.x);
            info.AddValue("y", v2.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Vector2(info.GetSingle("x"), info.GetSingle("y"));
        }
    }

    [SurrogateFor(typeof(Color), typeof(BinaryFormatterSerializer))]
    public class ColorSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var c = (Color)obj;
            info.AddValue("r", c.r);
            info.AddValue("g", c.g);
            info.AddValue("b", c.b);
            info.AddValue("a", c.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Color(info.GetSingle("r"), info.GetSingle("g"), info.GetSingle("b"), info.GetSingle("a"));
        }
    }

    [SurrogateFor(typeof(Color32), typeof(BinaryFormatterSerializer))]
    public class Color32Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var c = (Color32)obj;
            info.AddValue("r", c.r);
            info.AddValue("g", c.g);
            info.AddValue("b", c.b);
            info.AddValue("a", c.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Color(info.GetByte("r"), info.GetByte("g"), info.GetByte("b"), info.GetByte("a"));
        }
    }

    [SurrogateFor(typeof(Rect), typeof(BinaryFormatterSerializer))]
    public class RectSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var r = (Rect)obj;
            info.AddValue("xMin", r.xMin);
            info.AddValue("xMax", r.xMax);
            info.AddValue("yMin", r.yMin);
            info.AddValue("yMax", r.yMax);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            float left = info.GetSingle("xMin");
            float top = info.GetSingle("yMin");
            return new Rect(left, top, info.GetSingle("xMax") - left, info.GetSingle("yMax") - top);
        }
    }

    [SurrogateFor(typeof(Quaternion), typeof(BinaryFormatterSerializer))]
    public class QuaternionSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var q = (Quaternion)obj;
            info.AddValue("x", q.x);
            info.AddValue("y", q.y);
            info.AddValue("z", q.z);
            info.AddValue("w", q.w);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Vector4(info.GetSingle("x"), info.GetSingle("y"), info.GetSingle("z"), info.GetSingle("w"));
        }
    } 

    [SurrogateFor(typeof(Keyframe), typeof(BinaryFormatterSerializer))]
    public class KeyframeSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Keyframe kf = (Keyframe)obj;
            info.AddValue("tanMode", kf.tangentMode);
            info.AddValue("inTan"  , kf.inTangent);
            info.AddValue("outTan" , kf.outTangent);
            info.AddValue("value"  , kf.value);
            info.AddValue("time"   , kf.time);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var tanMode = info.GetInt32("tanMode");
            var inTan   = info.GetSingle("inTan");
            var outTan  = info.GetSingle("outTan");
            var time    = info.GetSingle("time");
            var value   = info.GetSingle("value");
            return new Keyframe(time, value, inTan, outTan) { tangentMode = tanMode };
        }
    }

    [SurrogateFor(typeof(LayerMask), typeof(BinaryFormatterSerializer))]
    public class LayerMaskSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", ((LayerMask)obj).value);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new LayerMask { value = info.GetInt32("value")};
        }
    }

    [SurrogateFor(typeof(Bounds), typeof(BinaryFormatterSerializer))]
    public class BoundsSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Bounds b = (Bounds)obj;
            info.AddValue("center", b.center);
            info.AddValue("size", b.size);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return new Bounds(
                (Vector3)info.GetValue("center", typeof(Vector3)),
                (Vector3)info.GetValue("size", typeof(Vector3))
            );
        }
    }

    [SurrogateFor(typeof(AnimationCurve), typeof(BinaryFormatterSerializer))]
    public class AnimationCurveSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var c = (AnimationCurve)obj;
            info.AddValue("preWrap", c.preWrapMode, typeof(WrapMode));
            info.AddValue("postWrap", c.postWrapMode, typeof(WrapMode));
            info.AddValue("keys", c.keys, typeof(Keyframe[]));
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var preWrapMode  = (WrapMode)info.GetValue("preWrap", typeof(WrapMode));
            var postWrapMode = (WrapMode)info.GetValue("postWrap", typeof(WrapMode));
            var keys         = (Keyframe[])info.GetValue("keys", typeof(Keyframe[]));
            return new AnimationCurve(keys) { preWrapMode = preWrapMode, postWrapMode = postWrapMode };
        }
    }

    public class UnityObjectSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var database = (context.Context as List<Object>);
            info.AddValue("index", database.Count);
            database.Add(obj as Object);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return (context.Context as List<Object>)[info.GetInt32("index")];
        }
    }
}