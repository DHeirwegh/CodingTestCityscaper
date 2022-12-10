using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIHandler
{
    public static IEnumerator GetTile(int x = 0, int y = 0, int id = 0, int zoomLevel = 0)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"https://www.wmts.nrw.de/geobasis/wmts_nw_dop/tiles/nw_dop/EPSG_25832_16/{zoomLevel}/{x}/{y}"))
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
                Tile t = new Tile();
                t.Texture = texture;
                t.ID = id;
                TileManager.AddTile(t);
            }
        }
    }
}