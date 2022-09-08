using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.U2D;
public class Test : MonoBehaviour
{
    [MenuItem("Assets/Create/Test SpriteAtlas")]
    public static void CreateSpriteAtlas()
    {
       var temp= Selection.GetFiltered<Object>(SelectionMode.Assets);

       string path=   AssetDatabase.GetAssetPath(temp[0]);
        SpriteAtlas spriteAtlas = new SpriteAtlas();

        CreateSpriteAtlas(path, spriteAtlas);
    }

    public void CreateAtlas(string path, SpriteAtlas spriteAtlas)
    {
        if (Directory.Exists(path))
        {
            string[] test = Directory.GetDirectories(path);
            foreach (var item in test)
            {
                CreateAtlas(item, spriteAtlas);
            }
            string[] test1 = Directory.GetFiles(path);
            foreach (var item in test1)
            {
                if (item.EndsWith("PNG") || item.EndsWith("JNG"))
                {
                  var picture=  AssetDatabase.LoadAssetAtPath<Sprite>(item);
                    spriteAtlas.GetType
                }
            }
        }
    }
}
