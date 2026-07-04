using System.Collections.Generic;
using UnityEngine;

namespace WildsOfDracoria.Visuals
{
    public class CharacterAttachmentRig : MonoBehaviour
    {
        private readonly Dictionary<CharacterAttachmentPointId, Transform> points = new Dictionary<CharacterAttachmentPointId, Transform>();

        public Transform GetPoint(CharacterAttachmentPointId pointId)
        {
            EnsurePoints();
            return points[pointId];
        }

        public void EnsurePoints()
        {
            if (points.Count > 0)
            {
                return;
            }

            CreatePoint(CharacterAttachmentPointId.Head, new Vector3(0f, 1.55f, 0f));
            CreatePoint(CharacterAttachmentPointId.Hair, new Vector3(0f, 1.78f, 0f));
            CreatePoint(CharacterAttachmentPointId.Face, new Vector3(0f, 1.58f, 0.36f));
            CreatePoint(CharacterAttachmentPointId.Chest, new Vector3(0f, 1.02f, 0f));
            CreatePoint(CharacterAttachmentPointId.Back, new Vector3(0f, 1.12f, -0.34f));
            CreatePoint(CharacterAttachmentPointId.Shoulders, new Vector3(0f, 1.28f, 0f));
            CreatePoint(CharacterAttachmentPointId.Hands, new Vector3(0f, 0.92f, 0f));
            CreatePoint(CharacterAttachmentPointId.Belt, new Vector3(0f, 0.72f, 0f));
            CreatePoint(CharacterAttachmentPointId.Legs, new Vector3(0f, 0.42f, 0f));
            CreatePoint(CharacterAttachmentPointId.Feet, new Vector3(0f, 0.08f, 0f));
            CreatePoint(CharacterAttachmentPointId.MainHand, new Vector3(0.55f, 0.9f, 0.1f));
            CreatePoint(CharacterAttachmentPointId.OffHand, new Vector3(-0.55f, 0.9f, 0.1f));
        }

        private void CreatePoint(CharacterAttachmentPointId pointId, Vector3 localPosition)
        {
            var point = new GameObject(pointId.ToString());
            point.transform.SetParent(transform, false);
            point.transform.localPosition = localPosition;
            points[pointId] = point.transform;
        }
    }
}