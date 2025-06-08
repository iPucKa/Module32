using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastructure.EntryPoint
{
	public class GameEntryPoint : MonoBehaviour
	{
		private void Awake()
		{
			Debug.Log("Старт проекта, сетап настроек");
			SetupAppSettings();

			Debug.Log("Процесс регистрации сервисов всего проекта");

			DIContainer projectContainer = new DIContainer();
			ProjectContextRegistrations.Process(projectContainer);

			projectContainer.Resolve<ICoroutinesPerformer>().StartPerform(Initialize(projectContainer));
		}

		private void SetupAppSettings()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;
		}

		private IEnumerator Initialize(DIContainer container)
		{
			//Debug.Log("Открывается штора загрузки");
			ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();
			SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();

			loadingScreen.Show();

			Debug.Log("Начинается инициализация сервисов");

			yield return container.Resolve<ConfigsProviderService>().LoadAsync();

			yield return new WaitForSeconds(1f);

			Debug.Log("Завершается инициализация сервисов");

			//Debug.Log("Закрывается штора загрузки");
			loadingScreen.Hide();

			//Debug.Log("Начинается переход на какую-то сцену");
			yield return sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu);
		}
	}
}