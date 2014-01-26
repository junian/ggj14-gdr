using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TagEnumerator : EditorWindow {

	[MenuItem ("Junian.Net/Enumerate Tags")]
	static void Init () {
		#if UNITY_EDITOR
		string path = EditorUtility.SaveFilePanelInProject("Save Tag Enums", "Tags", "cs", "Select save file path");
		if(!string.IsNullOrEmpty(path))
		{
			string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
			File.WriteAllText(path, GenerateTagEnums("Tags", tags));
			AssetDatabase.Refresh();
			EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
		}
		#endif
	}

	private static string GenerateTagEnums(string className, string[] tags)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("public class ").Append(className).Append("\n{\n");

		for(int i=0;i<tags.Length;i++)
		{
			sb.Append("    public const string ")
				.Append(tags[i].Replace(" ","").Trim())
				.Append(" = ")
				.Append("\"")
				.Append(tags[i])
				.Append("\";\n");
		}

		sb.Append("}\n");
		return sb.ToString();
	}
}
