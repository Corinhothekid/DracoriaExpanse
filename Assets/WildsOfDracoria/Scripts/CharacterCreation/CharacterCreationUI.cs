using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WildsOfDracoria.CharacterCreation
{
    public class CharacterCreationUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text stepTitleText;
        [SerializeField] private Text primaryText;
        [SerializeField] private Text secondaryText;
        [SerializeField] private Text finalNameText;
        [SerializeField] private InputField characterNameInput;
        [SerializeField] private InputField familyNameInput;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button optionAButton;
        [SerializeField] private Button optionBButton;
        [SerializeField] private Button optionCButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Text optionALabel;
        [SerializeField] private Text optionBLabel;
        [SerializeField] private Text optionCLabel;
        [SerializeField] private Renderer previewRenderer;
        [SerializeField] private Transform previewTransform;

        private readonly CharacterCreationData creationData = new CharacterCreationData();
        private CharacterCreationStartup startup;
        private int stepIndex;
        private int raceIndex;
        private int appearanceIndex;
        private int homelandIndex;
        private bool listenersBound;

        private const int StepRace = 0;
        private const int StepNames = 1;
        private const int StepAppearance = 2;
        private const int StepHomeland = 3;
        private const int StepConfirm = 4;

        private void Awake()
        {
            BindListeners();
            Close();
        }

        public void Configure(GameObject panelObject, Text stepTitle, Text primary, Text secondary, Text finalName, InputField characterName, InputField familyName, Button previous, Button next, Button optionA, Button optionB, Button optionC, Button confirm, Text optionAText, Text optionBText, Text optionCText, Renderer preview, Transform previewRoot)
        {
            panel = panelObject;
            stepTitleText = stepTitle;
            primaryText = primary;
            secondaryText = secondary;
            finalNameText = finalName;
            characterNameInput = characterName;
            familyNameInput = familyName;
            previousButton = previous;
            nextButton = next;
            optionAButton = optionA;
            optionBButton = optionB;
            optionCButton = optionC;
            confirmButton = confirm;
            optionALabel = optionAText;
            optionBLabel = optionBText;
            optionCLabel = optionCText;
            previewRenderer = preview;
            previewTransform = previewRoot;
            listenersBound = false;
            BindListeners();
            Close();
        }

        public void Open(CharacterCreationStartup startupController)
        {
            startup = startupController;
            stepIndex = StepRace;
            ApplyRaceSelection();
            ApplyAppearanceSelection();
            ApplyHomelandSelection();

            if (panel != null)
            {
                panel.SetActive(true);
            }

            Refresh();
        }

        public void Close()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void BindListeners()
        {
            if (listenersBound)
            {
                return;
            }

            if (previousButton != null) previousButton.onClick.AddListener(PreviousStep);
            if (nextButton != null) nextButton.onClick.AddListener(NextStep);
            if (optionAButton != null) optionAButton.onClick.AddListener(PreviousOption);
            if (optionBButton != null) optionBButton.onClick.AddListener(NextOption);
            if (optionCButton != null) optionCButton.onClick.AddListener(UseSuggestedOption);
            if (confirmButton != null) confirmButton.onClick.AddListener(Confirm);

            if (characterNameInput != null)
            {
                characterNameInput.text = creationData.characterName;
                characterNameInput.onValueChanged.AddListener(value =>
                {
                    creationData.characterName = value;
                    RefreshNamePreview();
                });
            }

            if (familyNameInput != null)
            {
                familyNameInput.text = creationData.familyName;
                familyNameInput.onValueChanged.AddListener(value =>
                {
                    creationData.familyName = value;
                    RefreshNamePreview();
                });
            }

            listenersBound = true;
        }

        private void NextStep()
        {
            CaptureNameInputs();
            stepIndex = Mathf.Min(StepConfirm, stepIndex + 1);
            Refresh();
        }

        private void PreviousStep()
        {
            CaptureNameInputs();
            stepIndex = Mathf.Max(StepRace, stepIndex - 1);
            Refresh();
        }

        private void PreviousOption()
        {
            switch (stepIndex)
            {
                case StepRace:
                    raceIndex = WrapIndex(raceIndex - 1, RaceRegistry.All.Count);
                    ApplyRaceSelection();
                    break;
                case StepAppearance:
                    appearanceIndex = WrapIndex(appearanceIndex - 1, CharacterAppearanceOptions.All.Count);
                    ApplyAppearanceSelection();
                    break;
                case StepHomeland:
                    homelandIndex = WrapIndex(homelandIndex - 1, HomelandRegistry.All.Count);
                    ApplyHomelandSelection();
                    break;
            }

            Refresh();
        }

        private void NextOption()
        {
            switch (stepIndex)
            {
                case StepRace:
                    raceIndex = WrapIndex(raceIndex + 1, RaceRegistry.All.Count);
                    ApplyRaceSelection();
                    break;
                case StepAppearance:
                    appearanceIndex = WrapIndex(appearanceIndex + 1, CharacterAppearanceOptions.All.Count);
                    ApplyAppearanceSelection();
                    break;
                case StepHomeland:
                    homelandIndex = WrapIndex(homelandIndex + 1, HomelandRegistry.All.Count);
                    ApplyHomelandSelection();
                    break;
            }

            Refresh();
        }

        private void UseSuggestedOption()
        {
            if (stepIndex == StepHomeland)
            {
                var race = RaceRegistry.Get(creationData.race);
                for (var i = 0; i < HomelandRegistry.All.Count; i++)
                {
                    if (HomelandRegistry.All[i] == race.homelandName)
                    {
                        homelandIndex = i;
                        break;
                    }
                }

                ApplyHomelandSelection();
                Refresh();
            }
        }

        private void Confirm()
        {
            CaptureNameInputs();
            startup?.ConfirmCharacter(creationData);
        }

        private void Refresh()
        {
            if (previousButton != null) previousButton.gameObject.SetActive(stepIndex > StepRace);
            if (nextButton != null) nextButton.gameObject.SetActive(stepIndex < StepConfirm);
            if (confirmButton != null) confirmButton.gameObject.SetActive(stepIndex == StepConfirm);
            if (characterNameInput != null) characterNameInput.gameObject.SetActive(stepIndex == StepNames);
            if (familyNameInput != null) familyNameInput.gameObject.SetActive(stepIndex == StepNames);

            var optionsVisible = stepIndex == StepRace || stepIndex == StepAppearance || stepIndex == StepHomeland;
            SetOptionButtonsVisible(optionsVisible);

            switch (stepIndex)
            {
                case StepRace: RefreshRaceStep(); break;
                case StepNames: RefreshNamesStep(); break;
                case StepAppearance: RefreshAppearanceStep(); break;
                case StepHomeland: RefreshHomelandStep(); break;
                case StepConfirm: RefreshConfirmStep(); break;
            }

            RefreshNamePreview();
        }

        private void RefreshRaceStep()
        {
            var race = RaceRegistry.Get(creationData.race);
            SetStepTitle("Step 1: Choose Race");
            SetPrimary(race.displayName);
            SetSecondary($"{race.shortDescription}\n\nHomeland: {race.homelandName}\nTheme: {race.culturalTheme}\n{race.startingBonusFlavorText}");
            SetOptionLabels("Previous Race", "Next Race", "");
            if (optionCButton != null) optionCButton.gameObject.SetActive(false);
        }

        private void RefreshNamesStep()
        {
            SetStepTitle("Step 2: Name Your House");
            SetPrimary("Choose your first name and family name.");
            SetSecondary("Your family name becomes your House name and will later connect to dynasty, estate, and legacy systems.");
        }

        private void RefreshAppearanceStep()
        {
            var preset = CharacterAppearanceOptions.All[appearanceIndex];
            SetStepTitle("Step 3: Basic Appearance");
            SetPrimary($"{preset.bodyType} Build");
            SetSecondary($"Skin: {preset.skinTone}\nHair: {preset.hairStyle}, {preset.hairColor}\nFacial Hair: {preset.facialHairStyle}\nEyes: {preset.eyeColor}");
            SetOptionLabels("Previous Look", "Next Look", "");
            if (optionCButton != null) optionCButton.gameObject.SetActive(false);
        }

        private void RefreshHomelandStep()
        {
            SetStepTitle("Step 4: Starting Homeland");
            SetPrimary(creationData.startingHomeland);
            SetSecondary("Your homeland is saved for future story, dynasty, reputation, and travel systems. You still begin this prototype in Ironhaven.");
            SetOptionLabels("Previous Home", "Next Home", "Use Race Home");
        }

        private void RefreshConfirmStep()
        {
            var race = RaceRegistry.Get(creationData.race);
            var builder = new StringBuilder();
            builder.AppendLine($"Name: {creationData.FullName}");
            builder.AppendLine($"House: {creationData.HouseName}");
            builder.AppendLine($"Race: {race.displayName}");
            builder.AppendLine($"Homeland: {creationData.startingHomeland}");
            builder.AppendLine($"Body: {creationData.bodyType}");
            builder.AppendLine($"Skin: {creationData.skinTone}");
            builder.AppendLine($"Hair: {creationData.hairStyle}, {creationData.hairColor}");
            builder.AppendLine($"Facial Hair: {creationData.facialHairStyle}");
            builder.AppendLine($"Eyes: {creationData.eyeColor}");

            SetStepTitle("Step 5: Confirm Character");
            SetPrimary("Ready to enter Ironhaven.");
            SetSecondary(builder.ToString());
        }

        private void ApplyRaceSelection()
        {
            var race = RaceRegistry.All[raceIndex];
            creationData.race = race.raceId;
            creationData.startingHomeland = race.homelandName;
            ApplyPreview(race);
        }

        private void ApplyAppearanceSelection()
        {
            var preset = CharacterAppearanceOptions.All[appearanceIndex];
            creationData.bodyType = preset.bodyType;
            creationData.skinTone = preset.skinTone;
            creationData.hairStyle = preset.hairStyle;
            creationData.hairColor = preset.hairColor;
            creationData.facialHairStyle = preset.facialHairStyle;
            creationData.eyeColor = preset.eyeColor;
        }

        private void ApplyHomelandSelection()
        {
            creationData.startingHomeland = HomelandRegistry.All[homelandIndex];
        }

        private void ApplyPreview(RaceDefinition race)
        {
            if (previewRenderer != null) previewRenderer.material.color = race.previewColor;
            if (previewTransform != null) previewTransform.localScale = race.previewScale;
        }

        private void CaptureNameInputs()
        {
            if (characterNameInput != null) creationData.characterName = characterNameInput.text;
            if (familyNameInput != null) creationData.familyName = familyNameInput.text;
        }

        private void RefreshNamePreview()
        {
            if (finalNameText != null)
            {
                finalNameText.text = $"{creationData.FullName}\n{creationData.HouseName}";
            }
        }

        private void SetOptionButtonsVisible(bool visible)
        {
            if (optionAButton != null) optionAButton.gameObject.SetActive(visible);
            if (optionBButton != null) optionBButton.gameObject.SetActive(visible);
            if (optionCButton != null) optionCButton.gameObject.SetActive(visible && stepIndex == StepHomeland);
        }

        private void SetOptionLabels(string a, string b, string c)
        {
            if (optionALabel != null) optionALabel.text = a;
            if (optionBLabel != null) optionBLabel.text = b;
            if (optionCLabel != null) optionCLabel.text = c;
        }

        private void SetStepTitle(string text)
        {
            if (stepTitleText != null) stepTitleText.text = text;
        }

        private void SetPrimary(string text)
        {
            if (primaryText != null) primaryText.text = text;
        }

        private void SetSecondary(string text)
        {
            if (secondaryText != null) secondaryText.text = text;
        }

        private static int WrapIndex(int value, int count)
        {
            if (count <= 0) return 0;
            if (value < 0) return count - 1;
            if (value >= count) return 0;
            return value;
        }
    }
}