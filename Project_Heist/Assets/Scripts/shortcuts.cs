using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class shortcuts : MonoBehaviour
{ 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [MenuItem("Extra Tools/Clear Console %q")] // CTRL + Q
    private static void ClearConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method?.Invoke(new object(), null);
    }
    
}
