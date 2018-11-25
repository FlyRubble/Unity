/// <summary>
/// Json配置基类
/// </summary>
public abstract class JsonConfig
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="text"></param>
    public abstract void Init(string text);

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected string Decrypt(string text)
    {
        return text;
    }
}