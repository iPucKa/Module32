using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Gameplay.PlayerInput;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.GameRules
{
	internal class MatchSymbolsRule : IRule
	{
		public event Action IsMatch;
		public event Action IsNotMatch;

		private readonly DIContainer _container;
		private readonly PlayerInputHandler _playerInputHandler;
		private readonly SymbolInputMode _mode;

		private const int StringLength = 5;
		private List<char> _initialSymbols = new();

		private List<char> _generatedSymbols = new();

		private string _generatedString;

		public MatchSymbolsRule(DIContainer container, SymbolInputMode mode, PlayerInputHandler playerInputHandler)
		{
			_container = container;	
			_mode = mode;
			_playerInputHandler = playerInputHandler;

			_playerInputHandler.IsTyped += OnPlayerFinishedTyping;			
		}

		public void Dispose()
		{
			_playerInputHandler.IsTyped -= OnPlayerFinishedTyping;
		}

		private void OnPlayerFinishedTyping(string text)
		{
			if (text.Equals(_generatedString))
				IsMatch?.Invoke();

			if (text.Equals(_generatedString) == false)
				IsNotMatch?.Invoke();
		}

		public void Start()
		{
			_initialSymbols = GetFrom(_mode);
			_generatedSymbols = GenerateRandomly();

			foreach (char symbol in _generatedSymbols)
				_generatedString += symbol;

			Debug.Log("Точно повторите последовательность: " + _generatedString);
		}

		private List<char> GenerateRandomly()
		{
			for (int i = 0; i < StringLength; i++)
			{
				int index = Random.Range(0, _initialSymbols.Count);
				_generatedSymbols.Add(_initialSymbols[index]);
			}

			return _generatedSymbols;
		}

		private List<char> GetFrom(SymbolInputMode mode)
		{
			switch (mode)
			{
				case SymbolInputMode.Chars:
					IConfig charConfig = _container.Resolve<ConfigsProviderService>().GetConfig<CharConfig>();
					return charConfig.CharsForGenerate;

				case SymbolInputMode.Numbers:
					IConfig numberConfig = _container.Resolve<ConfigsProviderService>().GetConfig<NumberConfig>();
					return numberConfig.CharsForGenerate;

				default:
					return null;
			}
		}
	}
}
