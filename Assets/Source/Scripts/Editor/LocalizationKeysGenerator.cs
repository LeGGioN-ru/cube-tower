using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Localization.Settings;
using UnityEditor.Localization;

public class LocalizationKeysGenerator : EditorWindow
{
    private const string OutputPath = "Assets/Source/Scripts/Text/LocalizationKeys.cs";

    [MenuItem("Tools/Generate Localization Keys")]
    public static void GenerateKeys()
    {
        var collections = AssetDatabase.FindAssets("t:StringTableCollection");
        
        StringBuilder sb = new StringBuilder();
        
        sb.AppendLine("// ЭТОТ ФАЙЛ СГЕНЕРИРОВАН АВТОМАТИЧЕСКИ. НЕ МЕНЯЙТЕ ЕГО ВРУЧНУЮ.");
        sb.AppendLine("public static class LocalizationKeys");
        sb.AppendLine("{");

        foreach (var guid in collections)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var collection = AssetDatabase.LoadAssetAtPath<StringTableCollection>(path);
            
            if (collection == null) 
                continue;

            string className = SanitizeName(collection.TableCollectionName);
            
            sb.AppendLine($"\tpublic static class {className}");
            sb.AppendLine("\t{");
            
            sb.AppendLine($"\t\tpublic const string TableName = \"{collection.TableCollectionName}\";");
            sb.AppendLine();

            var sharedData = collection.SharedData;

            foreach (var entry in sharedData.Entries)
            {
                string keyName = entry.Key;
                long keyId = entry.Id;

                string varName = SanitizeName(keyName);

                if (char.IsDigit(varName[0])) 
                    varName = "_" + varName;

                sb.AppendLine($"\t\tpublic const string {varName} = \"{keyName}\";");
            }

            sb.AppendLine("\t}");
        }

        sb.AppendLine("}");

        string folder = Path.GetDirectoryName(OutputPath);
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        File.WriteAllText(OutputPath, sb.ToString());
        
        AssetDatabase.Refresh();
        Debug.Log($"<color=green>Localization Keys сгенерированы по пути: {OutputPath}</color>");
    }

    private static string SanitizeName(string str)
    {
        string clean = Regex.Replace(str, "[^a-zA-Z0-9_]", "_");
        return clean;
    }
}