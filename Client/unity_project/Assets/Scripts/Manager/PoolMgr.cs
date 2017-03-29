using UnityEngine;
using System.Collections;
using Lit.Unity;
using System.Collections.Generic;


public class PoolMgr : SingletonBehaviour<PoolMgr> {

    public static readonly SpawnPoolsDict Pools = new SpawnPoolsDict();
 
}
