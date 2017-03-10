using UnityEngine;
using System.Collections;

namespace Lit.Unity
{
    public delegate void _D_Void();
    public delegate void _D_Void<T>(T t);
    public delegate void _D_Void<T1, T2>(T1 t1, T2 t2);
    public delegate void _D_Void_Params(string s, params object[] args);
     
    public delegate int _D_OuterInt();

    public delegate bool _D_OuterBool();

    public delegate float _D_OuterFloat();
  
    public delegate string _D_OuterString();

    
}
