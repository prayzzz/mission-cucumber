using System;
using System.Linq;

using Fasterflect;

using Vexe.Runtime.Extensions;

namespace Assets.Plugins.Vexe.Runtime.Types.Other
{
    /// <summary>
    /// An abstract base class for creating events - derive from it to create your own game events to use
    /// in the generic event system, EventManager.
    /// </summary>
    public abstract class GameEvent
    {
    }

    /// <summary>
    /// A generic event system based on C#'s delegates
    /// Subscribe (add), unsubscribe (remove) and raise (fire) GameEvents
    /// Derive from GameEvent to create your own events
    /// (There's no target differentiation here nor any other constraint, you could target anything)
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// Subscribe (add) a handler to the GameEvent specified by the generic argument `T`
        /// </summary>
        /// <typeparam name="T">The type of GameEvent to unsubscribe from</typeparam>
        /// <param name="handler">The handler that wants to unsubscribe (the handler to be removed)</param>
        public static void Subscribe<T>(Action<T> handler) where T : GameEvent
        {
            EventManagerInternal<T>.Subscribe(handler);
        }

        /// <summary>
        /// Unubscribe (remove) a handler to the GameEvent specified by the generic argument `T`
        /// </summary>
        /// <typeparam name="T">The type of GameEvent to subscribe to</typeparam>
        /// <param name="handler">The handler to subscribe (the handler to be added)</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : GameEvent
        {
            EventManagerInternal<T>.Unsubscribe(handler);
        }

        /// <summary>
        /// Raises the specified event - Resolves the event type at runtime (uses reflection)
        /// </summary>
        public static void DynamicRaise(GameEvent e)
        {
            var type = typeof(EventManagerInternal<>);
            var genType = type.MakeGenericType(e.GetType());
            var raise = genType.DelegateForCallMethod("Raise", Flags.StaticPublic);
            raise.Invoke(null, e);
        }

        /// <summary>
        /// Raise (fire) the GameEvent specified by the generic argument `T`
        /// </summary>
        /// <typeparam name="T">The type of GameEvent to be raised</typeparam>
        public static void Raise<T>(T e) where T : GameEvent
        {
            EventManagerInternal<T>.Raise(e);
        }

        /// <summary>
        /// Clears the GameEvent delegate
        /// </summary>
        public static void Clear<T>() where T : GameEvent
        {
            EventManagerInternal<T>.Clear();
        }

        /// <summary>
        /// Returns true if the specified handler is contained in the GameEvent's delegate invocation list
        /// </summary>
        public static bool Contains<T>(Action<T> handler) where T : GameEvent
        {
            return EventManagerInternal<T>.Contains(handler);
        }

        /// <summary>
        /// Rasies the game event 'e' to all handlers except the ones specified
        /// </summary>
        public static void RaiseToAllExcept<T>(T e, params Action<T>[] handlers) where T : GameEvent
        {
            EventManagerInternal<T>.RaiseToAllExcept(e, handlers);
        }

        /// <summary>
        /// Raises the game event 'e' only to the specified handlers
        /// </summary>
        public static void RaiseToOnly<T>(T e, params Action<T>[] handlers) where T : GameEvent
        {
            EventManagerInternal<T>.RaiseToOnly(e, handlers);
        }

        /// <summary>
        /// The internal event manager class - do not be afraid, doing a:
        /// EventManagerInternal`Event1`.Sub(...); and EventManagerInternal`Event2`.Sub(...);
        /// the compiler will _not_ create two seperate classes - they will use the same place in memory!
        /// If you don't know what I'm talking about, see Jamie King's: https://www.youtube.com/watch?v=9eZnUk0Gu7M 
        /// </summary>
        private static class EventManagerInternal<T> where T : GameEvent
        {
            private static Action<T> action;

            public static void Subscribe(Action<T> handler)
            {
                action += handler;
            }

            public static void Unsubscribe(Action<T> handler)
            {
                action -= handler;
            }

            public static void Raise(T e)
            {
                action.SafeInvoke(e);
            }

            public static void RaiseToAllExcept(T e, params Action<T>[] handlers)
            {
                var toInvoke = from d in action.GetInvocationList()
                                    where !handlers.Contains(d)
                                    select (Action<T>)d;

                foreach (var handler in toInvoke)
                    handler(e);
            }

            public static void RaiseToOnly(T e, params Action<T>[] handlers)
            {
                handlers.Foreach(x => x.Invoke(e));
            }

            public static void Clear()
            {
                action = null;
            }

            public static bool Contains(Action<T> handler)
            {
                return action.IsEmpty() ? false : action.GetInvocationList().Contains(d => d.Equals(handler));
            }
        }
    }
}