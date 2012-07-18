using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BatchProcessor : EditorWindow {
	
	private static string nameContains = "";
	private static string nameDoesNotContain = "";
	private static Component comp;
	private static GameObject addThisGameObject;
	private static Object addThisComponent;
	private static Component modifyThisComponent;
	private static Component rememberModifyThisComponent;
	private static List<string> fieldNames = new List<string>();
	private static List<System.Type> fieldType = new List<System.Type>();
	private static ArrayList fieldValue = new ArrayList();
	private static List<bool> applyValue = new List<bool>();
	
	private static List<GameObject> found = new List<GameObject>();
	private static List<GameObject> done = new List<GameObject>();
	
	private static Vector2 scrollPos = Vector2.zero;
	private static Vector2 scrollPos2 = Vector2.zero;
	private static Vector2 scrollPos3 = Vector2.zero;
	
	
	[MenuItem("Window/BatchProcessor")]
	public static void Init(){
		EditorWindow.GetWindow(typeof(BatchProcessor), false, "BatchProcessor");	
	}
	
	
	void OnGUI() {
		
		EditorGUILayout.BeginHorizontal();
		
		//FIND
		EditorGUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width * 0.25f));
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("FIND GameObjects");
		EditorGUILayout.Space();
		
		if(GUILayout.Button ("Clear")) Clear();
		nameContains = EditorGUILayout.TextField("Name must contain", nameContains);
		nameDoesNotContain = EditorGUILayout.TextField("Name must not contain", nameDoesNotContain);
		comp = EditorGUILayout.ObjectField("Has Component", comp, typeof(Component), true) as Component;
		EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button ("Find!")) Find();
			if(GUILayout.Button ("Filter!")) Filter();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Space();
		
		//FOUND
		Rect foundRect = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width * 0.25f));
		GUI.Box(foundRect, GUIContent.none);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("FOUND");
		EditorGUILayout.Space();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		for(int i = 0; i < found.Count; i++) {
			found[i] = (GameObject) EditorGUILayout.ObjectField(found[i], typeof(GameObject), true);
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Space();
		
		//DO
		EditorGUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width * 0.25f));
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("DO");
		EditorGUILayout.Space();
		scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3);
		addThisGameObject = (GameObject) EditorGUILayout.ObjectField("AddThisGameObject", addThisGameObject, typeof(GameObject), true);
		if(addThisComponent != null) if(!addThisComponent.GetType().IsSubclassOf(typeof(Component)) && addThisComponent.GetType() != typeof(MonoScript)) GUI.color = Color.red;
		addThisComponent = EditorGUILayout.ObjectField("AddThis", addThisComponent, typeof(Object), true) as Object;
		GUI.color = Color.white;
		modifyThisComponent = EditorGUILayout.ObjectField("Modify Component", modifyThisComponent, typeof(Component), true) as Component;
		if(modifyThisComponent != rememberModifyThisComponent) {
			rememberModifyThisComponent = modifyThisComponent;
			fieldNames.Clear();
			fieldType.Clear();
			fieldValue.Clear();
			if(modifyThisComponent != null) {
				foreach(System.Reflection.FieldInfo fi in modifyThisComponent.GetType().GetFields() ) {
			        //System.Object obj = (System.Object) c;
					fieldNames.Add(fi.Name);
					fieldType.Add(fi.FieldType);
					fieldValue.Add(fi.GetValue(modifyThisComponent));
					applyValue.Add(false);
			        EditorGUILayout.TextField("fi name "+fi.Name+" type "+ fi.FieldType); // + " val" fi.GetValue(obj));
			    }
			}
		}
		if(modifyThisComponent != null) {
			for(int i = 0; i < fieldNames.Count; i++) {
				if(fieldType[i] == typeof(string) || fieldType[i] == typeof(int) || fieldType[i] == typeof(float) || fieldType[i] == typeof(bool) || fieldType[i] == typeof(Vector3) || fieldType[i] == typeof(Vector2) || fieldType[i] == typeof(Rect) || fieldType[i] == typeof(Color) || fieldType[i] == typeof(GameObject) || fieldType[i] == typeof(Transform) || fieldType[i] == typeof(Material) || fieldType[i] == typeof(Mesh)) {
					EditorGUILayout.BeginHorizontal();
					applyValue[i] = EditorGUILayout.Toggle(applyValue[i], GUILayout.MaxWidth(20));
					if(!applyValue[i]) GUI.enabled = false;
					if(fieldType[i] == typeof(string)) {
						fieldValue[i] = EditorGUILayout.TextField(fieldNames[i], (string) fieldValue[i]);
					} else if(fieldType[i] == typeof(int)) {
						fieldValue[i] = EditorGUILayout.IntField(fieldNames[i], (int) fieldValue[i]);
					} else if(fieldType[i] == typeof(float)) {
						fieldValue[i] = EditorGUILayout.FloatField(fieldNames[i], (float) fieldValue[i]);
					} else if(fieldType[i] == typeof(bool)) {
						fieldValue[i] = EditorGUILayout.Toggle(fieldNames[i], (bool) fieldValue[i]);
					} else if(fieldType[i] == typeof(Vector3)) {
						fieldValue[i] = EditorGUILayout.Vector3Field(fieldNames[i], (Vector3) fieldValue[i]);
					} else if(fieldType[i] == typeof(Vector2)) {
						fieldValue[i] = EditorGUILayout.Vector2Field(fieldNames[i], (Vector2) fieldValue[i]);
					} else if(fieldType[i] == typeof(Rect)) {
						fieldValue[i] = EditorGUILayout.RectField(fieldNames[i], (Rect) fieldValue[i]);
					} else if(fieldType[i] == typeof(Color)) {
						fieldValue[i] = EditorGUILayout.ColorField(fieldNames[i], (Color) fieldValue[i]);
					} else if(fieldType[i] == typeof(GameObject)) {
						fieldValue[i] = EditorGUILayout.ObjectField(fieldNames[i], (GameObject) fieldValue[i], typeof(GameObject), true);
					} else if(fieldType[i] == typeof(Transform)) {
						fieldValue[i] = EditorGUILayout.ObjectField(fieldNames[i], (Transform) fieldValue[i], typeof(Transform), true);
					} else if(fieldType[i] == typeof(Material)) {
						fieldValue[i] = EditorGUILayout.ObjectField(fieldNames[i], (Material) fieldValue[i], typeof(Material), true);
					} else if(fieldType[i] == typeof(Mesh)) {
						fieldValue[i] = EditorGUILayout.ObjectField(fieldNames[i], (Mesh) fieldValue[i], typeof(Mesh), true);
					} else {
						EditorGUILayout.LabelField(fieldNames[i], "");
					}
					GUI.enabled = true;
					EditorGUILayout.EndHorizontal();
				}
			}
			
		}
		EditorGUILayout.EndScrollView();
		if(GUILayout.Button ("Process!")) DoIt();
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Space();
		
		//DONE
		foundRect = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width * 0.25f));
		GUI.Box(foundRect, GUIContent.none);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("DONE");
		EditorGUILayout.Space();
		scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
		for(int i = 0; i < done.Count; i++) {
			done[i] = (GameObject) EditorGUILayout.ObjectField(done[i], typeof(GameObject), true);
		}
		EditorGUILayout.EndScrollView();		
		EditorGUILayout.EndVertical();
	
		EditorGUILayout.EndHorizontal();
	}
	
	
	void Clear() {
		found.Clear();
		done.Clear();
	}
	
	void Find() {
		found.Clear();
		if(nameContains != "") {
			GameObject[] gos = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			foreach(GameObject g in gos) {
				if(g.name.Contains(nameContains)) {
					if(!found.Contains(g)) found.Add(g);
				}
			}
		}
		if(nameDoesNotContain != "") {
			List<GameObject> removeFromList = new List<GameObject>();
			foreach(GameObject g in found) {
				if(g.name.Contains(nameDoesNotContain)) removeFromList.Add(g);
			}
			foreach(GameObject g in removeFromList) {
				found.Remove(g);
			}
		}
		if(comp != null) {
			System.Type objType = comp.GetType();
			Component[] comps = (Component[]) Component.FindObjectsOfType(objType);
			foreach(Component g in comps) {
				EditorGUIUtility.PingObject(g);
				if(g != null) {
					if(!found.Contains(g.gameObject)) found.Add(g.gameObject);
				}
			}		
		}
		Selection.objects = found.ToArray();
	}
	
	
	void Filter() {
		if(nameContains != "") {
			List<GameObject> removeFromList = new List<GameObject>();
			foreach(GameObject g in found) {
				if(!g.name.Contains(nameContains)) {
					removeFromList.Add(g);
				}
			}
			foreach(GameObject g in removeFromList) {
				found.Remove(g);
			}
		}
		if(nameDoesNotContain != "") {
			List<GameObject> removeFromList = new List<GameObject>();
			foreach(GameObject g in found) {
				if(g.name.Contains(nameDoesNotContain)) removeFromList.Add(g);
			}
			foreach(GameObject g in removeFromList) {
				found.Remove(g);
			}
		}
		if(comp != null) {
			List<GameObject> removeFromList = new List<GameObject>();
			System.Type objType = comp.GetType();
			foreach(GameObject g in found) {
				if(!g.GetComponent(objType)) {
					removeFromList.Add(g);
				}
			}
			foreach(GameObject g in removeFromList) {
				found.Remove(g);
			}				
		} 	
		Selection.objects = found.ToArray();
	}
	
	
	void DoIt() {
		done.Clear();
		foreach(GameObject g in found) {
			if(g == null) continue;
			//EditorGUIUtility.PingObject(g);
//			if(!done.Contains(g)) done.Add(g);
			
			if(addThisGameObject != null) {
				GameObject newGO = GameObject.Instantiate(addThisGameObject) as GameObject;
				newGO.transform.parent = g.transform;
				newGO.transform.position = g.transform.position;
				if(!done.Contains(g)) done.Add(g);
			}
			
			if(addThisComponent != null) {
				Debug.Log (addThisComponent.GetType());
				System.Type t = addThisComponent.GetType();
				
				if(addThisComponent.GetType() == typeof(MonoScript)) {
					if(g != null) {
						g.AddComponent(((MonoScript) addThisComponent).GetClass());	
						if(!done.Contains(g)) done.Add(g);
					}
				} else if(addThisComponent.GetType().IsSubclassOf(typeof(Component))) {
					if(g != null) {
						g.AddComponent(addThisComponent.GetType());
						if(!done.Contains(g)) done.Add(g);
					}
				} else {
					Debug.LogWarning("Can't add this! Not a MonoScript or Class not a derived from Component!");
				}
				
			}
			
			if(modifyThisComponent != null) {
				ArrayList x = new ArrayList();
				x.Add(g.GetComponent(modifyThisComponent.GetType()));
				if(x[0] != null) {
					Debug.Log ("Component Found!");
					foreach(System.Reflection.FieldInfo fi in x[0].GetType().GetFields() ) {
						for(int i = 0; i < fieldNames.Count; i++) {
							if(fi.Name == fieldNames[i]) fi.SetValue(x[0], fieldValue[i]);
						}
					}
				}
				if(!done.Contains(g)) done.Add(g);
			}
			
			foreach(Object o in done) EditorUtility.SetDirty(o);
			Selection.objects = done.ToArray();
		}
	}
	

}
