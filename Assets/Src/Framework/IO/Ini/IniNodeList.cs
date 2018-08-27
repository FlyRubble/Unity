using System.Collections.Generic;

namespace Framework
{
	namespace IO
	{
		/// <summary>
		/// 节点表
		/// </summary>
		public sealed class IniNodeList
		{
			#region Variable
			/// <summary>
			/// 节点表名字
			/// </summary>
			private string m_name = string.Empty;

			/// <summary>
			/// 节点表数据
			/// </summary>
			private Dictionary<string, IniNode> m_data = new Dictionary<string, IniNode>(4);
			#endregion

			#region Property
			/// <summary>
			/// 节点表名字
			/// </summary>
			/// <value>The name.</value>
			public string name
			{
				get
				{
					return m_name;
				}
				set
				{
					m_name = value;
				}
			}

			/// <summary>
			/// 节点表数据
			/// </summary>
			/// <value>The data.</value>
			public Dictionary<string, IniNode> data
			{
				get
				{
					return m_data;
				}
			}

			/// <summary>
			/// 得到节点
			/// </summary>
			/// <param name="name">Name.</param>
			public IniNode this[string name]
			{
				get { return m_data.ContainsKey (name) ? m_data[name] : null; }
			}
			#endregion

			#region Function
			/// <summary>
			/// 构造
			/// </summary>
			public IniNodeList() { }

			/// <summary>
			/// 构造
			/// </summary>
			/// <param name="name">Name.</param>
			public IniNodeList(string name)
			{
				m_name = name;
			}

            /// <summary>
            /// 添加一个节点
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public void Append(string name, string value)
            {
                if (m_data.ContainsKey(name))
                {
                    m_data[name].value = value;
                }
                else
                {
                    m_data.Add(name, new IniNode(name, value));
                }
            }

            /// <summary>
            /// 添加一个节点
            /// </summary>
            /// <param name="node">Node.</param>
            public void Append(IniNode node)
			{
				if (m_data.ContainsKey (node.name)) {
					m_data [node.name] = node;
				} else {
					m_data.Add (node.name, node);
				}
			}

			/// <summary>
			/// 移除节点
			/// </summary>
			/// <param name="name">name.</param>
			public void Remove(string name)
			{
				if (m_data.ContainsKey (name)) {
					m_data.Remove (name);
				}
			}

			/// <summary>
			/// 移除节点
			/// </summary>
			/// <param name="node">Node.</param>
			public void Remove(IniNode node)
			{
				if (m_data.ContainsKey (node.name)) {
					m_data.Remove (node.name);
				}
			}

			/// <summary>
			/// 是否包含此节点
			/// </summary>
			/// <returns><c>true</c> if this instance is contain the specified nodeName; otherwise, <c>false</c>.</returns>
			/// <param name="name">name.</param>
			public bool Contains(string name)
			{
				return m_data.ContainsKey (name);
			}

			/// <summary>
			/// 清空节点表
			/// </summary>
			public void Clear()
			{
				m_data.Clear ();
			}
			#endregion
		}
	}
}
