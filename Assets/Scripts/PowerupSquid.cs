using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSquid : Powerup {

	override public void Activate(CharacterBehavior cb){
		cb.SquidAttack ();
	}
}
