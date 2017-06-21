using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : Singleton<Manager> {

    // spell 
    public string spellCast;
    public int num_players = 3;
    // for deverloping convenience
    static public bool isSerialRFID = true;
    static public bool isSerialLED = false;
    static public bool isWifi = true;
    static public bool isVision = true;
    static public bool isRedBook = true;
    static public bool isYellowBook = true;
    static public bool isGreenBook = true;

    static public bool isArrowNav = true;

    // states (static across multiple instances of Manager)
    static public bool allTracked = false;
    static public bool isRitualStarted = false;  // three phone screens visble to the camera

    static public bool isRitualFinished = false;

    public void Cast(string witch1, string witch2, string witch3) {
		spellCast = GetComponent<CastSpell>().Spell (witch1, witch2, witch3);
		// SceneManager.LoadSceneAsync ("Magic Circle");
	}

	public void FinishSpell() {
		//SceneManager.LoadSceneAsync ("Spell Result");
	}

	public string GetSpell() {
		return spellCast;
	}

	public void OnReset() {		
		spellCast = null;

        isSerialRFID = true;
        isSerialLED = false;
        isWifi = true;
        isVision = true;
        isRedBook = true;
        isYellowBook = true;
        isGreenBook = true;

        isArrowNav = true;
        allTracked = false;
        isRitualStarted = false;  // three phone screens visble to the camera

        isRitualFinished = false;
        SceneManager.LoadScene("Transeo Saltatus");
        
    }



    //  get and set static bool varialbe
    public bool getAllTracked()
    {
        return allTracked;
    }
    public void setAllTracked(bool s)
    {
        allTracked = s; 
    }
    public bool getIsRitualStarted()
    {
        return isRitualStarted;
    }
    public void setIsRitualStarted(bool s)
    {
        isRitualStarted = s;
    }
    public bool getIsRitualFinished()
    {
        return isRitualFinished;
    }
    public void setIsRitualFinished(bool s)
    {
        isRitualFinished = s;
    }


    // books
    public bool getIsRedBook()
    {
        return isRedBook;
    }
    public void setIsRedBook(bool s)
    {
        isRedBook = s;
    }

    public bool getIsYellowBook()
    {
        return isYellowBook;
    }
    public void setIsYellowBook(bool s)
    {
        isYellowBook = s;
    }

    public bool getIsGreenBook()
    {
        return isGreenBook;
    }
    public void setIsGreenBook(bool s)
    {
        isGreenBook = s;
    }

    // serials
    public bool getIsSerialRFID()
    {
        return isSerialRFID;
    }
    public void setIsSerialRFID(bool s)
    {
        isSerialRFID = s;
    }

    public bool getIsSerialLED()
    {
        return isSerialLED;
    }
    public void setIsSerialLED(bool s)
    {
        isSerialLED = s;
    }
    // isWifi
    public bool getIsWifi()
    {
        return isWifi;
    }
    public void setIsWifi(bool s)
    {
        isWifi = s;
    }

    // isVision
    public bool getIsVision()
    {
        return isVision;
    }
    public void setIsVision(bool s)
    {
        isVision = s;
    }

    public bool getIsArrowNav()
    {
        return isArrowNav;
    }
    public void setIsArrowNav(bool s)
    {
        isArrowNav = s;
    }
}
