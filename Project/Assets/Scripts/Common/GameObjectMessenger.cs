using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Assets.Scripts.Common
{
    public class GameObjectMessenger
    {
        private readonly Dictionary<Type, List<WeakAction>> registry;

        public GameObjectMessenger()
        {
            this.registry = new Dictionary<Type, List<WeakAction>>();
        }

        public void Register<T>(object recipient, Action<T> action)
        {
            var messageType = typeof(T);
            var reference = new WeakAction<T>(recipient, action);

            if (this.registry.ContainsKey(messageType))
            {
                this.registry[messageType].Add(reference);
                return;
            }

            this.registry.Add(messageType, new List<WeakAction> { reference });
        }

        public void Unregister<T>(Action<T> action)
        {
            var messageType = typeof(T);

            if (!this.registry.ContainsKey(messageType))
            {
                return;
            }

            foreach (var weakAction in this.registry[messageType].ToList())
            {
                var weakActionCasted = weakAction as WeakAction<T>;

                if (weakActionCasted == null)
                {
                    continue;
                }

                if (weakActionCasted.Action == action)
                {
                    this.registry[messageType].Remove(weakAction);
                }
            }
        }

        public void Send<T>(T message)
        {
            var messageType = typeof(T);

            if (!this.registry.ContainsKey(messageType))
            {
                //// Debug.LogWarning(string.Format("No actions registered for type '{0}.{1}'", messageType.Namespace, messageType.Name));
                return;
            }

            foreach (var weakAction in this.registry[messageType].Select(x => x as WeakAction<T>))
            {
                weakAction.Execute(message);
            }
        }
    }
}