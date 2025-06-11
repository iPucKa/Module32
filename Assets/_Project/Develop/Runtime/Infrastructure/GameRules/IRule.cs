using System;

namespace Assets._Project.Develop.Runtime.Infrastructure.GameRules
{
	public interface IRule : IDisposable
	{
		event Action IsMatch;
		event Action IsNotMatch;

		void Start();
	}
}
