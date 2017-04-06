using UnityEngine;
using System.Collections;
using System;

namespace Lit.BT {


    public abstract class BTPrecondition : BTNode {

        public abstract bool Check ();

		public override BTResult Tick () {
			bool success = Check();
            return success ? BTResult.Success : BTResult.Failure;
		}
	}
	
}