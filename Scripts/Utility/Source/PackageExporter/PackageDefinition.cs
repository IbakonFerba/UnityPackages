using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>Data Object defining a package</para>
///
/// v1.0 01/2020
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[CreateAssetMenu(fileName = "NewPackageDefinition", menuName = "DevTools/Package Definition", order = 0)]
public class PackageDefinition : ScriptableObject
{
    // ######################## EXPOSED VARS ######################## //

    #region EXPOSED VARS

    [SerializeField] private string _packageName;
    [SerializeField] private string _targetPath;
    [SerializeField] private string[] _paths;
    [SerializeField] private bool _showPackageAfterExport = true;

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
}