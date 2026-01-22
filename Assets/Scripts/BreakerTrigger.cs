using UnityEngine;

public class BreakerTrigger : MonoBehaviour
{
    public BreakerLeverAnimation leverAnimation; // assign in Inspector
    public NeonFlicker neonFlicker;              // assign in Inspector
    public AudioSource audioSource;
    public AudioClip snapSound;

    private bool breakerOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            ToggleBreaker();
        }
    }

    private void ToggleBreaker()
    {
        breakerOn = !breakerOn;

        if (audioSource && snapSound)
            audioSource.PlayOneShot(snapSound);

        if (leverAnimation != null)
        {
            if (breakerOn) leverAnimation.FlipUp();
            else leverAnimation.FlipDown();
        }

        if (neonFlicker != null)
        {
            if (breakerOn) neonFlicker.StartFlicker();
            else neonFlicker.TurnOff();
        }
    }
}
