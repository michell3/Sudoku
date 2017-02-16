using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupLion : Powerup {

	override public void Activate(CharacterBehavior cb){
		cb.LionAttack ();
	}
}
