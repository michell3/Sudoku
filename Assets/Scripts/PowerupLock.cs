﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupLock : Powerup {

	override public void Activate(CharacterBehavior cb){
		cb.ThrowLock ();
	}

}
