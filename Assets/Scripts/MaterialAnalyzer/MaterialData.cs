using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace DeveloperTools
{
    public class MaterialData
    {
        public List<string> common;
        public List<string> keywords;
        public List<string> properties;
        public List<string> shaderPasses;
    }
}
#endif
