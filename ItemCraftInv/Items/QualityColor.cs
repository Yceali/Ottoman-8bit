using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Quality { Common, Uncommon, Rare, Epic, Legendary }

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        {Quality.Common, "#d6d6d6" },
        {Quality.Uncommon, "#00ff00" },
        {Quality.Rare, "#0000ff" },
        {Quality.Epic, "#800080" },
        {Quality.Legendary, "#ffa500" }
    };

    public static Dictionary<Quality, string> MyColors { get => colors;}
}
