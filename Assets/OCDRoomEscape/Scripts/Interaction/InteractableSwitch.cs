using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class InteractableSwitch : InteractableObject
{
    public bool turnedOn;

    public UnityAction onAction = () => { };
    public UnityAction offAction = () => { };

    public GameObject[] enableWhenTurnedOn;
    public GameObject[] disableWhenTurnedOn;
//    public AnimationClip animateWhenTurnedOn;

    public GameObject[] enableWhenTurnedOff;
    public GameObject[] disableWhenTurnedOff;
//    public AnimationClip animateWhenTurnedOff;

    public AudioSource playWhenTurnedOn;
    public AudioSource playWhenTurnedOff;

    public Animator animator;

    protected override void Awake()
    {
        base.Awake();

        Toggle();
    }

    protected virtual void Toggle()
    {
        if (turnedOn)
        {
            foreach (var o in enableWhenTurnedOn)
            {
                o.SetActive(true);
            }
            foreach (var o in disableWhenTurnedOn)
            {
                o.SetActive(false);
            }
            onAction();

            if(playWhenTurnedOn) playWhenTurnedOn.Play();

            if (animator != null)
            {
                animator.SetBool("TurnOn", true);
            }
        }
        else
        {
            foreach (var o in enableWhenTurnedOff)
            {
                o.SetActive(true);
            }
            foreach (var o in disableWhenTurnedOff)
            {
                o.SetActive(false);
            }
            offAction();

            if(playWhenTurnedOff) playWhenTurnedOff.Play();

            if (animator != null)
            {
                animator.SetBool("TurnOn", false);
            }
        }
    }

    public override bool OnInteract(GazePointer pointer)
    {
        turnedOn = !turnedOn;
        Toggle();
        return false;
    }
}
