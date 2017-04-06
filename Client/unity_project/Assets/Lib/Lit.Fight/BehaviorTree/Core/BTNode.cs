using UnityEngine;
using System.Collections.Generic;

namespace Lit.BT {

	/// <summary>
	/// BT node is the base of any nodes in BT framework.
	/// </summary>
	public abstract class BTNode {

        public static GameObject self;

        public string name;
        protected List<BTNode> _children;
		public List<BTNode> children {get{return _children;}}


        public abstract BTResult Tick();

        public virtual void Activate () {}
		
		public virtual void AddChild (BTNode aNode) {
			if (_children == null) {
				_children = new List<BTNode>();	
			}
			if (aNode != null) {
				_children.Add(aNode);
			}
		}

        #region Log For Debug
        public void Log(string info, params object[] ps)
        {
            if (BTConfiguration.ENABLE_BTACTION_LOG)
            {
                Debug.LogFormat(info, ps);
            }
        }
        public void Error(string info, params object[] ps)
        {
            if (BTConfiguration.ENABLE_BTACTION_LOG)
            {
                Debug.LogErrorFormat(info, ps);
            }
        }
        public void Warring(string info, params object[] ps)
        {
            if (BTConfiguration.ENABLE_BTACTION_LOG)
            {
                Debug.LogWarningFormat(info, ps);
            }
        }

        #endregion
    }

    public enum BTResult {
        Failure = 1,
        Running = 2,
        Success = 3,
    }
}