using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_manager : MonoBehaviour
{
    public GameObject opciones;
    public GameObject niveles;
    public GameObject menuPausa;
    [SerializeField] AudioSource audioSource;
    [SerializeField] public AudioSource audioSonidos;
    [SerializeField] bool musica = true;
    private void Start()
    {
        if (opciones != null)
        {
            opciones.SetActive(false);
        }
        if (niveles != null)
        {
            niveles.SetActive(false);
        }
        if (menuPausa != null)
        {
            menuPausa.SetActive(false);
        }
      
    }

    private void Update()
    {
        audioSource.mute = !musica;
    }
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        opciones.SetActive(true);
    }

    public void Volver()
    {
        opciones.SetActive(false);
        niveles.SetActive(false);
    }

    public void MuteMusic()
    {
        musica = false;
    }
   public void MusicON()
    {
        musica = true;
    }
    public void Niveles()
    {
        niveles.SetActive(true);
    }

    public void SeleccionNivel()
    {
        SceneManager.LoadScene(2);
    }

    public void Pausa()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0;
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1;
    }

    public void VolverMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SonidoOFF()
    {
        audioSonidos.enabled  = false;
    }

    public void SonidoON()
    {
        audioSonidos.enabled = true;
    }

    public IEnumerator ReiniciarNivel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }

}
