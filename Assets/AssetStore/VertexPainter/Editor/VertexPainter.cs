using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public static class VertexShaders {
	static private Material  vertexMaterial;
	static public Material ShowVertexColor  {
		get {
			if (vertexMaterial != null) return vertexMaterial;
			const string shader = "Shader \"Vertex Data\" {" +
			                      "Properties {	_MainTex (\"Texture\", 2D) = \"white\" {} }" +
			                      "Category {" +
			                      "	Tags { \"Queue\"=\"Geometry\"}" +
			                      "	Lighting Off" +
			                      "	BindChannels {" +
			                      "	Bind \"Color\", color" +
			                      "	Bind \"Vertex\", vertex" +
			                      "	Bind \"TexCoord\", texcoord" +
			                      "}" +
			                      "SubShader {" +
			                      "	Pass {" +
			                      "	SetTexture [_MainTex] {" +
			                      "		combine texture * primary" +
			                      "	}" +
			                      "	SetTexture [_MainTex] {" +
			                      "		constantColor (1,1,1,1)" +
			                      "		combine previous lerp (previous) constant" +
			                      "	}" +
			                      "	}" +
			                      "}" +
			                      "SubShader {" +
			                      "	Pass {" +
			                      "	SetTexture [_MainTex] {" +
			                      "		constantColor (1,1,1,1)" +
			                      "		combine texture lerp(texture) constant" +
			                      "	}" +
			                      "}" +
			                      "}"+
			                      "}}";
			vertexMaterial = new Material(shader);
            vertexMaterial.hideFlags = HideFlags.HideAndDontSave;
			return vertexMaterial;
		}
	}
}

public class VertexPainter : EditorWindow {
  	
    delegate void OnGuiMode();

    private enum ToolModes
    {
        Painting,
        AO
    }
    private ToolModes toolMode;
    private Dictionary<ToolModes, OnGuiMode> toolModeGui; 
    private enum Mode { 
		None,
		Painting
	}
	private static Mode currentMode = Mode.None;
    private static float radius = 0.1f;
    private static float blendFactor = 0.5f;
    private static bool renderVertexColors;
    private static bool showWireFrame;
    private static float intensity = 2;
    private static float minRange = 0.0000000001f, maxRange = 0.03f;
    private static float samples = 128;

    private static Mesh currentSelectionMesh;
    private static Color currentColor;
    private static Material oldMaterial;
    
    private static VertexPainter window;
    private static VertexPainter Window
    {
        get
        {
            if (window != null) return window;
            window = (VertexPainter)GetWindow(typeof(VertexPainter));
            return window;
        }
    }
   
    private GUIStyle boxBackground;
    private GUIStyle BoxBackground
    {
        get
        {
            if (boxBackground != null) return boxBackground;
            boxBackground = new GUIStyle();
            boxBackground.padding.top += 5;
            boxBackground.padding.bottom += 5;
            return boxBackground;
        }
        
    }

    private GUIStyle colorButton;
    private GUIStyle ColorButton
    {
        get
        {
            if (colorButton != null) return colorButton;
            colorButton = new GUIStyle(GUI.skin.button);
            colorButton.fixedHeight = 16;
            colorButton.fixedWidth = 16;

            return colorButton;
        }

    }

	[MenuItem ("Window/VertexPaint")]
    private static void Init()
    {
        window = (VertexPainter)GetWindow (typeof (VertexPainter));
	    CheckSelection();
    }

    public void OnEnable()
    {
        colorButton = null;
        toolModeGui = new Dictionary<ToolModes, OnGuiMode>();
        toolModeGui.Add(ToolModes.Painting, OnGUIPainting);
        toolModeGui.Add(ToolModes.AO, OnGUIAo);
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }
 
    public void OnDisable()
    {
        if (oldMaterial != null)
        {
            if (Selection.activeGameObject != null)
                Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial = oldMaterial;
        }
      
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
      
    }

    public void Update()
    {
        Repaint();
    }

    public void OnSelectionChange()
    {
        currentMode = Mode.None;

        CheckSelection();

        Repaint();
    }

