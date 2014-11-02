using Assets.Plugins.Vexe.Runtime.Types.Attributes.API;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Categories;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Decorates;
using Assets.Plugins.Vexe.Runtime.Types.Attributes.User.Selections;
using Assets.Plugins.Vexe.Runtime.Types.Core;

using UnityEngine;

namespace Assets.Plugins.Vexe.Runtime.Examples.Scripts.Other
{
    /// <summary>
    /// A convenient script to load a selected scene
    /// Useful when hooked up to a delegate for remote scene loading
    /// when a certain event is fired like a UI button click etc
    /// </summary>
    [MinimalView]
    public class SceneTransition : BetterBehaviour
    {
        [SelectScene, Comment("Note: scene will only load if it was included in the build settings and the player is running")]
        public string scene;
        
        [Show]
        public void LoadScene()
        {
            Application.LoadLevel(this.scene);
        }
    }
}