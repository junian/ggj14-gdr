using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LayerEnumerator : EditorWindow {
	
	[MenuItem ("Junian.Net/Enumerate Layers")]
	static void Init () {
		#if UNITY_EDITOR
		string path = EditorUtility.SaveFilePanelInProject("Save Layer Enums", "Layers", "cs", "Select save file path");
		if(!string.IsNullOrEmpty(path))
		{
			List<string> layers = new List<string>();
			for(int i=8;i<=31;i++) //user defined layers start with layer 8 and unity supports 31 layers
			{
				var layerN=LayerMask.LayerToName(i); //get the name of the layer
				if(layerN.Length>0) //only add the layer if it has been named (comment this line out if you want every layer)
					layers.Add(layerN);
			}

			File.WriteAllText(path, GenerateLayerEnums("Layers", layers.ToArray()));
			AssetDatabase.Refresh();
			EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
		}
		#endif
	}
	
	private static string GenerateLayerEnums(string className, string[] layers)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("public class ").Append(className).Append("\n{\n");
		
		for(int i=0;i<layers.Length;i++)
		{
			sb.Append("    public const string ")
				.Append(layers[i].Replace(" ","").Trim())
					.Append(" = ")
					.Append("\"")
					.Append(layers[i])
					.Append("\";\n");
		}
		
		sb.Append("}\n");
		return sb.ToString();
	}
}