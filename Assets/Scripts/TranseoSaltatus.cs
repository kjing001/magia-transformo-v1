using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EasyWiFi.ServerBackchannels;

public class TranseoSaltatus : MonoBehaviour {


    /*
    public Toggle isRedBook;
    public Toggle isYellowBook;
    public Toggle isGreenBook;
    public Toggle isSerialRFID;
    public Toggle isSerialLED;
    public Toggle isWifi;
    public Toggle isVision;
    public Button restartButton;*/

    // list of the costumes for each witch 
    //		stored in form [Hat1, Cloak1, Hat2, Cloak2, Hat3, Cloak3]
    string[] costumes = new string[6];

    string witch1;
    string witch2;
    string witch3;
    string RFID;

    int curCheckpoint = 0;
    public int checkpointNum = 8;
   

    bool onetimeDressing = true;
    bool onetimeEnding = true;
    float currCountdownValue;

    Manager manager;
    SerialController serialControllerRFID;
    SerialController serialControllerLED;    
    AudioController audioController;
    VisionController visionController;

    // each phone (spell book / player) only listens to one of these msgs. But all these msgs are packaged and broadcast once per update()
    // to send multiple variables to one phone per update(), additional channels needed to be added.
    public StringServerBackchannel msg1, nav1;
    public StringServerBackchannel msg2, nav2;
    public StringServerBackchannel msg3, nav3;
    public StringServerBackchannel msgGameStateChannel;

    int curPos_red, curPos_yellow, curPos_green;
    string wifiMsg1, wifiMsg2, wifiMsg3;
    string navMsg1, navMsg2, navMsg3;
    string tracking1, tracking2, tracking3;
    string spell, ledMsg, gameStateMsg; // spell is sent when gameState is the result

    // Panel
    public Text witchList;
    public Text tagID;
    public Text wifiMsgDisplay1, wifiMsgDisplay2, wifiMsgDisplay3;
    public Text navDisplay1, navDisplay2, navDisplay3;
    public Text ledMsgDisplay, spellTextDisplay, gameStateDisplay, trackingDiplay1, trackingDiplay2, trackingDiplay3;

    // costume RFIDs -> corresponding strings
    //		energy items use "N-ergy" since witchLookup keys depend on first letter of costumeLookup values
    static Dictionary<string, string> costumeLookup = new Dictionary<string, string>() {
        {"535384", "Fire Hat"}, {" 0x4 0x24 0xFD 0x72 0xDF 0x4C 0x81", "Fire Hat"},
        { "4FCC6E4", "Fire Cloak"}, {" 0x4 0xF3 0xFF 0x72 0xDF 0x4C 0x80", "Fire Cloak"},
        {"1112EE74", "Water Hat"}, {" 0x4 0xEB 0xFF 0x72 0xDF 0x4C 0x80", "Water Hat"},
        { "4F36794", "Water Cloak"}, { "594294", "Water Cloak"}, {" 0x4 0xFB 0xFF 0x72 0xDF 0x4C 0x80", "Water Cloak"},
        {"116314", "Earth Hat"}, {" 0x4 0x1C 0xFD 0x72 0xDF 0x4C 0x81", "Earth Hat" },
        { "4F0B144", "Earth Cloak"}, {" 0x4 0xD 0xFE 0x72 0xDF 0x4C 0x81", "Earth Cloak"},
        {"10FAA824", "Air Hat"}, {" 0xEE 0xAC 0x41 0x70", "Air Hat"},
        { "4F6AEC4", "Air Cloak"}, {" 0x90 0xAA 0x20 0x27", "Air Cloak"},
        {"1143A24", "Dark Hat"}, {" 0x4 0x6 0xFF 0x72 0xDF 0x4C 0x81", "Dark Hat"},
        { "10FA9C94", "Dark Cloak"}, {" 0x4 0xE3 0xFF 0x72 0xDF 0x4C 0x80", "Dark Cloak"},
        {"5349094", "N-ergy Hat"}, {" 0x4 0x15 0xFE 0x72 0xDF 0x4C 0x81", "N-ergy Hat"},
        { "10FBC844", "N-ergy Cloak"}, {" 0x4 0xDB 0xFF 0x72 0xDF 0x4C 0x80", "N-ergy Cloak"}
    };

