using System.Collections;
using System.Collections.Generic;
using ResourceTools;
using UnityEngine;
using UnityEngine.UI;

public class TestPrefab : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(4);
        // GetComponentInChildren<Image>().sprite = AssetBundlesManager.LoadAsset<Sprite>("Assets/AssetBundleAssets/MiniGame/101/PuzzleLevel/1006/img_game_5.png");
        // GetComponentInChildren<RawImage>().texture = AssetBundlesManager.LoadAsset<Texture>("Assets/AssetBundleAssets/MiniGame/101/PuzzleLevel/1006/img_game_5.png");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
