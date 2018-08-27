using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.IO;

namespace Framework
{
    namespace IO
    {
        /// <summary>
        /// 配置
        /// </summary>
        public class ManifestConfig : Ini
        {
            #region Variable
            /// <summary>
            /// 数据
            /// </summary>
            Dictionary<string, Manifest> m_data = new Dictionary<string, Manifest>(36000);
            #endregion

            #region Property
            /// <summary>
            /// 得到数据
            /// </summary>
            /// <value>The data.</value>
            public Dictionary<string, Manifest> data
            {
                get { return m_data; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造
            /// </summary>
            public ManifestConfig()
                :base()
            { }

            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="path"></param>
            public ManifestConfig(string path)
                : base(path)
            { }

            /// <summary>
            /// 读取
            /// </summary>
            /// <param name="text"></param>
			public override void Read(string text)
            {
                // 清除原有的数据
                m_data.Clear();
                // 读取开始
                using (StringReader stream = new StringReader(text))
                {
                    string name = string.Empty;
                    while (stream.Peek() != -1)
                    {
                        text = stream.ReadLine();
                        // 注释判定
                        if (text.StartsWith(";"))
                        {
                            continue;
                        }
                        // 节表头判定
                        else if (text.StartsWith("[") && text.EndsWith("]"))
                        {
                            name = text.Substring(1, text.Length - 2);
                        }
                        // 节点
                        else if (text.IndexOf("=") > 0)
                        {
                            int index = text.IndexOf("=");
                            string[] key = text.Substring(0, index).Split('|');

                            switch (name)
                            {
                            case "Manifest":
                            {
                                Manifest manifest = new Manifest();
                                manifest.name = key[0];
                                for (int i = 1; i < key.Length; ++i)
                                {
                                    string[] attr = key[i].Split(':');
                                    if (attr.Length == 2)
                                    {
                                        switch (attr[0])
                                        {
                                        case "md5":
                                        {
                                            manifest.MD5 = attr[1];
                                        }
                                        break;
                                        case "size":
                                        {
                                            manifest.size = long.Parse(attr[1]);
                                        }
                                        break;
                                        }
                                    }
                                }
                                var directDependencies = text.Substring(index + 1, text.Length - index - 1).Split('|');
                                foreach (var dependent in directDependencies)
                                {
                                    manifest.directDependencies.Add(dependent);
                                }
                                m_data.Add(manifest.name, manifest);
                            } break;
                            }
                        }
                    }
                    stream.Dispose();
                    stream.Close();
                }
            }

            /// <summary>
            /// 写入
            /// </summary>
            public override void Write()
            {
                StreamWriter stream = new StreamWriter(m_path, false);
                // 节表名
                stream.WriteLine("[Manifest]");
                foreach (var kvp in m_data.Values)
                {
                    string value = kvp.name;
                    value += "|md5:" + kvp.MD5;
                    value += "|size:" + kvp.size;

                    value += "=";
                    for (int i = 0; i < kvp.directDependencies.Count; ++i)
                    {
                        value += kvp.directDependencies[i] + (i + 1 == kvp.directDependencies.Count ? "" : "|");
                    }
                    stream.WriteLine(value);
                }
                stream.Dispose();
                stream.Close();
            }

            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="t">T.</param>
            public void Add(Manifest manifest)
            {
                if (m_data.ContainsKey(manifest.name))
                {
                    m_data[manifest.name] = manifest;
                }
                else
                {
                    m_data.Add(manifest.name, manifest);
                }
            }

            /// <summary>
            /// 得到一份清单
            /// </summary>
            /// <param name="name">Name.</param>
            public Manifest Get(string name)
            {
                return m_data.ContainsKey(name) ? m_data[name] : null;
            }
            #endregion
        }
    }
}