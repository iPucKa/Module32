using Assets._Project.Develop.Runtime.Gameplay.GameModeManagement;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
	public class MainMenuContextRegistrations
	{
		public static void Process(DIContainer container)
		{
			//Регистрации			
			container.RegisterAsSingle(CreateModeService);

			Debug.Log("Процесс регистрации сервисов на сцене меню");
		}

		private static ModeService CreateModeService(DIContainer c) 
			=> new ModeService(c);		
	}
}
