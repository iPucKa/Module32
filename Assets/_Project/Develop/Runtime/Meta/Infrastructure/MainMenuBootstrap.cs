using Assets._Project.Develop.Runtime.Gameplay.GameModeManagement;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
	public class MainMenuBootstrap : SceneBootstrap
	{
		private DIContainer _container;
		private ModeService _modeService;

		public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			//MainMenuContextRegistrations.Process(_container);
		}

		public override IEnumerator Initialize()
		{
			Debug.Log("Инициализация сцены меню");

			_modeService = new ModeService(_container);			

			yield break;
		}

		public override void Run()
		{
			Debug.Log("Старт сцены меню");

			_container.Resolve<ICoroutinesPerformer>().StartPerform(_modeService.SelectMode());
		}		
	}
}
