using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using static UnityEditor.ShaderUtil;
#endif

#if UNITY_EDITOR
namespace DeveloperTools
{
    public static class MaterialAnalysis
    {
        static readonly IFormatProvider Formatter = new System.Globalization.CultureInfo("en-US");

        public static List<string> GetCommonSettings( Material material) {

            Shader shader = material.shader;

            List<string> list = new List<string>();

            list.Add( string.Format("Shader Name = {0}", shader.name));
            list.Add( string.Format("Render Queue = {0}", shader.renderQueue));

            return list;
        }

        public static List<string> GetKeywords(Material material)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < material.shaderKeywords.Length; i++)
            {
                list.Add(string.Format("{0}", material.shaderKeywords[i]));
            }

            return list;
        }

        public static List<string> GetProperties(Material material)
        {
            Shader shader = material.shader;

            List<string> list = new List<string>();

            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);
                ShaderPropertyType propertyType = (ShaderPropertyType)ShaderUtil.GetPropertyType(shader, i);
                string propertyDescription = ShaderUtil.GetPropertyDescription(shader, i);

                string value;

                switch (propertyType)
                {
                    case ShaderPropertyType.Color: // The property holds a Vector4 value representing a color.
                        value = string.Format(Formatter, "{0}", material.GetColor(propertyName));
                        break;
                    case ShaderPropertyType.Vector: // The property holds a Vector4 value.
                        value = string.Format(Formatter, "{0}", material.GetVector(propertyName));
                        break;
                    case ShaderPropertyType.Float: // The property holds a floating number value.
                        value = string.Format(Formatter, "{0}", material.GetFloat(propertyName));
                        break;
                    case ShaderPropertyType.Range: // 	The property holds a floating number value in a certain range.
                        value = string.Format(Formatter, "{0}", material.GetFloat(propertyName));
                        break;
                    case ShaderPropertyType.TexEnv: // The property holds a Texture object.
                        value = material.GetTexture(propertyName) == null ? "null" : string.Format(Formatter, "{0}", material.GetTexture(propertyName).dimension);
                        break;
                    default:
                        value = "<undefined>";
                        break;
                }

                list.Add( string.Format("{0} = {1} ({2}, {3})", propertyName, value, propertyType, propertyDescription));
            }

            return list;
        }

        public static List<string> GetShaderPasses(Material material)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < material.passCount; i++)
            {
                list.Add(string.Format("{0} = {1}", material.GetPassName(i), material.GetShaderPassEnabled(material.GetPassName(i))));
            }

            return list;
        }

        /// <summary>
        /// Log material and shader data to the console.
        /// </summary>
        /// <param name="material"></param>
        public static void LogMaterial(Material material)
        {
            string text = "";

            text += ListToString("Common", GetCommonSettings(material));
            text += ListToString("Shader Keywords", GetKeywords(material));
            text += ListToString("Properties", GetProperties(material));
            text += ListToString("Shader Passes", GetShaderPasses(material));

            Debug.Log(text);
        }

        private static string ListToString( string label, List<string> list)
        {
            string text = label;
            text += "\n";

            for (int i = 0; i < list.Count; i++)
            {
                text += string.Format("{0}: {1}", i, list[i]);
                text += "\n";
            }

            return text;
        }
    }
}
#endif