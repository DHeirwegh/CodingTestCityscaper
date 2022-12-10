using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIHandler : MonoBehaviour
{
    private static APIHandler instance = null;
    public List<Texture2D> Textures { get; private set; } = new List<Texture2D>();

    public int GetNrTextures()
    {
        return Textures.Count;
    }

    public static APIHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new APIHandler();
            }
            return instance;
        }
    }

    public Texture2D GetTileTexture(int zoomlevel, int x, int y)
    {
        return null;
        string requestUrl = $"https://www.wmts.nrw.de/geobasis/wmts_nw_dop/tiles/nw_dop/EPSG_25832_16/{zoomlevel}/{x}/{y}";

        // Create a new web request to the API endpoint
        UnityWebRequest request = UnityWebRequest.Get(requestUrl);

        // Send the request and wait for the response
        request.SendWebRequest();
        while (!request.isDone)
        {
            // Wait for the response
        }

        // Check for errors
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
            return null;
        }

        // Get the response data as a texture
        DownloadHandlerTexture textureData = (DownloadHandlerTexture)request.downloadHandler;

        Texture2D texture = textureData.texture;
        return texture;
        // Use the texture as needed
        // ...
    }

    private void Start()
    {
        StartCoroutine(GetText());
    }

    private IEnumerator GetText()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("https://www.wmts.nrw.de/geobasis/wmts_nw_dop/tiles/nw_dop/EPSG_25832_16/0/0/0"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
            }
        }
    }
}