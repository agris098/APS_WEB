using System;
using System.Timers;
using APS.Models;
using System.Diagnostics;

namespace APS
{
    public class ClassifiedSystemTimer
    {
        private static Timer aTimer;
        private static int _setTime;
        public ClassifiedSystemTimer(int time) {
            _setTime = time * 60000;
            SetTimer();
        }
        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            //Run every 30 min
            aTimer = new Timer(_setTime);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ResolveExpiredClassifieds();
        }
        private static void ResolveExpiredClassifieds() {
            DataAccess obdjs = new DataAccess();
            obdjs.ResolveExpiredClassifieds();
        }
    }
}