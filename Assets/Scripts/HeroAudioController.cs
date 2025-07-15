using UnityEngine;

public class HeroAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attacks;
    [SerializeField] private AudioClip[] superAttacks1;
    [SerializeField] private AudioClip[] superAttacks2;
    [SerializeField] private AudioClip[] damages;
    [SerializeField] private AudioClip die;
    [SerializeField] private AnimationListener _animationListener;

    private System.Random rand;

    private void Start()
    {
        rand = new System.Random();
        _animationListener.PlaySound += PlaySound;
    }

    public void PlaySound(string type)
    {
        AudioClip clip = null;
        if (type == "Attack")
        {
            if (attacks.Length == 0) return;
            clip = attacks[rand.Next(attacks.Length)];
        }
        if (type == "SuperAttack1")
        {
            if (superAttacks1.Length == 0) return;
            clip = superAttacks1[rand.Next(superAttacks1.Length)];
        }
        if (type == "SuperAttack2")
        {
            if (superAttacks2.Length == 0) return;
            clip = superAttacks2[rand.Next(superAttacks2.Length)];
        }
        if (type == "Damage")
        {
            if (damages.Length == 0) return;
            clip = damages[rand.Next(damages.Length)];
        }
        if (type == "Die")
        {
            clip = die;
        }

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