    static Dictionary<string,string> witchLookup = new Dictionary<string,string> () {
		{"FF", "Fire"}, 		{"WW", "Water"}, 		{"EE", "Earth"},
        {"AA", "Air"}, 		{"DD", "Dark"}, 		{"NN", "Energy"},
		{"FW", "Steam"},  
		{"FE", "Lava"},
		{"FA", "Fire Air"},
		{"FD", "Dark Fire"},
		{"FN", "Fire Energy"},
		{"WE", "Mud"},
		{"WA", "Rain"},
		{"WD", "Dark Water"},
		{"WN", "Water Energy"},
		{"EA", "Dust"},
		{"ED", "Dark Earth"},
		{"EN", "Gem"},
		{"AD", "Tornado"},
		{"AN", "Wind"},
		{"DN", "Dark Energy"}
	};

    static Dictionary<int, string> num2String = new Dictionary<int, string>() {
        {1, "One"}, {2, "Two"}, {3, "Three"}, {4, "Four"}
    };
    /* // old fashioned 
    static Dictionary<string, float> floatLookup = new Dictionary<string, float>() {
        {"Red", 1.0f}, {"Yellow", 2.0f}, {"Green", 3.0f},
        {"Left", 4.0f}, {"Stop", 5.0f}, {"Right", 6.0f},

        {"Null Hat", 10.0f}, {"Fire Hat", 11.0f}, {"Water Hat", 12.0f}, {"Earth Hat", 13.0f}, {"Air Hat", 14.0f}, {"Dark Hat", 15.0f}, {"N-ergy Hat", 16.0f},
        {"Null Cloak", 20.0f},{"Fire Cloak", 21.0f}, {"Water Cloak", 22.0f}, {"Earth Cloak", 23.0f}, {"Air Cloak", 24.0f}, {"Dark Cloak", 25.0f}, {"N-ergy Cloak", 26.0f},

        {"Null Witch", 30.0f}, {"Fire", 31.0f}, {"Water", 32.0f}, {"Earth", 33.0f}, {"Air", 34.0f}, {"Dark", 35.0f}, {"Energy", 36.0f},
        {"Steam", 37.0f}, {"Lava", 38.0f}, {"Fire Air", 39.0f}, {"Dark Fire", 40.0f}, {"Fire Energy", 41.0f}, {"Mud", 42.0f}, {"Rain", 43.0f},
        {"Dark Water", 44.0f}, {"Water Energy", 45.0f}, {"Dust", 46.0f}, {"Dark Earth", 47.0f}, {"Gem", 48.0f}, {"Tornado", 49.0f}, {"Wind", 50.0f}, {"Dark Energy", 51.0f},
    };*/


    void Awake() {
        costumes = new string[6];
        witch1 = null;
        witch2 = null;
        witch3 = null;

        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        serialControllerRFID = GameObject.Find("SerialControllerRFID").GetComponent<SerialController>();
        serialControllerLED = GameObject.Find("SerialControllerLED").GetComponent<SerialController>();

        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();

        // get the (x,y)s of three books from vision controller for the ritual process control
        visionController = GameObject.Find("VisionController").GetComponent<VisionController>();

        // audioController.playMusic("intro");
        manager.setIsRitualStarted(false);
        manager.setIsRitualFinished(false);
    }
    void Start() {
		
        StartCoroutine(Starting());
        // StartCoroutine(Ending());
    }

