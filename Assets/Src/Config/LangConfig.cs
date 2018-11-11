using System.Collections;
using System.Collections.Generic;
using Framework.JsonFx;

public class LangConfig : JsonConfig
{
    public class Language
    {
        private string m_uuid = string.Empty;
        private string m_cn = string.Empty;

        public string uuid
        {
            get
            {
                return m_uuid;
            }
            protected set
            {
                m_uuid = value;
            }
        }
    }

    private Dictionary<string, Language> m_language = new Dictionary<string, Language>();
    public List<Language> language;
    //public Dictionary<string, Language> language
    //{
    //    get
    //    {
    //        return m_language;
    //    }
    //    protected set
    //    {
    //        m_language = value;
    //    }
    //}

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="text"></param>
    public override void Init(string text)
    {
        var data = JsonReader.Deserialize<LangConfig>(text);
        m_language = data.m_language;
    }
}