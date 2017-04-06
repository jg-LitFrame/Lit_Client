using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.BT {

	public class BTSelector : BTNode {
        private int runingNodeIndex = 0;

        public override BTResult Tick () {
            if (children == null || children.Count == 0) return BTResult.Failure;
            BTResult result = BTResult.Failure;
    
            int startIndex = runingNodeIndex;
            runingNodeIndex = 0;

            for (int i = startIndex; i < children.Count; i++)
            {
                BTResult tmpRst = children[i].Tick();
                if(tmpRst == BTResult.Running)
                {
                    result = tmpRst;
                    runingNodeIndex = i;
                    break;
                }
                if(tmpRst == BTResult.Success)
                {
                    runingNodeIndex = 0;
                    result = tmpRst;
                    break;
                }
            }
            return result;
		}
	}
}