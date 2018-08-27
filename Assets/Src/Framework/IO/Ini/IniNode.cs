using System.Collections.Generic;

namespace Framework
{
	namespace IO
	{
		/// <summary>
		/// 节点
		/// </summary>
		public sealed class IniNode
		{
			#region Variable
			/// <summary>
			/// 节点名字
			/// </summary>
			private string m_name = string.Empty;

			/// <summary>
			/// 节点值
			/// </summary>
			private string m_value = string.Empty;

			/// <summary>
			/// 属性
			/// </summary>
			private Dictionary<string, string> m_attribute = new Dictionary<string, string>(2);
			#endregion

			#region Property
			/// <summary>
			/// 节点名字
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
			/// 节点值
			/// </summary>
			/// <value>The value.</value>
			public string value
			{
				get
				{
					return m_value;
				}
				set
				{
					m_value = value;
				}
			}

			/// <summary>
			/// 属性
			/// </summary>
			/// <value>The attribute.</value>
			public Dictionary<string, string> attribute
			{
				get
				{
					return m_attribute;
				}
			}
			#endregion

			#region Function
			/// <summary>
			/// 构造
			/// </summary>
			public IniNode() { }

			/// <summary>
			/// 构造
			/// </summary>
			/// <param name="name">Name.</param>
			public IniNode(string name)
			{
				m_name = name;
			}

            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public IniNode(string name, string value)
            {
                m_name = name;
                m_value = value;
            }

            /// <summary>
            /// 添加属性
            /// </summary>
            /// <param name="name">name.</param>
            /// <param name="value">value.</param>
            public void AppendAttribute(string name, string value)
			{
				if (m_attribute.ContainsKey (name)) {
					m_attribute [name] = value;
				} else {
					m_attribute.Add (name, value);
				}
			}

			/// <summary>
			/// 移除属性
			/// </summary>
			/// <param name="name">name.</param>
			public void RemoveAttribute(string name)
			{
				if (m_attribute.ContainsKey (name)) {
					m_attribute.Remove (name);
				}
			}

			/// <summary>
			/// 是否包含此属性
			/// </summary>
			/// <returns><c>true</c> if this instance is contain attribute the specified attributeName; otherwise, <c>false</c>.</returns>
			/// <param name="name">name.</param>
			public bool ContainsAttribute(string name)
			{
				return m_attribute.ContainsKey (name);
			}

			/// <summary>
			/// 得到属性
			/// </summary>
			/// <returns>The attribute.</returns>
			/// <param name="name">name.</param>
			public string GetAttribute(string name)
			{
				return m_attribute.ContainsKey (name) ? m_attribute[name] : null;
			}

			/// <summary>
			/// 清空属性
			/// </summary>
			public void ClearAttribute()
			{
				m_attribute.Clear ();
			}
			#endregion
		}
	}
}
