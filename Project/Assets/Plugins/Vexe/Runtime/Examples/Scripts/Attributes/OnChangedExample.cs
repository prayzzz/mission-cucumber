using System.Collections.Generic;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Popups;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Attributes
{
    [BasicView]
    public class OnChangedExample : BetterBehaviour
    {
        // when this string changes, set the `tag` property to the new value and log it
        //[Tags, OnChanged(Set = "tag", Call = "Log")] // could have also written OnChanged("Log", Set = "tag")
        [Tags] // could have also written OnChanged("Log", Set = "tag")
        public string playerTag;

        // if any vector of this array changes, we set the `position` property to that new vector
        [PerItem, OnChanged(Set = "position")]
        public Vector3[] vectors;

        // if any value of this dictionary changes, set our scale to that value
        // you could use PerKey to apply attributes on the dictionary's keys instead of values
        // we add IgnoreAddArea to tell that we don't want to apply our attributes to the
        // key/value pair that we use to add to the dictionary (the pair at the top that reads: "Add pair")
        // this is because, that pair belongs to an internal object and that object doesn't have a 'localScale'
        [PerValue, IgnoreAddArea, OnChanged(Set = "localScale")]
        public Dictionary<string, Vector3> dictionary;

        // Note that position and localScale are properties defined in BetterBehaviour (there's a cached transform there)
    }
}