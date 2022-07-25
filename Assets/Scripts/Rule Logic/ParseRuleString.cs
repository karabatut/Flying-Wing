using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseRuleString 
{
    public void parseRuleString(string ruleString)
    {
        string[] rules = ruleString.Split(';');

        foreach (string rule in rules)
        {
            int parameterIndex = 0;
            string[] parameters = rule.Split(',');
            string part = parameters[parameterIndex];
            string firstParameters = parameters[parameterIndex];
            string secondParameters = parameters[parameterIndex];
            string thirdParameters = parameters[parameterIndex];
            string sideSnap = parameters[parameterIndex];
        }

    }
}
