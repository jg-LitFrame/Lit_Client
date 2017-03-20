using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{
    public partial class LitLua : LitBehaviour {

        public List<string> events = new List<string>();

        public void RegistEvent(EventEntity litEvent)
        {
            events.Add(litEvent.HandleFunc);
        }
    }
}
