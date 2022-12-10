using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TileManager : MonoBehaviour
{
    private static List<Tile> _tiles = new List<Tile>();

    private int _zoomLevel = 3;

    public void Start()
    {
        StartCoroutine(DisplayTiles());
    }

    private void SpawnTiles()
    {
        _tiles.Sort((x, y) => y.ID.CompareTo(x.ID));

        int nrTiles = (int)Mathf.Pow(2, _zoomLevel);

        for (int y = 0; y < nrTiles; y++)
        {
            for (int x = 0; x < nrTiles; x++)
            {
                var currTile = _tiles[x * nrTiles + y];
                var tileWidth = currTile.Texture.width;
                Vector2 pos = new Vector2(nrTiles - x * tileWidth / 100f, y * tileWidth / 100f);
                SpawnTile(currTile, pos);
            }
        }
    }

    private void SpawnTile(Tile t, Vector2 pos)
    {
        var tileObj = new GameObject($"Tile: {t.ID}");

        SpriteRenderer tileRenderer = tileObj.AddComponent<SpriteRenderer>();
        tileRenderer.sprite = Sprite.Create(t.Texture, new Rect(0, 0, t.Texture.width, t.Texture.height), new Vector2(.5f, .5f));

        tileObj.transform.position = pos;
        //tileObj.transform.localScale *= t.Texture.width;
    }

    private IEnumerator DisplayTiles()
    {
        int id = 0;
        _tiles.Clear();
        int nrTiles = (int)Mathf.Pow(2, _zoomLevel);
        for (int x = 0; x < nrTiles; x++)
        {
            for (int y = 0; y < nrTiles; y++)
            {
                ++id;
                StartCoroutine(APIHandler.GetTile(x, y, id, _zoomLevel));
            }
        }
        while (_tiles.Count < nrTiles * nrTiles)
            yield return null;
        SpawnTiles();
    }

    public static void AddTile(Tile tile)
    {
        _tiles.Add(tile);
    }
}