using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayInputArgs : IInputSceneArgs
	{
		private List<char> _symbolsForGenerating;

		public GameplayInputArgs(List<char> symbols)
		{
			_symbolsForGenerating = new List<char> (symbols);
		}

		public List<char> Symbols => _symbolsForGenerating;
	}
}
