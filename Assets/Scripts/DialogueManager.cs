using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public Image animatedCharacter;
    public Sprite professorSprite;
    public Sprite lobeSprite;
    public GameObject fuelUIArrow;
    public GameObject timerUIArrow;
    public GameObject healthUIArrow;
    public GameObject fuelCircleHighlight;
    
    public bool alwaysShowTutorial;

    private string[] dialogues = {
        "Ah, you're awake! L.O.B.E., can you hear me?",
        "...",
        "There's no time to delve into all the details, but an experiment went terribly wrong. I’m talking to you telepathically on this BlueBrain device. Your consciousness was separated from your body and your only safe haven is the protective tank I made for you.",
        "The only problem is… you need to refuel your tank often, and you’ll have to venture out to collect the scattered fuel.",
        "....",
        "I know you have questions but I can’t tell you everything at the moment. Our research aimed to unlock the full potential of the human mind. Unfortunately, we pushed the boundaries too far. Now, the facility is in chaos, and dangerous creatures from our other experiments have overrun the area.",
        "....",
        "What do you need to do, you ask? Well, you need to escape this facility. Your tank allows you limited mobility, but it consumes fuel rapidly. See that gauge on your interface? That's your fuel level. You must collect fuel cells and fill that gauge to move on.",
        "There’s one of the fuel cells. Collect enough, and your tank can get you to the next area.",
        ".....",
        "Now, about those dangerous creatures—you have a unique ability. You can inhabit their bodies to create temporary safety, use their strengths, and navigate obstacles. But you must hurry, each body only provides a small amount of time for your survival. If you run out of time, you will die.",
        "...",
        "Focus your neural link on a creature shortly after they die. This will create a temporal portal that sends you into their body. But be careful when taking on these mutant experiments, you can only take so many hits before you perish.",
        "...",
        "Remember, each creature has its own abilities. Some can jump higher, others can even fly. Use this to your advantage. Press E to use each ability. Those Laser shooters seem to have a secondary ability, so use R for that. No matter what enemy you inhabit, you should be able to jump or fly by holding or pressing SPACE.",
        "...",
        "What’s the goal with all this you ask? We need to find a way to reintegrate your brain with a new body. Along the way, we'll uncover what caused the experiment to fail and how to fix it.",
        "....",
        "Lastly, if you need assistance, I'll communicate with you throughout your journey. Now, let's get moving. Time is of the essence.",
        "...",
        "Stay sharp, L.O.B.E. The path ahead won't be easy, but I have faith in you."
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        if (!alwaysShowTutorial && PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            dialoguePanel.SetActive(false);
            return;
        }
        
        ShowDialogue(); // Start tutorial if not completed
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            DisplayNextLine();
        }
    }

    private void DisplayNextLine()
    {
        currentLine++;
        if (currentLine < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndTutorial();
        }
    }

    private void ShowDialogue()
    {
        string line = dialogues[currentLine];
        CheckForUIHighlight(line);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line));

        SetSpeakerSprite(line);
    }

    private void CheckForUIHighlight(string line)
    {
        fuelUIArrow.SetActive(false);
        timerUIArrow.SetActive(false);
        healthUIArrow.SetActive(false);
        fuelCircleHighlight.SetActive(false);

        if (line.Contains("fuel level"))
        {
            fuelUIArrow.SetActive(true);
        }
        else if (line.Contains("fuel cells"))
        {
            fuelCircleHighlight.SetActive(true);
        }
        else if (line.Contains("time for your survival"))
        {
            timerUIArrow.SetActive(true);
        }
        else if (line.Contains("only take so many hits"))
        {
            healthUIArrow.SetActive(true);
        }
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Typewriter speed
        }
        isTyping = false;
    }

    private void SetSpeakerSprite(string line)
    {
        if (line.StartsWith("Professor Cortex"))
        {
            animatedCharacter.sprite = professorSprite;
        }
        else if (line.StartsWith("L.O.B.E."))
        {
            animatedCharacter.sprite = lobeSprite;
        }
    }

    private void EndTutorial()
    {
        dialoguePanel.SetActive(false);
        PlayerPrefs.SetInt("TutorialCompleted", 1); // Save tutorial completion status
        FindObjectOfType<CDTimer>().isCountdownActive = true;
    }
}