    void Update () {
        
        if (manager.getIsSerialRFID())
        {
            RFID = serialControllerRFID.ReadSerialMessage();

            if (RFID != null)
            {
                if (ReferenceEquals(RFID, SerialController.SERIAL_DEVICE_CONNECTED))
                    Debug.Log("Connection established RFID");
                else if (ReferenceEquals(RFID, SerialController.SERIAL_DEVICE_DISCONNECTED))
                    Debug.Log("Connection attempt failed or disconnection detected");
                else
                {
                    Debug.Log("Message arrived: " + RFID);
                    
                    if (RFID.Substring(0, 2) != "FF") // sometimes a tag comes too fast and its ID becomes FFFFFF
                        OnCostumeScanned(RFID); // scanned costume tags
                }
            }      
            
        }
        if (manager.getIsSerialLED() )
        {
            string message = serialControllerLED.ReadSerialMessage();
            if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
                Debug.Log("Connection established COM LED");
            else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
                Debug.Log("Connection attempt failed or disconnection detected");
        }

        // debug element combo        
        spell = manager.GetSpell();

        keyboardControls();

        // check if players are ready , if ready, setIsRitualStarted to true
        if (manager.getIsRitualStarted() == false)
        {
            OnReadyUp();
        }
        
        // ritual, when 3 checkpoints are reached, set finish to true
        if (manager.getIsRitualStarted() && (manager.getIsRitualFinished() == false) )
        {
            OnRitual();
        }

        // Ending, only once: get spell from manager and send to phones
        // MUST NOT set ritual finished to true, it will go to OnRitual again
        if (manager.getIsRitualFinished() && onetimeEnding)
        {
            spell = manager.GetSpell();
            // wifiNavAll("result");
            wifiAll(spell);
            StartCoroutine(Ending());

            onetimeEnding = false;
        }
    }

    IEnumerator Starting()
    {
        audioController.playMusic("intro");

        // audio intros
        yield return new WaitForSeconds(3);
        audioController.playSoundEffect("Intro Speech");
        yield return new WaitForSeconds(33);
        audioController.playSoundEffect("Evil Laugh 2");
        yield return new WaitForSeconds(6);
        audioController.playSoundEffect("Choose your hat and cloak carefully");
                
        yield return new WaitForSeconds(10);
        audioController.playSoundEffect("Sanctify elemental symbols at altar");
        yield return new WaitForSeconds(12);
        audioController.playSoundEffect("Now it is time to claim your spellbooks");
        
        yield return null;
    }