    public void OnDestroy()
    {
        Save();

        if (oldMaterial != null)
        {
            if (Selection.activeGameObject != null)
                Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial = oldMaterial;
        }
        oldMaterial = null;
        currentMode = Mode.None;
        window = null;
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Paint")) toolMode = ToolModes.Painting;
        if (GUILayout.Button("Ao")) toolMode = ToolModes.AO;
        EditorGUILayout.EndHorizontal();


        EditorGUIUtility.labelWidth = 100;
        EditorGUILayout.LabelField("Important :", "On mesh-reimport all changes are gone.", EditorStyles.miniLabel);
        EditorGUILayout.LabelField("Important :", "Press stop button to apply colors!!", EditorStyles.miniLabel);
        EditorGUILayout.Space();
        GUI.enabled = CheckSelection();

        toolModeGui[toolMode]();
        GUI.enabled = true;
    }

    private void OnGUIPainting()
    {
        EditorGUIUtility.labelWidth = 150;
   
        EditorGUILayout.ObjectField("Current selection ", Selection.activeGameObject, typeof(GameObject), true);

        EditorGUILayout.BeginVertical(BoxBackground);
        currentColor = EditorGUILayout.ColorField("Color to paint", currentColor);
        var old = GUI.color;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(153);
        GUI.color = Color.red;
        GUI.backgroundColor = new Color(1, 1, 1, 1);
        if (GUILayout.Button("", ColorButton)) currentColor = Color.red;
        GUI.color = Color.green;
        if (GUILayout.Button("", ColorButton)) currentColor = Color.green;
        GUI.color = Color.blue;
        if (GUILayout.Button("", ColorButton)) currentColor = Color.blue;
        GUI.color = Color.black;
        if (GUILayout.Button("", ColorButton)) currentColor = Color.black;
        GUI.color = new Color(1, 1, 1, 1);
        if (GUILayout.Button("", ColorButton)) currentColor = Color.white;
        GUI.color = old;
        GUI.backgroundColor = new Color(1, 1, 1, 1);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        radius = Mathf.Clamp(EditorGUILayout.FloatField("Radius", radius), 0.1f, 100f);
        blendFactor = Mathf.Clamp(EditorGUILayout.FloatField("Blend", blendFactor), 0.01f, 1f);

        var showVertexLbl = new GUIContent("Show vertex colors", "Shortcut CTRL-Q");
        var showWireFrameLbl = new GUIContent("Show wireframe", "Shortcut CTRL-W");
        renderVertexColors = EditorGUILayout.Toggle(showVertexLbl, renderVertexColors);
        showWireFrame = EditorGUILayout.Toggle(showWireFrameLbl, showWireFrame);
        if (currentSelectionMesh == null) return;
        GUILayout.FlexibleSpace();
        if (currentSelectionMesh.colors.Length != currentSelectionMesh.vertices.Length)
        {
            if (!GUILayout.Button("Generate vertex array")) return;
            currentSelectionMesh.colors = new Color[currentSelectionMesh.vertices.Length];
            EditorUtility.SetDirty(Selection.activeGameObject);
            EditorUtility.SetDirty(currentSelectionMesh);
            AssetDatabase.SaveAssets();
            EditorApplication.SaveAssets();
        }
        else
        {
            if (GUILayout.Button("Set all to choosen color"))
                ResetVertexColors();
            switch (currentMode)
            {
                case Mode.None:
                    if (GUILayout.Button("Paint"))
                    {
                        EditorUtility.SetSelectedWireframeHidden(Selection.activeGameObject.GetComponent<Renderer>(), true);
                        currentMode = Mode.Painting;
                    }
                    break;
                case Mode.Painting:
                    if (GUILayout.Button("Stop"))
                    {
                        Save();
                        currentMode = Mode.None;
                    }
                    break;
            }
        }
    }

