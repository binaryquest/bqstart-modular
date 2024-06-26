﻿using BinaryQuest.Framework.ModularCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Model
{
    public class Bootdata
    {        
        public Bootdata(AppConfigOptions configOptions)
        {            
            this.SecurityRulesDictionary = new Dictionary<string, Dictionary<string, SecurityRule>>();
            this.MetaDataValues = new Dictionary<Type, ModelMetadata>();
            if (configOptions.SecurityRulesProvider != null)
            {
                ParseRules(configOptions.SecurityRulesProvider);
            }
        }

        private void ParseRules(ISecurityRulesProvider securityRulesProvider)
        {
            var rules = securityRulesProvider.GetRules();
            foreach (var rule in rules)
            {
                if (rule.ModelType == null || (rule.RoleName == null && rule.RoleNames == null))
                    continue;

                if (!SecurityRulesDictionary.ContainsKey(rule.ModelType))
                {
                    SecurityRulesDictionary.Add(rule.ModelType, []);
                }

                Dictionary<string, SecurityRule> perModelDic = SecurityRulesDictionary[rule.ModelType];

                //check if Role name contains comma then multiple rule
                if (rule.RoleNames != null)
                {
                    var roles = rule.RoleNames.Split(',');
                    foreach (var role in roles)
                    {
                        if (!perModelDic.TryGetValue(role, out SecurityRule? value))
                        {
                            perModelDic.Add(role, rule);
                        }
                        else
                        {
                            //if already found then union merge with existing ones
                            var existingRule = value;
                            existingRule.AllowSelect = rule.AllowSelect;
                            existingRule.AllowInsert = rule.AllowInsert;
                            existingRule.AllowUpdate = rule.AllowUpdate;
                            existingRule.AllowDelete = rule.AllowDelete;
                        }
                    }
                }
                else if (rule.RoleName != null)
                {
                    if (!perModelDic.TryGetValue(rule.RoleName, out SecurityRule? value))
                    {
                        perModelDic.Add(rule.RoleName, rule);
                    }
                    else
                    {
                        //if already found then union merge with existing ones
                        var existingRule = value;
                        existingRule.AllowSelect = rule.AllowSelect;
                        existingRule.AllowInsert = rule.AllowInsert;
                        existingRule.AllowUpdate = rule.AllowUpdate;
                        existingRule.AllowDelete = rule.AllowDelete;
                    }
                }                
            }
        }

        public Dictionary<Type, ModelMetadata> MetaDataValues { get; set; }
        public Dictionary<string, Dictionary<string, SecurityRule>> SecurityRulesDictionary { get; set; }
    }
}
