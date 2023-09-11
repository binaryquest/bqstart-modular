using BinaryQuest.Framework.ModularCore.Interface;
using BinaryQuest.Framework.ModularCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Security
{
    public class FileBasedSecurityRulesProvider : ISecurityRulesProvider
    {
        private readonly string path;

        public FileBasedSecurityRulesProvider(string path)
        {
            this.path = path;
        }

        public List<SecurityRule> GetRules()
        {
            List<SecurityRule> ret = new();
            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            string[] ruleFiles = Directory.GetFiles(fullPath, "*.config");
            if (ruleFiles.Length > 0)
            {
                for (int i = 0; i < ruleFiles.Length; i++)
                {
                    var data = File.ReadAllText(ruleFiles[i]);
                    var modViews = SecurityRules.Deserialize(data);
                    if (modViews!=null)
                    foreach (var rule in modViews.SecurityRule)
                    {
                        ret.Add(rule);
                    }
                }

            }
            return ret;
        }
    }
}
