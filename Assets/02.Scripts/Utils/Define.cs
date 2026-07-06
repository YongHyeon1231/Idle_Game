using UnityEngine;

public static class Define
{
    public static class Character
    {
        public static readonly int hashIdle  = Animator.StringToHash("isIDLE");
        public static readonly int hashMove  = Animator.StringToHash("isMOVE");
        public static readonly int hashSpawn = Animator.StringToHash("isSPAWN");
        public static readonly int hashATK01 = Animator.StringToHash("isATK01");
        public static readonly int hashATK02 = Animator.StringToHash("isATK02");
        public static readonly int hashATK03 = Animator.StringToHash("isATK03");
    }
}
