using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;

[Serializable]
public class Command
{
    public string syntax;
    public string description;

    public Command(string syntax = "", string description = "")
    {
        this.syntax = syntax;
        this.description = description;
    }

    public virtual string Execute(string[] args = null, object[] targets = null)
    {
        return "";
    }
}