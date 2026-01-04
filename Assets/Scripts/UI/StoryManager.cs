using System;
using System.Text;
using TMPro;
using UnityEngine;

[Serializable]
public struct StorySection
{
    public int CollectibleRequirement;
    public string TextContent;

    public StorySection(int collectibleRequirement, string textContent)
    {
        CollectibleRequirement = collectibleRequirement;
        TextContent = textContent;
    }
}

public class StoryManager : MonoBehaviour
{
    [Header("GUI")]
    [SerializeField] public TextMeshProUGUI StoryText;
    [SerializeField] private Color unlockHintColor = Color.yellow;


    [Header("Story Configuration")]
    [SerializeField] private StorySection[] storySections = new StorySection[]
            {
                new(0, "Vesna Vulovic"),
                new(5, " was a Serbian flight attendant "),
                new(30, "\r\nwho survied the highest fall without a parachute \r\never recorded. "),
                new(50, "\r\nOn 26 January 1972\r\nshe was the sole survivor of JAT Flight 367\r\nwhich broke up mid-air after an explosion over Czechoslovakia;\r\n She fell from about"),
                new(100, "\r\n ~10,160m ~"),
                new(150, "\r\nshe suffered severe injuries but remarkably recovered."),
                new(300, "\r\n later becoming a Guinness World Records holder. "),
                new(500, "\r\nShe died in 2016 at age 66.")
            };

    private int collectedMemoriesCounter = 0;
    private int currentStorySectionIndex = 0;
    private StringBuilder storyTextBuilder = new StringBuilder();

    private void OnEnable()
    {
        SaveScript.OnLoadSaveData += UpdateCollectedMemoriesCounter;
    }

    private void OnDisable()
    {
        SaveScript.OnLoadSaveData -= UpdateCollectedMemoriesCounter;
    }

    private void Awake()
    {
        if (storySections == null || storySections.Length == 0)
        {
            throw new Exception("Story sections are not configured.");
        }

        BuildStoryText();
        UpdateStoryText();
    }

    public void UpdateCollectedMemoriesCounter(int loadedMemoriesCounter)
    {
        collectedMemoriesCounter = loadedMemoriesCounter;
        var newIndex = GetNewStorySectionIndex();
        currentStorySectionIndex = Math.Min(newIndex, storySections.Length);
        BuildStoryText();
        UpdateStoryText();
    }

    private void UpdateStoryText() => StoryText.text = storyTextBuilder.ToString();

    //TODO: Optimize to only rebuild when necessary
    public void BuildStoryText()
    {
        storyTextBuilder.Clear();

        for (int i = 0; i <= currentStorySectionIndex && i < storySections.Length; i++)
        {
            storyTextBuilder.Append(storySections[i].TextContent);
        }

        if (currentStorySectionIndex < storySections.Length - 1)
        {
            storyTextBuilder.Append(ToUnlockNextSectionTextFormat());
        }
    }

    private int GetNewStorySectionIndex()
    {
        for (int i = storySections.Length - 1; i >= 0; i--)
        {
            if (collectedMemoriesCounter >= storySections[i].CollectibleRequirement)
            {
                return i;
            }
        }

        return storySections.Length;
    }

    private int CalcAdditionMemoriesToNextSection() => storySections[Math.Min(currentStorySectionIndex + 1, storySections.Length - 1)].CollectibleRequirement - collectedMemoriesCounter;
    private string ToUnlockNextSectionTextFormat()
    {
        var hex = ColorUtility.ToHtmlStringRGB(unlockHintColor);
        return $"...<color=#{hex}>\nCollect {CalcAdditionMemoriesToNextSection()} More Memories to unlock the next section</color>\n";
    }
}
