using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDart : Powerup {

	override public void Activate(CharacterBehavior cb){
		cb.ThrowDart ();
	}
}
