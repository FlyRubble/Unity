using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Framework
{
    using Event;
	namespace IO
	{
		/// <summary>
		/// INI
		/// </summary>
		public class Ini
		{
			#region Variable
			/// <summary>
			/// 路径
			/// </summary>
			protected string m_path = string.Empty;

			/// <summary>
			/// 数据
			/// </summary>
			Dictionary<string, IniNodeList> m_data = new Dictionary<string, IniNodeList>(64);
			#endregion

			#region Property
			/// <summary>
			/// 得到所有的节点表名
			/// </summary>
			/// <value>The name list.</value>
			public List<string> nameList
			{
				get
				{
					return new List<string>(m_data.Keys);
				}
			}

			/// <summary>
			/// 得到所有节点表个数
			/// </summary>
			public int Count
			{
				get
				{
					return m_data.Count;
				}
			}
			#endregion

			#region Function
			/// <summary>
			/// 构造
			/// </summary>
			public Ini() { }

			/// <summary>
			/// 构造
			/// </summary>
			/// <param name="path">路径</param>
			public Ini(string path)
			{
				m_path = path;
			}

			/// <summary>
			/// 读取
			/// </summary>
			public void Read()
			{
				if (File.Exists(m_path))
				{
					// 读取开始
					using (StreamReader stream = new StreamReader (m_path)) {
						Read (stream.ReadToEnd());
						stream.Dispose();
						stream.Close();
                    }
				}
			}

			/// <summary>
			/// 读取
			/// </summary>
			/// <param name="text">Text.</param>
			public virtual void Read(string text)
			{
				// 清除原有的数据
				m_data.Clear();
				// 读取开始
				using (StringReader stream = new StringReader (text)) {
					IniNodeList nodeList = new IniNodeList ();
					m_data.Add(nodeList.name, nodeList);
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
							string name = text.Substring(1, text.Length - 2);
							if (m_data.ContainsKey(name))
							{
								nodeList = m_data[name];
							}
							else
							{
								nodeList = new IniNodeList ();
								m_data.Add(name, nodeList);
							}
						}
						// 节点
						else if (text.IndexOf("=") > 0)
						{
							int index = text.IndexOf("=");
							string[] key = text.Substring(0, index).Split ('|');

							IniNode node = new IniNode ();
							node.name = key [0];
							node.value = text.Substring(index + 1, text.Length - index - 1);
							for (int i = 1; i < key.Length; ++i) {
								string[] attr = key [i].Split (':');
								if (attr.Length == 2) {
									node.AppendAttribute (attr[0], attr[1]);
								}
							}
                            
                            nodeList.Append(node);
                        }
					}
					stream.Dispose();
					stream.Close();
				}
			}

            /// <summary>
            /// 异步读取
            /// </summary>
            /// <param name="text"></param>
            /// <param name="update"></param>
            /// <returns></returns>
            public IEnumerator AsynRead(string text, Action<float> update)
            {
                yield return AsynRead(text, update, null);
            }

            /// <summary>
            /// 异步读取
            /// </summary>
            /// <param name="text"></param>
            /// <param name="complete"></param>
            /// <returns></returns>
            public IEnumerator AsynRead(string text, Action complete)
            {
                yield return AsynRead(text, null, complete);
            }

            /// <summary>
            /// 异步读取
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public IEnumerator AsynRead(string text)
            {
                yield return AsynRead(text, null, null);
            }

            /// <summary>
            /// 异步读取
            /// </summary>
            /// <param name="text"></param>
            /// <param name="update"></param>
            /// <param name="complete"></param>
            /// <returns></returns>
            public virtual IEnumerator AsynRead(string text, Action<float> update, Action complete)
            {
                // 清除原有的数据
                m_data.Clear();
                // 读取开始
                using (StringReader stream = new StringReader(text))
                {
                    int current = default(int);
                    int length = text.Length;
                    int line = default(int);

                    IniNodeList nodeList = new IniNodeList();
                    m_data.Add(nodeList.name, nodeList);
                    while (stream.Peek() != -1)
                    {
                        text = stream.ReadLine();
                        
                        current += text.Length + 2;
                        if (++line % 900 == 0 && null != update)
                        {
                            yield return 0;
                            update(1f * current / length);
                        }

                        // 注释判定
                        if (text.StartsWith(";"))
                        {
                            continue;
                        }
                        // 节表头判定
                        else if (text.StartsWith("[") && text.EndsWith("]"))
                        {
                            string name = text.Substring(1, text.Length - 2);
                            if (m_data.ContainsKey(name))
                            {
                                nodeList = m_data[name];
                            }
                            else
                            {
                                nodeList = new IniNodeList();
                                m_data.Add(name, nodeList);
                            }
                        }
                        // 节点
                        else if (text.IndexOf("=") > 0)
                        {
                            int index = text.IndexOf("=");
                            string[] key = text.Substring(0, index).Split('|');

                            IniNode node = new IniNode();
                            node.name = key[0];
                            node.value = text.Substring(index + 1, text.Length - index - 1);
                            for (int i = 1; i < key.Length; ++i)
                            {
                                string[] attr = key[i].Split(':');
                                if (attr.Length == 2)
                                {
                                    node.AppendAttribute(attr[0], attr[1]);
                                }
                            }

                            nodeList.Append(node);
                        }
                    }
                    stream.Dispose();
                    stream.Close();

                    yield return 0;
                    if (null != update)
                    {
                        update(1f * current / length);
                    }
                    if (null != complete)
                    {
                        complete();
                    }
                }
            }

            /// <summary>
            /// 异步加载
            /// </summary>
            /// <param name="update"></param>
            /// <returns></returns>
            public IEnumerator AsynRead(Action<float> update)
            {
                yield return AsynRead(update, null);
            }

            /// <summary>
            /// 异步加载
            /// </summary>
            /// <param name="complete"></param>
            /// <returns></returns>
            public IEnumerator AsynRead(Action complete)
            {
                yield return AsynRead(update:null, complete:complete);
            }

            /// <summary>
            /// 异步加载
            /// </summary>
            /// <returns></returns>
            public IEnumerator AsynRead()
            {
                yield return AsynRead(update: null, complete: null);
            }

            /// <summary>
            /// 异步加载
            /// </summary>
            /// <param name="update"></param>
            /// <param name="complete"></param>
            /// <returns></returns>
            public IEnumerator AsynRead(Action<float> update, Action complete)
            {
                if (File.Exists(m_path))
                {
                    // 读取开始
                    using (StreamReader stream = new StreamReader(m_path))
                    {
                        yield return AsynRead(stream.ReadToEnd(), update, complete);
                        stream.Dispose();
                        stream.Close();
                    }
                }
                yield return 0;
            }

            /// <summary>
            /// 写入
            /// </summary>
            /// <param name="path">Path.</param>
            public void Write(string path)
            {
                m_path = path;
                Write();
            }

            /// <summary>
            /// 写入
            /// </summary>
            public virtual void Write()
			{
				StreamWriter stream = new StreamWriter(m_path, false);
				foreach (var kvp in m_data)
				{
                    // 节表名
                    stream.WriteLine("[" + kvp.Key + "]");
                    foreach (var kp in kvp.Value.data.Values)
					{
                        string key = kp.name;
                        foreach (var attr in kp.attribute)
                        {
                            key += "|" + attr.Key + ":" + attr.Value;
                        }
						stream.WriteLine(key + "=" + kp.value);
					}
				}
				stream.Dispose();
				stream.Close();
			}



			/// <summary>
			/// 设置一个Int
			/// </summary>
			/// <param name="section">Section.</param>
			/// <param name="key">Key.</param>
			/// <param name="value">Value.</param>
			public void SetInt(string section, string key, int value)
			{
				SetString (section, key, value.ToString());
			}

			/// <summary>
			/// 设置一个Int
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
			public void SetInt(string key, int value)
			{
				SetString("", key, value.ToString());
			}

			/// <summary>
			/// 设置一个Float
			/// </summary>
			/// <param name="section">Section.</param>
			/// <param name="key">Key.</param>
			/// <param name="value">Value.</param>
			public void SetFloat(string section, string key, float value)
			{
				SetString (section, key, value.ToString());
			}

			/// <summary>
			/// 设置一个Float
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
			public void SetFloat(string key, float value)
			{
				SetString("", key, value.ToString());
			}

			/// <summary>
			/// 设置一个Double
			/// </summary>
			/// <param name="section">Section.</param>
			/// <param name="key">Key.</param>
			/// <param name="value">Value.</param>
			public void SetDouble(string section, string key, double value)
			{
				SetString (section, key, value.ToString());
			}

			/// <summary>
			/// 设置一个Double
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
			public void SetDouble(string key, double value)
			{
				SetString("", key, value.ToString());
			}

			/// <summary>
			/// 设置Bool
			/// </summary>
			/// <param name="key">Key.</param>
			/// <param name="value">If set to <c>true</c> value.</param>
			public void SetBool(string section, string key, bool value)
			{
				SetString(section, key, value ? "true" : "false");
			}

			/// <summary>
			/// 设置Bool
			/// </summary>
			/// <param name="key">Key.</param>
			/// <param name="value">If set to <c>true</c> value.</param>
			public void SetBool(string key, bool value)
			{
				SetString("", key, value ? "true" : "false");
			}

			/// <summary>
			/// 设置一个String
			/// </summary>
			/// <param name="section"></param>
			/// <param name="key"></param>
			/// <param name="value"></param>
			public void SetString(string section, string key, string value)
			{
				IniNodeList nodeList = null;
				// 节
				if (m_data.ContainsKey(section))
				{
                    nodeList = m_data[section];
				}
				else
				{
                    nodeList = new IniNodeList();
					m_data.Add(section, nodeList);
				}
                // 键值
                nodeList.Append(key, value);
			}

			/// <summary>
			/// 设置一个String
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
			public void SetString(string key, string value)
			{
				SetString("", key, value);
			}

            /// <summary>
            /// 添加一个节点表
            /// </summary>
            /// <param name="nodeList"></param>
            public void AppendNodeList(IniNodeList nodeList)
            {
                // 节
                if (m_data.ContainsKey(nodeList.name))
                {
                    m_data[nodeList.name] = nodeList;
                }
                else
                {
                    m_data.Add(nodeList.name, nodeList);
                }
            }

            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="section"></param>
            /// <param name="node"></param>
            public void AppendNode(string section, IniNode node)
            {
                // 节
                if (m_data.ContainsKey(section))
                {
                    m_data[section].Append(node);
                }
                else
                {
                    IniNodeList nodeList = new IniNodeList(section);
                    nodeList.Append(node);
                    m_data.Add(nodeList.name, nodeList);
                }
            }

            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="node"></param>
            public void AppendNode(IniNode node)
            {
                AppendNode("", node);
            }

            /// <summary>
            /// 得到Int
            /// </summary>
            /// <param name="section"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            public int GetInt(string section, string key)
			{
				int value = 0;
				int.TryParse (GetString(section, key), out value);
				return value;
			}

			/// <summary>
			/// 得到Int
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			public int GetInt(string key)
			{
				int value = 0;
				int.TryParse (GetString("", key), out value);
				return value;
			}

			/// <summary>
			/// 得到Float
			/// </summary>
			/// <param name="section"></param>
			/// <param name="key"></param>
			/// <returns></returns>
			public float GetFloat(string section, string key)
			{
				float value = 0;
				float.TryParse (GetString(section, key), out value);
				return value;
			}

			/// <summary>
			/// 得到Float
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			public float GetFloat(string key)
			{
				float value = 0;
				float.TryParse (GetString("", key), out value);
				return value;
			}

			/// <summary>
			/// 得到Double
			/// </summary>
			/// <param name="section"></param>
			/// <param name="key"></param>
			/// <returns></returns>
			public double GetDouble(string section, string key)
			{
				double value = 0;
				double.TryParse (GetString(section, key), out value);
				return value;
			}

			/// <summary>
			/// 得到Double
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			public double GetDouble(string key)
			{
				double value = 0;
				double.TryParse (GetString("", key), out value);
				return value;
			}

			/// <summary>
			/// 得到Bool
			/// </summary>
			/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
			/// <param name="key">Key.</param>
			public bool GetBool(string section, string key)
			{
				bool value = false;
				bool.TryParse (GetString(section, key), out value);
				return value;
			}

			/// <summary>
			/// 得到Bool
			/// </summary>
			/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
			/// <param name="key">Key.</param>
			public bool GetBool(string key)
			{
				bool value = false;
				bool.TryParse (GetString("", key), out value);
				return value;
			}

			/// <summary>
			/// 得到String
			/// </summary>
			/// <param name="section"></param>
			/// <param name="key"></param>
			/// <returns></returns>
			public string GetString(string section, string key)
			{
				string value = null;
				// 节
				if (m_data.ContainsKey(section))
				{
                    var node = m_data[section][key];
                    if (null != node)
                    {
                        value = node.value;
                    }
				}
				return value;
			}

			/// <summary>
			/// 得到String
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			public string GetString(string key)
			{
				return GetString("", key);
			}

            /// <summary>
            /// 得到节点
            /// </summary>
            /// <param name="section"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            public IniNode GetNode(string section, string key)
            {
                IniNode node = null;
                // 节
                if (m_data.ContainsKey(section))
                {
                    node = m_data[section][key];
                }
                return node;
            }

            /// <summary>
            /// 得到节点
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public IniNode GetNode(string key)
            {
                return GetNode("", key);
            }

            /// <summary>
            /// 得到指定节的所有数据
            /// </summary>
            /// <param name="section"></param>
            /// <returns></returns>
            public IniNodeList GetNodeList(string section)
			{
				return m_data.ContainsKey(section) ? m_data[section] : null;
			}
            
			/// <summary>
			/// 是否包含节
			/// </summary>
			/// <returns><c>true</c>, if section was containsed, <c>false</c> otherwise.</returns>
			/// <param name="section">Section.</param>
			public bool Contains(string section)
			{
				return m_data.ContainsKey(section);
			}

			/// <summary>
			/// 包含key
			/// </summary>
			/// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
			/// <param name="key">Key.</param>
			public bool ContainsNode(string key)
			{
				return m_data.ContainsKey("") && m_data[""].Contains(key);
			}

			/// <summary>
			/// 包含key
			/// </summary>
			/// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
			/// <param name="section">Section.</param>
			/// <param name="key">Key.</param>
			public bool ContainsNode(string section, string key)
			{
				return m_data.ContainsKey(section) && m_data[section].Contains(key);
			}

			/// <summary>
			/// 移除数据
			/// </summary>
			public void RemoveNodeList(string section)
			{
				if (null != section && m_data.ContainsKey (section))
				{
					m_data.Remove (section);
				}
			}

            /// <summary>
            /// 移除数据
            /// </summary>
            public void RemoveNodeList(IniNodeList nodeList)
            {
                if (null != nodeList)
                {
                    RemoveNodeList(nodeList.name);
                }
            }

            /// <summary>
            /// 移除数据
            /// </summary>
            /// <param name="section"></param>
            /// <param name="key"></param>
            public void RemoveNode(string section, string key)
            {
                if (null != section && m_data.ContainsKey(section))
                {
                    m_data[section].Remove(key);
                }
            }

            /// <summary>
            /// 移除数据
            /// </summary>
            /// <param name="section"></param>
            /// <param name="node"></param>
            public void RemoveNode(string section, IniNode node)
            {
                if (null != section && m_data.ContainsKey(section))
                {
                    m_data[section].Remove(node);
                }
            }

            /// <summary>
            /// 移除数据
            /// </summary>
            /// <param name="key"></param>
            public void RemoveNode(string key)
            {
                RemoveNode("", key);
            }

            /// <summary>
            /// 移除数据
            /// </summary>
            /// <param name="node"></param>
            public void RemoveNode(IniNode node)
            {
                RemoveNode("", node);
            }

            /// <summary>
            /// 清除数据
            /// </summary>
            public void Clear()
			{
				m_data.Clear ();
			}
			#endregion
		}
	}
}
