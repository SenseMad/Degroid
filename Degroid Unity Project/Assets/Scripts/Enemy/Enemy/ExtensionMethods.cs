using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
	static List<Component> m_ComponentCache = new List<Component>();

	public static Component GetComponentNoAlloc(this GameObject @this, System.Type componentType)
	{
		@this.GetComponents(componentType, m_ComponentCache);
		var component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
		m_ComponentCache.Clear();
		return component;
	}

	public static T GetComponentNoAlloc<T>(this GameObject @this) where T : Component
	{
		@this.GetComponents(typeof(T), m_ComponentCache);
		var component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
		m_ComponentCache.Clear();
		return component as T;
	}
}