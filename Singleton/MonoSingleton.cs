﻿using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
	// Instance functions
	protected static T _Instance;
	public static T Instance {
		get {
			if (_Instance == null) {
				Instantiate();
			}

			return _Instance;
		}
	}

	// Instantiation function if you need to pre-instantiate rather than on demand
	public static void Instantiate() {
		if (_Instance != null) {
			return;
		}

		GameObject newObject = new GameObject(typeof(T).Name);
		GameObject.DontDestroyOnLoad(newObject);

		_Instance = newObject.AddComponent<T>();
	}

	// Use this for initialization before any start methods are called
	protected virtual void Awake() {
		if (_Instance != null) {
			GameObject.DestroyImmediate(gameObject);
			return;
		}

		_Instance = (T)(object)this;
		GameObject.DontDestroyOnLoad(gameObject);
	}
}
