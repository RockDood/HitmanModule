using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitmanScript : KtaneModule
{
    private readonly string[] OneLine = new string[] { 
        "Called queue early in TP", 
        "Mistyped Swan Code", 
        "Wrongly noted edgework", 
        "Gave guesses", 
        "Missed Turn the Key" };
    
    private string[,] TwoLine = new string[,] { 
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
        { "Commited tax fraud", "in Tax Returns" }
    };

    private static string[] _targetNames = new string[] { 
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

    private string[] _gunSounds = new string[] { "firing1", "firing2", "firing3", "firing4", "firing5", "firing6", "firing7" };

    private Targets[] TargetInformation = new Targets[] //I don't know why it only works like this. I wanted it shorter. I had a stroke before coming to this conclusion.
        {
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},
            new Targets {Name = "", Age = 0, Reason1 = "", Reason2 = ""},

        };
    
    //private Targets[] TargetInformation = new Targets[0];

    //Line 277 creates a NullReferenceException anytime that TargetInformation[].<item> is called. Line 236 is what I wanted to do instead of that attrocity above it.



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

    private int selectedAnswer;
    private int displayedOption;
    private int[] TargetOrder = new int[15];
    private int targetLimit;
    private int startingPoint;

    protected override void Awake()
    {
        base.Awake();
        for (var i = 0; i < Buttons.Length; i++)
        {
            int j = i;
            Buttons[i].OnInteract += delegate { ButtonOperation(j); return false; };
        }
        GenMod();
    }

    void GenMod()
    {
        targetLimit = Random.Range(5, 10);
        startingPoint = Random.Range(0, _targetNames.Length);
        displayedOption = Random.Range(0, 16);
        selectedAnswer = Random.Range(0, 16);

        for (var i = 0; i < 15; i++)
        {
            TargetOrder[i] = startingPoint + i >= _targetNames.Length + 1 ? startingPoint + i - _targetNames.Length : startingPoint + i;
            TargetInformation[i].Name = _targetNames[startingPoint + i >= _targetNames.Length + 1 ? startingPoint + i - _targetNames.Length : startingPoint + i];
            TargetInformation[i].Age = Random.Range(30, 70);
            TargetInformation[i].Alive = true;
            if (Random.Range(0f, 1f) > .5f) //If true, use Two Line Reasons
            {
                int line = Random.Range(0, TwoLine.GetLength(0));
                TargetInformation[i].Reason1 = TwoLine[line, 0];
                TargetInformation[i].Reason2 = TwoLine[line, 1];
            }
            else
            {
                int line = Random.Range(0, OneLine.Length);
                TargetInformation[i].Reason1 = OneLine[line];
                TargetInformation[i].Reason2 = "";
            }
            Debug.LogFormat("Name: {0}, Age: {1}, Reason: {2} {3}, Target Order: {4}", TargetInformation[i].Name, TargetInformation[i].Age, TargetInformation[i].Reason1, TargetInformation[i].Reason2, TargetOrder[i]);
        }
        TargetOrder.Shuffle();
    }

    /*void GenMod()
    {
        displayedOption = Random.Range(0, targetLimit);
        selectedAnswer = Random.Range(0, targetLimit);
        targetLimit = Random.Range(5, 10);
        startingPoint = Random.Range(0, _targetNames.Length);


        int position = 0;
        for (var i = startingPoint; i < startingPoint + targetLimit; i++)
        {
            position++;

            if (i > _targetNames.Length)
            {
                i = i - _targetNames.Length;
                Debug.Log("Reached end of Names! Wrapping around!");
            }
            TargetOrder[position] = i;
            TargetInformation[i].Name = _targetNames[i];
            TargetInformation[i].Age = Random.Range(23,65);
            if (Random.Range(0f,1f)>.5f) //If true, use Two Line Reasons
            {
                int line = Random.Range(0, TwoLine.GetLength(0));
                TargetInformation[i].Reason1 = TwoLine[line,0];
                TargetInformation[i].Reason2 = TwoLine[line, 1];
            }
            else
            {
                int line = Random.Range(0, OneLine.Length);
                TargetInformation[i].Reason1 = OneLine[line];
                TargetInformation[i].Reason2 = "";
            }
            TargetInformation[i].Alive = true;
            Debug.LogFormat("Name: {0}, Age: {1}, Reason: {2} {3}", TargetInformation[i].Age, TargetInformation[i].Age, TargetInformation[i].Reason1, TargetInformation[i].Reason2);
        }
        TargetOrder.Shuffle();
        targetPicture.sprite = Targets[TargetOrder[selectedAnswer]];
        nameEntry.text = TargetInformation[TargetOrder[displayedOption]].Name;
        ageEntry.text = TargetInformation[TargetOrder[displayedOption]].Age.ToString();
        reasonEntry1.text = TargetInformation[TargetOrder[displayedOption]].Reason1;
        reasonEntry2.text = TargetInformation[TargetOrder[displayedOption]].Reason2;
        Debug.LogFormat("[Hitman #{0}] Your target is {1}. Good luck, Agent 47.", ModuleID, TargetInformation[TargetOrder[selectedAnswer]].Name);
    }*/

    void ButtonOperation(int button)
    {
        if (_isSolved)
            return;
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        switch (button)
        {
            case 0:
                displayedOption--;
                if (displayedOption == -1)
                    displayedOption = 15;
                Debug.Log(displayedOption.ToString());
                break;
            case 1:
                displayedOption++;
                if (displayedOption == 16)
                    displayedOption = 0;
                Debug.Log(displayedOption.ToString());
                break;
            case 2:
                Audio.PlaySoundAtTransform(_gunSounds[Random.Range(0, _gunSounds.Length)], transform);
                TargetInformation[TargetOrder[displayedOption]].Alive = false;
                Debug.Log(displayedOption.ToString());
                if (TargetOrder[displayedOption] == TargetOrder[selectedAnswer])
                {
                    Blood.transform.localEulerAngles = new Vector3(Blood.transform.localEulerAngles.x, Blood.transform.localEulerAngles.y, Random.Range(0f, 360f));

                    Audio.PlaySoundAtTransform("Solve3", transform);
                    _isSolved = true;
                    Module.HandlePass();
                    Debug.LogFormat("[Hitman #{0}] You killed your target, {1}. Good job, Agent 47.", ModuleID, TargetInformation[TargetOrder[selectedAnswer]].Name);
                }
                else
                {
                    Blood.transform.localEulerAngles = new Vector3(Blood.transform.localEulerAngles.x, Blood.transform.localEulerAngles.y, Random.Range(0f, 360f));
                    Module.HandleStrike();
                    Debug.LogFormat("[Hitman #{0}] Agent 47, you shot {1} but your target is {2}. You know better than to kill non-targets.", ModuleID, TargetInformation[TargetOrder[displayedOption]].Name, TargetInformation[TargetOrder[selectedAnswer]].Name);
                }
                break;
        }
        
        /*nameEntry.text = TargetInformation[TargetOrder[displayedOption]].Name;
        ageEntry.text = TargetInformation[TargetOrder[displayedOption]].Age.ToString();
        reasonEntry1.text = TargetInformation[TargetOrder[displayedOption]].Reason1;
        reasonEntry2.text = TargetInformation[TargetOrder[displayedOption]].Reason2;*/
        if (!TargetInformation[TargetOrder[displayedOption]].Alive)
        {
            Blood.SetActive(true);
            targetPicture.sprite = Targets[TargetOrder[displayedOption]];
        }
        else
        {
            Blood.SetActive(false);
            targetPicture.sprite = Targets[TargetOrder[selectedAnswer]];
        }
    }
}

public class Targets
{
    public string Name;
    public int Age;
    public bool Alive;
    public string Reason1;
    public string Reason2;
    public int Target;
}