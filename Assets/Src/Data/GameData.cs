using Framework.Singleton;

public class GameData : Singleton<GameData>
{
    #region Variable
    private LangConfig m_langConfig = new LangConfig();
    #endregion

    #region Property
    public LangConfig langConfig
    {
        get
        {
            return m_langConfig;
        }
    }
    #endregion
}
