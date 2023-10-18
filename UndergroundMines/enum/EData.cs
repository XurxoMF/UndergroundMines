using System.Collections.Generic;

namespace UndergroundMines
{
    public class ModInfo
    {
        public const string MOD_NAME = "UndergroundMines";
    }

    public class ModStaticConfig
    {
        public static double DefaultHeight = 0.48;

        /* ORE-QUALITY LIST
        > nativecopper -------- "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "bountiful-nativecopper",
        > cassiterite --------- "poor-cassiterite", "medium-cassiterite", "rich-cassiterite", "bountiful-cassiterite",
        > chromite ------------ "poor-chromite", "medium-chromite", "rich-chromite", "bountiful-chromite",
        > ilmenite ------------ "poor-ilmenite", "medium-ilmenite", "rich-ilmenite", "bountiful-ilmenite",
        > limonite ------------ "poor-limonite", "medium-limonite", "rich-limonite", "bountiful-limonite",
        > sphalerite ---------- "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "bountiful-sphalerite",
        > bismuthinite -------- "poor-bismuthinite", "medium-bismuthinite", "rich-bismuthinite", "bountiful-bismuthinite",
        > magnetite ----------- "poor-magnetite", "medium-magnetite", "rich-magnetite", "bountiful-magnetite",
        > pentlandite --------- "poor-pentlandite", "medium-pentlandite", "rich-pentlandite", "bountiful-pentlandite",
        > uranium ------------- "poor-uranium", "medium-uranium", "rich-uranium", "bountiful-uranium",
        > rhodochrosite ------- "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "bountiful-rhodochrosite",
        > hematite ------------ "poor-hematite", "medium-hematite", "rich-hematite", "bountiful-hematite",
        > malachite ----------- "poor-malachite", "medium-malachite", "rich-malachite", "bountiful-malachite",
        > galena -------------- "poor-galena", "medium-galena", "rich-galena", "bountiful-galena",
        > corundum ------------ "corundum",
        > graphite ------------ "graphite",
        > olivine ------------- "olivine",
        > cinnabar ------------ "cinnabar",
        > alum ---------------- "alum",
        > quartz -------------- "quartz",
        > anthracite ---------- "anthracite",
        > borax --------------- "borax",
        > lignite ------------- "lignite",
        > bituminouscoal ------ "bituminouscoal",
        > sulfur -------------- "sulfur",
        > fluorite ------------ "fluorite",
        > kernite ------------- "kernite",
        > phosphorite --------- "phosphorite",
        > lapislazuli --------- "lapislazuli",
        > galena_nativesilver - "poor-galena_nativesilver", "medium-galena_nativesilver", "rich-galena_nativesilver", "bountiful-galena_nativesilver",
        > quartz_nativegold --- "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold",
        > quartz_nativesilver - "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver",
        */

