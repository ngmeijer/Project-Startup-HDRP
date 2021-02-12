using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AvatarAnimatorController))]
public class AvatarAnimatorControllerCustomEditor : Editor
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walking = Animator.StringToHash("Walking");

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Play Idle"))
        {
            var anim = (AvatarAnimatorController) target;
            EnableOnly(Idle, anim.Anim);
        }
        else if (GUILayout.Button("Play Walking"))
        {
            var anim = (AvatarAnimatorController) target;
            EnableOnly(Walking, anim.Anim);
        }
    }

    void EnableOnly(int paramId, Animator anim)
    {
        anim.SetBool(Idle, false);
        anim.SetBool(Walking, false);
        anim.SetBool(paramId, true);
    }
}