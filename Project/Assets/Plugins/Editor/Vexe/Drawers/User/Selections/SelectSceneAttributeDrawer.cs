﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Framework.Drawers
{
	public class SelectSceneAttributeDrawer : CompositeDrawer<string, SelectSceneAttribute>
	{
		protected override void OnInitialized()
		{
			if (dmValue == null)
				dmValue = string.Empty;
		}

		public override void OnRightGUI()
		{
			if (gui.SelectionButton())
			{
				Func<string[]> getScenes = () =>
						DirectoryHelper.GetFiles("Assets", "*.unity", SearchOption.AllDirectories)
											.Select(f => f.Path.Substring(f.Path.IndexOf("Assets") + 6).RemoveExtension())
											.ToArray();

				Func<string, string> getSceneName = path =>
					path.Substring(path.Replace('\\', '/').LastIndexOf('/') + 1);

				var dictionary = new KVPList<string, string>();
				var allScenes  = getScenes();
				foreach(var s in allScenes)
					dictionary.Add(getSceneName(s), s);

				Func<Func<string[]>, string, Tab<string>> sceneTab = (scenes, title) =>
					new Tab<string>(
						@getValues    : scenes,
						@getCurrent   : () => dictionary.ContainsKey(dmValue) ? dictionary[dmValue] : dmValue,
						@setTarget    : s => dmValue = getSceneName(s),
						@getValueName : s => s,
						@title        : title
					);

				var buildScenes = EditorBuildSettings.scenes.Select(s => s.path);

				SelectionWindow.Show("Select scene",
					sceneTab(getScenes, "All"),
					sceneTab(getScenes().Where(s => buildScenes.Any(bs => Regex.Replace(bs, "/", "\\").Contains(s))).ToArray, "Build")
				);
			}
		}
	}
}