    IEnumerator Restarting()
    {
        costumes = new string[6];
        witch1 = null;
        witch2 = null;
        witch3 = null;
        manager.setIsRitualStarted(false);
        manager.setIsRitualFinished(false);
        onetimeDressing = true;
        onetimeEnding = true;
        curCheckpoint = 0;
        manager.spellCast = null;
        yield return new WaitForSeconds(3);
        audioController.playMusic("intro");
        yield return new WaitForSeconds(5);
        audioController.playSoundEffect("Choose your hat and cloak carefully");
    }
    IEnumerator AfterDressingUp()
    {
        audioController.playSoundEffect("Open your spellbooks and gather around");
        yield return new WaitForSeconds(6);

        // turn to the spell casting page
        wifiGameState("spell casting");
        audioController.playSoundEffect("Follow the directions on the spellbooks");
        yield return null;
    }

    
    public IEnumerator StartCountdown(float countdownValue = 1)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(0.2f);
            currCountdownValue --;
        }
    }
    public IEnumerator ClearChannel(StringServerBackchannel c)
    {
        yield return 0; // wait for a frame
        c.setValue("clear");
    }
    IEnumerator CheckPointOneReached()
    {
        yield return new WaitForEndOfFrame();
        wifiNavAll("Arrived");
        audioController.playSoundEffect("checkpoint reached");
        yield return new WaitForSeconds(3);        
    }
    IEnumerator CheckPointTwoReached()
    {
        yield return new WaitForEndOfFrame();
        wifiNavAll("Arrived");
        audioController.playSoundEffect("spell ready 1");
        yield return new WaitForSeconds(3);
    }
    IEnumerator CheckPointThreeReached()
    {
        yield return new WaitForEndOfFrame();
        wifiNavAll("Arrived");
        audioController.playSoundEffect("checkpoint reached");
        yield return new WaitForSeconds(1);
        audioController.playSoundEffect("checkpoint reached");
        yield return new WaitForSeconds(1);
    }
    IEnumerator CheckPointFourReached()
    {
        yield return new WaitForEndOfFrame();
        wifiNavAll("Arrived");
        audioController.playSoundEffect("spell ready 1");
        yield return new WaitForSeconds(1);
        audioController.playSoundEffect("spell ready 1");
        yield return new WaitForSeconds(1);
    }
    IEnumerator Ending()
    {
        // hold the three showing arrived
        yield return new WaitForEndOfFrame();
        wifiNavAll("Arrived");
                
        // stop dance music                
        audioController.stopMusic("ritual");
        yield return new WaitForSeconds(1);
        
        // wait for the spellReady sound finishes
        audioController.playSoundEffect("spell ready 2");     // small cases for sound effects
        yield return new WaitForSeconds(4);

        yield return new WaitForEndOfFrame();
        wifiNavAll("clear");
        yield return new WaitForEndOfFrame();
        // switch to result page
        wifiGameState("result");

        // good ending
        if (spell != "Nothing happens")
        {
            // play success voice: "congrats" or "well done"
            int rnd = Random.Range(1, 3); //[1-2]
            audioController.playSoundEffect("Spell succeed " + rnd.ToString());
            yield return new WaitForSeconds(3);

            audioController.playSoundEffect(spell);
            yield return new WaitForSeconds(2);
            audioController.playSoundEffect(spell);
            yield return new WaitForSeconds(2);
            audioController.playSoundEffect(spell);
            yield return new WaitForSeconds(2);

            // play a random spellSucceed sound
            rnd = Random.Range(3, 5); //[3-4]
            audioController.playSoundEffect("Spell succeed " + rnd.ToString());
            yield return new WaitForSeconds(4); // all succeed sound are less than 4s
            
        }
        // bad ending
        else
        {
            int rnd = Random.Range(1, 4); //[1-3]
            audioController.playSoundEffect("Spell fail " + rnd.ToString());
            yield return new WaitForSeconds(7); // all fail sounds are less than 7s
        }

        // laughing, coughing
        int rnd_laugh = Random.Range(2, 5); //[2-4]
        audioController.playSoundEffect("Evil Laugh " + rnd_laugh.ToString());
        yield return new WaitForSeconds(7); // all these sounds are less than 7s

        wifiGameState("reset animation");

        // encouragement
        int rnd_encourage = Random.Range(1, 4); //[1-3]
        audioController.playSoundEffect("Encourage " + rnd_encourage.ToString());
        yield return new WaitForSeconds(4); // all these sounds are less than 7s
        

        // tell to restart
        audioController.playSoundEffect("You've exhausted the power of the costumes");
        yield return new WaitForSeconds(8);

        StartCoroutine(Restarting());

    }

        
    void OnRitual()
    {
        
        if(curCheckpoint >= checkpointNum) //"=="
        {
            manager.setIsRitualFinished(true); //
            curCheckpoint = 0; 
            // do not set start, otherwise onReadyUp
        }
        else
        {
            // send navigation commands once per second.
            if (currCountdownValue > 0)
            {
                StartCoroutine(StartCountdown());
            }

            else
            {
                if (curCheckpoint == 0) // red goes to 1, yelow goes to 2, green 3;
                {
                    Navigate(1, 2, 3);
                }
                else if (curCheckpoint == 1) // red goes to 4, yelow goes to 3, green 2;
                {
                    Navigate(4, 3, 2);
                }
                else if (curCheckpoint == 2) // red goes to 3, yelow goes to 1, green 4;
                {
                    Navigate(3, 1, 4);
                }
                else if (curCheckpoint == 3)
                {
                    Navigate(4, 3, 2);
                }
                else if (curCheckpoint == 4)
                {
                    Navigate(2, 4, 1);
                }
                else if (curCheckpoint == 5)
                {
                    Navigate(1, 2, 3);
                }
                else if (curCheckpoint == 6)
                {
                    Navigate(4, 3, 2);
                }
                else if (curCheckpoint == 7)
                {
                    Navigate(3, 1, 4);
                }
                // reset one second timer
                // currCountdownValue = 1.0f;
            }        

        }
    }

    void Navigate(int a, int b, int c)
    {
        // if all arrived
        bool condition = false;
        if (manager.num_players == 1)
        {
            condition = (checkPoint("red", a) || checkPoint("yellow", b) || checkPoint("green", c));
        }else if (manager.num_players == 3)
        {
            condition = (checkPoint("red", a) && checkPoint("yellow", b) && checkPoint("green", c));

        }else if (manager.num_players == 2)
        {
            condition = (  (checkPoint("red", a) && checkPoint("yellow", b)) || 
                           (checkPoint("red", a) && checkPoint("green", c)) || 
                           (checkPoint("yellow", b) && checkPoint("green", c))  );
        }

        if (condition)
        {
            if (curCheckpoint == 0)
            {
                StartCoroutine(CheckPointOneReached());
            }
            else if (curCheckpoint == 1)
            {
                StartCoroutine(CheckPointTwoReached());
            }
            else if (curCheckpoint == 2)
            {
                StartCoroutine(CheckPointThreeReached());

            }
            else if (curCheckpoint == 3)
            {
                StartCoroutine(CheckPointFourReached());
            }
            else if (curCheckpoint == 4)
            {
                StartCoroutine(CheckPointOneReached());
            }
            else if (curCheckpoint == 5)
            {
                StartCoroutine(CheckPointTwoReached());
            }
            else if (curCheckpoint == 6)
            {
                StartCoroutine(CheckPointThreeReached());
            }
            
            curCheckpoint = curCheckpoint + 1;
        }
        else // not yet All arrived
        {
            // arrow navigation
            if (manager.getIsArrowNav())
            {
                ArrowNav("red", a);
                ArrowNav("yellow", b);
                ArrowNav("green", c);
            }
            else // send curPos and dstPos to phones
            {
                wifiNav1("curPos" + locatePlayer("red").ToString() + "dstPos" + a.ToString());
                wifiNav2("curPos" + locatePlayer("yellow").ToString() + "dstPos" + b.ToString());
                wifiNav3("curPos" + locatePlayer("green").ToString() + "dstPos" + c.ToString());
            }
        }
    }
    // arrow navigations: 
    // find current position
    // send clockwise or cc arrows according to the postion wrt current checkpoint to ensure nearest route. 
    // Region:
    // 2, 1;
    // 4, 3.
    void ArrowNav(string player, int dstPos)
    {
        // get player's current region
        int curPos = locatePlayer(player);
        if (curPos == dstPos)
            wifiNav(player, "Arrived");
        else if (curPos == -1)
        {
            wifiNav(player, "I dont c u");
        }
        else
        {
            // the all the cc situations, but cc in the camera is c, so send c
            if (( (curPos == 1) && (dstPos == 3) ) || ((curPos == 2) && (dstPos == 1) )
                    || ( (curPos == 3)  && (dstPos == 4)) || ( (curPos == 4) && (dstPos == 2) ) )
                wifiNav(player, "Clockwise");  // don't spell it as "Cloakwise" ... it will waste you hours debugging!
            else
                wifiNav(player, "Counter-clockwise");
        }            
    }
    int locatePlayer(string player)
    {
        if (checkPoint(player, 1))
            return 1;
        else if (checkPoint(player, 2))
            return 2;
        else if (checkPoint(player, 3))
            return 3;
        else if (checkPoint(player, 4))
            return 4;
        else return -1; // not visible
    }
        
    // check if a book is within an area, once a second
    bool checkPoint(string player, int areaCode)
    {
        int x, y;
        int n = 4; // number of divided areas of the magic circle
        if (player == "red")
        {
            x = visionController.red_book.xm;
            y = visionController.red_book.ym;
        }
        else if (player == "yellow")
        {
            x = visionController.yellow_book.xm;
            y = visionController.yellow_book.ym;
        }
        else if (player == "green")
        {
            x = visionController.green_book.xm;
            y = visionController.green_book.ym;
        }
        else
            return false;

        if( (x == -1) || (y == -1))
        {
            return false;
        }

        if (n == 4)
        {
            if (areaCode == 1)
            {
                if (0 <= x && x < 960 && 0 <= y && y < 540)
                    return true;
            }
            else if (areaCode == 2)
            {
                if (960 <= x && x < 1920 && 0 <= y && y < 540)
                    return true;
            }
            else if (areaCode == 3)
            {
                if (0 <= x && x < 960 && 540 <= y && y < 1080)
                    return true;
            }
            else
            {
                if (960 <= x && x < 1920 && 540 <= y && y < 1080)
                    return true;
            }
            return false; 
        }
        else
        {
            return false;
        }

    }

    // Start the ritual if 
    // 1. there have been 6 costumes scanned, and
    // 2. 3 phone screens are visible in the camera view
    void OnReadyUp() {
		
        foreach (string costume in costumes)
            if (costume == null)
                return;

        // the first time all six costumes are sanctified, turn the spellbooks to the page of ritual
        // the color will be tracked by visioncontroller, then if three of them are all tracked,
        // start the ritual

        // tell players to go to the ritual
        if (manager.getAllTracked() == false)
        {
            if (onetimeDressing)
            {                
                StartCoroutine(AfterDressingUp());
                onetimeDressing = false;
            }                
        }
        else            
        {
            manager.Cast(witch1, witch2, witch3);  // start the spell casting ritual
            manager.setIsRitualStarted(true);

            // send spell - will do it at the ending
            // wifiAll(spell);

            // change music
            audioController.stopMusic("intro");
            audioController.playMusic("ritual");

            curCheckpoint = 0;
        }    
			
		
	}


    // lookup RFID-costume dictionary, send commands to LED, play effect audio, update spell book
    void OnCostumeScanned(string costumeID) {      
		string itemScanned = costumeLookup [costumeID];
        string element = itemScanned.Substring(0,1);

        //Debug.Log(itemScanned);
        led(element);              

        Debug.Log("Scanned:" + itemScanned + ", send element:" + element);

        // check if rescanning/deselecting the item
        int itemsSelected = 0;
		for (int i = 0; i < 6; i++) {
			if (costumes [i] == itemScanned) {
				//Debug.Log ("Deselecting " + itemScanned);
				costumes [i] = null;

                // play de-selecting audio
                // nullify corresponding costume, send nullifying msg correspondingly                
                if (i == 0)
                {
                    witch1 = null;
                    wifi1("Null Hat"); 
                }
                else if (i == 1)
                {
                    witch1 = null;
                    wifi1("Null Cloak"); 
                }
                else if (i == 2)
                {
                    witch2 = null;
                    wifi2("Null Hat"); 
                }
                else if (i == 3)
                {
                    witch2 = null;
                    wifi2("Null Cloak");
                }
                else if (i == 4)
                {
                    witch3 = null;
                    wifi3("Null Hat");
                }
                else
                {
                    witch3 = null;
                    wifi3("Null Cloak");
                }
                				
				return;
			}

			if (costumes [i] != null)
				itemsSelected++;
		}

		// if attempting to select new item after 6 items have been selected, break
		if (itemsSelected == 6)
			return;

        // selecting a new item - determine if user scanned a hat or cloak 
        // 		then, update first empty corresponding location in costumes array (first hat scanned redBook= witch1), send to phone player 1
        //		Note: hats in costumes 0,2,4 -- cloaks in 1,3,5 (witch1 = 0,1)

        // Play element sound
        audioController.playSoundEffect(element);

		int costumeIndex = -1;

		if (itemScanned.Contains ("Hat")) {
			if (costumes [0] == null) {
				costumes [0] = itemScanned;
				costumeIndex = 0;
                wifi1(itemScanned);

			} else if (costumes [2] == null) {
				costumes [2] = itemScanned;
				costumeIndex = 2;
                wifi2(itemScanned);

            } else if (costumes [4] == null) {
				costumes [4] = itemScanned;
				costumeIndex = 4;
                wifi3(itemScanned);
            }
		} else if (itemScanned.Contains("Cloak")) {
			if (costumes [1] == null) {
				costumes [1] = itemScanned;
				costumeIndex = 1;
                wifi1(itemScanned);
            } else if (costumes [3] == null) {
				costumes [3] = itemScanned;
				costumeIndex = 3;
                wifi2(itemScanned);
            } else if (costumes [5] == null) {
				costumes [5] = itemScanned;
				costumeIndex = 5;
                wifi3(itemScanned);
            }
		}

		// if there are no available hat/cloak spots, cannot add new item, return
		if (costumeIndex == -1) {
			if (itemScanned.Contains ("Hat"))
				Debug.Log ("3 Hats have already been selected");
			else
				Debug.Log ("3 Cloaks have already been selected");			
			return;
		}

        // update witch values corresponding to costume array (ex: witch1 = hat in costumes[0] & cloak in costumes[1])
        // the witch name on the spell book will be determined on the phone, not by receving msg from here, 
        // otherwise we need another channel to avoid overlapping with costume msgs.
		if ((costumeIndex == 0 || costumeIndex == 1) && (costumes[0] != null && costumes[1] != null)) {
			if (witchLookup.ContainsKey (costumes [0].Substring(0,1) + costumes [1].Substring(0,1)))
				witch1 = witchLookup [costumes [0].Substring(0,1) + costumes [1].Substring(0,1)];
			else
				witch1 = witchLookup [costumes [1].Substring(0,1) + costumes [0].Substring(0,1)];
			
		} else if ((costumeIndex == 2 || costumeIndex == 3) && (costumes[2] != null && costumes[3] != null)) {
			if (witchLookup.ContainsKey (costumes [2].Substring(0,1) + costumes [3].Substring(0,1)))
				witch2 = witchLookup [costumes [2].Substring(0,1) + costumes [3].Substring(0,1)];
			else
				witch2 = witchLookup [costumes [3].Substring(0,1) + costumes [2].Substring(0,1)];

		} else if ((costumeIndex == 4 || costumeIndex == 5) && (costumes[4] != null && costumes[5] != null)) {
			if (witchLookup.ContainsKey (costumes [4].Substring(0,1) + costumes [5].Substring(0,1)))
				witch3 = witchLookup [costumes [4].Substring(0,1) + costumes [5].Substring(0,1)];
			else
				witch3 = witchLookup [costumes [5].Substring(0,1) + costumes [4].Substring(0,1)];
        }
	}



    // send wifi messages and visualize
    void wifiNav1(string s)
    {
        if (manager.getIsWifi() && manager.getIsRedBook())
        {
            navMsg1 = s;
            nav1.setValue(navMsg1);
        }
    }
    void wifiNav2(string s)
    {
        if (manager.getIsWifi() && manager.getIsYellowBook())
        {
            navMsg2 = s;
            nav2.setValue(navMsg2);
        }
    }
    void wifiNav3(string s)
    {
        if (manager.getIsWifi() && manager.getIsGreenBook())
        {
            navMsg3 = s;
            nav3.setValue(navMsg3);
        }
    }
    void wifiNav(string player, string s)
    {
        if (player == "red")
            wifiNav1(s);
        else if (player == "yellow")
            wifiNav2(s);
        else if (player == "green")
            wifiNav3(s);
        else
            return;
    }
    void wifiNavAll(string s)
    {
        wifiNav1(s);
        wifiNav2(s);
        wifiNav3(s);
        gameStateMsg = s;
    }
    /*
    void wifiSpell(string s)
    {
        if (manager.getIsWifi() && manager.GetSpell() != null)
        {
            spell = s;
            spellChannel.setValue(spell);
        }
    }*/
    void wifiGameState(string s)
    {
        if (manager.getIsWifi())
        {
            gameStateMsg = s;
            msgGameStateChannel.setValue(gameStateMsg);
        }
        // clear gameStateChannel
        StartCoroutine(ClearChannel(msgGameStateChannel));
    }
    
    void wifi1(string s)
    {
        if (manager.getIsWifi() && manager.getIsRedBook() )
        {
            wifiMsg1 = s;
            msg1.setValue(wifiMsg1);
        }
        StartCoroutine(ClearChannel(msg1));
    }
    void wifi2(string s)
    {
        if(manager.getIsWifi() && manager.getIsYellowBook() )
        {
            wifiMsg2 = s;
            msg2.setValue(wifiMsg2);
        }
        StartCoroutine(ClearChannel(msg2));
    }
    void wifi3(string s)
    {
        if (manager.getIsWifi() && manager.getIsGreenBook() )
        {
            wifiMsg3 = s;
            msg3.setValue(wifiMsg3);
        }
        StartCoroutine(ClearChannel(msg3));
    }
    void wifi(string player, string s)
    {
        if (player == "red") 
            wifi1(s);
        else if (player == "yellow") 
            wifi2(s);
        else if (player == "green")
            wifi3(s);
        else 
            return;

        
    }
    void wifiAll(string s)
    {
        wifi1(s);
        wifi2(s);
        wifi3(s);
        spell = s;
    }
    
    // send message to set led
    void led(string s)
    {
        if (manager.getIsSerialLED() )
        {
            ledMsg = s;            
            serialControllerLED.SendSerialMessage(ledMsg);
        }
            
    }

    void OnGUI()
    {
        if (visionController.red_book.isTrackedm)
        {
            tracking1 = "yes tracking red.";
        }
        else
        {
            tracking1 = "not tracking red.";
        }
        if (visionController.yellow_book.isTrackedm)
        {
            tracking2 = "yes tracking yellow.";
        }
        else
        {
            tracking2 = "not tracking yellow.";
        }
        if (visionController.green_book.isTrackedm)
        {
            tracking3 = "yes tracking green.";
        }
        else
        {
            tracking3 = "not tracking green.";
        }
        curPos_red = locatePlayer("red");
        curPos_yellow = locatePlayer("yellow");
        curPos_green = locatePlayer("green");

        GUILayout.BeginVertical();
        trackingDiplay1.text = tracking1 + "curPos_red: " + curPos_red.ToString();
        trackingDiplay2.text = tracking2 + "curPos_yellow: " + curPos_yellow.ToString();
        trackingDiplay3.text = tracking3 + "curPos_green: " + curPos_green.ToString();
        gameStateDisplay.text = "Game State: " + gameStateMsg;
        spellTextDisplay.text = "Spell: " + spell;
        tagID.text = "tagID: " + RFID;
        wifiMsgDisplay1.text = "Wifi1: " + wifiMsg1;
        wifiMsgDisplay2.text = "Wifi2: " + wifiMsg2;
        wifiMsgDisplay3.text = "Wifi3: " + wifiMsg3;
        navDisplay1.text = "Nav1: " + navMsg1;
        navDisplay2.text = "Nav2: " + navMsg2;
        navDisplay3.text = "Nav3: " + navMsg3;
        ledMsgDisplay.text = "LED: " + ledMsg; 
        witchList.text = "Witch 1: " + costumes[0] + " & " + costumes[1] +
            "\nWitch 2: " + costumes[2] + " & " + costumes[3] +
            "\nWitch 3: " + costumes[4] + " & " + costumes[5];
        

        GUILayout.EndVertical();       

        // timer to display some messages for certain seconds
    }

    void keyboardControls(){
        // for debug purposes
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("play intro");
            audioController.playMusic("intro");
            wifiGameState("spell list");
            //serialControllerLED.SendSerialMessage("A");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("pause intro");
            // audioController.pauseMusic("intro");
            wifiGameState("scan item");
            //serialControllerLED.SendSerialMessage("Z");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Debug.Log("stop intro");
            // audioController.stopMusic("intro");
            wifiGameState("spell casting");
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            wifiGameState("result");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            wifiNav1("curPos2desPos3");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            wifiNav1("curPos4desPos1");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            wifiNav2("curPos2desPos3");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            wifiNav2("curPos4desPos1");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            wifiNav3("curPos2desPos3");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            wifiNav3("curPos4desPos1");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            wifiNav1("Clockwise");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            wifiNav1("Counter-clockwise");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            wifiNav2("Clockwise");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            wifiNav2("Counter-clockwise");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            wifiNav3("Clockwise");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            wifiNav3("Counter-clockwise");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            wifiNav1("Arrived");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            wifiNav2("Arrived");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            wifiNav3("Arrived");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            wifiAll("Flight");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            wifiAll("INFINITE RUBBER DUCKS");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            wifiAll("CONGRATULATIONS YOUR CAULDRON SET OFF THE FIRE ALARM");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            wifiGameState("reset animation");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            wifiGameState("bb");
        }
    }
}
