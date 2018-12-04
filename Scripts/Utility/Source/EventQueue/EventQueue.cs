using System;
using System.Collections.Generic;
using FK.Utility;

namespace FK.Sequencing
{
    /// <summary>
    /// <para>Event Queue for things that should happen in Sequence</para>
    ///
    /// v2.0 12/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class EventQueue
    {
        // ######################## STRUCTS & CLASSES ######################## //
        /// <summary>
        /// Base for event data
        /// </summary>
        public class EventData
        {
            /// <summary>
            /// Duration of the Event, defines when the next event happens
            /// </summary>
            public float Duration = 0f;
        }

        /// <summary>
        /// Contains Information for a queue event
        /// </summary>
        private struct QueueEvent
        {
            public Action<EventData> Action;
            public EventData Data;
        }

        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// Timer running between queue events
        /// </summary>
        private static Timer QueueTimer
        {
            get
            {
                if (_queueTimer == null)
                    _queueTimer = new Timer(1f, NextQueueEvent);

                return _queueTimer;
            }
        }

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The queue of events
        /// </summary>
        private static readonly List<QueueEvent> Queue = new List<QueueEvent>();

        /// <summary>
        /// Backing for QueueTimer
        /// </summary>
        private static Timer _queueTimer;


        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Adds a new Event to the Queue. If the queue was empty before, it is executed immediately
        /// </summary>
        /// <param name="action">Delegate for starting the event</param>
        /// <param name="data">Data object derived from EventData containing all the information the event needs</param>
        public static void AddEvent(Action<EventData> action, EventData data)
        {
            // create the event and add it to the queue
            QueueEvent evt = new QueueEvent()
            {
                Action = action,
                Data = data
            };
            Queue.Add(evt);

            // if the queue Timer is not running, the queue is not active, we need to start it
            if (!QueueTimer.Running)
                NextQueueEvent();
        }

        /// <summary>
        /// Activates the next Event and keeps the queue running until it is empty
        /// </summary>
        private static void NextQueueEvent()
        {
            // if the queue is empty, stop
            if (Queue.Count <= 0)
                return;

            // get the next event and remove it from the queue
            QueueEvent evt = Queue[0];
            Queue.RemoveAt(0);

            // set the timer to the duration of the event so it can trigger the next one after this is finished and invoke the event
            QueueTimer.Duration = evt.Data.Duration;
            evt.Action?.Invoke(evt.Data);

            // Start the timer
            QueueTimer.Reset();
            QueueTimer.Start();
        }
    }
}