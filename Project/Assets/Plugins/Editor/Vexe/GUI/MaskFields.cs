using System;
using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		/// <summary>
		/// Credits to Bunny83: http://answers.unity3d.com/questions/393992/custom-inspector-multi-select-enum-dropdown.html?sort=oldest
		/// </summary>
		public int EnumMaskFieldThatWorks(int currentValue, int[] enumValues, string[] enumNames, GUIContent content)
		{
			int maskVal = 0;
			for (int i = 0; i < enumValues.Length; i++)
			{
				if (enumValues[i] != 0)
				{
					if ((currentValue & enumValues[i]) == enumValues[i])
						maskVal |= 1 << i;
				}
				else if (currentValue == 0)
					maskVal |= 1 << i;
			}

			var newMaskVal = MaskField(content, maskVal, enumNames);
			int changes = maskVal ^ newMaskVal;

			for (int i = 0; i < enumValues.Length; i++)
			{
				if ((changes & (1 << i)) != 0) // has this list item changed?
				{
					if ((newMaskVal & (1 << i)) != 0) // has it been set?
					{
						if (enumValues[i] == 0) // special case: if "0" is set, just set the val to 0
						{
							currentValue = 0;
							break;
						}
						else
						{
							currentValue |= enumValues[i];
						}
					}
					else
					{ // it has been reset
						currentValue &= ~enumValues[i];
					}
				}
			}
			return currentValue;
		}

		public int EnumMaskFieldThatWorks(int currentValue, int[] enumValues, string[] enumNames, string text)
		{
			return EnumMaskFieldThatWorks(currentValue, enumValues, enumNames, GetContent(text));
		}

		public int EnumMaskFieldThatWorks(Enum enumValue, GUIContent label)
		{
			var enumType = enumValue.GetType();
			var enumNames = Enum.GetNames(enumType);
			var enumValues = Enum.GetValues(enumType) as int[];
			return EnumMaskFieldThatWorks(Convert.ToInt32(enumValue), enumValues, enumNames, label);
		}

		public int EnumMaskFieldThatWorks(Enum enumValue, string label)
		{
			return EnumMaskFieldThatWorks(enumValue, GetContent(label));
		}

		public int MaskField(int mask, string[] displayedOptions, params GUILayoutOption[] option)
		{
			return MaskField("", mask, displayedOptions, option);
		}

		public int MaskField(string label, int mask, string[] displayedOptions)
		{
			return MaskField(GetContent(label), mask, displayedOptions);
		}

		public int MaskField(GUIContent label, int mask, string[] displayedOptions)
		{
			return MaskField(label, mask, displayedOptions, (GUILayoutOption[])null);
		}

		public int MaskField(GUIContent label, int mask, string[] displayedOptions, params GUILayoutOption[] option)
		{
			return MaskField(label, mask, displayedOptions, EditorStyles.popup, option);
		}

		public int MaskField(int mask, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] option)
		{
			return MaskField("", mask, displayedOptions, style, option);
		}

		public int MaskField(string label, int mask, string[] displayedOptions, params GUILayoutOption[] option)
		{
			return MaskField(label, mask, displayedOptions, EditorStyles.popup, option);
		}

		public int MaskField(string label, int mask, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] option)
		{
			return MaskField(GetContent(label), mask, displayedOptions, style, option);
		}

		public int MaskField(GUIContent label, int mask, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] option)
		{
			return EditorGUILayout.MaskField(label, mask, displayedOptions, style, option);
		}
	}
}
