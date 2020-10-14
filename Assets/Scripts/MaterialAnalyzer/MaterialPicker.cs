using DeveloperTools;
using UnityEngine;

#if UNITY_EDITOR
namespace DeveloperTools
{
    /// <summary>
    /// Pick a gameobject and perform the analyzation on it
    /// </summary>
    public class MaterialPicker : MonoBehaviour
    {
        public GameObject analyzedObject;

        public MaterialData materialDataA;
        public MaterialData materialDataB;

    }
}
#endif