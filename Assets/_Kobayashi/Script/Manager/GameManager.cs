using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    Player _player;
    TPSCamera _camera;
    void Start()
    {
        Application.targetFrameRate = 60;
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        _player = FindAnyObjectByType<Player>();
        _camera = FindAnyObjectByType<TPSCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        _player.PlayerInput();
    }
    private void LateUpdate()
    {
        _camera.CameraInput();
        _camera.CameraFollow();
    }
    private void FixedUpdate()
    {
        _player.PlayerMove();
        _player.GroundChecker();
    }
}
