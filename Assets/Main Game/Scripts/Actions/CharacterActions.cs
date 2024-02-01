using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/**
 * Author: Julia Bugaj
 * 
 * The CharacterActions class provides static UnityActions for character-related events. 
 */
public class CharacterActions
{
    public static UnityAction<GameObject, int> characterDamaged; /* UnityAction for the character being damaged, with GameObject (character) and int (damage) parameters. */
    public static UnityAction<GameObject, int> characterHealed; /* UnityAction for the character being healed, with GameObject (character) and int (healing amount) parameters. */
}
