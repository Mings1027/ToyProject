using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    // //EnemySpawn and GameReStart
    // private Tilemap tilemap;
    // private List<Vector3> tilePos = new List<Vector3>();
    // private List<GameObject> enemyList = new List<GameObject>();
    // private int ranSpPos;

    // public void ReStart()
    // {
    //     tilePos.Clear();
    //     foreach (var pos in tilemap.cellBounds.allPositionsWithin)
    //     {
    //         Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
    //         Vector3 place = tilemap.CellToWorld(localPlace);
    //         if (tilemap.HasTile(localPlace))
    //         {
    //             tilePos.Add(place);
    //         }
    //     }
    //     var enemyCount = enemyList.Count;
    //     for (int i = 0; i < enemyCount; i++)
    //     {
    //         if (enemyList[i].activeSelf)
    //         {
    //             enemyList[i].SetActive(false);
    //         }
    //     }
    //     enemyList.Clear();
    //     for (int i = 0; i < 30; i++)
    //     {
    //         ranSpPos = Random.Range(0, tilePos.Count);
    //         enemyList.Add(ObjectPooler.SpawnFromPool("GreenEnemy", tilePos[ranSpPos]));
    //     }
    //     for (int i = 0; i < 30; i++)
    //     {
    //         ranSpPos = Random.Range(0, tilePos.Count);
    //         enemyList.Add(ObjectPooler.SpawnFromPool("PinkEnemy", tilePos[ranSpPos]));
    //     }
    //     for (int i = 0; i < 30; i++)
    //     {
    //         ranSpPos = Random.Range(0, tilePos.Count);
    //         enemyList.Add(ObjectPooler.SpawnFromPool("EyeEnemy", tilePos[ranSpPos]));
    //     }
    //     // for (int i = 0; i < 50; i++)
    //     // {
    //     //     ranSpPos = Random.Range(0, tilePos.Count);
    //     //     lightList.Add(ObjectPooler.SpawnFromPool("SpotLight2D", tilePos[ranSpPos]));
    //     // }
    // }




}