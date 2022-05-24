using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Sisus.ComponentNames.EditorOnly.InspectorContents;

namespace Sisus.ComponentNames.EditorOnly
{
	/// <summary>
	/// Class responsible for injecting <see cref="RenameableHeaderWrapper"/>
	/// into the component header elements of open Inspector windows.
	/// </summary>
	[InitializeOnLoad]
	internal static class RenameableHeaderWrapperToInspectorInjector
    {
		static RenameableHeaderWrapperToInspectorInjector()
        {
            Editor.finishedDefaultHeaderGUI -= AfterInspectorRootEditorHeaderGUI;
            Editor.finishedDefaultHeaderGUI += AfterInspectorRootEditorHeaderGUI;
        }

        private static void AfterInspectorRootEditorHeaderGUI([NotNull] Editor editor)
		{
			if(editor.target is GameObject)
			{
				// Handle InspectorWindow
				AfterGameObjectHeaderGUI(editor);
			}
			else if(editor.target is Component)
			{
				// Handle PropertyEditor window opened via "Properties..." context menu item
				AfterPropertiesHeaderGUI(editor);
			}
		}

		private static void AfterGameObjectHeaderGUI([NotNull] Editor gameObjectEditor)
		{
			foreach((Editor editor, IMGUIContainer header) editorAndHeader in GetComponentHeaderElementsFromInspectorOrPropertyEditorWindow(gameObjectEditor))
			{
				var onGUIHandler = editorAndHeader.header.onGUIHandler;
				if(onGUIHandler.Method is MethodInfo onGUI && onGUI.Name == nameof(RenameableHeaderWrapper.DrawWrappedHeaderGUI))
				{
					continue;
				}

				var component = editorAndHeader.editor.target as Component;
				string tooltip = component == null ? "" : ComponentTooltip.Get(component);
				var renameableComponentEditor = new RenameableHeaderWrapper(editorAndHeader.header, component, true);
				editorAndHeader.header.onGUIHandler = renameableComponentEditor.DrawWrappedHeaderGUI;
			}
		}

		private static void AfterPropertiesHeaderGUI([NotNull] Editor componentEditor)
		{
			var found = GetComponentHeaderElementFromPropertyEditorOf(componentEditor);
			if(!found.HasValue)
			{
				return;
			}
			
			(Editor editor, IMGUIContainer header) editorAndHeader = found.Value;
			var onGUIHandler = editorAndHeader.header.onGUIHandler;
			if(onGUIHandler.Method is MethodInfo onGUI && onGUI.Name == nameof(RenameableHeaderWrapper.DrawWrappedHeaderGUI))
			{
				return;
			}

			var component = editorAndHeader.editor.target as Component;
			string tooltip = component == null ? "" : ComponentTooltip.Get(component);
			var renameableComponentEditor = new RenameableHeaderWrapper(editorAndHeader.header, component, false);

			editorAndHeader.header.onGUIHandler = renameableComponentEditor.DrawWrappedHeaderGUI;
		}
    }
}