using System;

namespace UnityEngine.Events
{
  [Serializable]
  public class CustomUnityEvent : UnityEvent { }
  [Serializable]
  public class CustomUnityEvent<T> : UnityEvent<T> { }
  [Serializable]
  public class CustomUnityEvent<T0, T1> : UnityEvent<T0, T1> { }
}