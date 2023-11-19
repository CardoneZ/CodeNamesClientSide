using NAudio.Wave;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Windows.Controls;

public class MusicManager
{
    private WaveOutEvent waveOut;
    private AudioFileReader audioFileReader;
    private bool isMusicEnabled = true;
    private float volume = 0.5f; // Establece un valor predeterminado para el volumen

    #region Singletone
    private static MusicManager musicClient;

    public static MusicManager MusicClient
    {
        get
        {
            if (musicClient == null)
            {
                musicClient = new MusicManager("Media/Music/BackgroundCheck.wav");
            }
            return musicClient;
        }
    }

    #endregion

    public MusicManager(string musicFilePath)
    {
        audioFileReader = new AudioFileReader(musicFilePath);
        waveOut = new WaveOutEvent();
        waveOut.Init(audioFileReader);
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;

        if (isMusicEnabled)
        {
            PlayMusic();
        }
        else
        {
            StopMusic();
        }
    }

    public void PlayMusic()
    {
        if (isMusicEnabled)
        {
            waveOut.Volume = volume;
            waveOut.Play();
        }
    }

    public void StopMusic()
    {
        waveOut.Stop();
    }

    public float Volume
    {
        get { return volume; }
        set
        {
            if (value < 0) volume = 0f;
            else if (value > 1) volume = 1f;
            else volume = value;

            if (isMusicEnabled)
            {
                waveOut.Volume = volume;
            }
        }
    }

    public bool IsMusicEnabled
    {
        get { return isMusicEnabled; }
    }
}