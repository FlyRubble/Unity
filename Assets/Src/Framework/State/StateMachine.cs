namespace Framework
{
    using Singleton;
    public class StateMachine : Singleton<StateMachine>
    {
        #region Variable
        /// <summary>
        /// 当前状态
        /// </summary>
        private State m_current = null;
        #endregion

        #region Property
        /// <summary>
        /// 得到当前状态
        /// </summary>
        public State current
        {
            get
            {
                return m_current;
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="param"></param>
        public void OnEnter(State state, Param param = null)
        {
            if (m_current != null)
            {
                m_current.OnExit(param);
            }
            m_current = state;
            if (m_current != null)
            {
                m_current.OnEnter(param);
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void Update()
        {
            m_current.Update();
        }
        #endregion
    }
}
