using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.BT {

	public abstract class BTAction : BTNode {

        private GameObject self;

        public BTAction(GameObject self)
        {
            this.self = self;
        }

        public override void AddChild (BTNode aNode) {
			Debug.LogError("BTAction: Cannot add a node into BTAction.");
		}
		
	}
}