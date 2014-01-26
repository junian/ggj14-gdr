using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneEnumerator : EditorWindow {
	
	[MenuItem ("Junian.Net/Enumerate Scenes")]
	static void Init () {
		#if UNITY_EDITOR
		string path = EditorUtility.SaveFilePanelInProject("Save Scene Enums", "Scenes", "cs", "Select save file path");
		if(!string.IsNullOrEmpty(path))
		{
			List<string> scenes = new List<string>();
			foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
			{
				if (scene.enabled)
				{
					string name = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
					name = name.Substring(0, name.Length - 6);
					scenes.Add(name);
				}
			}

			File.WriteAllText(path, GenerateSceneEnums("Scenes", scenes.ToArray()));
			AssetDatabase.Refresh();
			EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
		}
		#endif
	}
	
	private static string GenerateSceneEnums(string className, string[] scenes)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("public class ").Append(className).Append("\n{\n");
		
		for(int i=0;i<scenes.Length;i++)
		{
			sb.Append("    public const string ")
				.Append(scenes[i].Replace(" ","").Trim())
					.Append(" = ")
					.Append("\"")
					.Append(scenes[i])
					.Append("\";\n");
		}
		
		sb.Append("}\n");
		return sb.ToString();
	}
}