using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.BT
{

    public abstract class BTTree : MonoBehaviour {
	    protected BTNode _root = null;

	    public bool isRunning = true;
        public float tickInterval = 0.2f;
        private float lastTickTime = 0;


        /// <summary>
        /// 组装BT
        /// </summary>
        public virtual void Init() { }

	    void Awake () {

		    Init();
		    _root.Activate();
	    }

	    void Update () {
		    if (!isRunning) return;

            if(lastTickTime < Time.time)
            {
			     _root.Tick();
                lastTickTime = Time.time + tickInterval;
            }
	    }
    }
}

