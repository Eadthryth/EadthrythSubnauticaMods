using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Options;
using SMLHelper.V2.Utility;
using UnityEngine.UI;

namespace FabricatorColor
{
    internal static class Main
    {
        public static void Patch()
        {
            Config.Load();
            OptionsPanelHandler.RegisterModOptions(new Options());

            HarmonyInstance harmonyInstance = HarmonyInstance.Create("com.eadthryth.fabricatorcolor.mod");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

        }

        public static class GUIFormatter
        {
            public static void PaintNodeColor(uGUI_CraftNode node)
            {
                if (node.action == TreeAction.Expand)
                {
                    Color color = new Color(Config.SliderValueR / 255, Config.SliderValueG / 255, Config.SliderValueB / 255, Config.alpha / 255);
                    Main.GUIFormatter.SetIconColors(node.icon, frontColor, color);
                }
                else if (node.action == TreeAction.Craft)
                {
                    Color color = new Color(Config.SliderValueR1 / 255, Config.SliderValueG1 / 255, Config.SliderValueB1 / 255, Config.alpha / 255);
                    Main.GUIFormatter.SetIconColors(node.icon, frontColor, color);
                }
            }

            private static void SetIconColors(uGUI_ItemIcon icon, Color frontColor, Color backColor)
            {
                if (!icon)
                {
                    return;
                }
                icon.SetForegroundColors(frontColor, frontColor, frontColor);
                icon.SetBackgroundColors(backColor, backColor, backColor);
            }

            private static readonly Color backColor = new Color(1f, 0f, 0f);
            private static readonly Color frontColor = new Color(1f, 1f, 1f);
        }

        public static bool Active { get; private set; }

    }

    [HarmonyPatch(typeof(uGUI_CraftNode))]
    [HarmonyPatch("CreateIcon")]
    public static class uGUI_CraftNode_CreateIcon
    {
        [HarmonyPostfix]
        private static void Postfix(uGUI_CraftNode __instance)
        {
            
            Main.GUIFormatter.PaintNodeColor(__instance);
            Console.WriteLine("***PostFix Active***");
        }
    }

    public static class Config
    {
        public static float SliderValueR;
        public static float SliderValueG;
        public static float SliderValueB;
        public static float SliderValueR1;
        public static float SliderValueG1;
        public static float SliderValueB1;
        public static float alpha;

        public static void Load()
        {
            SliderValueR = PlayerPrefs.GetFloat("Red", 0f);
            SliderValueG = PlayerPrefs.GetFloat("Green", 255f);
            SliderValueB = PlayerPrefs.GetFloat("Blue", 65f);
            SliderValueR1 = PlayerPrefs.GetFloat("Red1", 255f);
            SliderValueG1 = PlayerPrefs.GetFloat("Green1", 255f);
            SliderValueB1 = PlayerPrefs.GetFloat("Blue1", 255f);
            alpha = PlayerPrefs.GetFloat("Alpha", 255f);
        }
    }

    public class Options : ModOptions
    {
        public Options() : base("Fabricator Item Color")
        {
            SliderChanged += Options_SliderChangedR;
            SliderChanged += Options_SliderChangedG;
            SliderChanged += Options_SliderChangedB;
            SliderChanged += Options_SliderChangedR1;
            SliderChanged += Options_SliderChangedG1;
            SliderChanged += Options_SliderChangedB1;
            SliderChanged += Options_SliderChangeda;
        }

        public void Options_SliderChangedR(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Red") return;
            Config.SliderValueR = e.Value;
            PlayerPrefs.SetFloat("RedSlider", e.Value);
        }
        public void Options_SliderChangedG(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Green") return;
            Config.SliderValueG = e.Value;
            PlayerPrefs.SetFloat("GreenSlider", e.Value);
        }
        public void Options_SliderChangedB(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Blue") return;
            Config.SliderValueB = e.Value;
            PlayerPrefs.SetFloat("BlueSlider", e.Value);
        }
        public void Options_SliderChangedR1(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Red1") return;
            Config.SliderValueR1 = e.Value;
            PlayerPrefs.SetFloat("RedSlider1", e.Value);
        }
        public void Options_SliderChangedG1(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Green1") return;
            Config.SliderValueG1 = e.Value;
            PlayerPrefs.SetFloat("GreenSlider1", e.Value);
        }
        public void Options_SliderChangedB1(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Blue1") return;
            Config.SliderValueB1 = e.Value;
            PlayerPrefs.SetFloat("BlueSlider1", e.Value);
        }
        public void Options_SliderChangeda(object sender, SliderChangedEventArgs e)
        {
            if (e.Id != "Alpha") return;
            Config.alpha = e.Value;
            PlayerPrefs.SetFloat("AlphaSlider", e.Value);
        }

        public override void BuildModOptions()
        {
            AddSliderOption("Red", "Red", 0, 255, Config.SliderValueR, 0f);
            AddSliderOption("Green", "Green", 0, 255, Config.SliderValueG, 0f);
            AddSliderOption("Blue", "Blue", 0, 255, Config.SliderValueB, 0f);
            AddSliderOption("Red1", "Item Red", 0, 255, Config.SliderValueR1, 0f);
            AddSliderOption("Green1", "Item Green", 0, 255, Config.SliderValueG1, 0f);
            AddSliderOption("Blue1", "Item Blue", 0, 255, Config.SliderValueB1, 0f);
            AddSliderOption("Alpha", "Alpha", 0, 255, Config.alpha, 0f);
        }
    }

}
