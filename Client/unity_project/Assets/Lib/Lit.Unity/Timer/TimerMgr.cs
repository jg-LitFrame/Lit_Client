using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Lit.Unity
{
    public class TimerMgr : SingletonBehaviour<TimerMgr>{
        private List<TimerEvent> eventLists = new List<TimerEvent>();
        private List<TimerEvent> invalidEvents = new List<TimerEvent>();

        public void RegistTimer(TimerEvent e)
        {
            eventLists.Add(e);
        }

        public TimerEvent RegistTimer(_D_Void onHandler, float interval = 0, int execTimes = 1)
        {
            TimerEvent e = new TimerEvent(onHandler, interval, execTimes);
            RegistTimer(e);
            return e;
        }

        public void RemoveTimer(TimerEvent e)
        {
            eventLists._Remove(e);
        }

        private void Update()
        {
            for (int i = 0; i < eventLists.Count; i++)
            {
                Tick(eventLists[i]);
            }
            if(invalidEvents.Count > 0) {
                ClearEvents();
            }
        }

        private void Tick(TimerEvent e)
        {
            if (!e.CheckValid())
                invalidEvents.Add(e);
            else
                e.Tick();
        }

        private void ClearEvents()
        {
            for (int i = 0; i < invalidEvents.Count; i++)
            {
                RemoveTimer(invalidEvents[i]);
            }
            invalidEvents.Clear();
        }
    }
}
