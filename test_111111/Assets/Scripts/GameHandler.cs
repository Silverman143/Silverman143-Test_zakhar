using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private bool gameIsStarted = false;
    private PlayerController player;
    [SerializeField] private PointHandler finishPoint;
    [SerializeField] private Text greetingText;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        var _points = FindObjectsOfType<PointHandler>();
        foreach(PointHandler _point in _points)
        {
            _point.enemiesEnded.AddListener(Send_player_further);
        }
    }

    private void Update()
    {
        if (!gameIsStarted)
        {
            Game_start_action();
        }
    }

    private void Game_start_action()
    {
        if (Input.touchCount>0)
        {
            gameIsStarted = true;
            greetingText.enabled = false;
            player.SendMessage("Change_status", Status.Walk);
        }
    }

    private void Send_player_further()
    {
        player.Change_status(Status.Walk);
    }

    public void Check_finish(PointHandler _point)
    {
        if (_point == finishPoint)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
