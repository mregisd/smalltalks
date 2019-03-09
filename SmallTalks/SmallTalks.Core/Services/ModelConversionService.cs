using SmallTalks.Core.Models;
using SmallTalks.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SmallTalks.Core.Services
{
    public class ModelConversionService : IConversionService
    {
        public SmallTalksIntent ToIntent(Rule rule)
        {
            var stIntent = new SmallTalksIntent
            {
                Name = rule.Name
            };

            var patterns = rule.Patterns
                .Select(p => Regex.IsMatch(p, "^\\w") ? $"\\b{p}" : p)
                .Select(p => Regex.IsMatch(p, "\\w$") ? $"{p}\\b" : p)
                //.Select(p => TypeDetector(rule.Position, p))
                .ToList();
            var patterns2 = PositionDetector(rule.Position, patterns);

            var regexPattern = string.Join('|', patterns2);

            stIntent.Regex = new Regex(regexPattern, Configuration.ST_REGEX_OPTIONS);
            stIntent.Priority = rule.Priority;

            return stIntent;
        }

        public SmallTalksDectectorData ToDetectorData(RulesData rules)
        {
            return new SmallTalksDectectorData
            {
                SmallTalksIntents = rules.Rules.Select(r => ToIntent(r)).ToList()
            };
        }

        #region private methods
        private List<string> PositionDetector(List<string> positions, List<string> patterns)
        {
            var finalPatterns = new List<string>();
            foreach (var position in positions)
            {
                switch (position)
                {
                    case "alone":
                        finalPatterns.InsertRange(finalPatterns.Count, RegexAdd(patterns, position));
                        break;

                    case "beginning":
                        finalPatterns.InsertRange(finalPatterns.Count, RegexAdd(patterns, position));
                        break;

                    case "ending":
                        finalPatterns.InsertRange(finalPatterns.Count, RegexAdd(patterns, position));
                        break;

                    default:
                        break;
                }
            }

            return finalPatterns;
        }

        private List<string> RegexAdd(List<string> patterns, string regexType)
        {
            var newPatterns = new List<string>();

            foreach (var pattern in patterns)
            {
                if(regexType == "alone")
                {
                    newPatterns.Add(string.Concat("^", pattern, "$"));
                }
                else if (regexType == "beginning")
                {
                    newPatterns.Add(string.Concat("^", pattern));
                }
                else
                {
                    newPatterns.Add(string.Concat(pattern, "$"));
                }
            }

            return newPatterns;
        }
        #endregion
    }
}
