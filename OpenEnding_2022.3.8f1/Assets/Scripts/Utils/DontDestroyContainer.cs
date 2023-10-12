public class DontDestroyContainer : Singleton<DontDestroyContainer>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
