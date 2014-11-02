using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

using Vexe.Runtime.Helpers;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Serialization
{
    [DefineCategory("Settings")]
    public class PropertiesExample : BetterBehaviour
    {
        // RULES:
        // public fields/properties are serialized impclitly
        // non-public fields/properties are serialized only if they have any of serialization attributes on them
         //        SerialzieField, Serialize, Save
        // const, readonly, delegates, 


        // public auto-properties are serialized implictly
        public int IntProp { get; set; }

        // non-public ones are not serialized, unless marked with Save, Serialize or SerializerField
        private float radius { get; set; }

        [Show, Comment("// this is not serialized, yet visible")]
        private Vector3 Vector3Prop { get; set; }

        // this is serialized and hidden
        [Save, Hide]
        private Color color;

        [Category("Settings")]
        public float
            minRadius = 1f,
            maxRadius = 5f,
            lerpSpeed = 1f;

        // I decided only to serialize auto-properties, so this one will not serialize
        // this property has a side effect and a backing field, so if we want, we could
        // just serialize the backing field instead (which is 'strengthColor' in this case
        [Show, Category("Settings")]
        public Color Color
        {
            get { return this.color; }
            set
            {
                float red = value.r;
                this.radius = Mathf.Max(this.minRadius, red * this.maxRadius);
                value.b = 0f;
                this.color = value;
                this.renderer.sharedMaterial.color = value;
            }
        }

        private void Update()
        {
            float r = Mathf.PingPong(Time.time * this.lerpSpeed, 1f);
            float g = 1 - r;
            this.Color = new Color(r, g, 0);
        }

        private void OnDrawGizmos()
        {
            GizHelper.DrawWireSphere(this.position, this.radius, this.Color);
        }
    }
}