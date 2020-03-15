using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace QuarantineJam
{
    public static class SoundEffectPlayer
    {
        private static List<SoundEffect> FramePlaylist = new List<SoundEffect>(), SoundPlayedThisFrame;
        private static List<float> FramePlaylistVolume = new List<float>();
        private static List<float> FramePlaylistPitch = new List<float>();
        public static void Play(SoundEffect se, float Volume = 1f, float Pitch = 0f)
        {
            //se.Play(Volume * Save.Instance.SEVolume, Pitch, 0.5f);
            FramePlaylist.Add(se);
            FramePlaylistVolume.Add(Volume);
            FramePlaylistPitch.Add(Pitch);
        }

        public static void Update()
        {
            SoundPlayedThisFrame = new List<SoundEffect>();
            int i = 0;
            foreach (SoundEffect se in FramePlaylist)
            {
                if (!SoundPlayedThisFrame.Contains(se))
                {
                    se.Play(FramePlaylistVolume[i], FramePlaylistPitch[i], 0.5f);
                    SoundPlayedThisFrame.Add(se);
                }
                i++;
            }
            FramePlaylist = new List<SoundEffect>() { };
            FramePlaylistVolume = new List<float>() { };
            FramePlaylistPitch = new List<float>() { };
        }
    }
}