        public static readonly Dictionary<string, string[]> RockTypeAndOres = new(){
            {"andesite",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-cassiterite", "medium-cassiterite", "poor-chromite", "medium-chromite", "poor-ilmenite", "medium-ilmenite", "poor-sphalerite", "medium-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-bismuthinite", "medium-bismuthinite", "rich-bismuthinite", "poor-magnetite", "medium-magnetite", "rich-magnetite", "poor-pentlandite", "medium-pentlandite", "poor-uranium", "medium-uranium", "rich-uranium", "cinnabar", "quartz"
                }},
            {"chalk",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-magnetite", "medium-magnetite", "poor-uranium", "medium-uranium", "rich-uranium", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "phosphorite"
                }},
            {"chert",
                new[]{
                     "poor-nativecopper", "medium-nativecopper", "poor-limonite", "medium-limonite", "rich-limonite", "bountiful-limonite", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "rich-galena", "bountiful-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-galena_nativesilver", "medium-galena_nativesilver", "poor-uranium", "medium-uranium", "rich-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "kernite", "phosphorite"
                }},
            {"conglomerate",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-galena_nativesilver", "medium-galena_nativesilver", "poor-magnetite", "medium-magnetite", "poor-uranium", "medium-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "kernite", "phosphorite"
                }},
            {"limestone",
                new[]{
                    "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-galena_nativesilver", "medium-galena_nativesilver", "poor-hematite", "medium-hematite", "rich-hematite", "poor-malachite", "medium-malachite", "rich-malachite", "bountiful-malachite", "poor-uranium", "medium-uranium", "rich-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "kernite", "phosphorite"
                }},
            {"claystone",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "rich-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-galena_nativesilver", "medium-galena_nativesilver", "poor-magnetite", "medium-magnetite", "poor-uranium", "medium-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "kernite", "phosphorite"
                }},
            {"granite",
                new[]{
                     "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-cassiterite", "medium-cassiterite", "rich-cassiterite", "bountiful-cassiterite", "poor-chromite", "medium-chromite", "poor-ilmenite", "medium-ilmenite", "poor-sphalerite", "medium-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-bismuthinite", "medium-bismuthinite", "rich-bismuthinite", "poor-hematite", "medium-hematite", "rich-hematite", "bountiful-hematite", "poor-pentlandite", "medium-pentlandite", "poor-uranium", "medium-uranium", "rich-uranium", "cinnabar", "quartz"
                }},
            {"sandstone",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "rich-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-galena_nativesilver", "medium-galena_nativesilver", "poor-hematite", "medium-hematite", "rich-hematite", "poor-uranium", "medium-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "kernite", "phosphorite"
                }},
            {"shale",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "poor-limonite", "medium-limonite", "rich-limonite", "bountiful-limonite", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-galena", "medium-galena", "rich-galena", "bountiful-galena", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-galena_nativesilver", "medium-galena_nativesilver", "poor-uranium", "medium-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "alum", "anthracite", "borax", "lignite", "bituminouscoal", "quartz", "sulfur", "fluorite", "kernite", "phosphorite"
                }},
            {"basalt",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "bountiful-nativecopper", "poor-limonite", "medium-limonite", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-cassiterite", "medium-cassiterite", "poor-chromite", "medium-chromite", "rich-chromite", "poor-ilmenite", "medium-ilmenite", "rich-ilmenite", "poor-sphalerite", "medium-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-bismuthinite", "medium-bismuthinite", "rich-bismuthinite", "bountiful-bismuthinite", "poor-pentlandite", "medium-pentlandite", "poor-uranium", "medium-uranium", "cinnabar", "quartz"
                }},
            {"peridotite",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-cassiterite", "medium-cassiterite", "poor-chromite", "medium-chromite", "rich-chromite", "poor-ilmenite", "medium-ilmenite", "rich-ilmenite", "poor-sphalerite", "medium-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-bismuthinite", "medium-bismuthinite", "rich-bismuthinite", "poor-hematite", "medium-hematite", "rich-hematite", "bountiful-hematite", "poor-pentlandite", "medium-pentlandite", "rich-pentlandite", "bountiful-pentlandite", "poor-uranium", "medium-uranium", "corundum", "cinnabar", "quartz", "olivine"
                }},
            {"phyllite",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-cassiterite", "medium-cassiterite", "poor-ilmenite", "medium-ilmenite", "rich-ilmenite", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "bountiful-sphalerite",
                     "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-bismuthinite", "medium-bismuthinite", "poor-hematite", "medium-hematite", "poor-uranium", "medium-uranium", "rich-uranium", "bountiful-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "bountiful-rhodochrosite", "corundum", "cinnabar", "quartz", "fluorite", "graphite"
                }},
            {"slate",
                new[]{
                    "poor-nativecopper", "medium-nativecopper", "rich-nativecopper", "poor-quartz_nativegold", "medium-quartz_nativegold", "rich-quartz_nativegold", "bountiful-quartz_nativegold", "poor-cassiterite", "medium-cassiterite", "poor-ilmenite", "medium-ilmenite", "rich-ilmenite", "poor-sphalerite", "medium-sphalerite", "rich-sphalerite", "bountiful-sphalerite", "poor-quartz_nativesilver", "medium-quartz_nativesilver", "rich-quartz_nativesilver", "bountiful-quartz_nativesilver", "poor-bismuthinite", "medium-bismuthinite", "poor-magnetite", "medium-magnetite", "rich-magnetite", "bountiful-magnetite", "poor-uranium", "medium-uranium", "rich-uranium", "bountiful-uranium", "poor-rhodochrosite", "medium-rhodochrosite", "rich-rhodochrosite", "bountiful-rhodochrosite", "corundum", "cinnabar", "quartz", "fluorite", "graphite"
                }},
            {"obsidian",
                null},
            {"kimberlite",
                new[]{
                    "poor-chromite", "medium-chromite", "rich-chromite", "bountiful-chromite", "poor-ilmenite", "medium-ilmenite", "rich-ilmenite", "bountiful-ilmenite", "poor-uranium", "medium-uranium", "rich-uranium", "bountiful-uranium"
                }},
            {"scoria",
                null},
            {"tuff",
                null},
            {"bauxite",
                null},
            {"halite",
                null},
            {"suevite",
                null},
            {"whitemarble",
                new[]{
                    "poor-malachite", "medium-malachite", "rich-malachite", "lapislazuli", "graphite", "corundum"
                }},
            {"redmarble",
                new[]{
                    "poor-malachite", "medium-malachite", "rich-malachite", "lapislazuli", "graphite", "corundum"
                }},
            {"greenmarble",
                new[]{
                    "poor-malachite", "medium-malachite", "rich-malachite", "lapislazuli", "graphite", "corundum"
                }},
        };
    }

    public enum ESchematicType
    {
        Null, // No structure, used to not generate anything
        UndergroundCross, // default exit all sides
        UndergroundEnd, // default exit only north
        UndergroundMine, // default exit north-south
        UndergroundAngle // default exit north-east
    }

    public enum ERotation : int
    {
        North = 0,
        East = 90,
        South = 180,
        West = 270
    }
}