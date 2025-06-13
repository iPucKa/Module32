using Assets._Project.Develop.Runtime.Gameplay.GameRules;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.GameModeManagement
{
	public class GameMode
	{
		public event Action Win;
		public event Action Defeat;

		private readonly IRule _gameRule;

		public GameMode(IRule condition)
		{
			_gameRule = condition;
		}

		public void Start()
		{
			_gameRule.IsMatch += OnWinConditionCompleted;
			_gameRule.IsNotMatch += OnDefeatConditionCompleted;
		}		

		private void OnDefeatConditionCompleted() => ProcessDefeat();		

		private void OnWinConditionCompleted() => ProcessWin();		

		private void ProcessDefeat()
		{
			ProcessEndGame();
			Defeat?.Invoke();
		}

		private void ProcessWin()
		{
			ProcessEndGame();
			Win?.Invoke();
		}

		private void ProcessEndGame()
		{
			_gameRule.IsMatch -= OnWinConditionCompleted;
			_gameRule.IsNotMatch -= OnDefeatConditionCompleted;
		}
	}
}
