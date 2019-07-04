using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BeatPatternButton))]
public class BeatPatternButtonEditor : Editor
{
    /*private BeatPatternResolverTest _targetScript;

    private int _onResolvedFunctionIndex;
    private int _onInputFunctionIndex;

    void OnEnable()
    {
        _targetScript = (BeatPatternResolverTest)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        List<BeatPattern> itemsToRemove = new List<BeatPattern>();
        foreach(BeatPatternResolverTest.BeatPatternResolutionData resolver in _targetScript._resolutionDatas.Values)
        {
            RenderDelegates(resolver.onResolvedFunction, resolver.onInputFunction);

            BeatPattern pattern = resolver.beatPatternResolver.GetPattern();

            RenderPatternGUI(pattern);

            resolver.beatPatternResolver.SetPattern(pattern);

            if (GUILayout.Button("-"))
            {
                itemsToRemove.Add(pattern);
            }
        }
       
        if (GUILayout.Button("+"))
        {
            _targetScript.AddPattern(new BeatPattern());
        }

        EditorGUILayout.EndVertical();

        foreach(BeatPattern pattern in itemsToRemove)
        {
            _targetScript.RemovePattern(pattern);
        }

        itemsToRemove.Clear();
    }

    private void RenderPatternGUI(BeatPattern pattern)
    {
        EditorGUILayout.BeginVertical();

        

        List<int> itemsToRemove = new List<int>();
        for(int i = 0; i < pattern.pattern.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-"))
            {
                itemsToRemove.Add(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("+"))
        {
            pattern.pattern.Add(BeatPattern.Input.Tap);
        }

        EditorGUILayout.EndVertical();

        for(int i = 0; i < itemsToRemove.Count; i++)
        {
            pattern.pattern.RemoveAt(i);
        }

        itemsToRemove.Clear();
    }

    private void RenderDelegates(BeatPatternResolver.OnResolvedAction resolvedAction, BeatPatternResolver.OnInputAction inputAction)
    {
        SerializedProperty serprop = serializedObject.FindProperty("scriptName");
        EditorGUILayout.PropertyField(serprop);
        serializedObject.ApplyModifiedProperties();
        Type type = Type.GetType(_targetScript.scriptName);
        MethodInfo[] infos = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        string[] methodNames = new string[infos.Length];
        for (int i = 0; i < methodNames.Length; i++)
        {
            methodNames[i] = infos[i].Name;
        }

        _onResolvedFunctionIndex = EditorGUILayout.Popup("OnResolvedAction", _onResolvedFunctionIndex, methodNames);
        _onInputFunctionIndex = EditorGUILayout.Popup("OnInputAction", _onInputFunctionIndex, methodNames);

        try
        {
            BeatPatternResolver.OnResolvedAction resolvedFunction = (BeatPatternResolver.OnResolvedAction)infos[_onResolvedFunctionIndex].CreateDelegate(typeof(BeatPatternResolver.OnResolvedAction));
            BeatPatternResolver.OnInputAction inputFunction = (BeatPatternResolver.OnInputAction)infos[_onInputFunctionIndex].CreateDelegate(typeof(BeatPatternResolver.OnInputAction));
            _targetScript.AddPattern(new BeatPattern(), resolvedFunction, inputFunction);
        }
        catch (ArgumentException argumentException)
        {

        }
    }*/
}
