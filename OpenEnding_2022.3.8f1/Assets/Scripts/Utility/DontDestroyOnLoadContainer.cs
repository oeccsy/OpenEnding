public class DontDestroyOnLoadContainer : Singleton<DontDestroyOnLoadContainer>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
