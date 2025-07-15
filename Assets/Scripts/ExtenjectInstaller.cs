using Zenject;

public class ExtenjectInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<GameController>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<Market>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}