using System;
using System.Collections;
using UnityEngine;

namespace FK.Utility
{
    /// <summary>
    /// <para>A Timer that can be started, stopped and paused. You can also set it to loop so it restarts automatically when it's time elapses.</para>
    ///
    /// v2.0 09/2018
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
        /// Count modes of a timer
        /// </summary>
        public enum CountMode
        {
            /// <summary>
            /// Makes the Timer count up
            /// </summary>
            UP,

            /// <summary>
            /// Makes the Timer count down
            /// </summary>
            DOWN
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
        public float ElapsedTime
        {
            get { return CountDirection == CountMode.UP ? _elapsedTime : Duration - _elapsedTime; }
        }

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

        /// <summary>
        /// Defines whether the timer counts up or down
        /// </summary>
        public CountMode CountDirection;

        // ######################## PRIVATE VARS ######################## //
        /// <summary>
        /// The run Coroutine
        /// </summary>
        private Coroutine _runRoutine;

        /// <summary>
        /// The current Time of the Timer
        /// </summary>
        private float _elapsedTime;


        // ######################## INITS ######################## //
        #region CONSTRUCTOR

        /// <summary>
        /// Creates a Timer with the given Time as duration
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, bool loop = false)
        {
            Init(duration, CountMode.UP, null, null, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="countDirection">Defines whether the Timer counts up or down</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, CountMode countDirection, bool loop = false)
        {
            Init(duration, countDirection, null, null, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers the provided function as a callback when its time is elapsed
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, Action onTimeElapsed, bool loop = false)
        {
            Init(duration, CountMode.UP, null, onTimeElapsed, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers the provided function as a callback when its time is elapsed
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="countDirection">Defines whether the Timer counts up or down</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, CountMode countDirection, Action onTimeElapsed, bool loop = false)
        {
            Init(duration, countDirection, null, onTimeElapsed, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers the provided function as a callback that is called each time it updates
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, Action<float> onTimerUpdate, bool loop = false)
        {
            Init(duration, CountMode.UP, onTimerUpdate, null, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers the provided function as a callback that is called each time it updates
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="countDirection">Defines whether the Timer counts up or down</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, CountMode countDirection, Action<float> onTimerUpdate, bool loop = false)
        {
            Init(duration, countDirection, onTimerUpdate, null, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers a callback for when it updates and when it finishes
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, Action<float> onTimerUpdate, Action onTimeElapsed, bool loop = false)
        {
            Init(duration, CountMode.UP, onTimerUpdate, onTimeElapsed, loop);
        }

        /// <summary>
        /// Creates a Timer with the given Time as duration and registers a callback for when it updates and when it finishes
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="countDirection">Defines whether the Timer counts up or down</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        public Timer(float duration, CountMode countDirection, Action<float> onTimerUpdate, Action onTimeElapsed, bool loop = false)
        {
            Init(duration, countDirection, onTimerUpdate, onTimeElapsed, loop);
        }

        #endregion

        /// <summary>
        /// Initializes the Timer
        /// </summary>
        /// <param name="host">MonoBehaviour to run the Timer on</param>
        /// <param name="duration">Duration of the Timer in seconds</param>
        /// <param name="countDirection">Defines whether the Timer counts up or down</param>
        /// <param name="onTimerUpdate">Callback for the timer Update</param>
        /// <param name="onTimeElapsed">Callback for when the Timer finishes</param>
        /// <param name="loop">If true, the Timer restarts itself automatically</param>
        private void Init(float duration, CountMode countDirection, Action<float> onTimerUpdate, Action onTimeElapsed, bool loop)
        {
            Duration = duration;
            _elapsedTime = 0.0f;
            Loop = loop;
            Mode = UpdateMode.SCALED_TIME;
            CountDirection = countDirection;

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

            _runRoutine = CoroutineHost.Instance.StartCoroutine(Run());
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
            _elapsedTime = 0.0f;
        }


        // ######################## COROUTINES ######################## //
        /// <summary>
        /// Runs the Timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator Run()
        {
            // Run as long the Elapsed Time is less than the set duration
            while (_elapsedTime < Duration)
            {
                yield return null;
                _elapsedTime += Mode == UpdateMode.SCALED_TIME ? Time.deltaTime : Time.unscaledDeltaTime;
                OnTimerUpdate?.Invoke(ElapsedTime);
            }

            // finish
            _elapsedTime = 0.0f;
            OnTimeElapsed?.Invoke();

            _runRoutine = null;

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
            CoroutineHost.Instance.StopCoroutine(_runRoutine);
            _runRoutine = null;
        }
    }
}