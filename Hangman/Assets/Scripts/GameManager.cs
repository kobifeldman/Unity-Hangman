﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/*
 * Description: Manages the flow of the game
 * Methods: void Awake(), void Start(), void OnClicked(), void DisableLetterButton(char letter),
 */
public class GameManager : MonoBehaviour {

    private ReadText readText;
    private SetupGame setupGame;
    private Word word;
    private Character character;
    private PrefabManager prefabManager;

    //An array of all the letter spawn locations
    [SerializeField]
    private GameObject[] letterSpawnLocations;

    //An array of all the blank spots a letter can go
    [SerializeField]
    private GameObject[] letterSpaces;

    //The Text that appears when you win or lose
    [SerializeField]
    private TextMeshProUGUI wonText;
    [SerializeField]
    private TextMeshProUGUI lostText;

    /*
     * Description: Used for initialization and getting script references
     */
    private void Awake() {
        readText = GameObject.FindObjectOfType<ReadText>();
        setupGame = GameObject.FindObjectOfType<SetupGame>();
        word = GameObject.FindObjectOfType<Word>();
        character = GameObject.FindObjectOfType<Character>();
        prefabManager = GameObject.FindObjectOfType<PrefabManager>();
    }

    /*
     * Description: Used after Awake. Use and pass variable references through Start instead of Awake
     */
    private void Start() {
        //Name of the currently open scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "GetWord") {
            if (Word.RandomizeWord) {
                readText.ReadTextFile();
            } else {
                readText.EnableInputField();
            }
        } else if (currentSceneName == "Main") {
            setupGame.EnableLetterSpaces(letterSpaces);
            setupGame.SpawnLetters(letterSpawnLocations);
        }
    }

    /*
     * Description: Called when a letter button is clicked. Passes the button's tag as a char
     */
    public void OnClicked(Button button) {
        if (word.IsLetterInWord(button.tag[0])) {
            if (word.IsGameWon()) {
                wonText.text = "Game over. You won!!";
            }
        } else if (!word.IsLetterInWord(button.tag[0])) {
            character.EnableNextPiece();
            if (character.IsGameLost()) {
                lostText.text = "Game over. You Lost!!";
            }
        }

        DisableLetterButton(button);
    }

    /*
     * Description: Disables the button that was just used and places a red X on it
     * Parameters: letter = the letter just guessed
     */
    private void DisableLetterButton(Button button) {
        button.interactable = false;
        GameObject redXPrefabClone = Instantiate(prefabManager.GetPrefab("redX"));
        redXPrefabClone.transform.position = button.transform.position;
    }

}
