using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SmokeModGTAIV.Config
{
    internal class IniParser
    {
        private readonly Dictionary<string, string> _values =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;

            foreach (string line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed[0] == ';' || trimmed[0] == '[') continue;

                int idx = trimmed.IndexOf('=');
                if (idx < 1) continue;

                string key   = trimmed.Substring(0, idx).Trim();
                string value = trimmed.Substring(idx + 1).Trim();
                _values[key] = value;
            }
        }

        public string Get(string key, string defaultValue = "")
            => _values.TryGetValue(key, out string val) ? val : defaultValue;

        public int GetInt(string key, int defaultValue = 0)
            => int.TryParse(Get(key), out int result) ? result : defaultValue;

        public bool GetBool(string key, bool defaultValue = false)
        {
            string val = Get(key).ToLowerInvariant();
            if (val == "true"  || val == "1" || val == "yes") return true;
            if (val == "false" || val == "0" || val == "no")  return false;
            return defaultValue;
        }

        public Keys GetKey(string key, Keys defaultValue = Keys.None)
        {
            string val = Get(key);
            if (string.IsNullOrEmpty(val)) return defaultValue;
            return Enum.TryParse(val, true, out Keys result) ? result : defaultValue;
        }
    }
}
