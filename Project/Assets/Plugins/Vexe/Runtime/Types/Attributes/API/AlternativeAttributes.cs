using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.API
{
    /// <summary>
    /// A shorter alternative to SerializeField - applicable to fields and properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializeAttribute : Attribute
    {
    }

    /// <summary>
    /// Fields/auto-properties annotated with this don't get serialized
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DontSerializeAttribute : Attribute
    {
    }

    /// <summary>
    /// A shorter alternative to SerializeField - applicable to fields and properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SaveAttribute : Attribute
    {
    }

    /// <summary>
    /// Fields/auto-properties annotated with this don't get serialized
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DontSaveAttribute : Attribute
    {
    }

    /// <summary>
    /// A shorter alternative to HideInInspector - applicable to fields and properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class HideAttribute : Attribute
    {
    }
}
