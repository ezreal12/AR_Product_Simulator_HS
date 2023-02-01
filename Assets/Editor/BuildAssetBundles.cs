using UnityEditor;
using UnityEngine;
using System.IO;
public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        // BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);

        string assetBundleDir = "Assets/AssetBundles";

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDir);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDir, BuildAssetBundleOptions.None, BuildTarget.Android);

    }
}