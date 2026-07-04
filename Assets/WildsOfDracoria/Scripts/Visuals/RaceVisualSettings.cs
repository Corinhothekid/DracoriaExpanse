using System;
using System.Collections.Generic;
using UnityEngine;

namespace WildsOfDracoria.Visuals
{
    [Serializable]
    public class RaceVisualSettings
    {
        public string race;
        public float heightScale = 1f;
        public float bodyScale = 1f;
        public float headScale = 1f;
        public string defaultSkinTone = "warm";
        public List<string> allowedSkinToneIds = new List<string>();
        public List<string> allowedHairStyleIds = new List<string>();
        public List<string> allowedFaceIds = new List<string>();
        public Color racePreviewMaterialColor = Color.white;

        public RaceVisualSettings(string race, float heightScale, float bodyScale, float headScale, string defaultSkinTone, Color previewColor, IEnumerable<string> skinTones, IEnumerable<string> hairStyles, IEnumerable<string> faceIds)
        {
            this.race = race;
            this.heightScale = heightScale;
            this.bodyScale = bodyScale;
            this.headScale = headScale;
            this.defaultSkinTone = defaultSkinTone;
            racePreviewMaterialColor = previewColor;
            allowedSkinToneIds.AddRange(skinTones);
            allowedHairStyleIds.AddRange(hairStyles);
            allowedFaceIds.AddRange(faceIds);
        }
    }
}