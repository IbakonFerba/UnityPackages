using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using FK.Editor;

#endif

/// <summary>
/// <para>Projection of a 4D Hypercube in 3 Dimensions</para>
///
/// v1.0 08/2018
/// Written by Fabian Kober
/// fabian-kober@gmx.net
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Hypercube : MonoBehaviour
{
    // ######################## STRUCTS & CLASSES ######################## //
    /// <summary>
    /// Struct for Rotations around the W axis
    /// </summary>
    [System.Serializable]
    public struct RotationW
    {
        public float XW;
        public float YW;
        public float ZW;
    }

    // ######################## PROPERTIES ######################## //
    /// <summary>
    /// Distance of the 4D Camera to the Hypercube used for projection
    /// </summary>
    public float ProjectionDist
    {
        get { return _projectionDist; }
        set
        {
            _projectionDist = value;
            UpdateMesh();
        }
    }

    /// <summary>
    /// Rotation around the W Axis
    /// </summary>
    public RotationW WRotation
    {
        get { return _wRot; }
        set
        {
            _wRot = value;
            UpdateMesh();
        }
    }

    /// <summary>
    /// The 3D Mesh of the Projection
    /// </summary>
    private Mesh Mesh3D
    {
        get
        {
            if (_mesh == null)
                _mesh = new Mesh();

            return _mesh;
        }
    }

    /// <summary>
    /// The Mesh filter for the Mesh
    /// </summary>
    private MeshFilter Filter
    {
        get
        {
            if (_meshFilter == null)
                _meshFilter = GetComponent<MeshFilter>();

            return _meshFilter;
        }
    }

    /// <summary>
    /// The Vertices of the Hypercube in 4D space
    /// </summary>
    private Vector4[] Verts
    {
        get
        {
            if (_verts == null)
                GenerateHypercube();

            return _verts;
        }
    }

    /// <summary>
    /// The Vertices of the Hypercube projected into 3D Space
    /// </summary>
    private Vector3[] ProjectedVerts
    {
        get
        {
            if (_projectedVerts == null)
                _projectedVerts = new Vector3[_verts.Length];

            return _projectedVerts;
        }
    }


    // ######################## PRIVATE VARS ######################## //
    /// <summary>
    /// Distance of the 4D Camera to the Hypercube used for projection
    /// </summary>
    [SerializeField] private float _projectionDist = 3f;

    /// <summary>
    /// Rotation around the W Axis
    /// </summary>
    [SerializeField] private RotationW _wRot;

    /// <summary>
    /// The Vertices of the Hypercube in 4D space
    /// </summary>
    private Vector4[] _verts;

    /// <summary>
    /// The Vertices of the Hypercube projected into 3D Space
    /// </summary>
    private Vector3[] _projectedVerts;

    /// <summary>
    /// The Vertices of the projection mesh
    /// </summary>
    private Vector3[] _meshVerts;

    /// <summary>
    /// The Triangles of the projection mesh
    /// </summary>
    private int[] _tris;

    /// <summary>
    /// The mesh used to display the Projection
    /// </summary>
    private Mesh _mesh;

    private MeshFilter _meshFilter;

    /// <summary>
    /// Matrix for XW Rotation
    /// </summary>
    private Matrix4x4 _rotateXW = Matrix4x4.identity;

    /// <summary>
    /// Matrix for YW Rotation
    /// </summary>
    private Matrix4x4 _rotateYW = Matrix4x4.identity;

    /// <summary>
    /// Matrix for ZW Rotation
    /// </summary>
    private Matrix4x4 _rotateZW = Matrix4x4.identity;


    // ######################## INITS ######################## //
    ///<summary>
    /// Generates the Hypercube
    ///</summary>
    private void GenerateHypercube()
    {
        _verts = new Vector4[16];
        for (int i = 0; i < 8; i += 4)
        {
            _verts[i] = new Vector4(1, 1, i < 4 ? 1 : -1, 1);
            _verts[i + 1] = new Vector4(-1, 1, i < 4 ? 1 : -1, 1);
            _verts[i + 2] = new Vector4(-1, -1, i < 4 ? 1 : -1, 1);
            _verts[i + 3] = new Vector4(1, -1, i < 4 ? 1 : -1, 1);
        }

        for (int i = 0; i < 8; i += 4)
        {
            _verts[i + 8] = new Vector4(1, 1, i < 4 ? 1 : -1, -1);
            _verts[i + 8 + 1] = new Vector4(-1, 1, i < 4 ? 1 : -1, -1);
            _verts[i + 8 + 2] = new Vector4(-1, -1, i < 4 ? 1 : -1, -1);
            _verts[i + 8 + 3] = new Vector4(1, -1, i < 4 ? 1 : -1, -1);
        }

        // set mesh
        Filter.mesh = Mesh3D;
        
        UpdateMesh();
    }


    // ######################## FUNCTIONALITY ######################## //
    /// <summary>
    /// Rotates the Hypercube and projects it on the mesh
    /// </summary>
    public void UpdateMesh()
    {
        // rotate and project
        for (int i = 0; i < Verts.Length; ++i)
        {
            // set up rotation amtrices
            _rotateXW.SetRow(0, new Vector4(Mathf.Cos(_wRot.XW), 0, 0, -Mathf.Sin(_wRot.XW)));
            _rotateXW.SetRow(3, new Vector4(Mathf.Sin(_wRot.XW), 0, 0, Mathf.Cos(_wRot.XW)));

            _rotateYW.SetRow(1, new Vector4(0, Mathf.Cos(_wRot.YW), 0, -Mathf.Sin(_wRot.YW)));
            _rotateYW.SetRow(3, new Vector4(0, Mathf.Sin(_wRot.YW), 0, Mathf.Cos(_wRot.YW)));

            _rotateZW.SetRow(2, new Vector4(0, 0, Mathf.Cos(_wRot.ZW), -Mathf.Sin(_wRot.ZW)));
            _rotateZW.SetRow(3, new Vector4(0, 0, Mathf.Sin(_wRot.ZW), Mathf.Cos(_wRot.ZW)));

            // rotate vertex
            Vector4 vert = Verts[i];
            vert = _rotateXW * _rotateYW * _rotateZW * vert;

            // set up projection matrix
            float projDist = 1 / (_projectionDist - vert.w);
            Matrix4x4 projection = new Matrix4x4(new Vector4(projDist, 0, 0, 0), new Vector4(0, projDist, 0, 0), new Vector4(0, 0, projDist, 0), new Vector4(0, 0, 0, 1));
            
            // project vertex
            ProjectedVerts[i] = projection.MultiplyPoint3x4(vert);
        }


        // set up per-face vertices of the mesh
        _meshVerts = new Vector3[]
        {
            //Front
            ProjectedVerts[0],
            ProjectedVerts[1],
            ProjectedVerts[2],
            ProjectedVerts[3],

            //Front Flipped
            ProjectedVerts[3],
            ProjectedVerts[2],
            ProjectedVerts[1],
            ProjectedVerts[0],

            // left
            ProjectedVerts[0],
            ProjectedVerts[3],
            ProjectedVerts[7],
            ProjectedVerts[4],

            // left flipped
            ProjectedVerts[4],
            ProjectedVerts[7],
            ProjectedVerts[3],
            ProjectedVerts[0],

            // top
            ProjectedVerts[0],
            ProjectedVerts[4],
            ProjectedVerts[5],
            ProjectedVerts[1],

            // top flipped
            ProjectedVerts[1],
            ProjectedVerts[5],
            ProjectedVerts[4],
            ProjectedVerts[0],

            // right
            ProjectedVerts[1],
            ProjectedVerts[5],
            ProjectedVerts[6],
            ProjectedVerts[2],

            // right flipped
            ProjectedVerts[2],
            ProjectedVerts[6],
            ProjectedVerts[5],
            ProjectedVerts[1],

            // bottom
            ProjectedVerts[2],
            ProjectedVerts[6],
            ProjectedVerts[7],
            ProjectedVerts[3],

            // bottom flipped
            ProjectedVerts[3],
            ProjectedVerts[7],
            ProjectedVerts[6],
            ProjectedVerts[2],

            // back
            ProjectedVerts[4],
            ProjectedVerts[7],
            ProjectedVerts[6],
            ProjectedVerts[5],

            // back flipped
            ProjectedVerts[5],
            ProjectedVerts[6],
            ProjectedVerts[7],
            ProjectedVerts[4],

            //Front
            ProjectedVerts[8 + 0],
            ProjectedVerts[8 + 1],
            ProjectedVerts[8 + 2],
            ProjectedVerts[8 + 3],

            //FrontW Flipped
            ProjectedVerts[8 + 3],
            ProjectedVerts[8 + 2],
            ProjectedVerts[8 + 1],
            ProjectedVerts[8 + 0],

            // leftW
            ProjectedVerts[8 + 0],
            ProjectedVerts[8 + 3],
            ProjectedVerts[8 + 7],
            ProjectedVerts[8 + 4],

            // leftW flipped
            ProjectedVerts[8 + 4],
            ProjectedVerts[8 + 7],
            ProjectedVerts[8 + 3],
            ProjectedVerts[8 + 0],

            // topW
            ProjectedVerts[8 + 0],
            ProjectedVerts[8 + 4],
            ProjectedVerts[8 + 5],
            ProjectedVerts[8 + 1],

            // topW flipped
            ProjectedVerts[8 + 1],
            ProjectedVerts[8 + 5],
            ProjectedVerts[8 + 4],
            ProjectedVerts[8 + 0],

            // rightW
            ProjectedVerts[8 + 1],
            ProjectedVerts[8 + 5],
            ProjectedVerts[8 + 6],
            ProjectedVerts[8 + 2],

            // rightW flipped
            ProjectedVerts[8 + 2],
            ProjectedVerts[8 + 6],
            ProjectedVerts[8 + 5],
            ProjectedVerts[8 + 1],

            // bottomW
            ProjectedVerts[8 + 2],
            ProjectedVerts[8 + 6],
            ProjectedVerts[8 + 7],
            ProjectedVerts[8 + 3],

            // bottomW flipp8+ed
            ProjectedVerts[8 + 3],
            ProjectedVerts[8 + 7],
            ProjectedVerts[8 + 6],
            ProjectedVerts[8 + 2],

            // backW
            ProjectedVerts[8 + 4],
            ProjectedVerts[8 + 7],
            ProjectedVerts[8 + 6],
            ProjectedVerts[8 + 5],

            // backW flipped
            ProjectedVerts[8 + 5],
            ProjectedVerts[8 + 6],
            ProjectedVerts[8 + 7],
            ProjectedVerts[8 + 4],

            //FrontFrontWLeft
            ProjectedVerts[0],
            ProjectedVerts[8],
            ProjectedVerts[11],
            ProjectedVerts[3],

            //FrontFrontWTop
            ProjectedVerts[0],
            ProjectedVerts[1],
            ProjectedVerts[9],
            ProjectedVerts[8],

            //FrontFrontWRight
            ProjectedVerts[9],
            ProjectedVerts[1],
            ProjectedVerts[2],
            ProjectedVerts[10],

            //FrontFrontWBottom
            ProjectedVerts[11],
            ProjectedVerts[10],
            ProjectedVerts[2],
            ProjectedVerts[3],

            //BackBackWLeft
            ProjectedVerts[4],
            ProjectedVerts[7],
            ProjectedVerts[15],
            ProjectedVerts[12],

            //BackBackWTop
            ProjectedVerts[5],
            ProjectedVerts[4],
            ProjectedVerts[12],
            ProjectedVerts[13],

            //BackBackWRight
            ProjectedVerts[5],
            ProjectedVerts[13],
            ProjectedVerts[14],
            ProjectedVerts[6],

            //BackBackWBottom
            ProjectedVerts[7],
            ProjectedVerts[6],
            ProjectedVerts[14],
            ProjectedVerts[15],

            //LeftLeftWLeft
            ProjectedVerts[4],
            ProjectedVerts[12],
            ProjectedVerts[15],
            ProjectedVerts[7],

            //LeftLeftWTop
            ProjectedVerts[4],
            ProjectedVerts[0],
            ProjectedVerts[8],
            ProjectedVerts[12],

            //LeftLeftWRight
            ProjectedVerts[0],
            ProjectedVerts[3],
            ProjectedVerts[11],
            ProjectedVerts[8],

            //LeftLeftWBottom
            ProjectedVerts[3],
            ProjectedVerts[7],
            ProjectedVerts[15],
            ProjectedVerts[11],

            //TopTopWLeft
            ProjectedVerts[4],
            ProjectedVerts[12],
            ProjectedVerts[8],
            ProjectedVerts[0],

            //TopTopWTop
            ProjectedVerts[4],
            ProjectedVerts[5],
            ProjectedVerts[13],
            ProjectedVerts[12],

            //TopTopWRight
            ProjectedVerts[5],
            ProjectedVerts[1],
            ProjectedVerts[9],
            ProjectedVerts[13],

            //TopTopWBottom
            ProjectedVerts[1],
            ProjectedVerts[0],
            ProjectedVerts[8],
            ProjectedVerts[9],

            //RightRightWLeft
            ProjectedVerts[2],
            ProjectedVerts[1],
            ProjectedVerts[9],
            ProjectedVerts[10],

            //RightRightWTop
            ProjectedVerts[1],
            ProjectedVerts[5],
            ProjectedVerts[13],
            ProjectedVerts[9],

            //RightRightWRight
            ProjectedVerts[5],
            ProjectedVerts[6],
            ProjectedVerts[14],
            ProjectedVerts[13],

            //RightRightWBottom
            ProjectedVerts[6],
            ProjectedVerts[2],
            ProjectedVerts[10],
            ProjectedVerts[14],

            //BottomBottomWLeft
            ProjectedVerts[7],
            ProjectedVerts[3],
            ProjectedVerts[11],
            ProjectedVerts[15],

            //BottomBottomWTop
            ProjectedVerts[3],
            ProjectedVerts[2],
            ProjectedVerts[10],
            ProjectedVerts[11],

            //BottomBottomWRight
            ProjectedVerts[2],
            ProjectedVerts[6],
            ProjectedVerts[14],
            ProjectedVerts[10],

            //BottomBottomWBottom
            ProjectedVerts[6],
            ProjectedVerts[7],
            ProjectedVerts[15],
            ProjectedVerts[14],
        };

        Mesh3D.vertices = _meshVerts;
        
        // generate triangles
        if (_tris == null)
        {
            _tris = new int[_meshVerts.Length + _meshVerts.Length / 2];

            // set up triangles
            for (int i = 0; i <= _tris.Length - 6; i += 6)
            {
                _tris[i + 0] = (i / 6) * 4 + 0;
                _tris[i + 1] = (i / 6) * 4 + 1;
                _tris[i + 2] = (i / 6) * 4 + 2;
                _tris[i + 3] = (i / 6) * 4 + 2;
                _tris[i + 4] = (i / 6) * 4 + 3;
                _tris[i + 5] = (i / 6) * 4 + 0;
            }
            Mesh3D.triangles = _tris;
        }

       
        // finalize mesh
        Mesh3D.RecalculateNormals();
        Mesh3D.RecalculateTangents();
        Mesh3D.RecalculateBounds();
    }

    /// <summary>
    /// Rotates the Hypercube around the W axis in the XW, YW and ZW plane
    /// </summary>
    /// <param name="rot"></param>
    public void RotateW(Vector3 rot)
    {
        _wRot.XW += rot.x;
        _wRot.YW += rot.y;
        _wRot.ZW += rot.z;

        UpdateMesh();
    }

    // ######################## COROUTINES ######################## //


    // ######################## UTILITIES ######################## //
}

#if UNITY_EDITOR
/// <summary>
/// Custom Editor for the Hypercube
/// </summary>
[CustomEditor(typeof(Hypercube))]
[CanEditMultipleObjects]
public class TessaractEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        MultiObjectEditorGUI.FloatField(serializedObject, "_projectionDist", "Projection Distance");

        
        EditorGUILayout.BeginHorizontal();
        
        EditorGUIUtility.labelWidth = 70;
        EditorGUILayout.LabelField("W Rotation");
        EditorGUIUtility.labelWidth = 30;
        
        SerializedProperty wRot = serializedObject.FindProperty("_wRot");
        MultiObjectEditorGUI.FloatField(wRot.FindPropertyRelative("XW"), "XW");
        MultiObjectEditorGUI.FloatField(wRot.FindPropertyRelative("YW"), "YW");
        MultiObjectEditorGUI.FloatField(wRot.FindPropertyRelative("ZW"), "ZW");
        EditorGUIUtility.labelWidth = 0;
        
        EditorGUILayout.EndHorizontal();

        

        if (EditorGUI.EndChangeCheck() || Event.current.keyCode == KeyCode.Return)
        {
            foreach (Object t in targets)
            {
                Hypercube tess = t as Hypercube;
                tess.UpdateMesh();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif