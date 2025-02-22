using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skutta
{
    public class AudioDevice
    {
        private Dictionary<string, Song> _songs;
        private Dictionary<string, SoundEffect> _soundEffects;

        public AudioDevice()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            _songs = new Dictionary<string, Song>();
            _soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadContent(ContentManager content)
        {
            LoadSoundEffect(content, "jump", "Audio/Effects/jump");
            LoadSoundEffect(content, "coin-pickup", "Audio/Effects/coin-pickup");

            LoadSong(content, "alien", "Audio/Music/alien");
            LoadSong(content, "loop-4", "Audio/Music/loop-4");
            LoadSong(content, "pattaya", "Audio/Music/pattaya-by-scandinavianz");
        }

        public void PlayRandomSong()
        {
            if (_songs.Count > 0)
            {
                var random = new Random();
                var randomSongKey = _songs.Keys.ElementAt(random.Next(_songs.Count));
                PlaySong(randomSongKey);
            }
        }

        public void PlaySong(string name)
        {
            if (_songs.ContainsKey(name))
            {
                MediaPlayer.Play(_songs[name]);
            }
        }

        public void PauseSong()
        {
            MediaPlayer.Pause();
        }

        public void ResumeSong()
        {
            MediaPlayer.Resume();
        }

        public void PlaySoundEffect(string name)
        {
            if (_soundEffects.ContainsKey(name))
            {
                _soundEffects[name].Play();
            }
        }

        private void LoadSong(ContentManager content, string name, string path)
        {
            var loadedSong = content.Load<Song>(path);
            _songs.Add(name, loadedSong);
        }

        private void LoadSoundEffect(ContentManager content, string name, string path)
        {
            var loadedSoundEffect = content.Load<SoundEffect>(path);
            _soundEffects.Add(name, loadedSoundEffect);
        }
    }
}
