#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WildsOfDracoria.AI;
using WildsOfDracoria.CameraRig;
using WildsOfDracoria.Combat;
using WildsOfDracoria.Crafting;
using WildsOfDracoria.Interaction;
using WildsOfDracoria.Player;
using WildsOfDracoria.Systems;
using WildsOfDracoria.UI;

namespace WildsOfDracoria.EditorTools
{
    public static class IronhavenPrototypeBuilder
    {
        [MenuItem("Wilds of Dracoria/Build Ironhaven Prototype Scene")]
        public static void BuildScene()
        {
            ClearScene();

            var materials = CreateMaterials();
            CreateGameManager();
            var player = CreatePlayer(materials.player);
            CreateCamera(player.transform);
            CreateWorld(materials);
            CreateInteractables(materials);
            CreateEnemies(materials);
            CreateUI(player);

            RenderSettings.ambientLight = new Color(0.65f, 0.7f, 0.8f);
            var lightObject = new GameObject("Sun");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            lightObject.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

            Selection.activeObject = player;
        }

        private static void ClearScene()
        {
            var objects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include);
            foreach (var sceneObject in objects)
            {
                Object.DestroyImmediate(sceneObject);
            }
        }

        private static SceneMaterials CreateMaterials()
        {
            return new SceneMaterials
            {
                grass = MakeMaterial("Prototype_Grass", new Color(0.32f, 0.58f, 0.24f)),
                path = MakeMaterial("Prototype_Path", new Color(0.48f, 0.39f, 0.28f)),
                water = MakeMaterial("Prototype_Water", new Color(0.1f, 0.45f, 0.72f, 0.72f)),
                wood = MakeMaterial("Prototype_Wood", new Color(0.42f, 0.25f, 0.12f)),
                stone = MakeMaterial("Prototype_Stone", new Color(0.45f, 0.45f, 0.42f)),
                roof = MakeMaterial("Prototype_Roof", new Color(0.48f, 0.12f, 0.08f)),
                forge = MakeMaterial("Prototype_Forge", new Color(0.85f, 0.28f, 0.08f)),
                player = MakeMaterial("Prototype_Player", new Color(0.1f, 0.35f, 0.85f)),
                npc = MakeMaterial("Prototype_NPC", new Color(0.85f, 0.75f, 0.22f)),
                tree = MakeMaterial("Prototype_Tree", new Color(0.12f, 0.38f, 0.16f)),
                wolf = MakeMaterial("Prototype_Wolf", new Color(0.28f, 0.28f, 0.25f))
            };
        }

        private static Material MakeMaterial(string name, Color color)
        {
            var material = new Material(Shader.Find("Standard"));
            material.name = name;
            material.color = color;
            return material;
        }

        private static void CreateGameManager()
        {
            new GameObject("GameManager").AddComponent<GameManager>();
        }

