using Assets._Project.Develop.Runtime.Gameplay.PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Infrastructure.GameRules
{
	internal class MatchSymbolsRule : IRule
	{
		public event Action IsMatch;
		public event Action IsNotMatch;

		private const int StringLength = 5;
		private List<char> _symbols;

		private PlayerInputHandler _playerInputHandler;
		private List<char> _generatedSymbols = new();

		private string _generatedString;

		public MatchSymbolsRule(List<char> symbols, PlayerInputHandler playerInputHandler)
		{
			_symbols = new List<char>(symbols);
			_playerInputHandler = playerInputHandler;

			_playerInputHandler.IsTyped += OnPlayerFinishedTyping;

			_generatedSymbols = GenerateFromList();
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
			foreach (char symbol in _generatedSymbols)
				_generatedString += symbol;

			Debug.Log("Точно повторите последовательность: " + _generatedString);
		}

		private List<char> GenerateFromList()
		{
			for (int i = 0; i < StringLength; i++)
			{
				int index = Random.Range(0, _symbols.Count);
				_generatedSymbols.Add(_symbols[index]);
			}

			return _generatedSymbols;
		}		
	}
}
