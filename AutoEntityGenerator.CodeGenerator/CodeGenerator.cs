﻿using AutoEntityGenerator.Common.CodeInfo;
using System;
using System.Text;

namespace AutoEntityGenerator.CodeGenerator
{
    internal abstract class CodeGenerator
    {
        protected string Comments =>
    $@"/*
    Generated by {nameof(AutoEntityGenerator)} on {DateTime.Now}
    For more information about {nameof(AutoEntityGenerator)}, Visit https://github.com/Peled-Zohar/AutoEntityGenerator
*/
";
        protected void GenerateIndentation(StringBuilder sb, int indentationLevel)
        {
            for (var i = 0; i < indentationLevel; i++)
            {
                sb.Append("\t");
            }
        }

        protected string GenerateTypeParameters(Entity entity)
            => entity.TypeParameters.Count > 0
                ? $"<{string.Join(", ", entity.TypeParameters)}>"
                : "";

        protected string GenerateGenericConstraints(Entity entity)
            => entity.GenericConstraints.Count > 0
                ? " " + string.Join(" ", entity.GenericConstraints)
                : "";

    }
}
