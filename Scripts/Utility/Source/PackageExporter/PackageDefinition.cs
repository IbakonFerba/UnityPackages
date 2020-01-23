using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>Class Info</para>
///
/// v1.0 mm/20yy
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CreateAssetMenu(fileName = "NewPackageDefinition", menuName = "DevTools/Package Definition", order = 0)]
public class PackageDefinition : ScriptableObject
{
    // ######################## STRUCTS & CLASSES ######################## //

    #region STRUCTS & CLASSES

    #endregion


    // ######################## ENUMS & DELEGATES ######################## //

    #region ENUMS & DELEGATES

    #endregion


    // ######################## EVENTS ######################## //

    #region EVENTS

    #endregion


    // ######################## PROPERTIES ######################## //

    #region PROPERTIES

    #endregion


    // ######################## EXPOSED VARS ######################## //

    #region EXPOSED VARS

    [SerializeField] private string _packageName;
    [SerializeField] private string _targetPath;
    [SerializeField] private string[] _paths;
    [SerializeField] private bool _showPackageAfterExport = true;

    #endregion


    // ######################## PUBLIC VARS ######################## //

    #region PUBLIC VARS

    #endregion


    // ######################## PROTECTED VARS ######################## //

    #region PROTECTED VARS

    #endregion


    // ######################## PRIVATE VARS ######################## //

    #region PRIVATE VARS

    #endregion


    // ######################## INITS ######################## //

    #region CONSTRUCTORS

    #endregion

    #region INITS
    
    #endregion


    // ######################## UNITY EVENT FUNCTIONS ######################## //

    #region UNITY EVENT FUNCTIONS

    private void Reset()
    {
        _targetPath = Application.dataPath;
        _targetPath = _targetPath.Remove(_targetPath.Length - 7);
    }

    #endregion


    // ######################## FUNCTIONALITY ######################## //

    #region FUNCTIONALITY

    public void Export()
    {
        ExportPackageOptions exportFlags = ExportPackageOptions.Recurse;
        if (_showPackageAfterExport)
            exportFlags |= ExportPackageOptions.Interactive;
        
        AssetDatabase.ExportPackage(_paths, $"{_targetPath}/{_packageName}.unitypackage", exportFlags);
    }
    #endregion


    // ######################## COROUTINES ######################## //

    #region COROUTINES 

    #endregion


    // ######################## UTILITIES ######################## //

    #region UTILITIES

    #endregion
}