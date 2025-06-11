using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
	public class MainMenuBootstrap : SceneBootstrap
	{
		private DIContainer _container;

		public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			MainMenuContextRegistrations.Process(_container);
		}

		public override IEnumerator Initialize()
		{
			Debug.Log("Инициализация сцены меню");
			yield break;
		}

		public override void Run()
		{
			Debug.Log("Старт сцены меню");
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Alpha1))
			{
				IConfig config = _container.Resolve<ConfigsProviderService>().GetConfig<NumberConfig>();

				Debug.Log("Принят файл настроек генерации цифр");
				SetupGameplayMode(config);
				return;
			}

			if (Input.GetKeyUp(KeyCode.Alpha2))
			{
				IConfig config = _container.Resolve<ConfigsProviderService>().GetConfig<CharConfig>();

				Debug.Log("Принят файл настроек генерации букв");
				SetupGameplayMode(config);
				return;
			}
		}

		private void SetupGameplayMode(IConfig config)
		{
			SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
			
			_container.Resolve<ICoroutinesPerformer>().StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(config.CharsForGenerate)));
		}
	}
}
