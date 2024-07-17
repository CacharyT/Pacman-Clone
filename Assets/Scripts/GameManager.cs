using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public int ghostMultiplier {get; private set;} = 1;
    public int score{get; private set;}
    public int lives{get; private set;}
    

    //Default Unity method when game begins
    private void Start()
    {
        NewGame();
    }

    //
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    //Default unity method; updates game per frame
    private void Update()
    {
        //Restarts new game if game over beforehand
        if(this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewRound()
    {
        foreach(Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    //When Pacman dies, reset pacman, or when all pellets are eaten
    private void ResetState()
    {
        ResetGhostMultiplier();

        for(int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    //Disables every game object and displays UI
    private void GameOver()
    {
        for(int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    //Updates points for ghost eaten
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f); //only reset pacman and ghosts, not pellets
        }
        else
        {
            GameOver();
        }
    }

    //Checks if pellet is eaten and adds points to score
    //Resets game if all eaten
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        if(!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    //Simmilar to pelleteaten but for power pellets
    public void PowerPelletEaten(Power_Pellet pellet)
    {
        //Frighetened state
        for(int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        //Checks if there any pellets remaining in the field
        foreach(Transform pellet in this.pellets)
        {
            if(pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
