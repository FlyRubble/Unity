using System.Collections;
using System.Collections.Generic;
using Framework.JsonFx;

public class LangConfig : JsonConfig
{
    private Dictionary<string, string> m_language = null;

    /// <summary>
    /// 是否包含语言
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public bool Contains(string uuid)
    {
        return m_language.ContainsKey(uuid);
    }

    /// <summary>
    /// 得到语言
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public string GetLang(string uuid)
    {
        return m_language.ContainsKey(uuid) ? m_language[uuid] : null;
    }

    /// <summary>
    /// 尝试得到语言配置值
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetLang(string uuid, out string value)
    {
        return m_language.TryGetValue(uuid, out value);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="text"></param>
    public override void Init(string text)
    {
        var data = JsonReader.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(Decrypt(text));
        if (data != null && data.ContainsKey("language"))
        {
            var language = data["language"];
            if (language != null)
            {
                m_language = new Dictionary<string, string>();
                string key = string.Empty, value = string.Empty;
                for (int i = 0; i < language.Count; ++i)
                {
                    if (language[i].TryGetValue("uuid", out key) && language[i].TryGetValue("value", out value))
                    {
                        if (!m_language.ContainsKey(key))
                        {
                            m_language.Add(key, value);
                        }
                    }
                }
            }
        }
    }
}