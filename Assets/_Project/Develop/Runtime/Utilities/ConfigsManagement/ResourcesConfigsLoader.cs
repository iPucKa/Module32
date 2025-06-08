﻿using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagement
{
	public class ResourcesConfigsLoader : IConfigsLoader
	{
		private readonly ResourcesAssetsLoader _resources;

		private readonly Dictionary<Type, string> _configsResourcesPath = new()
		{
			{typeof(NumberConfig), "NumberConfig" },
			{typeof(CharConfig), "CharConfig"}
		};

		public ResourcesConfigsLoader(ResourcesAssetsLoader resources)
		{
			_resources = resources;
		}

		public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded)
		{
			Dictionary<Type, object> loadedConfigs = new();

			foreach (KeyValuePair<Type, string> configResourcesPath in _configsResourcesPath)
			{
				ScriptableObject config = _resources.Load<ScriptableObject>(configResourcesPath.Value);
				loadedConfigs.Add(configResourcesPath.Key, config);
				yield return null;
			}

			onConfigsLoaded?.Invoke(loadedConfigs);
		}
	}
}
