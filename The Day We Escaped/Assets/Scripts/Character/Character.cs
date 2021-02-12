using UnityEngine;

namespace CharacterNS
{
    public class Character : MonoBehaviour
    {
        public static Character Player { get; internal set; }
        public static Character Enemy { get; internal set; }
    }
}