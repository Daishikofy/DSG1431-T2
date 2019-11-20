using UnityEngine;
public class Explosion : MonoBehaviour
{
    public float time;
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {

            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
                Destroy(this.gameObject, clips[0].clip.length);
        }
        else
            Destroy(this.gameObject, 1.0f);
    }
}
