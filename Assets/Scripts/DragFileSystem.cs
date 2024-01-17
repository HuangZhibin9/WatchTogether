using B83.Win32;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DragFileSystem : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoManager videoManager;
    private DropInfo dropInfo = null;

    [FilePath]
    public string testPath;

    private class DropInfo
    {
        public string file;
        public Vector2 pos;
    }

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }

    private void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    private void OnFiles(List<string> aFiles, POINT aPos)
    {
        string file = "";
        foreach (var f in aFiles)
        {
            var fi = new System.IO.FileInfo(f);
            var ext = fi.Extension.ToLower();
            if (ext == ".mp4")
            {
                file = f;
                break;
            }
        }
        if (file != "")
        {
            var info = new DropInfo
            {
                file = file,
                pos = new Vector2(aPos.x, aPos.y)
            };
            dropInfo = info;
            LoadVideo(dropInfo);
        }
    }

    [Button]
    public void testfun()
    {
        LoadVideo(testPath);
    }

    private void LoadVideo(DropInfo aInfo)
    {
        if (aInfo == null)
        {
            return;
        }
        videoPlayer.url = "file://" + aInfo.file;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    [Button]
    private void LoadVideo(string file)
    {
        videoPlayer.url = "file://" + file;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        videoManager.OnVideoPrepared();
    }
}