        private static GameObject CreatePlayer(Material material)
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0f, 1f, -4f);
            player.GetComponent<Renderer>().sharedMaterial = material;
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            player.AddComponent<CharacterController>();
            var trigger = player.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 2.25f;
            player.AddComponent<PlayerInteractor>();
            player.AddComponent<PlayerVitals>();
            player.AddComponent<PlayerCombat>();
            player.AddComponent<ThirdPersonPlayerController>();
            return player;
        }

        private static void CreateCamera(Transform player)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 5f, -10f);
            cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            var follow = cameraObject.AddComponent<ThirdPersonCameraFollow>();
            follow.SetTarget(player);
        }

        private static void CreateWorld(SceneMaterials materials)
        {
            CreateBox("Ironhaven Ground", Vector3.zero, new Vector3(38f, 0.25f, 30f), materials.grass);
            CreateBox("Village Path", new Vector3(0f, 0.15f, -1f), new Vector3(4f, 0.08f, 22f), materials.path);
            CreateBox("Harbor Water", new Vector3(0f, -0.05f, 14f), new Vector3(38f, 0.12f, 8f), materials.water);
            CreateBox("Main Dock", new Vector3(0f, 0.35f, 9.5f), new Vector3(4f, 0.25f, 9f), materials.wood);
            CreateBox("Left Dock Arm", new Vector3(-4f, 0.35f, 12.5f), new Vector3(6f, 0.25f, 2f), materials.wood);
            CreateBox("Right Dock Arm", new Vector3(4f, 0.35f, 12.5f), new Vector3(6f, 0.25f, 2f), materials.wood);

            CreateHouse("Blacksmith", new Vector3(-9f, 0.25f, 0f), materials);
            CreateHouse("Harbor House", new Vector3(8f, 0.25f, 1f), materials);
            CreateHouse("Village Storehouse", new Vector3(0f, 0.25f, 5f), materials);

            for (var i = 0; i < 12; i++)
            {
                var x = Random.Range(-17f, 17f);
                var z = Random.Range(-13f, 5f);
                if (Mathf.Abs(x) < 4f)
                {
                    x += 7f;
                }

                CreateTree(new Vector3(x, 0.25f, z), materials);
            }
        }

        private static void CreateInteractables(SceneMaterials materials)
        {
            var fishingSpot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            fishingSpot.name = "FishingSpot";
            fishingSpot.transform.position = new Vector3(-2.5f, 0.45f, 13f);
            fishingSpot.transform.localScale = new Vector3(1.2f, 0.15f, 1.2f);
            fishingSpot.GetComponent<Renderer>().sharedMaterial = materials.water;
            fishingSpot.GetComponent<Collider>().isTrigger = true;
            fishingSpot.AddComponent<FishingSpot>();

            var forge = CreateBox("Forge", new Vector3(-10.8f, 0.75f, 2.8f), new Vector3(1.5f, 1f, 1.2f), materials.forge);
            forge.GetComponent<Collider>().isTrigger = true;
            forge.AddComponent<Forge>();

            var campfire = CreateBox("Campfire", new Vector3(-4.2f, 0.45f, 2.6f), new Vector3(1.2f, 0.35f, 1.2f), materials.forge);
            campfire.GetComponent<Collider>().isTrigger = true;
            var campfireStation = campfire.AddComponent<CraftingStation>();
            campfireStation.Configure(CraftingStationType.Campfire);

            var board = CreateBox("NoticeBoard", new Vector3(3.2f, 1f, 3.5f), new Vector3(1.8f, 1.6f, 0.2f), materials.wood);
            board.GetComponent<Collider>().isTrigger = true;
            board.AddComponent<NoticeBoard>();

            CreateNPC("Captain Alden", new Vector3(2.5f, 1f, 9f), materials.npc);
            CreateNPC("Dock Worker", new Vector3(-3f, 1f, 7f), materials.npc);
            CreateNPC("Village Smith", new Vector3(-7.2f, 1f, 2.5f), materials.npc);
        }

        private static void CreateEnemies(SceneMaterials materials)
        {
            CreateWolf("Forest Wolf", new Vector3(12f, 0.8f, -8f), materials.wolf);
            CreateWolf("Forest Wolf", new Vector3(15f, 0.8f, -3f), materials.wolf);
        }

        private static void CreateWolf(string name, Vector3 position, Material material)
        {
            var wolf = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            wolf.name = name;
            wolf.transform.position = position;
            wolf.transform.localScale = new Vector3(1.2f, 0.8f, 1.8f);
            wolf.GetComponent<Renderer>().sharedMaterial = material;
            wolf.AddComponent<EnemyHealth>();
            wolf.AddComponent<EnemyAI>();
        }

        private static void CreateNPC(string name, Vector3 position, Material material)
        {
            var npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            npc.name = name;
            npc.transform.position = position;
            npc.GetComponent<Renderer>().sharedMaterial = material;
            npc.GetComponent<Collider>().isTrigger = true;
            npc.AddComponent<NPCDialogue>();
        }

        private static void CreateUI(GameObject player)
        {
            var canvasObject = new GameObject("Prototype UI");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();

            var combatUI = canvasObject.AddComponent<CombatUI>();
            var healthSlider = CreateSlider(canvasObject.transform, "Health Bar", new Vector2(0f, 1f), new Vector2(18f, -22f), new Vector2(220f, 18f), new Color(0.72f, 0.08f, 0.08f));
            var staminaSlider = CreateSlider(canvasObject.transform, "Stamina Bar", new Vector2(0f, 1f), new Vector2(18f, -46f), new Vector2(220f, 14f), new Color(0.14f, 0.65f, 0.2f));
            var enemyPanel = CreatePanel(canvasObject.transform, "Enemy Target Panel", new Vector2(0.5f, 1f), new Vector2(-150f, -24f), new Vector2(300f, 54f));
            var enemyNameText = CreateText(enemyPanel.transform, "Enemy Name", "Forest Wolf", 16, TextAnchor.UpperCenter);
            var enemySlider = CreateSlider(enemyPanel.transform, "Enemy Health Bar", new Vector2(0.5f, 0f), new Vector2(-120f, 8f), new Vector2(240f, 14f), new Color(0.8f, 0.12f, 0.1f));
            SetSerializedField(combatUI, "healthSlider", healthSlider);
            SetSerializedField(combatUI, "staminaSlider", staminaSlider);
            SetSerializedField(combatUI, "enemyPanel", enemyPanel);
            SetSerializedField(combatUI, "enemyNameText", enemyNameText);
            SetSerializedField(combatUI, "enemyHealthSlider", enemySlider);
            enemyPanel.SetActive(false);

            var prompt = CreatePanel(canvasObject.transform, "Interaction Prompt", new Vector2(0.5f, 0f), new Vector2(0f, 72f), new Vector2(260f, 44f));
            var promptText = CreateText(prompt.transform, "Prompt Text", "E - Interact", 18, TextAnchor.MiddleCenter);
            var promptUI = prompt.AddComponent<InteractionPromptUI>();
            SetSerializedField(promptUI, "panel", prompt);
            SetSerializedField(promptUI, "promptText", promptText);
            prompt.SetActive(false);
            player.GetComponent<PlayerInteractor>().RegisterPromptUI(promptUI);

            var dialogue = CreatePanel(canvasObject.transform, "Dialogue Panel", new Vector2(0.5f, 0f), new Vector2(0f, 18f), new Vector2(760f, 56f));
            var dialogueText = CreateText(dialogue.transform, "Dialogue Text", "", 18, TextAnchor.MiddleCenter);
            var dialogueUI = dialogue.AddComponent<DialogueUI>();
            SetSerializedField(dialogueUI, "panel", dialogue);
            SetSerializedField(dialogueUI, "dialogueText", dialogueText);

            var inventory = CreatePanel(canvasObject.transform, "Inventory Panel", new Vector2(1f, 1f), new Vector2(-160f, -130f), new Vector2(300f, 240f));
            var inventoryText = CreateText(inventory.transform, "Inventory Text", "Inventory", 16, TextAnchor.UpperLeft);
            var inventoryUI = inventory.AddComponent<InventoryUI>();
            SetSerializedField(inventoryUI, "panel", inventory);
            SetSerializedField(inventoryUI, "inventoryText", inventoryText);

            CreateCraftingUI(canvasObject.transform);
        }

        private static void CreateCraftingUI(Transform parent)
        {
            var panel = CreatePanel(parent, "Crafting Panel", new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(560f, 380f));
            var title = CreateTextInRect(panel.transform, "Crafting Title", "Crafting", 20, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(16f, -50f), new Vector2(-16f, -10f));
            var recipeList = CreateTextInRect(panel.transform, "Recipe List", "Recipes", 15, TextAnchor.UpperLeft, new Vector2(0f, 0f), new Vector2(0.38f, 1f), new Vector2(14f, 70f), new Vector2(-8f, -58f));
            var details = CreateTextInRect(panel.transform, "Recipe Details", "Details", 14, TextAnchor.UpperLeft, new Vector2(0.38f, 0.18f), new Vector2(1f, 1f), new Vector2(10f, 8f), new Vector2(-14f, -58f));
            var warning = CreateTextInRect(panel.transform, "Crafting Warning", "", 14, TextAnchor.MiddleLeft, new Vector2(0.38f, 0f), new Vector2(1f, 0.18f), new Vector2(10f, 48f), new Vector2(-14f, -8f));
            warning.color = new Color(1f, 0.82f, 0.35f);
            var nextButton = CreateUIButton(panel.transform, "Next Recipe Button", "Next", new Vector2(0f, 0f), new Vector2(16f, 16f), new Vector2(100f, 34f));
            var craftButton = CreateUIButton(panel.transform, "Craft Button", "Craft", new Vector2(1f, 0f), new Vector2(-116f, 16f), new Vector2(100f, 34f));

            var craftingUI = panel.AddComponent<CraftingUI>();
            SetSerializedField(craftingUI, "panel", panel);
            SetSerializedField(craftingUI, "titleText", title);
            SetSerializedField(craftingUI, "recipeListText", recipeList);
            SetSerializedField(craftingUI, "detailsText", details);
            SetSerializedField(craftingUI, "warningText", warning);
            SetSerializedField(craftingUI, "nextRecipeButton", nextButton);
            SetSerializedField(craftingUI, "craftButton", craftButton);
            panel.SetActive(false);
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchor, Vector2 anchoredPosition, Vector2 size)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var image = panel.AddComponent<Image>();
            image.color = new Color(0.05f, 0.06f, 0.07f, 0.82f);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return panel;
        }

        private static Text CreateText(Transform parent, string name, string text, int size, TextAnchor alignment)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var uiText = textObject.AddComponent<Text>();
            uiText.text = text;
            uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            uiText.fontSize = size;
            uiText.alignment = alignment;
            uiText.color = Color.white;
            var rect = uiText.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(12f, 8f);
            rect.offsetMax = new Vector2(-12f, -8f);
            return uiText;
        }

        private static Text CreateTextInRect(Transform parent, string name, string text, int size, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var uiText = CreateText(parent, name, text, size, alignment);
            var rect = uiText.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            return uiText;
        }

        private static Button CreateUIButton(Transform parent, string name, string label, Vector2 anchor, Vector2 anchoredPosition, Vector2 size)
        {
            var buttonObject = CreatePanel(parent, name, anchor, anchoredPosition, size);
            buttonObject.GetComponent<Image>().color = new Color(0.12f, 0.15f, 0.18f, 0.95f);
            CreateText(buttonObject.transform, "Label", label, 14, TextAnchor.MiddleCenter);
            return buttonObject.AddComponent<Button>();
        }

        private static Slider CreateSlider(Transform parent, string name, Vector2 anchor, Vector2 anchoredPosition, Vector2 size, Color fillColor)
        {
            var sliderObject = new GameObject(name);
            sliderObject.transform.SetParent(parent, false);
            var rect = sliderObject.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var background = new GameObject("Background");
            background.transform.SetParent(sliderObject.transform, false);
            var backgroundImage = background.AddComponent<Image>();
            backgroundImage.color = new Color(0.02f, 0.02f, 0.02f, 0.85f);
            Stretch(background.GetComponent<RectTransform>());

            var fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObject.transform, false);
            Stretch(fillArea.AddComponent<RectTransform>());

            var fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            var fillImage = fill.AddComponent<Image>();
            fillImage.color = fillColor;
            Stretch(fill.GetComponent<RectTransform>());

            var slider = sliderObject.AddComponent<Slider>();
            slider.transition = Selectable.Transition.None;
            slider.targetGraphic = fillImage;
            slider.fillRect = fill.GetComponent<RectTransform>();
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.value = 100f;
            return slider;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static void CreateHouse(string name, Vector3 position, SceneMaterials materials)
        {
            CreateBox($"{name} Body", position + Vector3.up * 0.9f, new Vector3(4f, 1.8f, 3.5f), materials.stone);
            CreateBox($"{name} Roof", position + Vector3.up * 2f, new Vector3(4.6f, 0.7f, 4f), materials.roof);
        }

        private static void CreateTree(Vector3 position, SceneMaterials materials)
        {
            CreateBox("Tree Trunk", position + Vector3.up * 0.75f, new Vector3(0.35f, 1.5f, 0.35f), materials.wood);
            var crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            crown.name = "Tree Crown";
            crown.transform.position = position + Vector3.up * 2f;
            crown.transform.localScale = new Vector3(1.6f, 1.4f, 1.6f);
            crown.GetComponent<Renderer>().sharedMaterial = materials.tree;
        }

        private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Material material)
        {
            var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.name = name;
            box.transform.position = position;
            box.transform.localScale = scale;
            box.GetComponent<Renderer>().sharedMaterial = material;
            return box;
        }

        private static void SetSerializedField(Object target, string fieldName, Object value)
        {
            var serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(fieldName).objectReferenceValue = value;
            serializedObject.ApplyModifiedProperties();
        }

        private struct SceneMaterials
        {
            public Material grass;
            public Material path;
            public Material water;
            public Material wood;
            public Material stone;
            public Material roof;
            public Material forge;
            public Material player;
            public Material npc;
            public Material tree;
            public Material wolf;
        }
    }
}
#endif
