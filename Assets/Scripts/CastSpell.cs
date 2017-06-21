using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : MonoBehaviour {

	static Dictionary<string, List<string>> spells = new Dictionary<string, List<string>>() {
		{"Summon Cat Familiar", new List<string> {"Earth", "Energy", "Dark Water"}},
		{"Summon Ghost", new List<string> {"Air", "Energy", "Dark"}},
		{"Summon Dragon", new List<string> {"Energy", "Lava", "Dark Fire"}},
		{"Good Luck", new List<string> {"Earth", "Air", "Energy"}},
		{"Bad Luck", new List<string> {"Dark", "Lava", "Dust"}},
		{"Flight", new List<string> {"Air", "Energy", "Steam"}},
		{"$$$", new List<string> {"Rain", "Gem", "Dark Energy"}},
		{"Your Code Works", new List<string> {"Dark Fire", "Fire Energy", "Dark Energy"}},
		{"Perfect Coffee", new List<string> {"Earth", "Steam", "Dark Energy"}},
		{"Zombie Apocalypse", new List<string> {"Dark Fire", "Dark Earth", "Tornado"}},
		{"Apocalypse", new List<string> {"Dark Fire", "Rain", "Tornado"}},
		{"Transforms Drinker into a Newt\n(you get better)", new List<string> {"Mud", "Dark Earth", "Dark Energy"}},
		{"Makes Drinker Weigh Same as a Duck", new List<string> {"Earth", "Water Energy", "Dark Energy"}},
		{"Makes Drinker Weigh Not Same as a Duck", new List<string> {"Earth", "Water Energy", "Tornado"}},
		{"INFINITE RUBBER DUCKS", new List<string> {"Earth", "Water", "Energy"}},
		{"Fireworks", new List<string> {"Fire Air", "Gem", "Wind"}},
		{"Truth Serum", new List<string> {"Air", "Dark Water", "Water Energy"}},
		{"Productivity 500%", new List<string> {"Fire", "Air", "Energy"}},
		{"CONGRATULATIONS YOUR CAULDRON SET OFF THE FIRE ALARM", new List<string> {"Steam", "Steam"}}

	};

	public string Spell(string witch1, string witch2, string witch3) {
		foreach (string key in spells.Keys) {
			if (spells [key].Count == 3) { // Spell requires three specific witches
				if (spells [key].Contains (witch1) && spells [key].Contains (witch2) && spells [key].Contains (witch3)) {
					if (witch1 != witch2 && witch2 != witch3 && witch1 != witch3) {
						return key;
					} else {
						List<string> maybeSpell = new List<string> ();
						foreach (string witch in spells[key])
							maybeSpell.Add (witch);

						if (maybeSpell.Remove (witch1) && maybeSpell.Remove (witch2) && maybeSpell.Remove (witch3))
							return key;
					}
				}
			} else { // spell requires two specific witches (third is irrelevant) - example: fire alarm is set off when 2 steam witches are present
				List<string> maybeSpell = new List<string> ();
				foreach (string witch in spells[key])
					maybeSpell.Add (witch);

				if (witch1 == witch2) {
					maybeSpell.Remove (witch1);
					maybeSpell.Remove (witch2);
				} else if (witch2 == witch3) {
					maybeSpell.Remove (witch2);
					maybeSpell.Remove (witch3);
				} else if (witch3 == witch1) {
					maybeSpell.Remove (witch3);
					maybeSpell.Remove (witch1);
				}

				if (maybeSpell.Count == 0)
					return key;
			}
		}

		return "Nothing happens";
	}
}
