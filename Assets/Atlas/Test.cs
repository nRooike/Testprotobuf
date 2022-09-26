using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class Test : MonoBehaviour
{
    [MenuItem("Assets/Create/将文件夹下所有图片打成图集",false,20)]
    public static void CreateSpriteAtlas()
    {
        var temp = Selection.GetFiltered<Object>(SelectionMode.Assets);

        string path = AssetDatabase.GetAssetPath(temp[0]);
        SpriteAtlas spriteAtlas = new SpriteAtlas();

        CreateAtlas(System.Environment.CurrentDirectory + "\\" + path.Replace("/", "\\"), spriteAtlas);

        AssetDatabase.CreateAsset(spriteAtlas, PathConnect(path,"new.spriteatlas"));
    }

    public static void CreateAtlas(string path, SpriteAtlas spriteAtlas)
    {
        Debug.Log(path);
        if (Directory.Exists(path))
        {
            string[] test = Directory.GetDirectories(path);
            foreach (var item in test)
            {
                CreateAtlas(item, spriteAtlas);
            }
            string[] test1 = Directory.GetFiles(path);
            int index = 1;
            foreach (var item in test1)
            {
           
                if (item.EndsWith("png") || item.EndsWith("jpg"))
                {
                    string relativePath = GetRelativePath(System.Environment.CurrentDirectory, item);
                    Debug.Log(relativePath.Replace("\\", "/"));
                    var picture = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
                    spriteAtlas.Add(new Object[] { picture });
                    EditorUtility.DisplayProgressBar("打包图集中...", "正在处理:" + item, index / item.Length);
                }
                index++;
            }
            EditorUtility.ClearProgressBar();
        }
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

    public static string PathConnect(string path1,string path2)
    {
        return path1 + "\\" + path2;
    }
}
