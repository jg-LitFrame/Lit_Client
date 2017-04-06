using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lit.BT;

namespace Lit.BT {
	public class BTParallel : BTNode {

        public override BTResult Tick () {
            if (children == null || children.Count == 0) return BTResult.Failure;
            int failureCount = 0;
            int successCount = 0;
            for (int i = 0; i < children.Count; i++)
            {
                var rst = children[i].Tick();
                if (rst == BTResult.Failure)
                    failureCount++;
                else if (rst == BTResult.Success)
                    successCount++;
            }
            if (failureCount > 0)
                return BTResult.Failure;
            if (successCount == children.Count)
                return BTResult.Success;

			return BTResult.Running;
		}

	}

}