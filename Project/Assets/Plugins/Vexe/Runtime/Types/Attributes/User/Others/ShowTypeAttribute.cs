﻿using System;

using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;

namespace Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Others
{
    /// <summary>
    /// A very useful attribute - Annotate System.Type fields/properties with this attribute
    /// to make it possible to select a type from a list of possible types by means of a popup
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ShowTypeAttribute : DrawnAttribute
    {
        /// <summary>
        /// The base Type to start from. All non-abstract child types of this type will be shown
        /// </summary>
        public readonly Type baseType;

        public ShowTypeAttribute(Type baseType)
        {
            this.baseType = baseType;
        }
    }
}