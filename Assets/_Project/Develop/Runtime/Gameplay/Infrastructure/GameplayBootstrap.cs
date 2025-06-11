using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Infrastructure.GameRules;
using Assets._Project.Develop.Runtime.Meta.Infrastructure;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Gameplay.PlayerInput;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayBootstrap : SceneBootstrap
	{
		[SerializeField] private PlayerInputHandler _playerInputHandler;

		private DIContainer _container;
		private GameplayInputArgs _inputArgs;

		private IRule _gameRule;
		private GameMode _gameMode;

		public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
		{
			_container = container;

			if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
				throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)}");

			_inputArgs = gameplayInputArgs;
		}

		public override IEnumerator Initialize()
		{
			_gameRule = new MatchSymbolsRule(_inputArgs.Symbols, _playerInputHandler);
			_gameMode = new GameMode(_gameRule);

			_gameMode.Win += OnGameModeWin;
			_gameMode.Defeat += OnGameModeDefeat;

			Debug.Log("Инициализация геймплейной сцены");
			yield break;
		}

		public override void Run()
		{
			Debug.Log("Старт геймплейной сцены");

			_gameRule.Start();
			_gameMode.Start();
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

			_container.Resolve<ICoroutinesPerformer>().StartPerform(sceneSwitcherService.ProcessSwitchTo(sceneName, new GameplayInputArgs(_inputArgs.Symbols)));
		}
	}
}
