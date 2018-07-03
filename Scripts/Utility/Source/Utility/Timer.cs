using System;
using System.Collections;
using UnityEngine;

namespace FK.Utility
{
    /// <summary>
    /// <para>A Timer that can be started, stopped and paused. You can also set it to loop so it restarts automatically when it's time elapses.</para>
    ///
    /// v1.0 07/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public class Timer
    {
        // ######################## ENUMS & DELEGATES ######################## //
        /// <summary>
        /// Update Modes of a Timer
        /// </summary>
        public enum UpdateMode
        {
            /// <summary>
            /// Uses Time.deltaTime to update the timer
            /// </summary>
            SCALED_TIME,
            /// <summary>
            /// Uses Time.unscaledDeltaTime to update the timer
            /// </summary>
            USCALED_TIME
        }
        
        /// <summary>
        /// This Event is Invoked when the Timer reaches its end
        /// </summary>
        public Action OnTimeElapsed;
        /// <summary>
        /// This Event is Invoked each Time the timer Updates and contains the current elapsed Time;
        /// </summary>
        public Action<float> OnTimerUpdate;

        // ######################## PROPERTIES ######################## //
        /// <summary>
        /// Is the timer currently Running?
        /// </summary>
        public bool Running
        {
            get { return _runRoutine != null; }
        }

        /// <summary>
        /// Is the Timer currently paused?
        /// </summary>
        public bool Paused
        {
            get { return !Running && ElapsedTime > 0.0f; }
        }

        /// <summary>
        /// The current Time of the Timer
        /// </summary>
        public float ElapsedTime { get; private set; }

        // ######################## PUBLIC VARS ######################## //
        /// <summary>
        /// The amount of Time in Seconds the Timer should run for
        /// </summary>
        public float Duration;
        /// <summary>
        /// If this is set to true, the Timer restarts itself automatically
        /// </summary>
        public bool Loop;
        /// <summary>
        /// Defines whether the Timer uses scaled or unscaled Time to update
        /// </summary>
        public UpdateMode Mode;

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The Monobehaviour to run the Coroutine on
        /// </summary>
        private MonoBehaviour _host;

        /// <summary>
        /// The run Coroutine
        /// </summary>
        private Coroutine _runRoutine;


        // ######################## INITS ######################## //
        /// <summary>
        /// Creates a Timer with the given Time as duration
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(MonoBehaviour host, float duration, bool loop = false)
        {
            Init(host, duration, null, null, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers the provided function as a callback when its time is elapsed
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(MonoBehaviour host, float duration, Action onTimeElapsed, bool loop = false)
        {
            Init(host, duration, null, onTimeElapsed, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers the provided function as a callback that is called each time it updates
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(MonoBehaviour host, float duration, Action<float> onTimerUpdate, bool loop = false)
        {
            Init(host, duration, onTimerUpdate, null, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers a callback for when it updates and when it finishes
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(MonoBehaviour host, float duration, Action<float> onTimerUpdate, Action onTimeElapsed, bool loop = false)
        {
            Init(host, duration, onTimerUpdate, onTimeElapsed, loop);
        }

        /// <summary>
        /// Initializes the Timer
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        private void Init(MonoBehaviour host, float duration, Action<float> onTimerUpdate, Action onTimeElapsed, bool loop)
        {
            // set values
            _host = host;
            Duration = duration;
            ElapsedTime = 0.0f;
            Loop = loop;
            Mode = UpdateMode.SCALED_TIME;
            
            // register Callbacks
            if (onTimeElapsed != null)
                OnTimeElapsed += onTimeElapsed;

            if (onTimerUpdate != null)
                OnTimerUpdate += onTimerUpdate;
        }

        // ######################## FUNCTIONALITY ######################## //
        /// <summary>
        /// Starts the Timer (if the timer was paused, it will resume from where it was paused)
        /// </summary>
        public void Start()
        {
            if (Running)
            {
                Debug.LogWarning("Can't start Timer because it is already running!");
                return;
            }
            
            _runRoutine = _host.StartCoroutine(Run());
        }

        /// <summary>
        /// Pauses the Timer (stops it without resetting the Elapsed Time)
        /// </summary>
        public void Pause()
        {
            // do nothing if the Timer is not Running
            if (!Running)
            {
                Debug.LogWarning("Can't pause Timer because it is not running!");
                return;
            }
            
            // stop
            StopTimer();
        }

        /// <summary>
        /// Stops the Timer and resets it
        /// </summary>
        public void Stop()
        {
            // Do noting if timer is not running
            if (!Running)
            {
                Debug.LogWarning("Can't stop Timer because it is not running!");
                return;
            }
            
            // stop and reset
            StopTimer();
            Reset();
        }

        /// <summary>
        /// Resets the Timer (if it is running, this also stops it)
        /// </summary>
        public void Reset()
        {
            // stop timer if running
            if (Running)
            {
                StopTimer();
            }
            
            // reset
            ElapsedTime = 0.0f;
        }
        
        
        // ######################## COROUTINES ######################## //
        /// <summary>
        /// Runs the Timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator Run()
        {
            // Run as long the Elapsed Time is less than the set duration
            while (ElapsedTime < Duration)
            {
                yield return null;
                ElapsedTime += Mode == UpdateMode.SCALED_TIME ? Time.deltaTime : Time.unscaledDeltaTime;
                OnTimerUpdate?.Invoke(ElapsedTime);
            }

            // finish
            ElapsedTime = 0.0f;
            OnTimeElapsed?.Invoke();

            // restart if looping
            if (Loop)
                Start();
        }

        // ######################## UTILITIES ######################## //
        /// <summary>
        /// stops the timer
        /// </summary>
        private void StopTimer()
        {
            _host.StopCoroutine(_runRoutine);
            _runRoutine = null;
        }
    }
}