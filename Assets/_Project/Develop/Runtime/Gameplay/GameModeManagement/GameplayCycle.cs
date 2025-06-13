using Assets._Project.Develop.Runtime.Gameplay.GameRules;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Gameplay.PlayerInput;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.GameModeManagement
{
	public class GameplayCycle : IDisposable
	{
		private readonly DIContainer _container;
		private readonly PlayerInputHandler _playerInputHandler;
		private readonly SymbolInputMode _mode;

		private IRule _gameRule;
		private GameMode _gameMode;

		public GameplayCycle(DIContainer container,PlayerInputHandler playerInputHandler, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
				throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)}");

			_playerInputHandler = playerInputHandler;
			_mode = gameplayInputArgs.Mode;
		}

		public IEnumerator Launch()
		{
			_gameRule = new MatchSymbolsRule(_container, _mode, _playerInputHandler);
			_gameMode = new GameMode(_gameRule);

			_gameMode.Win += OnGameModeWin;
			_gameMode.Defeat += OnGameModeDefeat;

			yield return null;
		}

		public void Start()
		{
			_gameRule.Start();
			_gameMode.Start();
		}

		public void Dispose()
		{
			OnGameModeEnded();
		}

		private void OnGameModeDefeat()
		{
			OnGameModeEnded();
			Debug.Log("ПОРАЖЕНИЕ");
			_container.Resolve<ICoroutinesPerformer>().StartPerform(ResetProcess(Scenes.Gameplay));
		}

		private void OnGameModeWin()
		{
			OnGameModeEnded();
			Debug.Log("ПОБЕДА");
			_container.Resolve<ICoroutinesPerformer>().StartPerform(ResetProcess(Scenes.MainMenu));
		}

		private void OnGameModeEnded()
		{
			if (_gameMode != null)
			{
				_gameMode.Win -= OnGameModeWin;
				_gameMode.Defeat -= OnGameModeDefeat;
			}

			if (_gameMode != null)
				_gameRule.Dispose();
		}

		private IEnumerator ResetProcess(string sceneName)
		{
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

			SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();

			_container.Resolve<ICoroutinesPerformer>().StartPerform(sceneSwitcherService.ProcessSwitchTo(sceneName, new GameplayInputArgs(_mode)));
		}		
	}
}
