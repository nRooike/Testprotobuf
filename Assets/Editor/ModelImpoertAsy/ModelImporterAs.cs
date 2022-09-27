using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class ModelImporterAs : MonoBehaviour
{
    [MenuItem("Tools/ModeImportDataOutput")]
    public static void OutputImportData()
    {
        var temp = Selection.GetFiltered<Object>(SelectionMode.Assets);

        string path = AssetDatabase.GetAssetPath(temp[0]);


        ModelGetImportInfor(path);
    }

    public static void ModelGetImportInfor(string path)
    {
        /*if (Directory.Exists(path))
        {
            string[] test = Directory.GetDirectories(path);
            foreach (var item in test)
            {
                ModelGetImportInfor(item);
            }
            string[] test1 = Directory.GetFiles(path);
            int index = 1;
            
            foreach (var item in test1)
            {
              
                string relativePath = Assetpath(Application.dataPath, item);
           
                var assetImport = ModelImporter.GetAtPath(relativePath);
                if (assetImport is ModelImporter)
                {
                    ModelImporter modelImporter = assetImport as ModelImporter;
                    Debug.Log(modelImporter.assetPath);
                }
                index++;
            }*/
        // EditorUtility.ClearProgressBar();
        //}
    }

    private static string GetRelativePath(string pathA, string pathB)
    {
        string[] pathAArray = pathA.Split('\\');
        string[] pathBArray = pathB.Split('\\');
        //返回2者之间的最小长度
        int s = pathAArray.Length >= pathBArray.Length ? pathBArray.Length : pathAArray.Length;
        //两个目录最底层的共用目录的索引
        int closestRootIndex = -1;
        for (int i = 0; i < s; i++)
        {
            if (pathAArray[i] == pathBArray[i])
            {
                closestRootIndex = i;
            }
            else
            {
                break;
            }
        }
        //由pathA计算 ‘../’部分
        string pathADepth = "";
        for (int i = 0; i < pathAArray.Length; i++)
        {
            if (i > closestRootIndex + 1)
            {
                pathADepth += "../";
            }
        }
        //由pathB计算‘../’后面的目录
        string pathBdepth = "";
        for (int i = closestRootIndex + 1; i < pathBArray.Length; i++)
        {
            pathBdepth += "/" + pathBArray[i];
        }
        pathBdepth = pathBdepth.Substring(1);//去掉重复的斜杠 “ / ”
        return pathADepth + pathBdepth;//pathB相对于pathA的相对路径
    }

    public static string PathConnect(string path1, string path2)
    {
        return path1 + "\\" + path2;
    }

    [MenuItem("Utilies/CopyModelImporterData")]
    public static void ComparePath()
    {
        TextAsset textAsset = Selection.activeObject as TextAsset;
        string[] arrStr = textAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        List<string> pathArr = new List<string>(400);

        foreach (var item in arrStr)
        {

            ModelImporter modelImporter = ModelImporter.GetAtPath(item) as ModelImporter;
            if (null != modelImporter && modelImporter.isReadable)
            {
                pathArr.Add(item);
            }
        }

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in pathArr)
        {
            stringBuilder.Append(item + "\n");
        }
        string fullPath = Application.dataPath.Split(new string[] { "Assets" }, System.StringSplitOptions.None)[0] + AssetDatabase.GetAssetPath(textAsset);

        if (File.Exists(fullPath))
        {
            File.WriteAllText(fullPath, stringBuilder.ToString());
        }
    }

    [MenuItem("Utilies/PasteModeImporter")]
    public static void PasteModeImporter()
    {
        TextAsset textAsset = Selection.activeObject as TextAsset;
        string[] arrStr = textAsset.text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        float a = 0;
        foreach (var item in arrStr)
        {
            ModelImporter modelImporter = ModelImporter.GetAtPath(item) as ModelImporter;
            if (null != modelImporter)
            {
                modelImporter.isReadable = true;
                modelImporter.SaveAndReimport();
            }
            a++;
            EditorUtility.DisplayProgressBar("改写ModelImporter的read/write中", "", a / arrStr.Length);
        }
        EditorUtility.ClearProgressBar();
    }
}
