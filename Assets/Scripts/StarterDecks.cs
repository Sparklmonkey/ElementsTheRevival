using System.Collections.Generic;

public class StarterDecks
{
    static StarterDecks()
    {
    }

    private StarterDecks()
    {
    }

    public static StarterDecks Instance { get; } = new();

    public List<string> GetStarterDeck(Element element)
    {
        return element switch
        {
            Element.Aether => _aether,
            Element.Air => _air,
            Element.Darkness => _darkness,
            Element.Light => _light,
            Element.Death => _death,
            Element.Earth => _earth,
            Element.Entropy => _entropy,
            Element.Time => _time,
            Element.Fire => _fire,
            Element.Gravity => _gravity,
            Element.Life => _life,
            Element.Water => _water,
            _ => _aether,
        };
    }


    public (string, string) GetDisplayCards(Element element)
    {
        return element switch
        {
            Element.Aether => _aetherDisplayCards,
            Element.Air => _airDisplayCards,
            Element.Darkness => _darknessDisplayCards,
            Element.Light => _lightDisplayCards,
            Element.Death => _deathDisplayCards,
            Element.Earth => _earthDisplayCards,
            Element.Entropy => _entropyDisplayCards,
            Element.Time => _timeDisplayCards,
            Element.Fire => _fireDisplayCards,
            Element.Gravity => _gravityDisplayCards,
            Element.Life => _lifeDisplayCards,
            Element.Water => _waterDisplayCards,
            _ => _aetherDisplayCards,
        };
    }

    private readonly List<string> _entropy = new("4sa 4sa 4sa 4t3 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vc 4vd 4ve 4ve 4ve 4vf 4vg 4vh 4vh 4vi 4vi 4vi 4vk 4vk 4vk 52g 52g 52g 52g 52g 52g 52i 52j 52k 52m 52m 52o".Split(" "));
    private readonly List<string> _death = new("4sa 4sa 4sa 4t3 52g 52g 52g 52g 52g 52g 52g 52g 52g 52g 52g 52g 52h 52i 52i 52i 52i 52j 52k 52k 52m 52m 52m 52n 52n 52o 52o 52p 52r 55k 55k 55k 55l 55n 55q 55r".Split(" "));
    private readonly List<string> _gravity = new("4sa 4sa 4sa 4t3 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55k 55l 55l 55m 55m 55n 55n 55o 55p 55q 55q 55r 55r 55t 55t 58o 58o 58o 58o 58q 58t 58t 590 592".Split(" "));
    private readonly List<string> _earth = new("4sa 4sa 4sa 4t3 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58o 58p 58p 58q 58r 58s 58t 58t 58u 58u 590 590 591 592 593 593 5bs 5bs 5bs 5bs 5bu 5c0 5c1 5c2".Split(" "));
    private readonly List<string> _life = new("4sa 4sa 4sa 4t3 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bs 5bt 5bu 5bu 5bv 5bv 5c0 5c0 5c0 5c1 5c1 5c2 5c2 5c3 5c4 5c6 5c6 5f0 5f0 5f0 5f0 5f1 5f1 5f4 5f6".Split(" "));
    private readonly List<string> _fire = new("4sa 4sa 4sa 4t3 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f0 5f1 5f1 5f1 5f1 5f2 5f3 5f3 5f4 5f4 5f4 5f5 5f6 5f6 5f8 5f8 5f9 5f9 5f9 5i4 5i4 5i4 5i6 5i7 5ia".Split(" "));
    private readonly List<string> _water = new("4sa 4sa 4sa 4t3 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i4 5i5 5i5 5i6 5i6 5i7 5i7 5i7 5i7 5i8 5i8 5i8 5i8 5i9 5ia 5id 5l8 5l8 5l8 5l8 5l9 5l9 5lb 5lc 5lf".Split(" "));
    private readonly List<string> _light = new("4sa 4sa 4sa 4t3 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l8 5l9 5l9 5la 5lb 5lb 5lb 5lc 5lc 5ld 5le 5le 5lf 5lf 5lf 5lg 5oc 5oc 5oc 5oc 5oc 5od 5oe 5oh 5ok".Split(" "));
    private readonly List<string> _air = new("4sa 4sa 4sa 4t3 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5oc 5od 5od 5od 5oe 5oe 5oe 5of 5og 5oh 5oh 5oh 5oi 5oj 5ok 5ok 5ok 5rg 5rg 5rg 5rg 5rh 5ri 5ri 5rk".Split(" "));
    private readonly List<string> _time = new("4sa 4sa 4sa 4t3 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rg 5rh 5rh 5rh 5ri 5ri 5ri 5rj 5rk 5rk 5rl 5rl 5rm 5rn 5rn 5uk 5uk 5uk 5uk 5um 5um 5un 5up".Split(" "));
    private readonly List<string> _darkness = new("4sa 4sa 4sa 4t3 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5uk 5ul 5um 5um 5um 5um 5un 5un 5un 5uo 5up 5up 5up 5uq 5us 5us 5us 61o 61o 61o 61o 61p 61q 61r 61s".Split(" "));
    private readonly List<string> _aether = new("4sa 4sa 4sa 4t3 4vc 4vc 4vc 4vc 4ve 4vh 4vi 4vj 4vk 61o 61o 61o 61o 61o 61o 61o 61o 61o 61o 61o 61o 61p 61p 61p 61p 61q 61q 61q 61q 61r 61r 61r 61s 61s 61t 61v".Split(" "));

    private readonly (string, string) _entropyDisplayCards = ("4vd", "4vi");
    private readonly (string, string) _deathDisplayCards = ("52n", "52o");
    private readonly (string, string) _gravityDisplayCards = ("55q", "55m");
    private readonly (string, string) _earthDisplayCards = ("58s", "58p");
    private readonly (string, string) _lifeDisplayCards = ("5c2", "5bv");
    private readonly (string, string) _fireDisplayCards = ("5f4", "5f6");
    private readonly (string, string) _waterDisplayCards = ("5i7", "5ia");
    private readonly (string, string) _lightDisplayCards = ("5lf", "5la");
    private readonly (string, string) _airDisplayCards = ("5oh", "5oe");
    private readonly (string, string) _timeDisplayCards = ("5rl", "5rh");
    private readonly (string, string) _darknessDisplayCards = ("5ul", "5up");
    private readonly (string, string) _aetherDisplayCards = ("61r", "61t");
}
