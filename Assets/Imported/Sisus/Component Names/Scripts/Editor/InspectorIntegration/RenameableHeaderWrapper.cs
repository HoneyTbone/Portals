using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sisus.ComponentNames.EditorOnly
{
    /// <summary>
    /// Class that can wrap the <see cref="IMGUIContainer.onGUIHandler"/>
    /// of a component header <see cref="IMGUIContainer"/> and augment it with the ability to
    /// rename said component.
    /// </summary>
    internal class RenameableHeaderWrapper
    {
        private readonly IMGUIContainer headerElement;
        private readonly Component component;
        private readonly Action wrappedOnGUIHandler;
        private readonly bool supportsRichText;

        public Component Component => component;

        public RenameableHeaderWrapper(IMGUIContainer headerElement, Component component, bool supportsRichText)
		{
            this.headerElement = headerElement;
            this.component = component;
            this.supportsRichText = supportsRichText;
            wrappedOnGUIHandler = headerElement.onGUIHandler;
        }

		public void DrawWrappedHeaderGUI()
        {
            if(headerElement is null || component == null)
            {
                return;
            }

            Rect headerRect = headerElement.contentRect;
            bool HeaderIsSelected = headerElement.focusController.focusedElement == headerElement;
            ComponentHeaderGUIUtility.BeginComponentHeader(component, headerRect, HeaderIsSelected, supportsRichText);
            wrappedOnGUIHandler?.Invoke();
        }
    }
}