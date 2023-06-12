using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ResourceTools;
public class TestUpdateImage : MonoBehaviour
{

    public Image updateImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateImage()
    {
        updateImage.sprite = AssetBundlesManager.LoadAsset<Sprite>("Assets/AssetBundleAssets/MiniGame/101/101/imgStoryWomanBaby.png");
        updateImage.SetNativeSize();
    }

    public void UpdateBundle()
    {
        List<Updater> updaters =  AssetBundlesManager.GetAllUpdater();

        foreach (Updater updater in updaters)
        {
            AssetBundlesManager.UpdateAssets(OnFileDownloaded, updater.UpdateGroup);
        }
    }
    
    private void OnFileDownloaded(bool success, int updatedCount, long updatedLength, int totalCount, long totalLength, string fileName, string group)
    {
        if (!success)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("已更新数量：" + updatedCount);
        sb.AppendLine("已更新大小：" + updatedLength);
        sb.AppendLine("总数量：" + totalCount);
        sb.AppendLine("总大小：" + totalLength);
        sb.AppendLine("资源名：" + fileName);
        sb.AppendLine("资源组：" + group);

        Debug.Log(sb.ToString());
        sb.Clear();
       
        AssetBundlesManager.SendDebugEvent(String.Format("Downloaded_Bundle_Name={0}_Group={1}", fileName, group));


        if (updatedCount >= totalCount)
        {
            Debug.Log(group + "组的所有资源下载完毕");

            Debug.Log($"请打开 {Application.persistentDataPath}  查看");
           
            AssetBundlesManager.SendDebugEvent("Downloaded_Bundle_Finish");

        }
        
        
    } 
}
