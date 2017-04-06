using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.BT {

	/// <summary>
	/// BTParallelFlexible evaluates all children, if all children fails evaluation, it fails. 
	/// Any child passes the evaluation will be regarded as active.
	/// 
	/// BTParallelFlexible ticks all active children, if all children ends, it ends.
	/// 
	/// NOTE: Order of child node added does matter!
	/// </summary>
	public class BTParallelFlexible : BTNode {

        public override BTResult Tick () {
            if (children == null || children.Count == 0) return BTResult.Failure;
			int failsCount = 0;
            for (int i=0; i<children.Count; i++) {
                var rst = children[i].Tick();
                if (rst == BTResult.Failure)
                    failsCount++;
			}
			if (failsCount == children.Count) {
				return BTResult.Failure;
			}
			return BTResult.Running;
		}

	}

}