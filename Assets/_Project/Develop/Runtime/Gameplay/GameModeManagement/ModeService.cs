using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.GameModeManagement
{
	public class ModeService
	{
		private readonly DIContainer _container;

		private GameplayInputArgs _args;

		public ModeService(DIContainer container)
		{
			_container = container;
		}

		public IEnumerator SelectMode()
		{
			while (true)
			{
				if (Input.GetKeyUp(KeyCode.Alpha1))
				{
					_args = new GameplayInputArgs(SymbolInputMode.Numbers);
					MoveToGameplayScene();
					Debug.Log("Выбран режим генерации цифр");

					yield break;
				}

				if (Input.GetKeyUp(KeyCode.Alpha2))
				{
					_args = new GameplayInputArgs(SymbolInputMode.Chars);
					MoveToGameplayScene();
					Debug.Log("Выбран режим генерации букв");

					yield break;
				}

				yield return null;
			}					
		}

		private void MoveToGameplayScene()
		{
			SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();

			_container.Resolve<ICoroutinesPerformer>().StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, _args));
		}
	}
}
