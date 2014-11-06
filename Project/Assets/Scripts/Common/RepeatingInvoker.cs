using System;

using UnityEngine;

namespace Assets.Scripts.Common
{
    public class RepeatingInvoker
    {
        private readonly float maxTime;

        private readonly Action actionToInvoke;

        private float currentTime;

        public RepeatingInvoker(float interval, Action actionToInvoke)
        {
            this.currentTime = 0;
            this.maxTime = interval;
            this.actionToInvoke = actionToInvoke;
            this.IsRunning = false;
        }

        public bool IsRunning { get; private set; }

        public float Progress
        {
            get
            {
                return this.currentTime / this.maxTime;
            }
        }

        public void Start()
        {
            this.currentTime = 0;
            this.IsRunning = true;
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        public void Update()
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.currentTime += Time.deltaTime;

            if (this.currentTime < this.maxTime)
            {
                return;
            }

            this.actionToInvoke.Invoke();
            this.currentTime = 0;
        }
    }
}