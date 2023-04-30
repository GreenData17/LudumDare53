using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("References")]
    // references
    public GameObject player;
    public TMPro.TextMeshProUGUI collectedBoxes_text;
    public TMPro.TextMeshProUGUI deliveredBoxes_text;
    [Space]
    public CanvasGroup inGame;
    public CanvasGroup endScreen;
    public TMPro.TextMeshProUGUI endPoints_text;

    [Header("Box-info")]
    public int collectedBoxes = 0;
    public int deliveredBoxes = 0;

    [Header("Post-processing")]
    // Post-processing
    [SerializeField]
    private Volume volume;
    private Vignette vignette;

    [Header("Timer")]
    // Timer
    public TMPro.TextMeshProUGUI TimerText;
    public float timerDuration = 3f * 60f;
    public float timer;


    void Start()
    {
        if(instance != null){
            Destroy(this);
        }

        instance = this;

        ResetTimer();
        SwitchUI(false);
    }

    void Update()
    {
        UpdateTimer();
        UpdateVignette();
        UpdateCollectionText();

        if(Input.GetKeyDown(KeyCode.Escape)) EndGame();
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    public void EndGame(){
        Debug.Log("Game Quitting...");
        Application.Quit();
    }

    public void SwitchUI(bool isEndScreen){
        endScreen.alpha = isEndScreen ? 1 : 0;
        endScreen.blocksRaycasts = isEndScreen;
        endScreen.interactable = isEndScreen;
    }

    public void EndRound(){

        endPoints_text.text = $"{collectedBoxes}";
        SwitchUI(true);
    }

    #region Inventory

    public void AddBoxToInventory(bool isBigBox){
        collectedBoxes++;
    }

    public void RemoveBoxFromInventory(int index){
        collectedBoxes--;
    }

    void UpdateCollectionText(){
        collectedBoxes_text.text = "Packages Collected: " + collectedBoxes;
        deliveredBoxes_text.text = "Packages Delivered: " + deliveredBoxes;
    }

    #endregion

    #region VignetteEffect

    void UpdateVignette(){
        if(player.transform.position.x <= -100 || player.transform.position.y <= -100){
            if(player.transform.position.x <= player.transform.position.y){
                CalculateVignetteIntensity(player.transform.position.x + 100);
            }
            else{
                CalculateVignetteIntensity(player.transform.position.y + 100);
            }
        }

        if(player.transform.position.x >= 100 || player.transform.position.y >= 100){
            if(player.transform.position.x >= player.transform.position.y){
                CalculateVignetteIntensity(player.transform.position.x - 100);
            }
            else{
                CalculateVignetteIntensity(player.transform.position.y - 100);
            }
        }

    }

    void CalculateVignetteIntensity(float difference){
        volume.profile.TryGet(out vignette);
        {
            vignette.intensity.value = difference / 100;
        }

        if(difference / 100 >= .9f){
            ResetVignetteAndPosition();
        }

    }

    void ResetVignetteAndPosition(){
        player.transform.position = new Vector3(0,0,0);

        volume.profile.TryGet(out vignette);
        {
            vignette.intensity.value = 0;
        }
    }

    #endregion

    #region Timer

    void UpdateTimer(){
        if(timer >= 0){
            timer -= Time.deltaTime;
            UpdateTimerText();
        }else{
            playerManager.instance.canMove = false;
            EndRound();
        }
    }

    void UpdateTimerText(){
        int seconds = Mathf.FloorToInt(timer % 60);
        string secondsInString = seconds.ToString().Length > 1 ? seconds.ToString() : "0" + seconds;

        TimerText.text = $"0{Mathf.FloorToInt(timer / 60)}:{secondsInString}";
    }

    public void ResetTimer(){
        timer = timerDuration;
    }

    #endregion
}
