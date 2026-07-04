using UnityEngine;

namespace WildsOfDracoria.Visuals
{
    public class CharacterVisualManager : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Renderer headRenderer;
        [SerializeField] private CharacterAttachmentRig attachmentRig;

        public CharacterVisualProfile CurrentProfile { get; private set; } = new CharacterVisualProfile();

        private void Awake()
        {
            EnsureVisualRig();
            ApplyProfile(CurrentProfile);
        }

        public void ApplyProfile(CharacterVisualProfile profile)
        {
            EnsureVisualRig();
            CurrentProfile = profile ?? new CharacterVisualProfile();
            CurrentProfile.Normalize();

            var raceSettings = RaceVisualRegistry.Get(CurrentProfile.race);
            var bodyScale = GetBodyTypeScale(CurrentProfile.bodyType);
            visualRoot.localScale = new Vector3(raceSettings.bodyScale * bodyScale.x, raceSettings.heightScale * bodyScale.y, raceSettings.bodyScale * bodyScale.z);

            SetRendererColor(bodyRenderer, CharacterVisualPalette.Skin(CurrentProfile.skinToneId));
            SetRendererColor(headRenderer, CharacterVisualPalette.Skin(CurrentProfile.skinToneId));
            headRenderer.transform.localScale = Vector3.one * raceSettings.headScale;

            RebuildAttachment(CharacterAttachmentPointId.Hair, CurrentProfile.hairStyleId, CharacterVisualPalette.Hair(CurrentProfile.hairColorId));
            RebuildAttachment(CharacterAttachmentPointId.Face, CurrentProfile.faceId, CharacterVisualPalette.Eye(CurrentProfile.eyeColorId));
            RebuildAttachment(CharacterAttachmentPointId.Chest, CurrentProfile.outfitId, new Color(0.18f, 0.28f, 0.42f));
            RebuildAttachment(CharacterAttachmentPointId.Back, CurrentProfile.capeId, new Color(0.46f, 0.06f, 0.08f));
            RebuildAttachment(CharacterAttachmentPointId.MainHand, CurrentProfile.weaponVisualId, new Color(0.75f, 0.75f, 0.72f));
            RebuildAttachment(CharacterAttachmentPointId.OffHand, "shield_round", new Color(0.22f, 0.28f, 0.36f));
        }

        public void RefreshPreview(CharacterVisualProfile profile)
        {
            ApplyProfile(profile);
        }

        private void EnsureVisualRig()
        {
            if (visualRoot == null)
            {
                var existingRenderer = GetComponent<Renderer>();
                if (existingRenderer != null)
                {
                    existingRenderer.enabled = false;
                }

                var root = new GameObject("Character Visual Root");
                root.transform.SetParent(transform, false);
                visualRoot = root.transform;
            }

            if (attachmentRig == null)
            {
                attachmentRig = visualRoot.GetComponent<CharacterAttachmentRig>() ?? visualRoot.gameObject.AddComponent<CharacterAttachmentRig>();
                attachmentRig.EnsurePoints();
            }

            if (bodyRenderer == null)
            {
                var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                body.name = "Placeholder Body";
                body.transform.SetParent(visualRoot, false);
                body.transform.localPosition = new Vector3(0f, 0.78f, 0f);
                body.transform.localScale = new Vector3(0.72f, 0.9f, 0.72f);
                bodyRenderer = body.GetComponent<Renderer>();
                Destroy(body.GetComponent<Collider>());
            }

            if (headRenderer == null)
            {
                var head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                head.name = "Placeholder Anime Head";
                head.transform.SetParent(visualRoot, false);
                head.transform.localPosition = new Vector3(0f, 1.62f, 0f);
                head.transform.localScale = new Vector3(0.58f, 0.52f, 0.58f);
                headRenderer = head.GetComponent<Renderer>();
                Destroy(head.GetComponent<Collider>());
            }
        }

        private void RebuildAttachment(CharacterAttachmentPointId pointId, string visualId, Color color)
        {
            var point = attachmentRig.GetPoint(pointId);
            for (var i = point.childCount - 1; i >= 0; i--)
            {
                Destroy(point.GetChild(i).gameObject);
            }

            if (string.IsNullOrWhiteSpace(visualId) || visualId.EndsWith("_none"))
            {
                return;
            }

            var piece = CreatePlaceholderPiece(visualId, pointId);
            piece.transform.SetParent(point, false);
            SetRendererColor(piece.GetComponent<Renderer>(), color);
            var collider = piece.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }
        }

        private static GameObject CreatePlaceholderPiece(string visualId, CharacterAttachmentPointId pointId)
        {
            var primitive = pointId == CharacterAttachmentPointId.MainHand ? PrimitiveType.Cylinder : PrimitiveType.Cube;
            if (pointId == CharacterAttachmentPointId.Face)
            {
                primitive = PrimitiveType.Sphere;
            }

            var piece = GameObject.CreatePrimitive(primitive);
            piece.name = visualId;

            switch (pointId)
            {
                case CharacterAttachmentPointId.Hair:
                    piece.transform.localPosition = new Vector3(0f, 0.02f, 0f);
                    piece.transform.localScale = visualId == "hair_long" ? new Vector3(0.62f, 0.28f, 0.54f) : new Vector3(0.58f, 0.16f, 0.52f);
                    break;
                case CharacterAttachmentPointId.Face:
                    piece.transform.localPosition = new Vector3(0f, 0f, 0f);
                    piece.transform.localScale = new Vector3(0.08f, 0.04f, 0.04f);
                    break;
                case CharacterAttachmentPointId.Chest:
                    piece.transform.localScale = new Vector3(0.8f, 0.5f, 0.26f);
                    break;
                case CharacterAttachmentPointId.Back:
                    piece.transform.localScale = new Vector3(0.72f, 0.9f, 0.08f);
                    break;
                case CharacterAttachmentPointId.MainHand:
                    piece.transform.localRotation = Quaternion.Euler(0f, 0f, 35f);
                    piece.transform.localScale = visualId.Contains("fishing") ? new Vector3(0.04f, 0.8f, 0.04f) : new Vector3(0.08f, 0.55f, 0.08f);
                    break;
                case CharacterAttachmentPointId.OffHand:
                    piece.transform.localScale = new Vector3(0.32f, 0.42f, 0.08f);
                    break;
                default:
                    piece.transform.localScale = Vector3.one * 0.2f;
                    break;
            }

            return piece;
        }

        private static Vector3 GetBodyTypeScale(string bodyType)
        {
            switch (string.IsNullOrWhiteSpace(bodyType) ? "average" : bodyType.Trim().ToLowerInvariant())
            {
                case "tall": return new Vector3(0.96f, 1.1f, 0.96f);
                case "stout": return new Vector3(1.12f, 0.96f, 1.12f);
                case "lean": return new Vector3(0.88f, 1.04f, 0.88f);
                default: return Vector3.one;
            }
        }

        private static void SetRendererColor(Renderer renderer, Color color)
        {
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}