    private void OnGUIAo()
    {
        EditorGUIUtility.labelWidth = 140;
        samples = EditorGUILayout.FloatField("AO Samples", samples);
        intensity = EditorGUILayout.FloatField("AO intensity", intensity);
        var label = "Min:" + minRange + " Max" + maxRange;
        EditorGUILayout.MinMaxSlider(new GUIContent(label,"Minimum and Maximum ray length"), ref minRange, ref maxRange, 0f, 1f);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Calculate vertex AO")) BakeVertexAo();
    }

    private static bool CheckSelection()
    {
        if (Selection.activeGameObject == null)
        {
            currentSelectionMesh = null;
            return false;
        }
        var currentSelectionMeshFilter = Selection.activeGameObject.GetComponent<MeshFilter>();
        if (currentSelectionMeshFilter != null)
        {
            currentSelectionMesh = currentSelectionMeshFilter.sharedMesh;
            return true;
        }
        currentSelectionMesh = null;
        return false;
    }
    
    private static void PaintVertexColors()
    {
		var r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		RaycastHit hit;
        if (!Physics.Raycast(r, out hit, float.MaxValue)) return;
        var vertices = currentSelectionMesh.vertices;
        var colrs  = currentSelectionMesh.colors;
			
        Undo.RecordObject(currentSelectionMesh, "Vertex paint");

        var pos = Selection.activeGameObject.transform.InverseTransformPoint(hit.point);
        for (var i=0;i<vertices.Length;i++)
        {
            var sqrMagnitude = (vertices[i] - pos).magnitude;
            if (sqrMagnitude > radius) continue;
            var newColor = Color.Lerp (colrs[i], currentColor, blendFactor);
            colrs[i] = newColor;
        }
        currentSelectionMesh.colors = colrs;
    }

    private static void DrawHandle()
    {
        if (currentMode == Mode.None) return;
		DrawMouseHandle();
    }

