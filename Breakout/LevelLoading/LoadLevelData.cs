using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace Breakout.LevelLoading {
    public class LoadLevelData {

        private string filename;

        public int mapHeight = 0;
        public int mapWidth  = 0;

        public List<string> map = new List<string> {};
        public Dictionary<string, string> meta   = new Dictionary<string, string>();
        public Dictionary<string, string> legend = new Dictionary<string, string>();

        public LoadLevelData(string filename, string prefix = "") {
            this.filename = filename;

            // Looked in the wrong place when testing
            string[] lines = System.IO.File.ReadAllLines(
                Path.Combine(prefix + "Assets", "Levels", filename)
            );
            ValidateFile(lines);
            GetMap(lines);
            GetMeta(lines);
            GetLegend(lines);
        }

        /// <summary> Throws an exception with invalid or empty files.
        /// Regex assistance at https://regexr.com </summary>
        private void ValidateFile(string[] lines) {
            string contents = "";
            foreach (string line in lines)
                contents += line;

            Regex Map = new Regex(@"Map:.*Map/");
            if (!Map.IsMatch(contents))
                throw new FileLoadException(
                    filename + " does not contain valid Map."
                );
            Regex Meta = new Regex(@"Meta:[[a-zA-Z0-9]+: .+]*Meta/");
            if (!Meta.IsMatch(contents))
                throw new FileLoadException(
                    filename + " does not contain valid Meta."
                );
            Regex Legend = new Regex(@"Legend:(.\) [a-zA-Z\-]+\.png)*Legend/");
            if (!Legend.IsMatch(contents))
                throw new FileLoadException(
                    filename + " does not contain valid Legend."
                );
        }

        /// <summmary> Gets the map from the file,
        /// and ensures that it has appropriate dimensions. </summary>
        private void GetMap(string[] lines) {
            bool field = false;
            int  width = 0, height = 0;
            foreach (string line in lines) {
                if (line == "Map/")
                    break;
                if (field) {
                    if (line.Length == width || width == 0) {
                        map.Add(line);
                        width = line.Length;
                        height++;
                    } else {
                        throw new FileLoadException(
                            filename + " has unequal row widths."
                        );
                    }
                }
                if (line == "Map:")
                    field = true;
            }
            if (width * 2 != height)
                throw new FileLoadException(
                    filename + " does not have 1:2 dimensions."
                );
            else
                mapHeight = height;
                mapWidth  = width;
        }

        /// <summmary> Gets the meta field from the file. </summary>
        private void GetMeta(string[] lines) {
            bool field = false;
            foreach (string line in lines) {
                if (line == "Meta/")
                    break;
                if (field) {
                    string[] strings = line.Split(": ");
                    meta[strings[0]] = strings[1];
                }
                if (line == "Meta:") 
                    field = true;
            }
        }
        
        /// <summmary> Gets the legend field from the file. </summary>
        private void GetLegend(string[] lines) {
            bool field = false;
            foreach (string line in lines) {
                if (line == "Legend/")
                    break;
                if (field) {
                    string[] strings   = line.Split(") ");
                    legend[strings[0]] = strings[1];
                }
                if (line == "Legend:")
                    field = true;
            }
        }
    }
}