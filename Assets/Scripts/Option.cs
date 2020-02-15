using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Option
{
    public string description;
    public UnityAction onpress;

    public Option(string s, UnityAction op) {
        description = s;
        onpress = op;
    }







    // Prebuilt Options **********************

    public static Option tempA(Player player) {
        return new Option("Strength UP", () => { player.stats.strength++; });
    }
    public static Option tempB(Player player) {
        return new Option("Recover Stamina", () => { player.stats.stamina++; });
    }
    public static Option tempC(Player player) {
        return new Option("Increase HP", () => { player.stats.hp += 2; });
    }
    public static Option tempD(Player player) {
        return new Option("Do nothing", () => {; });

    }

    public static Option[] tempPackage(Player player) {
        return new Option[] { tempA(player), tempB(player), tempC(player), tempD(player) };
    }
    

    
}
