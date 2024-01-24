using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetingAi
{
    
//    if(targeting == "creature")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.ce" + i + ".defnow") < eval("_root.ce" + i + ".def"))
//             {
//                score[45 + i] = 0.01 * eval("_root.ce" + i + ".atknow") * (eval("_root.ce" + i + ".def") - eval("_root.ce" + i + ".defnow")) * estimate;
//             }
//             if(eval("_root.ce" + i + ".atknow") < 0)
//             {
//                score[45 + i] = 0.1 * eval("_root.ce" + i + ".atknow") * estimate;
//             }
//             if(eval("_root.ce" + i + ".poison") < 0 and skill == "purify")
//             {
//                score[45 + i] = 0;
//             }
//             if(skill == "holy light" and (eval("_root.ce" + i + ".element") == 2 or eval("_root.ce" + i + ".element") == 11))
//             {
//                score[45 + i] = - estimate;
//             }
//             if(skill == "reverse time" and (eval("_root.ce" + i + ".passive") == "mummy" or eval("_root.ce" + i + ".passive") == "undead"))
//             {
//                score[45 + i] = 1;
//             }
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             skillscore = 0;
//             if(eval("_root.ce" + i + ".skill") != "")
//             {
//                skillscore = 3;
//             }
//             score[2 + i] = (eval("_root.c" + i + ".atknow") + 1 + skillscore) / (- estimate) / 20;
//             if(eval("_root.c" + i + ".statu") == skill or eval("_root.c" + i + ".frozen") > 0 or eval("_root.c" + i + ".delay") > 0)
//             {
//                score[2 + i] = 0;
//             }
//             if(skill == "holy light" and (eval("_root.c" + i + ".element") == 2 or eval("_root.c" + i + ".element") == 11))
//             {
//                score[2 + i] = (eval("_root.c" + i + ".atknow") + 1 + skillscore) / estimate;
//             }
//             if(skill == "reverse time" and (eval("_root.c" + i + ".passive") == "mummy" or eval("_root.c" + i + ".passive") == "undead"))
//             {
//                score[2 + i] = 0;
//             }
//             if(skill == "reverse time" and eval("_root.c" + i + ".passive") == "voodoo" and eval("_root.c" + i + ".defnow") > 25)
//             {
//                score[2 + i] = eval("_root.c" + i + ".defnow") / 25;
//             }
//             if(skill == "shockwave" and eval("_root.c" + i + ".frozen") > 0)
//             {
//                score[2 + i] = 1;
//             }
//             if(skill == "web" and eval("_root.c" + i + ".passive2") != "airborne")
//             {
//                score[2 + i] = 0;
//             }
//          }
//          i++;
//       }
//    }
//    if(targeting == "immortals")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and onlyfoe == 0 and eval("_root.ce" + i + ".statu") == "invulnerable" and eval("_root.ce" + i + ".burrow") != 1)
//          {
//             score[45 + i] = estimate / eval("_root.ce" + i + ".immortal");
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and onlyfriend == 0 and eval("_root.c" + i + ".statu") == "invulnerable" and eval("_root.c" + i + ".burrow") != 1)
//          {
//             score[2 + i] = 2 * estimate / eval("_root.c" + i + ".immortal");
//          }
//          i++;
//       }
//    }
//    if(targeting == "creaturehighatk")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable" and eval("_root.ce" + i + ".atknow") > eval("_root.ce" + i + ".defnow"))
//          {
//             if(eval("_root.ce" + i + ".poison") > 0 or eval("_root.ce" + i + ".def") - eval("_root.ce" + i + ".defnow") > 0)
//             {
//                score[45 + i] = estimate;
//             }
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable" and eval("_root.c" + i + ".atknow") > eval("_root.c" + i + ".defnow"))
//          {
//             score[2 + i] = eval("_root.c" + i + ".atknow") / (- estimate) / 20;
//             if(eval("_root.c" + i + ".statu") == skill or eval("_root.c" + i + ".poison") > 0)
//             {
//                score[2 + i] = 0;
//             }
//          }
//          i++;
//       }
//    }
//    if(targeting == "creaturelowatk")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable" and eval("_root.ce" + i + ".atknow") < definevalue)
//          {
//             score[45 + i] = estimate * (eval("_root.ce" + i + ".defnow") - eval("_root.ce" + i + ".poison")) / 10;
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable" and eval("_root.c" + i + ".atknow") < definevalue)
//          {
//             score[2 + i] = 0;
//          }
//          i++;
//       }
//    }
//    if(targeting == "trebuchet")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             score[45 + i] = (estimate * eval("_root.ce" + i + ".defnow") / (_root.rethp() + 1) + eval("_root.ce" + i + ".frozen") + eval("_root.ce" + i + ".poison") * 3 - eval("_root.ce" + i + ".atknow")) / 20;
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             score[2 + i] = 0;
//          }
//          i++;
//       }
//    }
//    if(targeting == "pandemonium")
//    {
//       if(level == "pvp" and who != "player")
//       {
//          mirrorme = -1;
//       }
//       else
//       {
//          mirrorme = 1;
//       }
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             score[45 + i] = (100 + mirrorme) * (1 - eval("_root.ce" + i + ".panded"));
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             score[2 + i] = (100 - mirrorme) * (1 - eval("_root.c" + i + ".panded"));
//          }
//          i++;
//       }
//    }
//    if(targeting == "alphacreature")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          skillscore = 0;
//          if(eval("_root.ce" + i + ".ctrl") > 0 and onlyfoe == 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.ce" + i + ".skill") != "")
//             {
//                skillscore = 3;
//             }
//             if(eval("_root.ce" + i + ".skill") == "deja vu" or eval("_root.ce" + i + ".defnow") == 0 or eval("_root.ce" + i + ".skill") == "devour")
//             {
//                skillscore = 10;
//             }
//             score[45 + i] = (eval("_root.ce" + i + ".atknow") + skillscore - eval("_root.ce" + i + ".frozen") - eval("_root.ce" + i + ".delay")) / 10;
//             if(eval("_root.ce" + i + ".momentum") > 0 and skill == "momentum")
//             {
//                score[45 + i] = 0;
//             }
//             if(eval("_root.ce" + i + ".skill") == "hatch")
//             {
//                score[45 + i] = 0;
//             }
//             if(skill == "parallel universe" and eval("_root.ce" + i + ".passive") == "chimera")
//             {
//                score[45 + i] = 0;
//             }
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and onlyfriend == 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.c" + i + ".skill") != "")
//             {
//                skillscore = 3;
//             }
//             score[2 + i] = (eval("_root.c" + i + ".atknow") + skillscore - eval("_root.c" + i + ".frozen") - eval("_root.c" + i + ".delay")) / 10;
//             if(skill == "parallel universe" and eval("_root.c" + i + ".passive") == "chimera")
//             {
//                score[2 + i] = _root.monitor.nc / 5;
//             }
//          }
//          i++;
//       }
//    }
//    if(targeting == "skillcreature")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          skillscore = 0;
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.c" + i + ".skill") != "")
//             {
//                score[2 + i] = (eval("_root.c" + i + ".cost") + 1) * (- estimate);
//             }
//          }
//          i++;
//       }
//       i = 1;
//       while(i <= 23)
//       {
//          skillscore = 0;
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.ce" + i + ".skill") != "")
//             {
//                score[45 + i] = eval("_root.ce" + i + ".skillcost") * estimate;
//                if(eval("_root.ce" + i + ".costelement") == 10)
//                {
//                   score[45 + i] = eval("_root.ce" + i + ".skillcost") * estimate * 3;
//                }
//             }
//          }
//          i++;
//       }
//    }
//    if(targeting == "betacreature")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          skillscore = 0;
//          if(eval("_root.ce" + i + ".ctrl") > 0 and onlyfoe == 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.ce" + i + ".skill") != "")
//             {
//                skillscore = 3;
//             }
//             score[45 + i] = (7 + estimate - (eval("_root.ce" + i + ".defnow") + eval("_root.ce" + i + ".atknow") + eval("_root.ce" + i + ".cost") + skillscore)) / 5;
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and onlyfriend == 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.c" + i + ".skill") != "")
//             {
//                skillscore = 3;
//             }
//             score[2 + i] = (-8 + eval("_root.c" + i + ".defnow") + eval("_root.c" + i + ".atknow") + skillscore) / 100;
//          }
//          i++;
//       }
//    }
//    if(targeting == "defineatk")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          skillscore = 0;
//          if(eval("_root.ce" + i + ".ctrl") > 0 and onlyfoe == 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             score[45 + i] = estimate * (deftolerance - Math.abs(definevalue - eval("_root.ce" + i + ".atknow"))) / 10;
//             if(eval("_root.ce" + i + ".adrenaline") > 0 and skill == "adrenaline")
//             {
//                score[45 + i] = 0;
//             }
//             if(eval("_root.ce" + i + ".skill") == "mitosis" and skill == "mitosis")
//             {
//                score[45 + i] = 0;
//             }
//             if(eval("_root.ce" + i + ".passive") == "singularity" and skill == "antimatter")
//             {
//                score[45 + i] = 0;
//             }
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and onlyfriend == 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             score[2 + i] = (- estimate) * (deftolerance - Math.abs(definevalue - eval("_root.c" + i + ".atknow"))) / 10;
//          }
//          i++;
//       }
//    }
//    if(targeting == "definedef")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and onlyfoe == 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             score[45 + i] = estimate * (eval("_root.ce" + i + ".defnow") - definevalue) / definevalue;
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and onlyfriend == 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             score[2 + i] = (- estimate) * 0.1 * (eval("_root.c" + i + ".cost") + eval("_root.c" + i + ".atknow")) * ((eval("_root.c" + i + ".defnow") - definevalue - 1) / (Math.abs(eval("_root.c" + i + ".defnow") - definevalue - 1) + 0.001));
//          }
//          i++;
//       }
//    }
//    if(targeting == "fractal")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             costelement = eval("_root.ce" + i + ".costelement");
//             if(estimate > 0)
//             {
//                score[45 + i] = (_root.retqe(costelement) + 1) / (eval("_root.ce" + i + ".cost") + 0.2) / (_root.cardshande * _root.cardshande * 5 + 1);
//             }
//             else if(eval("_root.ce" + i + ".passive") == "obsession")
//             {
//                score[45 + i] = 1;
//             }
//             else
//             {
//                score[45 + i] = (eval("_root.ce" + i + ".cost") / (_root.retq(costelement) + 1) + 0.2) / (_root.cardshand * _root.cardshand * 5 + 1);
//             }
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             costelement = eval("_root.c" + i + ".costelement");
//             if(estimate > 0)
//             {
//                score[2 + i] = (_root.retqe(costelement) + 1) / (eval("_root.c" + i + ".cost") + 0.2) / (_root.cardshande * _root.cardshande * 5 + 1);
//             }
//             else
//             {
//                score[2 + i] = (eval("_root.c" + i + ".cost") / (_root.retq(costelement) + 1) + 0.2) / (_root.cardshand * _root.cardshand * 5 + 1);
//             }
//          }
//          i++;
//       }
//    }
//    if(targeting == "permanent")
//    {
//       i = 1;
//       while(i <= 19)
//       {
//          if(eval("_root.pe" + i + ".ctrl") > 0 and eval("_root.pe" + i + ".statu") != "invulnerable")
//          {
//             score[69 + i] = (eval("_root.pe" + i + ".cost") + 1) * estimate / (25 - _root.cardshande);
//          }
//          if(eval("_root.p" + i + ".ctrl") > 0 and eval("_root.p" + i + ".statu") != "invulnerable")
//          {
//             score[25 + i] = (eval("_root.p" + i + ".cost") + 1) * (- estimate) / 20;
//             if(eval("_root.p" + i + ".statu") == skill)
//             {
//                score[25 + i] = 0;
//             }
//             if(eval("_root.p" + i + ".passive") == "sundial" and skill != "steal")
//             {
//                score[25 + i] = _root.monitor.dmge * (- estimate) / 20;
//             }
//          }
//          i++;
//       }
//    }
//    if(targeting == "pillars")
//    {
//       i = 1;
//       while(i <= 19)
//       {
//          if(eval("_root.pe" + i + ".ctype") == "pillar" and eval("_root.pe" + i + ".ctrl") > 0 and eval("_root.pe" + i + ".statu") != "invulnerable")
//          {
//             score[69 + i] = (estimate + Math.random() / 10) / 2;
//          }
//          if(eval("_root.p" + i + ".ctype") == "pillar" and eval("_root.p" + i + ".ctrl") > 0 and eval("_root.p" + i + ".statu") != "invulnerable")
//          {
//             score[25 + i] = eval("_root.p" + i + ".ctrl") / (Math.abs(eval("_root.p" + i + ".ctrl") - estimate) + 0.5) * 10 * (- estimate);
//          }
//          i++;
//       }
//    }
//    if(targeting == "tears")
//    {
//       wassa = 0;
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".skill") == "nymph")
//          {
//             wassa++;
//          }
//          i++;
//       }
//       i = 1;
//       while(i <= 19)
//       {
//          if((Math.floor(eval("_root.pe" + i + ".card") / 100) != 27 or wassa == 0) and (Math.floor(eval("_root.pe" + i + ".card") / 100) != 7 or wassa == 0) and eval("_root.pe" + i + ".ctype") == "pillar" and eval("_root.pe" + i + ".ctrl") > 0 and eval("_root.pe" + i + ".statu") != "invulnerable")
//          {
//             score[69 + i] = (estimate + Math.random() / 10) / 2;
//          }
//          i++;
//       }
//    }
//    if(targeting == "weapon")
//    {
//       i = 1;
//       while(i <= 19)
//       {
//          if(eval("_root.pe" + i + ".ctype") == "weapon" and eval("_root.pe" + i + ".ctrl") > 0 and eval("_root.pe" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.pe" + i + ".skillcost") != "")
//             {
//                skillscore = retqe(eval("_root.pe" + i + ".skillelement")) / (1 + eval("_root.pe" + i + ".skillcost"));
//             }
//             else
//             {
//                skillscore = 1;
//             }
//             score[69 + i] = eval("_root.pe" + i + ".atk") + skillscore;
//          }
//          if(eval("_root.p" + i + ".ctype") == "weapon" and eval("_root.p" + i + ".ctrl") > 0 and eval("_root.p" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.p" + i + ".skillcost") != "")
//             {
//                skillscore = retqe(eval("_root.p" + i + ".skillelement")) / (1 + eval("_root.p" + i + ".skillcost"));
//             }
//             else
//             {
//                skillscore = 1;
//             }
//             score[25 + i] = eval("_root.p" + i + ".atk") + skillscore;
//          }
//          i++;
//       }
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.c" + i + ".ctype") == "weapon" and eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.c" + i + ".skillcost") != "")
//             {
//                skillscore = retqe(eval("_root.c" + i + ".skillelement")) / (1 + eval("_root.c" + i + ".skillcost"));
//             }
//             else
//             {
//                skillscore = 1;
//             }
//             score[2 + i] = eval("_root.c" + i + ".atk") + skillscore;
//          }
//          i++;
//       }
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctype") == "weapon" and eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable")
//          {
//             if(eval("_root.ce" + i + ".skillcost") != "")
//             {
//                skillscore = retqe(eval("_root.ce" + i + ".skillelement")) / (1 + eval("_root.ce" + i + ".skillcost"));
//             }
//             else
//             {
//                skillscore = 1;
//             }
//             score[45 + i] = eval("_root.ce" + i + ".atk") + skillscore;
//          }
//          i++;
//       }
//    }
//    if(targeting == "smaller")
//    {
//       i = 1;
//       while(i <= 23)
//       {
//          if(eval("_root.ce" + i + ".ctrl") > 0 and eval("_root.ce" + i + ".statu") != "invulnerable" and smaller > eval("_root.ce" + i + ".defnow"))
//          {
//             if(eval("_root.ce" + i + ".poison") > 0 or eval("_root.ce" + i + ".atknow") + eval("_root.ce" + i + ".defnow") <= 2 and eval("_root.ce" + i + ".skill") == "")
//             {
//                score[45 + i] = (- estimate) / 10;
//             }
//             if(_root.monitor.nc > 20)
//             {
//                score[45 + i] = (- estimate) / eval("_root.ce" + i + ".atknow");
//             }
//          }
//          if(eval("_root.c" + i + ".ctrl") > 0 and eval("_root.c" + i + ".statu") != "invulnerable" and smaller > eval("_root.c" + i + ".defnow"))
//          {
//             if(eval("_root.c" + i + ".skill") != "")
//             {
//                skillscore = 3;
//             }
//             score[2 + i] = (skillscore + 1 + eval("_root.c" + i + ".atknow") / (- estimate)) / 10;
//          }
//          i++;
//       }
//    }

    public TargetType GetTargetType(string skillName)
    {
        switch (skillName)
        {
            case "lightning":
            case "purify":
            case "icebolt":
            case "firebolt":
            case "holylight":
            case "drainlife":
            case "shockwave":
                return TargetType.All;
            case "freeze":
            case "congeal":
            case "paralleluniverse":
            case "momentum":
            case "blessing":
            case "chaos":
            case "chaospower":
            case "armor":
            case "heavyarmor":
            case "reversetime":
            case "mutation":
            case "improve":
            case "gravitypull":
            case "adrenaline":
            case "liquidshadow":
            case "aflatoxin":
            case "rage":
            case "berserk":
            case "antimatter":
            case "immortality":
            case "petrify":
            case "fractal":
            case "readiness":
            case "nightmare":
            case "mitosis":
            case "acceleration":
            case "overdrive":
                return TargetType.Creature;
            case "immolate": 
            case "cremation":
                return TargetType.MyCreature;
            case "butterfly":
                return TargetType.CreatureLowAtk;
            case "destroy":
            case "steal":
            case "enchant":
                return TargetType.Permanent;
            case "earthquake":
            case "tsunami":
            case "nymph":
                return TargetType.Pillar;
            case "wisdom":
                return TargetType.Immortals;
            default:
                return TargetType.All;
        }
    }

    public AiTargetType GetAITargetType(string skillName)
    {
        var prge = DuelManager.Instance.enemy.HealthManager.GetCurrentHealth() / (Math.Abs(DuelManager.Instance.GetPossibleDamage(true)) + 1);
        var aiTarget = new AiTargetType(false, false, false, TargetType.Creature, 0,0,0);
        switch (skillName)
        {
            case "reversetime" when DuelManager.Instance.player.DeckManager.GetDeckCount() < 5:
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 8;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "infect" or "infection" or "reversetime" or "sniper" or "aflatoxin":
                aiTarget.Targeting = TargetType.Creature;
                aiTarget.Estimate = -1;
                break;
            case "petrify" when DuelManager.Instance.GetCardCount(new List<string>{"74h", "561"}) > 0:
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "gravitypull" when prge < 5 && DuelManager.Instance.player.playerCreatureField.GetCreatureWithGravity().Equals(default):
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 5;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "paradox":
                aiTarget.Targeting = TargetType.CreatureHighAtk;
                aiTarget.Estimate = -1;
                break;
            case "lightning" or "shockwave":
                aiTarget.Estimate = -5;
                aiTarget.Targeting = TargetType.All;
                break;
            case "purify":
                aiTarget.Estimate = 2;
                aiTarget.Targeting = TargetType.All;
                break;
            case "firebolt":
                aiTarget.Estimate = -3;
                aiTarget.Targeting = TargetType.All;
                break;
            case "icebolt":
                aiTarget.Estimate = -2;
                aiTarget.Targeting = TargetType.All;
                break;
            case "drainlife":
                aiTarget.Estimate = -2;
                aiTarget.Targeting = TargetType.All;
                break;
            case "guard" or "freeze" or "congeal" or "petrify":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "paralleluniverse":
                aiTarget.Estimate = 0;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "holylight":
                aiTarget.Estimate = 10;
                aiTarget.Targeting = TargetType.All;
                break;
            case "destroy" or "steal":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Permanent;
                break;
            case "accretion":
                aiTarget.Estimate = -10;
                aiTarget.Targeting = TargetType.Permanent;
                break;
            case "enchant":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Permanent;
                break;
            case "momentum":
                aiTarget.Estimate = 1;
                aiTarget.OnlyFriend = true;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "blessing" or "armor" or "heavyarmor" or "immortality" or "chaospower":
                aiTarget.Estimate = 3;
                aiTarget.OnlyFriend = true;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "chaos":
                aiTarget.Estimate = -1;
                aiTarget.OnlyFoe = true;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "adrenaline":
                aiTarget.Estimate = 1;
                aiTarget.OnlyFriend = true;
                aiTarget.DefineValue = 3;
                aiTarget.DefTolerance = 7;
                aiTarget.Targeting = TargetType.DefineAtk;
                break;
            case "antimatter":
                aiTarget.Estimate = -1;
                aiTarget.DefineValue = 25;
                aiTarget.DefTolerance = 25;
                aiTarget.Targeting = TargetType.DefineAtk;
                break;
            case "liquidshadow":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 3;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "mitosis":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 25;
                aiTarget.DefTolerance = 25;
                aiTarget.Targeting = TargetType.DefineAtk;
                break;
            case "butterfly":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 3;
                aiTarget.Targeting = TargetType.CreatureLowAtk;
                break;
            case "readiness":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "catapult":
                aiTarget.Estimate = 20;
                aiTarget.Targeting = TargetType.Trebuchet;
                break;
            case "rage":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 5;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "berserk":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 6;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "acceleration" or "overdrive":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 3;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "heal":
                aiTarget.Estimate = 5;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "devour":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Smaller;
                break;
            case "mutation" or "improve":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "immolate" or "cremation":
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "lobotomize" or "liquidshadow" when Random.Range(0.0f, 1.0f) > 0.5f:
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.SkillCreature;
                break;
            case "tsunami" or "earthquake":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Pillar;
                break;
            case "nymph":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Tears;
                break;
            case "fractal":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Fractal;
                break;
            case "nightmare":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Fractal;
                break;
            case "pandemonium":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Pandemonium;
                break;
            case "web":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "endow":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Weapon;
                break;
            case "wisdom":
            {
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Immortals;
                if (DuelManager.Instance.enemy.playerPassiveManager.GetShield().Item2.skill != "reflect")
                {
                    aiTarget.OnlyFriend = true;
                }
                if (DuelManager.Instance.player.playerPassiveManager.GetShield().Item2.skill == "reflect")
                {
                    aiTarget.OnlyFoe = true;
                }

                break;
            }
        }

        return aiTarget;
    }
}
public enum TargetType
{
    All,
    Creature,
    MyCreature,
    CreatureLowAtk,
    Permanent,
    Pillar,
    Immortals,
    BetaCreature,
    DefineDef,
    CreatureHighAtk,
    AlphaCreature,
    DefineAtk,
    Trebuchet,
    Smaller,
    SkillCreature,
    Weapon,
    Pandemonium,
    Fractal,
    Tears
}