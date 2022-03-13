﻿using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace XLMapExtensions
{
    [Serializable]
    public class DownloadImageOnTrigger : MonoBehaviour
    {
        [Tooltip("Enter the URL to download the image from.")]
        public string imageDownloadUrl;

        [Tooltip("This path is relative to the user's gear folder.  For example, if you put 'Temp' here, the file will be downloaded to <user's gear folder>/Temp.  If left empty, file will be downloaded to root of gear folder.")]
        public string downloadPath;

        [Tooltip("This is the filename to write to disk when saving the downloaded image.")]
        public string downloadFilename;

        private Collider boardCollider;

        private void Start()
        {
            boardCollider = PlayerController.Instance.boardController.boardColliders[0];
        }

        private void OnTriggerEnter(Collider c)
        {
            if (c != boardCollider) return;

            if (string.IsNullOrEmpty(imageDownloadUrl)) return;

            StartCoroutine(DownloadImageCoroutine());
        }

        private IEnumerator DownloadImageCoroutine()
        {
            using (var request = UnityWebRequestTexture.GetTexture(imageDownloadUrl))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError) yield break;

                var textureDownloadHandler = request.downloadHandler as DownloadHandlerTexture;
                if (textureDownloadHandler == null) yield break;

                var bytes = textureDownloadHandler.texture.EncodeToPNG();

                var destinationPath = GetDestinationPath();
                if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);

                var destinationFileName = Path.Combine(destinationPath, downloadFilename);

                File.WriteAllBytes(destinationFileName, bytes);

                ForceCustomGearUpdate();
            }
        }

        private string GetDestinationPath()
        {
            var path = SaveManager.Instance.CustomGearDir;

            if (!string.IsNullOrEmpty(downloadPath))
            {
                path = Path.Combine(path, downloadPath);
            }

            return path;
        }

        private void ForceCustomGearUpdate()
        {
            var traverse = Traverse.Create(SaveManager.Instance);
            traverse.Method("OnApplicationFocus", true).GetValue();
        }
    }
}