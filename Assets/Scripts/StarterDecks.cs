using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterDecks 
{
    private static readonly StarterDecks instance = new();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static StarterDecks()
    {
    }

    private StarterDecks()
    {
    }

    public static StarterDecks Instance
    {
        get
        {
            return instance;
        }
    }

    public List<string> GetStarterDeck(Element element)
    {
        return element switch
        {
            Element.Aether => aether,
            Element.Air => air,
            Element.Darkness => darkness,
            Element.Light => light,
            Element.Death => death,
            Element.Earth => earth,
            Element.Entropy => entropy,
            Element.Time => time,
            Element.Fire => fire,
            Element.Gravity => gravity,
            Element.Life => life,
            Element.Water => water,
            _ => aether,
        };
    }


    public (string,string) GetDisplayCards(Element element)
    {
        return element switch
        {
            Element.Aether => aetherDisplayCards,
            Element.Air => airDisplayCards,
            Element.Darkness => darknessDisplayCards,
            Element.Light => lightDisplayCards,
            Element.Death => deathDisplayCards,
            Element.Earth => earthDisplayCards,
            Element.Entropy => entropyDisplayCards,
            Element.Time => timeDisplayCards,
            Element.Fire => fireDisplayCards,
            Element.Gravity => gravityDisplayCards,
            Element.Life => lifeDisplayCards,
            Element.Water => waterDisplayCards,
            _ => aetherDisplayCards,
        };
    }

    private readonly List<string> entropy = new("4sa 4sa 4sa 4t3 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vd 4ve 4ve 4ve 4vf 4vg 4vh 4vh 4vi 4vi 4vi 4vk 4vk 4vk 52g 52g 52g 52g 52g 52g 52i 52j 52k 52m 52m 52o".Split(" "));
    private readonly List<string> death = new("4sa 4sa 4sa 4t3 52g 52g 52g 52g 52g 52g 52g 52g 52g 52g 52g 52g 52h 52i 52i 52i 52i 52j 52k 52k 52m 52m 52m 52n 52n 52o 52o 52p 52r 55k 55k 55k 55l 55n 55q 55r".Split(" "));
    private readonly List<string> gravity = new("4sa 4sa 4sa 4t3 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55l 55l 55m 55m 55n 55n 55o 55p 55q 55q 55r 55r 55t 55t 58o 58o 58o 58o 58q 58t 58t 590 592".Split(" "));
    private readonly List<string> earth = new("4sa 4sa 4sa 4t3 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58p 58p 58q 58r 58s 58t 58t 58u 58u 590 590 591 592 593 593 5bs 5bs 5bs 5bs 5bu 5c0 5c1 5c2".Split(" "));
    private readonly List<string> life = new("4sa 4sa 4sa 4t3 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bt 5bu 5bu 5bv 5bv 5c0 5c0 5c0 5c1 5c1 5c2 5c2 5c3 5c4 5c6 5c6 5f0 5f0 5f0 5f0 5f1 5f1 5f4 5f6".Split(" "));
    private readonly List<string> fire = new("4sa 4sa 4sa 4t3 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f1 5f1 5f1 5f1 5f2 5f3 5f3 5f4 5f4 5f4 5f5 5f6 5f6 5f8 5f8 5f9 5f9 5f9 5i4 5i4 5i4 5i6 5i7 5ia".Split(" "));
    private readonly List<string> water = new("4sa 4sa 4sa 4t3 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i5 5i5 5i6 5i6 5i7 5i7 5i7 5i7 5i8 5i8 5i8 5i8 5i9 5ia 5id 5l8 5l8 5l8 5l8 5l9 5l9 5lb 5lc 5lf".Split(" "));
    private readonly List<string> light = new("4sa 4sa 4sa 4t3 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l9 5l9 5la 5lb 5lb 5lb 5lc 5lc 5ld 5le 5le 5lf 5lf 5lf 5lg 5oc 5oc 5oc 5oc 5oc 5od 5oe 5oh 5ok".Split(" "));
    private readonly List<string> air = new("4sa 4sa 4sa 4t3 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5od 5od 5od 5oe 5oe 5oe 5of 5og 5oh 5oh 5oh 5oi 5oj 5ok 5ok 5ok 5rg 5rg 5rg 5rg 5rh 5ri 5ri 5rk".Split(" "));
    private readonly List<string> time = new("4sa 4sa 4sa 4t3 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rh 5rh 5rh 5ri 5ri 5ri 5rj 5rk 5rk 5rl 5rl 5rm 5rn 5rn 5uk 5uk 5uk 5uk 5um 5um 5un 5up".Split(" "));
    private readonly List<string> darkness = new("4sa 4sa 4sa 4t3 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5ul 5um 5um 5um 5um 5un 5un 5un 5uo 5up 5up 5up 5uq 5us 5us 5us 61o 61o 61o 61o 61p 61q 61r 61s".Split(" "));
    private readonly List<string> aether = new("4sa 4sa 4sa 4t3 4vc 4vc 4vc 4vc 4ve 4vh 4vi 4vj 4vk 61o 61o 61o 61o 61o 61o 61o 61o 61o 61o 61o 61o 61p 61p 61p 61p 61q 61q 61q 61q 61r 61r 61r 61s 61s 61t 61v".Split(" "));

    private readonly (string, string) entropyDisplayCards = ("4vd", "4vi");
    private readonly (string, string) deathDisplayCards = ("52n", "52o");
    private readonly (string, string) gravityDisplayCards = ("55q", "55m");
    private readonly (string, string) earthDisplayCards = ("58s", "58p");
    private readonly (string, string) lifeDisplayCards = ("5c2", "5bv");
    private readonly (string, string) fireDisplayCards = ("5f4", "5f6");
    private readonly (string, string) waterDisplayCards = ("5i7", "5ia");
    private readonly (string, string) lightDisplayCards = ("5lf", "5la");
    private readonly (string, string) airDisplayCards = ("5oh", "5oe");
    private readonly (string, string) timeDisplayCards = ("5rl", "5rh");
    private readonly (string, string) darknessDisplayCards = ("5ul", "5up");
    private readonly (string, string) aetherDisplayCards = ("61r", "61t");
}
