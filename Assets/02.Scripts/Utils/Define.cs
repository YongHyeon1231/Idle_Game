using UnityEngine;

public static class Define
{
    public static class Character
    {
        public static readonly int hashIdle  = Animator.StringToHash("isIDLE");
        public static readonly int hashMove  = Animator.StringToHash("isMOVE");
        public static readonly int hashSpawn = Animator.StringToHash("isSPAWN");
    }
}
