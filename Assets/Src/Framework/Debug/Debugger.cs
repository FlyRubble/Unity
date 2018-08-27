using System;
using System.Diagnostics;

namespace Common
{
	/// <summary>
	/// 调试器
	/// </summary>
	public class Debugger
	{
		#region Variable
		/// <summary>
		/// 是否显示日志
		/// </summary>
		static bool m_isDebug = true;

		/// <summary>
		/// 是否开启Web日志
		/// </summary>
		static bool m_isWebDebug;
		#endregion
		
		#region Property
		/// <summary>
		/// 是否需要Debug日志
		/// </summary>
		/// <value><c>true</c> if is debug; otherwise, <c>false</c>.</value>
		public static bool isDebug
		{
			get { return m_isDebug; }
			set { m_isDebug = value; }
		}

		/// <summary>
		/// 是否需要Web日志
		/// </summary>
		/// <value><c>true</c> if is web debug; otherwise, <c>false</c>.</value>
		public static bool isWebDebug
		{
			get { return m_isWebDebug; }
			set { m_isWebDebug = value; }
		}
		#endregion

		#region Function
		/// <summary>
		/// 日志
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Log(object message)
		{
			if (m_isDebug)
			{
				UnityEngine.Debug.Log(message);
			}
			if (m_isWebDebug)
			{
				WebDebug(message);
			}
		}

		/// <summary>
		/// 错误日志
		/// </summary>
		/// <param name="message">Message.</param>
		public static void LogError(object message)
		{
			if (m_isDebug)
			{
				UnityEngine.Debug.LogError(message);
			}
			if (m_isWebDebug)
			{
				WebDebug(message);
			}
		}

		/// <summary>
		/// 异常日志
		/// </summary>
		/// <param name="exception">Exception.</param>
		public static void LogException(Exception exception)
		{
			if (m_isDebug)
			{
				UnityEngine.Debug.LogException(exception);
			}
			if (m_isWebDebug)
			{
				WebDebug(exception);
			}
		}

		/// <summary>
		/// 警告日志
		/// </summary>
		/// <param name="message">Message.</param>
		public static void LogWarning(object message)
		{
			if (m_isDebug)
			{
				UnityEngine.Debug.LogWarning(message);
			}
			if (m_isWebDebug)
			{
				WebDebug(message);
			}
		}

		/// <summary>
		/// Web日志
		/// </summary>
		/// <param name="message">Message.</param>
		static void WebDebug(object message)
		{
			StackTrace st = new StackTrace(1, true);
			string type = st.GetFrame(0).GetMethod().Name;
			UnityEngine.Debug.Log(type + message + st.ToString());
		}
		#endregion
	}
}