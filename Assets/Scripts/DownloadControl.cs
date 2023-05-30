using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ResourceTools;
using UnityEngine;
using UnityEngine.UI;

public class DownloadControl : MonoBehaviour
{
    public Slider Progress;
    public GameObject DownloadButton;

    public Text Info, ProgressInfo;
    

    private bool _updateSliderFlag = false;
    
    public void Update()
    {
        if (!_updateSliderFlag)
        {
            return;
        }

        long totaLength = 0;
        long updateLength = 0;
        long totalCount = 0;
        long updateCount = 0;
        List<Updater> updaters =  AssetBundlesManager.GetAllUpdater();
        foreach (Updater updater in updaters)
        {
            totaLength += updater.TotalLength;
            updateLength += updater.UpdatedLength;
            totalCount += updater.TotalCount;
            updateCount += updater.UpdatedCount;
        }

        ProgressInfo.text = updateCount + "/" + totalCount;

        if (totalCount == 0)
        {
            _updateSliderFlag = false;
            Progress.value = 1;
            return;
        }
        
        Progress.value = (updateLength * 1.0f) / totaLength;
        if (Progress.value >= 1)
        {
            _updateSliderFlag = false;
        }
    }


    public void Download()
    {
        List<Updater> updaters =  AssetBundlesManager.GetAllUpdater();

        foreach (Updater updater in updaters)
        {
            AssetBundlesManager.UpdateAssets(OnFileDownloaded, updater.UpdateGroup);
        }

        _updateSliderFlag = true;
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
