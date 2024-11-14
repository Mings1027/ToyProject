using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [SerializeField] AbstractDungeonGenerator generator;
    public float speed;
    public bool enter;

    [SerializeField] GameController gameController;

    private void Update()
    {
        if (enter)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                generator.GenerateDungeon();
                gameController.InGame();
                gameController.ReStart();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            enter = false;
        }
    }
}
