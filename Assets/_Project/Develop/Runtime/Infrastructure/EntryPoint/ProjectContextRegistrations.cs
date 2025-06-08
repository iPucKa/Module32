using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Infrastructure.EntryPoint
{
	public class ProjectContextRegistrations
	{
		public static void Process(DIContainer container)
		{
			//Регистрации
			container.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);
			container.RegisterAsSingle(CreateConfigsProviderService);
			container.RegisterAsSingle(CreateResourcesAssetsLoader);
			container.RegisterAsSingle(CreateSceneLoaderService);
			container.RegisterAsSingle(CreateSceneSwitcherService);
			container.RegisterAsSingle<ILoadingScreen>(CreateLoadingScreen);
		}

		//Способ создания
		private static SceneSwitcherService CreateSceneSwitcherService(DIContainer c)
			=> new SceneSwitcherService(
				c.Resolve<SceneLoaderService>(),
				c.Resolve<ILoadingScreen>(),
				c);

		//Способ создания
		private static SceneLoaderService CreateSceneLoaderService(DIContainer c)
			=> new SceneLoaderService();

		//Способ создания
		private static ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c) => new ResourcesAssetsLoader();

		//Способ создания
		private static ConfigsProviderService CreateConfigsProviderService(DIContainer c)
		{
			ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

			ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(resourcesAssetsLoader);

			return new ConfigsProviderService(resourcesConfigsLoader);
		}

		//Способ создания
		private static CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
		{
			ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

			CoroutinesPerformer coroutinesPerformerPrefab = resourcesAssetsLoader
				.Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

			return Object.Instantiate(coroutinesPerformerPrefab);
		}

		//Способ создания
		private static StandardLoadingScreen CreateLoadingScreen(DIContainer c)
		{
			ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

			StandardLoadingScreen standartLoadingScreenPrefab = resourcesAssetsLoader
				.Load<StandardLoadingScreen>("Utilities/StandardLoadingScreen");

			return Object.Instantiate(standartLoadingScreenPrefab);
		}
	}
}
