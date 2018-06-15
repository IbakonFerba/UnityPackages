using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// A Button placed in the 3D World (optimized for Touch)
/// </summary>
public class ThreeDButton : Interactable
{
    // ######################## ENUMS & DELEGATES ######################## //
    public enum ButtonState
    {
        RELEASED,
        JUST_PRESSED,
        HELD
    }

    // ######################## PUBLIC VARS ######################## //
    /// <summary>
    /// Event that should happen when Button is Pressed
    /// </summary>
    public Button.ButtonClickedEvent OnPress;
    /// <summary>
    /// Event that should happen every Frame the Button is held
    /// </summary>
    public Button.ButtonClickedEvent OnHold;
    /// <summary>
    /// Event that should happen when Button is released
    /// </summary>
    public Button.ButtonClickedEvent OnRelease;

    // ######################## PRIVATE VARS ######################## //
    private ButtonState _state;

    /// <summary>
    /// List of touches that started inside the Button and did't leave it
    /// </summary>
    private List<int> _touchIDsStartedInButton;




    // ######################## UNITY START & UPDATE ######################## //

    void Start() { Init(); }

    void Update()
    {
        //don't do anything if we are not active
        if (!Active)
        {
            //Reset Button if it was pressed when deactivating
            if (_state != ButtonState.RELEASED)
            {
                Init();
            }
            return;
        }



        //interaction
        HandleInteraction();
    }



    // ######################## INITS ######################## //
    /// <summary>Does the init for this behaviour</summary>
    private void Init()
    {
        _touchIDsStartedInButton = new List<int>();
        _state = ButtonState.RELEASED;
    }


    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Register pressing, holding and releasing of the Button
    /// </summary>
    private void HandleInteraction()
    {
        if (Input.touchCount > 0) //for touch
        {
            bool foundInteraction = false;

            //go through all touches and test if they are interacting with the Button
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch touch = Input.GetTouch(i); //get the thouch

                //If this is not a touch that started inside the Button or a new Touch, don't bother, look at the next one
                if (!(touch.phase == TouchPhase.Began || _touchIDsStartedInButton.Contains(touch.fingerId)))
                {
                    continue;
                }



                //cast a ray from the touch into the scene and see if it hits the Button
                if (InteractablesRaycaster.Instance.HitGameObjectOrChild(gameObject, touch.fingerId))
                {
                    foundInteraction = true;
                    switch (_state)
                    {
                        case ButtonState.RELEASED: //if the Button currently is released, this is the frame we first press it
                            ButtonPressed();
                            break;
                        case ButtonState.JUST_PRESSED: //if the Button was just pressed or Held, continue holding
                        case ButtonState.HELD:
                            ButtonHeld();
                            break;
                    }




                    if (touch.phase == TouchPhase.Began) //if this is a new touch, add it to the list
                    {
                        _touchIDsStartedInButton.Add(touch.fingerId);
                    }
                    else if (touch.phase == TouchPhase.Ended) //if this is the last frame of this touch, remove it from the list
                    {
                        _touchIDsStartedInButton.Remove(touch.fingerId);
                    }
                }
                else if (_touchIDsStartedInButton.Contains(touch.fingerId)) //if the touch ray hit nothing, but it is in the list of touches inside the Button, remove it
                {
                    _touchIDsStartedInButton.Remove(touch.fingerId);
                }
            }



            //if we found no interaction for any touch but the Button was pressed or held before, this is the frame we released it
            if (!foundInteraction && (_state == ButtonState.HELD || _state == ButtonState.JUST_PRESSED))
            {
                ButtonReleased();
            }
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) //for mouse (might not work as convinient as touch)
        {

            //cast a ray from the mouse into the scene and see if it hits the Button
            if (InteractablesRaycaster.Instance.HitGameObjectOrChild(gameObject, true))
            {
                    switch (_state)
                    {
                        case ButtonState.RELEASED: //if the Button currently is released, this is the frame we first press it
                            ButtonPressed();
                            break;
                        case ButtonState.JUST_PRESSED: //if the Button was just pressed or Held, continue holding
                        case ButtonState.HELD:
                            ButtonHeld();
                            break;
                    }
            }
        }
        else if (_state == ButtonState.HELD || _state == ButtonState.JUST_PRESSED) //if the Button was pressed or held before, this is the frame we released it
        {
            ButtonReleased();
        }
    }


    /// <summary>
    /// Function for the First Frame the Button is pressed in
    /// </summary>
    private void ButtonPressed()
    {
        _state = ButtonState.JUST_PRESSED;
        OnPress.Invoke();
    }

    /// <summary>
    /// Function for every Frame the Button is Held
    /// </summary>
    private void ButtonHeld()
    {
        _state = ButtonState.HELD;
        OnHold.Invoke();
    }

    /// <summary>
    /// Function for the Frame the Button is released in
    /// </summary>
    private void ButtonReleased()
    {
        _state = ButtonState.RELEASED;
        OnRelease.Invoke();
    }
}
