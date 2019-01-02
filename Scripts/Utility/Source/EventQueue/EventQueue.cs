using System;
using System.Collections.Generic;
using FK.Utility;

namespace FK.Sequencing
{
    /// <summary>
    /// <para>Event Queue for things that should happen in Sequence</para>
    ///
    /// v3.1 12/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class EventQueue
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
        public bool Running { get; private set; }

        /// <summary>
        /// If true, the Queue starts automatically as soon as there is one event in it
        /// </summary>
        public bool PlayAutomatic
        {
            get { return _autoplay; }
            set
            {
                _autoplay = value; 
                Start();
            }
        }

        /// <summary>
        /// Called when the last event of the Queue finished
        /// </summary>
        public Action OnQueueFinished;
        

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The queue of events
        /// </summary>
        private readonly List<QueueEvent> Queue = new List<QueueEvent>();

        /// <summary>
        /// Timer running between queue events
        /// </summary>
        private readonly Timer _queueTimer;

        /// <summary>
        /// Backing for PlayAutomatic
        /// </summary>
        private bool _autoplay;

        // ######################## INITS ######################## //
        public EventQueue(bool autoplay, Action onQueueFinished = null)
        {
            OnQueueFinished = onQueueFinished;
            _autoplay = autoplay;
            _queueTimer = new Timer(1f, NextQueueEvent);
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Adds a new Event to the Queue. If the queue was empty before, it is executed immediately
        /// </summary>
        /// <param name="action">Delegate for starting the event</param>
        /// <param name="data">Data object derived from EventData containing all the information the event needs</param>
        public void AddEvent(Action<EventData> action, EventData data)
        {
            // create the event and add it to the queue
            QueueEvent evt = new QueueEvent()
            {
                Action = action,
                Data = data
            };
            Queue.Add(evt);
            
            // if the queue is not running we need to start it
            if (_autoplay && !Running)
                NextQueueEvent();
        }

        /// <summary>
        /// Starts the Queue
        /// </summary>
        public void Start()
        {
            if (!Running)
                NextQueueEvent();
        }

        /// <summary>
        /// Stops the queue after the current event is done
        /// </summary>
        /// <param name="onStop"></param>
        public void StopAfterCurrent(Action onStop)
        {
            _queueTimer.OnTimeElapsed = () => Running = false;
            _queueTimer.OnTimeElapsed += onStop;
        }

        /// <summary>
        /// Activates the next Event and keeps the queue running until it is empty
        /// </summary>
        private void NextQueueEvent()
        {
            // if the queue is empty, stop
            if (Queue.Count <= 0)
            {
                Running = false;
                OnQueueFinished?.Invoke();
                return;
            }

            Running = true;

            // get the next event and remove it from the queue
            QueueEvent evt = Queue[0];
            Queue.RemoveAt(0);

            // set the timer to the duration of the event so it can trigger the next one after this is finished and invoke the event
            _queueTimer.Duration = evt.Data.Duration;
            _queueTimer.OnTimeElapsed = NextQueueEvent;
            evt.Action?.Invoke(evt.Data);

            // Start the timer
            _queueTimer.Reset();
            _queueTimer.Start();
        }
    }
}