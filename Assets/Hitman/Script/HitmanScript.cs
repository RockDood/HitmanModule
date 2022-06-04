using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitmanScript : KtaneModule
{
    private static readonly string[] OneLine = new string[] {
        "Called queue early in TP",
        "Mistyped Swan Code",
        "Wrongly noted edgework",
        "Gave guesses",
        "Missed Turn the Key",
        "Uses Arbitrary Difficulty",
        "Misread edgework",
        "Disrespected Logbot",
        "Missed Refill the Beer",
        "Cursed in the KTANE server",
        "Identity of Jason Bourne",
        "Impatient with new players",
        "Incessently pings roles",
        "Posted NSFW in #general",
        "Posted SFW in #NSFW",
        "Posted Memes in #general",
        "KTANE Server Moderator",
        "KTANE Server Admin",
        "Defends Impossible Mods",
        "Incorrect translation"
    };

    private static readonly string[,] TwoLine = new string[,] {
        { "Blamed Expert for", "Defuser error" },
        { "Hit Self Destruct", "in Lightspeed" },
        { "Misclicked on", "the last module" },
        { "Didn't announce", "a Boss Module" },
        { "Cleared notes while", "Souvenir was present" },
        { "Left the channel", "during a bomb" },
        { "Didn't subtract Troll", "from 'non-Troll modules'" },
        { "Forgot to disable", "advantageous features" },
        { "Forgot to read a", "Forget Me Not stage" },
        { "Blamed the expert", "for defuser error" },
        { "Blamed the defuser", "for expert error" },
        { "Misidentified a ", "look-a-like module" },
        { "Ignored solve order", "w/ Turn the Keys" },
        { "Didn't Stay Crazy or", "Stay Cool" },
        { "Used Wrong", "Knob Position" },
        { "Commited tax fraud", "in Tax Returns" },
        { "Reported a bug but", "it was player error" },
        { "Pinged @everyone", "on Discord" },
        { "Made a module that", "was extremely difficult" },
        { "KTANE Discord", "Server Owner" },
        { "Uses @everyone", "frequently" },
        { "KTANE Discord", "Community Manager" }
    };

    private static readonly string[] _targetNames = new string[] { 
        "Adalrice Candelaria", 
        "Anthony Troutt", 
        "Bradley Paine", 
        "Brendan Conner", 
        "Brother Akram", 
        "Claus Strandberg", 
        "Craig Black", 
        "Dalia Margolis", 
        "Dino Bosco", 
        "Erich Soders", 
        "Etta Davis", 
        "Ezra Berg", 
        "Francesca de Santis", 
        "Gabriel Santos", 
        "Harry Bagnato", 
        "Inez Ekwensi", 
        "Jasper Knight", 
        "Jonathan Smythe", 
        "Jordan Cross", 
        "Kalvin Ritter", 
        "Ken Morgan", 
        "Kieran Hudson", 
        "Klaus Liebleid", 
        "Kong Tuo Kwang", 
        "Marco Abiatti", 
        "Marv Gonif", 
        "Matthieu Mendola", 
        "Maya Parvati", 
        "Nila Torvik", 
        "Owen Cage", 
        "Oybek Nabazov", 
        "Penelope Graves", 
        "Reza Zaydan", 
        "Richard Ekwensi", 
        "Richard Foreman", 
        "Richard Magee", 
        "Sean Rose", 
        "Silvio Caruso", 
        "Sister Yulduz", 
        "Viktor Novikov", 
        "Yuki Yamazaki",
        "Ajit Krish",
        "Alma Reynard",
        "Andrea Martinez",
        "Athena Savalas",
        "Basil Carnaby",
        "Blair Reddington",
        "Barbara Keating",
        "Dawood Rangan",
        "Dmitri Fedorov",
        "Dorian Lang",
        "Doris Lee",
        "Galen Vholes",
        "Guillaume Maison",
        "Janus",
        "Jimmy Chen",
        "Jin Noo",
        "Jorge Franco",
        "Lhom Kwai",
        "Ljudmila Vetrova",
        "Mark Faba",
        "Miranda Jamison",
        "Nolan Cassidy",
        "Re Thak",
        "Rico Delgado",
        "Robert Knox",
        "Roman Khabko",
        "Sierra Knox",
        "Sophia Washington",
        "Steven Bradley",
        "The Censor",
        "Tyson Williams",
        "Vanya Shah",
        "Vincente Murillo",
        "Vitaly Reznikov",
        "Wazir Kale",
        "Zoe Washington",
        "Agent Banner",
        "Agent Chamberlin",
        "Agent Davenport",
        "Agent Green",
        "Agent Lowenthal",
        "Agent Montgomery",
        "Agent Price",
        "Agent Rhodes",
        "Agent Swan",
        "Agent Thames",
        "Agent Tremaine",
        "Alexa Carlisle",
        "Arthur Edwards",
        "Carl Ingram",
        "Archibald Yates",
        "Hush",
        "Imogen Royce",
        "Marcus Stuyvesant",
        "Tamara Vidal"
    };

    private static readonly string[] _gunSounds = new string[] { "firing1", "firing2", "firing3", "firing4", "firing5", "firing6", "firing7" };

    //We'll fill in this array with info later.
    private Targets[] TargetInformation = new Targets[_targetNames.Length];

    public KMAudio Audio;
    public KMBombModule Module;
    public TextMesh nameEntry;
    public TextMesh ageEntry;
    public TextMesh reasonEntry1;
    public TextMesh reasonEntry2;
    public GameObject Blood;
    public KMSelectable[] Buttons;
    public SpriteRenderer targetPicture;
    public Sprite[] Targets;

    private bool _isSolved;
    private bool _isTPSolved;
    private bool TwitchPlaysActive;

    private int selectedAnswer;
    private int displayedOption;
    private int targetLimit;

    private Targets[] chosenTargets;
    private Targets answer;

    protected override void Awake()
    {
        base.Awake();
        for (var i = 0; i < Buttons.Length; i++)
        {
            int j = i;
            Buttons[i].OnInteract += delegate { ButtonOperation(j); return false; };
        }
        LoadTargets();
        GenMod();
        SetDisplay();
        Debug.LogFormat("[Hitman #{0}] Good morning Agent 47. Your target is {1}. Get the job done.", ModuleID, answer.Name);
        Debug.LogFormat("[Hitman #{0}] Among the crowd also lies {1}. Eyes on your target.", ModuleID, chosenTargets.Where(x => x.TargetID != answer.TargetID).Select(x => x.Name).Join(", "));
    }

    void LoadTargets()
    {
        for (int i = 0; i < TargetInformation.Length; i++)
        {
            //Target initialization needs to be done here, or else every target will be index 96.
            Targets thisTarget = new Targets();
            thisTarget.Name = _targetNames[i];
            thisTarget.Age = Random.Range(30, 70);
            thisTarget.Alive = true;
            thisTarget.TargetID = i;
            if (Random.Range(0f, 1f) > .5f) //If true, use Two Line Reasons
            {
                int line = Random.Range(0, TwoLine.GetLength(0));
                thisTarget.Reason1 = TwoLine[line, 0];
                thisTarget.Reason2 = TwoLine[line, 1];
            }
            else
            {
                int line = Random.Range(0, OneLine.Length);
                thisTarget.Reason1 = OneLine[line];
                thisTarget.Reason2 = "";
            }
            TargetInformation[i] = thisTarget;
            Debug.LogFormat("<Hitman #{5}> Name: {0}, Age: {1}, Reason: {2} {3}, Target ID: {4}", thisTarget.Name, thisTarget.Age, thisTarget.Reason1, thisTarget.Reason2, thisTarget.TargetID, ModuleID);
        }
    }

    void GenMod()
    {
        targetLimit = Random.Range(10, 16);

        displayedOption = Random.Range(0, targetLimit);
        TargetInformation = TargetInformation.Shuffle();
        chosenTargets = TargetInformation.Take(targetLimit).ToArray();
        answer = chosenTargets[0];
        chosenTargets = chosenTargets.Shuffle();

    }
    void SetDisplay()
    {
        Targets thisTarget = chosenTargets[displayedOption];
        nameEntry.text = thisTarget.Name;
        ageEntry.text = thisTarget.Age.ToString();
        reasonEntry1.text = thisTarget.Reason1;
        reasonEntry2.text = thisTarget.Reason2;
        if (!thisTarget.Alive)
        {
            Blood.SetActive(true);
            targetPicture.sprite = Targets[thisTarget.TargetID];
        }
        else
        {
            Blood.SetActive(false);
            targetPicture.sprite = Targets[answer.TargetID];
        }
    }

    void ButtonOperation(int button)
    {
        if (_isSolved)
            return;
        Targets thisTarget = chosenTargets[displayedOption];
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        switch (button)
        {
            case 0:
                displayedOption--;
                if (displayedOption < 0)
                    displayedOption += targetLimit;
                break;
            case 1:
                displayedOption++;
                displayedOption %= targetLimit;
                break;
            case 2:
                Buttons[2].AddInteractionPunch(0.4f);
                if (thisTarget == answer)
                {
                    _isSolved = true;
                    if (TwitchPlaysActive)
                        StartCoroutine(TPSolve());
                    else
                    {
                        thisTarget.Alive = false;
                        Audio.PlaySoundAtTransform(_gunSounds.PickRandom(), transform);
                        Blood.transform.localEulerAngles = new Vector3(Blood.transform.localEulerAngles.x, Blood.transform.localEulerAngles.y, Random.Range(0f, 360f));
                        Audio.PlaySoundAtTransform("Solve3", transform);
                        Module.HandlePass();
                        Debug.LogFormat("[Hitman #{0}] You killed your target, {1}. Good job, Agent 47.", ModuleID, answer.Name);
                    }
                }
                else
                {
                    Blood.transform.localEulerAngles = new Vector3(Blood.transform.localEulerAngles.x, Blood.transform.localEulerAngles.y, Random.Range(0f, 360f));
                    Module.HandleStrike();
                    Debug.LogFormat("[Hitman #{0}] Agent 47, you shot {1} but your target is {2}. You know better than to kill non-targets.", ModuleID, thisTarget.Name, answer.Name);
                }
                break;
        }
        SetDisplay();
    }

    

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use <!{0} kill Marcus Stuyvesant> to shoot them.";
#pragma warning restore 414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim().ToUpperInvariant();
        List<string> parameters = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        string[] availableNames = chosenTargets.Select(x => x.Name.ToUpper()).ToArray();

        if (new string[] { "KILL", "SHOOT", "FIRE", "SUBMIT", "HIT", "ELIMINATE" }.Contains(parameters.First()))
        {
            parameters.RemoveAt(0);
            if (!availableNames.Contains(parameters.Join()))
            {
                yield return "sendtochaterror No such person could be located.";
                yield break;
            }
            int targetIndex = Array.IndexOf(availableNames, parameters.Join());
            KMSelectable closestButton = Math.Abs(displayedOption - targetIndex) < targetLimit / 2 ^ targetIndex > displayedOption ? Buttons[0] : Buttons[1]; //Determines which button is the shortest path to the answer.
            yield return null;
            while (displayedOption != targetIndex)
            {
                closestButton.OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
            Buttons[2].OnInteract();
            yield return targetIndex == Array.IndexOf(IDs, answer.TargetID) ? "solve" : "strike";
        }
    }
    private IEnumerator TwitchHandleForcedSolve()
    {
        int[] IDs = chosenTargets.Select(x => x.TargetID).ToArray();
        int targetIndex = Array.IndexOf(IDs, answer.TargetID);
        KMSelectable closestButton = Math.Abs(displayedOption - targetIndex) < targetLimit / 2 ^ targetIndex > displayedOption ? Buttons[0] : Buttons[1]; //Determines which button is the shortest path to the answer.
        while (displayedOption != targetIndex)
        {            closestButton.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        Buttons[2].OnInteract();
        yield return new WaitForSeconds(0.1f);
        while (!_isTPSolved) yield return true;
    }
    private IEnumerator TPSolve()
    {
        Audio.PlaySoundAtTransform("beemp", transform);
        yield return new WaitForSeconds(1);
        Audio.PlaySoundAtTransform("beemp", transform);
        yield return new WaitForSeconds(1);
        Audio.PlaySoundAtTransform(_gunSounds.PickRandom(), transform);
        Audio.PlaySoundAtTransform("Solve3", transform);
        chosenTargets[displayedOption].Alive = false;
        SetDisplay();
        Blood.transform.localEulerAngles = new Vector3(Blood.transform.localEulerAngles.x, Blood.transform.localEulerAngles.y, Random.Range(0f, 360f));
        Module.HandlePass();
        _isTPSolved = true;
        yield return new WaitForSeconds(7);
        Audio.PlaySoundAtTransform("Small Explosion", transform);
        Debug.LogFormat("[Hitman #{0}] You killed your target, {1}. Good job, Agent 47.", ModuleID, answer.Name);
    }
}

public class Targets
{
    public string Name;
    public int Age;
    public bool Alive;
    public string Reason1;
    public string Reason2;
    public int TargetID;
    public static bool operator ==(Targets t1, Targets t2)
        { return t1.TargetID == t2.TargetID; }
    public static bool operator !=(Targets t1, Targets t2)
        { return t1.TargetID != t2.TargetID; }
}
