#if POWER_INSPECTOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sisus.ComponentNames.EditorOnly
{
	/// <summary>
	/// Class responsible for injecting <see cref="RenameableHeaderWrapper"/>
	/// into the component header elements of open Power Inspector windows.
	/// </summary>
	[InitializeOnLoad]
	internal static class RenameableHeaderWrapperToPowerInspectorInjector
	{
		private const BindingFlags AnyStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

		static RenameableHeaderWrapperToPowerInspectorInjector()
		{
			Assembly powerInspectorAssembly;
			try
			{
				powerInspectorAssembly = Assembly.Load("PowerInspector");
			}
			catch(FileNotFoundException)
			{
				return;
			}

			if(powerInspectorAssembly is null)
			{
				#if DEV_MODE
				Debug.LogWarning("Assembly \"PowerInspector\" not found.");
				#endif
				return;
			}
			
			var editorGUIDrawer = powerInspectorAssembly.GetType("Sisus.EditorGUIDrawer");
			if(editorGUIDrawer is null)
			{
				#if DEV_MODE
				Debug.LogWarning($"Type \"Sisus.EditorGUIDrawer\" not found in assembly {powerInspectorAssembly.GetName()}.");
				#endif
				return;
			}

			var @event = editorGUIDrawer.GetEvent("BeforeComponentHeaderGUI", AnyStatic);
			if(@event is null)
			{
				#if DEV_MODE
				Debug.LogWarning($"Event \"BeforeComponentHeaderGUI\" not found in class {editorGUIDrawer}.");
				#endif
				return;
			}

			var method = typeof(RenameableHeaderWrapperToPowerInspectorInjector).GetMethod(nameof(OnBeforeComponentHeaderGUI), AnyStatic);
			if(method is null)
			{
				#if DEV_MODE
				Debug.LogWarning($"Method \"{nameof(OnBeforeComponentHeaderGUI)}\" not found in class {nameof(RenameableHeaderWrapperToPowerInspectorInjector)}.");
				#endif
				return;
			}

			Type delegateType = @event.EventHandlerType;
			Delegate @delegate = Delegate.CreateDelegate(delegateType, method, false);
			if(@delegate is null)
			{
				#if DEV_MODE
				Debug.LogWarning($"Failed to create delegate from method {method.Name}.");
				#endif
				return;
			}

			@event.RemoveEventHandler(null, @delegate);
			@event.AddEventHandler(null, @delegate);
		}

		private static void OnBeforeComponentHeaderGUI(Object[] targets, Rect headerRect, bool headerIsSelected)
		{
			ComponentHeaderGUIUtility.BeginComponentHeader(targets[0] as Component, headerRect, headerIsSelected, true);
		}
	}
}
#endif
