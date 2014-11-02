using System;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Constraints
{
    /// <summary>
    /// Annotate a string with this attribute to make it an IP-valid-only field
    /// i.e. if an invalid IP is entered an error helpbox appears
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IPAttribute : RegexAttribute
    {
        public IPAttribute() : base(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")
        {
        }
    }
}