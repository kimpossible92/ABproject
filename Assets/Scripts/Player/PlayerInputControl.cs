using System.Collections;
using System.Collections.Generic;
using SnakeMaze.Audio;
using SnakeMaze.SO;
using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    [SerializeField] private PlayerVariableSO playerVariable;
    [SerializeField] private BusGameManagerSO gameManager;
    [SerializeField] private AudioRequest boostIn;
    [SerializeField] private AudioRequest boostOut;
    private bool _isOnBoost; 
    private void Awake()
    {
        _isOnBoost = false;
    }
    public void GetHorizontalValue()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        //float value = 0;
        //value = ctx.ReadValue<float>();
        playerVariable.Horizontal = Input.GetAxis("Horizontal");
#endif
    }

    public void GetVerticalValue()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        //float value = 0;
        //value = ctx.ReadValue<float>();
        playerVariable.Vertical = Input.GetAxis("Vertical");
#endif
    }
    public void GetDelta()
    {
#if UNITY_ANDROID
        if (!_isOnBoost)
        {
            playerVariable.Horizontal = Input.GetAxis("Horizontal");
            playerVariable.Vertical = Input.GetAxis("Vertical");
        }
#endif
    }
    public void Boost()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (gameManager.GameStarted)
            {
                playerVariable.CurrentSpeed = playerVariable.BoostSpeed;
                boostIn.PlayAudio();
                _isOnBoost = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameManager.GameStarted && playerVariable.IsAlive)
                gameManager.StartGame?.Invoke();
        }
    }
    public void ResetVel()
    {
        if (!gameManager.GameStarted) return;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!_isOnBoost) return;
            _isOnBoost = false;
            playerVariable.CurrentSpeed = playerVariable.NormalSpeed;
            boostOut.PlayAudio();
        }
    }
    public void PauseGame()
    {
        if (!gameManager.GameStarted) return;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameManager.PauseGame?.Invoke(!gameManager.GamePaused);
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Boost();ResetVel();PauseGame();
        GetHorizontalValue(); GetVerticalValue();
        GetDelta();
    }
}
