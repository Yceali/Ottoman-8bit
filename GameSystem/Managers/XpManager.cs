using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class XpManager
{
    public static int CalculateXp(Enemy e)
    {
        int baseXP = (Player.m_instance.MyLevel * 5) + 45;
        int greyLevel = Player.m_instance.MyLevel - 10;
        int totalXP = 0;

        if (e.MyLevel >= Player.m_instance.MyLevel)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (e.MyLevel - Player.m_instance.MyLevel)));
        }
        else if (e.MyLevel > greyLevel)
        {
            totalXP = (baseXP) * (1 - (Player.m_instance.MyLevel - e.MyLevel) / ZeroDifference());
        }
        return totalXP;
    }

    public static int CalculateXp(Quest e)
    {
        if (Player.m_instance.MyLevel <= e.MyLevel + 5)
        {
            return e.MyXp;
        }
        if (Player.m_instance.MyLevel == e.MyLevel + 6)
        {
            return (int)(e.MyXp * 0.8 / 5) * 5;
        }
        if (Player.m_instance.MyLevel == e.MyLevel + 7)
        {
            return (int)(e.MyXp * 0.6 / 5) * 5;
        }
        if (Player.m_instance.MyLevel == e.MyLevel + 8)
        {
            return (int)(e.MyXp * 0.4 / 5) * 5;
        }
        if (Player.m_instance.MyLevel == e.MyLevel + 9)
        {
            return (int)(e.MyXp * 0.2 / 5) * 5;
        }
        if (Player.m_instance.MyLevel >= e.MyLevel + 10)
        {
            return (int)(e.MyXp * 0.1 / 5) * 5;
        }
        return 0;
    }

    private static int ZeroDifference()
    {
        if (Player.m_instance.MyLevel <= 7)
        {
            return 5;
        }
        if (Player.m_instance.MyLevel >= 8 && Player.m_instance.MyLevel <=9 )
        {
            return 6;
        }
        if (Player.m_instance.MyLevel >= 10 && Player.m_instance.MyLevel <= 11)
        {
            return 7;
        }
        if (Player.m_instance.MyLevel >= 12 && Player.m_instance.MyLevel <= 15)
        {
            return 8;
        }
        if (Player.m_instance.MyLevel >= 16 && Player.m_instance.MyLevel <= 19)
        {
            return 9;
        }
        if (Player.m_instance.MyLevel >= 20 && Player.m_instance.MyLevel <= 29)
        {
            return 11;
        }
        if (Player.m_instance.MyLevel >= 30 && Player.m_instance.MyLevel <= 39)
        {
            return 12;
        }
        if (Player.m_instance.MyLevel >= 40 && Player.m_instance.MyLevel <= 44)
        {
            return 13;
        }
        if (Player.m_instance.MyLevel >= 45 && Player.m_instance.MyLevel <= 49)
        {
            return 14;
        }
        if (Player.m_instance.MyLevel >= 50 && Player.m_instance.MyLevel <= 54)
        {
            return 15;
        }
        if (Player.m_instance.MyLevel >= 55 && Player.m_instance.MyLevel <= 59)
        {
            return 16;
        }

        return 17;
    }

    public static int CalculateGraylevel()
    {
        if (Player.m_instance.MyLevel <= 5)
        {
            return 0;
        }
        else if (Player.m_instance.MyLevel >= 6 && Player.m_instance.MyLevel <= 49)
        {
            return Player.m_instance.MyLevel - (Player.m_instance.MyLevel / 10) - 5;
        }
        else if (Player.m_instance.MyLevel == 50)
        {
            return Player.m_instance.MyLevel - 10;
        }
        else if (Player.m_instance.MyLevel >= 51 && Player.m_instance.MyLevel <= 59)
        {
            return Player.m_instance.MyLevel - (Player.m_instance.MyLevel / 5) - 1;
        }
        return Player.m_instance.MyLevel - 9;
    }

}

