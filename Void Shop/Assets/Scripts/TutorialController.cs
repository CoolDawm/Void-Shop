using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [Header("Tutorial Elements")]
    [SerializeField] private List<GameObject> tutorialElementParents = new List<GameObject>();
    [SerializeField] private List<string> tutorialDescriptions = new List<string>();

    [Header("UI Settings")]
    [SerializeField] private Image dimmingPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField][Range(0, 1)] private float fadedAlpha = 0.1f;
    [SerializeField] private float highlightAlpha = 1f;

    [Header("Exclusions")]
    [SerializeField] private List<Graphic> excludedElements = new List<Graphic>();

    [Header("Text Settings")]
    [SerializeField] private string startPrompt = "Press SPACE to begin tutorial";
    [SerializeField] private string continuePrompt = "Press SPACE to continue";

    private List<Graphic> allUIElements = new List<Graphic>();
    private List<List<Graphic>> tutorialElementGraphics = new List<List<Graphic>>();
    private int currentStep = -1;
    private bool isTutorialRunning = false;

    private void Awake()
    {
        allUIElements.AddRange(FindObjectsOfType<Graphic>(true));

        foreach (var parent in tutorialElementParents)
        {
            if (parent != null)
            {
                var graphics = new List<Graphic>();
                graphics.AddRange(parent.GetComponentsInChildren<Graphic>(true));
                tutorialElementGraphics.Add(graphics);
            }
        }

        AddToExclusions(dimmingPanel);
        AddToExclusions(descriptionText);
        AddToExclusions(promptText);
    }

    private void AddToExclusions(Graphic element)
    {
        if (element != null && !excludedElements.Contains(element))
        {
            excludedElements.Add(element);
        }
    }

    private void Start()
    {
        InitializeTutorial();
    }

    private void Update()
    {
        if (isTutorialRunning && Input.GetKeyDown(KeyCode.Space))
        {
            ProceedToNextStep();
        }
    }

    private void InitializeTutorial()
    {
        if (tutorialElementParents.Count == 0 || tutorialDescriptions.Count == 0)
        {
            Debug.LogWarning("Tutorial elements or descriptions not set up properly");
            return;
        }

        FadeAllUIElements();
        ActivateTutorialUI();

        isTutorialRunning = true;
        currentStep = -1;
    }

    private void FadeAllUIElements()
    {
        foreach (var element in allUIElements)
        {
            if (ShouldFadeElement(element))
            {
                SetElementAlpha(element, fadedAlpha);
                element.raycastTarget = false;
            }
        }
    }

    private bool ShouldFadeElement(Graphic element)
    {
        return element != null &&
               !excludedElements.Contains(element) &&
               element.gameObject.activeInHierarchy;
    }

    private void ActivateTutorialUI()
    {
        dimmingPanel.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
        promptText.gameObject.SetActive(true);

        promptText.text = startPrompt;
        descriptionText.text = "";

        SetElementAlpha(dimmingPanel, 1f);
        SetElementAlpha(promptText, 1f);
        SetElementAlpha(descriptionText, 1f);
    }

    private void ProceedToNextStep()
    {
        if (currentStep >= 0 && currentStep < tutorialElementGraphics.Count)
        {
            SetGraphicsAlpha(tutorialElementGraphics[currentStep], fadedAlpha);
        }

        currentStep++;

        if (currentStep >= tutorialElementGraphics.Count)
        {
            CompleteTutorial();
            return;
        }

        if (currentStep < tutorialElementGraphics.Count)
        {
            SetGraphicsAlpha(tutorialElementGraphics[currentStep], highlightAlpha);
        }

        UpdateTutorialText();
    }

    private void UpdateTutorialText()
    {
        descriptionText.text = currentStep < tutorialDescriptions.Count ?
            tutorialDescriptions[currentStep] : "No description available";

        promptText.text = continuePrompt;
    }

    private void CompleteTutorial()
    {
        RestoreAllUIElements();
        HideTutorialUI();
        isTutorialRunning = false;
        GameManager.Instance?.CompleteTutorial();
    }

    private void RestoreAllUIElements()
    {
        foreach (var element in allUIElements)
        {
            if (element != null && !excludedElements.Contains(element))
            {
                SetElementAlpha(element, 1f);
                element.raycastTarget = true;
            }
        }
    }

    private void HideTutorialUI()
    {
        SetElementAlpha(dimmingPanel, 0f);
        SetElementAlpha(descriptionText, 0f);
        SetElementAlpha(promptText, 0f);

        dimmingPanel.raycastTarget = false;
        descriptionText.raycastTarget = false;
        promptText.raycastTarget = false;
    }

    private void SetGraphicsAlpha(List<Graphic> graphics, float alpha)
    {
        if (graphics == null) return;

        foreach (var graphic in graphics)
        {
            SetElementAlpha(graphic, alpha);
        }
    }

    private void SetElementAlpha(Graphic element, float alpha)
    {
        if (element != null)
        {
            Color color = element.color;
            color.a = alpha;
            element.color = color;
        }
    }

    public bool IsTutorialActive()
    {
        return isTutorialRunning;
    }
}