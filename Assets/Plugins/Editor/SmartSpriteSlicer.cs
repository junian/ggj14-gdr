using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Linq;

public enum SpriteTextType
{
	XML,
	TXT
}

public class SmartSpriteSlicer : EditorWindow {

	Texture2D texture;
	TextAsset xmlTexture;
	SpriteTextType type = SpriteTextType.XML;

	[MenuItem("Junian.Net/Smart Sprite Slicer")]
	static void Init()
	{
		SmartSpriteSlicer window = EditorWindow.GetWindow<SmartSpriteSlicer>();
		window.title = "Smart Sprite Slicer";
		window.ShowPopup();
	}

	void OnGUI()
	{
		texture = (Texture2D) EditorGUILayout.ObjectField(
			texture,
			typeof(Texture2D),
			false);

		xmlTexture = (TextAsset) EditorGUILayout.ObjectField(
			xmlTexture,
			typeof(TextAsset),
			false);

		type = (SpriteTextType) EditorGUILayout.EnumPopup(type);

		if(GUILayout.Button("Execture"))
		{
			if(texture == null || xmlTexture == null)
			{
				//ShowNotification("Please select texture and its xml file");
			}
			else
			{
				if(type == SpriteTextType.XML)
					GenerateXmlSpriteSheet();
				else if(type == SpriteTextType.TXT)
					GenerateTxtSpriteSheet();
			}
		}
	}

	void GenerateTxtSpriteSheet()
	{
		var path = AssetDatabase.GetAssetPath(texture);
		var importer = (TextureImporter) TextureImporter.GetAtPath(path);

		var doc = xmlTexture.text;
		var sprites = doc.Split(new string[]{System.Environment.NewLine},System.StringSplitOptions.RemoveEmptyEntries);
		var spriteSheet = new List<SpriteMetaData>();
		
		foreach(var s in sprites)
		{
			SpriteMetaData meta = new SpriteMetaData();

			var split = s.Split(new char[]{'='});

			meta.name = split[0].Trim();
			
			split = split[1].Trim().Split(new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);

			float width  = float.Parse(split[2]);
			float height = float.Parse(split[3]);
			
			float x = float.Parse(split[0]);
			float y = float.Parse(split[1]);
			
			y = texture.height - y - height;
			
			meta.pivot = new Vector2(0.5f, 0.5f);
			meta.rect = new Rect(x,y,width,height);
			
			spriteSheet.Add(meta);
		}
		
		importer.spriteImportMode = SpriteImportMode.Multiple;
		importer.spritesheet = spriteSheet.ToArray();
		
		EditorUtility.SetDirty(importer);
		
		AssetDatabase.ImportAsset(path);
		AssetDatabase.Refresh();
		
		EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
	}

	void GenerateXmlSpriteSheet()
	{
		var path = AssetDatabase.GetAssetPath(texture);
		var importer = (TextureImporter) TextureImporter.GetAtPath(path);
		var doc = XDocument.Parse(xmlTexture.text);
		var sprites = doc.Element("TextureAtlas").Elements("SubTexture");
		var spriteSheet = new List<SpriteMetaData>();

		foreach(var s in sprites)
		{
			SpriteMetaData meta = new SpriteMetaData();

			meta.name = (string) s.Attribute("name");

			float width  = float.Parse((string) s.Attribute("width"));
			float height = float.Parse((string) s.Attribute("height"));

			float x = float.Parse((string) s.Attribute("x"));
			float y = float.Parse((string) s.Attribute("y"));

			y = texture.height - y - height;

			meta.pivot = new Vector2(0.5f, 0.5f);
			meta.rect = new Rect(x,y,width,height);

			spriteSheet.Add(meta);
		}

		importer.spriteImportMode = SpriteImportMode.Multiple;
		importer.spritesheet = spriteSheet.ToArray();

		EditorUtility.SetDirty(importer);

		AssetDatabase.ImportAsset(path);
		AssetDatabase.Refresh();

		EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
	}
}
