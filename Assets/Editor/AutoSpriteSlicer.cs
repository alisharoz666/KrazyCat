using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.IO;

public class AutoSpriteSlicer : EditorWindow
{
    DefaultAsset selectedFolder;

    [MenuItem("Tools/Auto Slice Sprites in Folder")]
    static void Show() => GetWindow<AutoSpriteSlicer>("Auto Sprite Slicer");

    void OnGUI()
    {
        selectedFolder = (DefaultAsset)EditorGUILayout.ObjectField("Folder", selectedFolder, typeof(DefaultAsset), false);
        if (selectedFolder != null && GUILayout.Button("Slice All Sprites"))
            ProcessFolder();
    }

    void ProcessFolder()
    {
        string folderPath = AssetDatabase.GetAssetPath(selectedFolder);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("Not a valid folder.");
            return;
        }

        string fullPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + folderPath;
        var files = Directory.GetFiles(fullPath, "*.png", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string assetPath = "Assets" + file.Substring(Application.dataPath.Length);
            SliceAsset(assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ All done.");
    }

    void SliceAsset(string assetPath)
    {
        var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null) return;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.isReadable = true;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;

        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        if (texture == null) return;

        var rects = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(texture, 4, 0);
        var metas = new SpriteMetaData[rects.Length];
        for (int i = 0; i < rects.Length; i++)
        {
            metas[i] = new SpriteMetaData
            {
                name = Path.GetFileNameWithoutExtension(assetPath) + "_" + i,
                rect = rects[i],
                alignment = (int)SpriteAlignment.Center,
                pivot = new Vector2(0.5f, 0.5f)
            };
        }

        importer.spritesheet = metas;
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        Debug.Log($"Sliced {assetPath} into {metas.Length} sprites.");
    }
}
