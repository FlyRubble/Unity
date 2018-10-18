using System;
using System.Diagnostics;

namespace Framework
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
		static bool m_logEnabled = true;

		/// <summary>
		/// 是否开启Web日志
		/// </summary>
		static bool m_webLogEnabled = false;
        #endregion

        #region Property
        /// <summary>
        /// 是否需要日志
        /// </summary>
        public static bool logEnabled
        {
			get
            {
                return m_logEnabled && UnityEngine.Debug.unityLogger.logEnabled;
            }
			set
            {
                m_logEnabled = value;
                UnityEngine.Debug.unityLogger.logEnabled = m_logEnabled;
            }
		}

        /// <summary>
        /// 是否需要Web日志
        /// </summary>
        public static bool webLogEnabled
        {
			get { return m_webLogEnabled; }
			set { m_webLogEnabled = value; }
		}
		#endregion

		#region Function
		/// <summary>
		/// 日志
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
            WebDebug(message);
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
            WebDebugFormat(format, args);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">Message.</param>
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
            WebDebug(message);
		}

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="exception">Exception.</param>
        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
            WebDebug(exception);
		}

		/// <summary>
		/// 警告日志
		/// </summary>
		/// <param name="message">Message.</param>
		public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
            WebDebug(message);
		}

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args);
            WebDebugFormat(format, args);
        }

        /// <summary>
        /// Web日志
        /// </summary>
        /// <param name="message">Message.</param>
        static void WebDebug(object message)
		{
            if (m_webLogEnabled)
            {
                StackTrace st = new StackTrace(1, true);
                string type = st.GetFrame(0).GetMethod().Name;
                string value = type + message + st.ToString();
            }
		}

        /// <summary>
        /// Web日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        static void WebDebugFormat(string format, params object[] args)
        {
            if (m_webLogEnabled)
            {
                StackTrace st = new StackTrace(1, true);
                string type = st.GetFrame(0).GetMethod().Name;
                string value = string.Format(type + format + st.ToString(), args);
            }
        }
        #endregion
    }
}