    private static void DrawMouseHandle()
    {
        var r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(r, out hit, float.MaxValue)) return;
        Handles.color = Color.black;
        Handles.DrawWireDisc(hit.point, hit.normal, radius);
    }

    private static void SetMaterial()
    {
        if (Selection.activeGameObject == null) return;
        if (currentMode == Mode.None)
        {
            if (oldMaterial != null) 
                Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial = oldMaterial;
            return;
        }
        
        EditorUtility.SetSelectedWireframeHidden(Selection.activeGameObject.GetComponent<Renderer>(), !showWireFrame);
        

		if (renderVertexColors) {
		    if (oldMaterial != null) return;
            oldMaterial = Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial;
            Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial = VertexShaders.ShowVertexColor;
		} else {
		    if (oldMaterial == null) return;
            Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial = oldMaterial;
		    oldMaterial = null;
		}
	}

    private static void OnSceneGUI(SceneView sceneview)
	{
		if (currentSelectionMesh == null) 
			return;

        SetMaterial();
		
        if (currentMode == Mode.None) return;
		
		var ctrlId = GUIUtility.GetControlID ( Window.GetHashCode(), FocusType.Native);
		var current = Event.current;
		
		switch(current.type){
		case EventType.keyUp :
			if ((current.keyCode == KeyCode.Q) && (current.control))
				renderVertexColors = !renderVertexColors;
            if ((current.keyCode == KeyCode.W) && (current.control))
                showWireFrame = !showWireFrame;
			break;
			
		case EventType.mouseUp:
			switch (currentMode) {
				case Mode.Painting:
				break;
			}
			break;
		case EventType.mouseDown:
			
			switch (currentMode) {
				case Mode.Painting:
				    if (Event.current.button==0)
                        current.Use();
				break;
			}
			break;
		case EventType.mouseDrag:
			switch (currentMode) {
				case Mode.None:
					
				break;
				case Mode.Painting:
					EditorUtility.SetDirty(currentSelectionMesh);
					PaintVertexColors();
				break;
			}
			DrawHandle();
			HandleUtility.Repaint();
			break;
		case EventType.mouseMove:
			HandleUtility.Repaint();
			break;
		case EventType.repaint:
			DrawHandle();
		        
            break;
		case EventType.layout:
			HandleUtility.AddDefaultControl(ctrlId);
			break; 
		}
	}
  
    private static void ResetVertexColors()
    {
		var colrs  = currentSelectionMesh.colors;
        for (var i = 0; i < colrs.Length; i++)
            colrs[i] = currentColor;
        currentSelectionMesh.colors = colrs;
	}
    
    private static void Save()
    {
        if ((currentSelectionMesh == null))
            return;

        if (Selection.activeGameObject == null) return;
        
        if (oldMaterial != null)
            Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial = oldMaterial;
        
        oldMaterial = null;

        AssetDatabase.Refresh();

        var id = Selection.activeGameObject.GetInstanceID();
        var p = AssetDatabase.GetAssetPath(currentSelectionMesh);
        if (string.IsNullOrEmpty(p)) p = "Assets/";
        var toDelete = "";
        if ((p.Contains(".asset")) && (!p.Contains(id.ToString(CultureInfo.InvariantCulture))))
            toDelete = p;

        var newMesh = new Mesh
        {
            vertices = currentSelectionMesh.vertices,
            triangles = currentSelectionMesh.triangles,
            colors = currentSelectionMesh.colors,
            tangents = currentSelectionMesh.tangents,
            normals = currentSelectionMesh.normals,
            uv = currentSelectionMesh.uv,
            /*uv1 = currentSelectionMesh.uv2,*/
            uv2 = currentSelectionMesh.uv2
        };
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();

        var newPath = System.IO.Path.GetDirectoryName(p) + "/" + Selection.activeGameObject.name + "_" + id + ".asset";
        AssetDatabase.CreateAsset(newMesh, newPath);
        Selection.activeGameObject.GetComponent<MeshFilter>().sharedMesh = newMesh;
        if (toDelete != "") AssetDatabase.DeleteAsset(toDelete);
        EditorUtility.SetSelectedWireframeHidden(Selection.activeGameObject.GetComponent<Renderer>(), false);
        EditorUtility.SetDirty(Selection.activeGameObject);
        AssetDatabase.Refresh();

    }

    private static void BakeVertexAo()
    {
        var selection = Selection.activeGameObject;
        var verts = currentSelectionMesh.vertices;
        var colors = currentSelectionMesh.colors;
        if (colors.Length == 0)
            colors = new Color[verts.Length];

        for (var ic = 0; ic < colors.Length; ic++)
            colors[ic] = new Color(0, 0, 0, 0);

        var normals = currentSelectionMesh.normals;
        
        if (normals.Length == 0)
        {
            currentSelectionMesh.RecalculateNormals();
            normals = currentSelectionMesh.normals;
        }
        var l = verts.Length;
        for (var i = 0; i < l; i++)
        {
            var nrm = normals[i];
            var v = selection.transform.TransformPoint(verts[i]);
            var n = selection.transform.TransformPoint(verts[i] + nrm);
            var wnrm = (n - v).normalized;
            var occlusionColor = 0f;
            for (var j = 0; j < samples; j++)
            {
                var rot = 180.0f;
                var rot2 = rot / 2.0f;
                var rotx = ((rot * Random.value) - rot2);
                var roty = ((rot * Random.value) - rot2);
                var rotz = ((rot * Random.value) - rot2);
                var dir = Quaternion.Euler(rotx, roty, rotz) * Vector3.up;
                var dirq = Quaternion.FromToRotation(Vector3.up, wnrm);
                var ray = dirq * dir;
                var offset = Vector3.Reflect(ray, wnrm);
                ray = ray * (maxRange / ray.magnitude);
                var hit = new RaycastHit();
                var start = v + (offset * maxRange);
                var end = v + ray;

                if (Physics.Linecast(start, end, out hit))
                {
                    if ((hit.distance > minRange))
                    {
                        occlusionColor += Mathf.Clamp01(1 - (hit.distance / maxRange));
                    }
                }

            }
            occlusionColor = Mathf.Clamp01(1 - ((occlusionColor * intensity) / samples));
            colors[i].a = occlusionColor;
        }
        
        currentSelectionMesh.colors = colors;
        Save();
    }

}

