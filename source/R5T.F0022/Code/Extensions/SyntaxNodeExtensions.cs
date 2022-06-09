using System;
using System.Linq;

using Microsoft.CodeAnalysis;

using Instances = R5T.F0022.Instances;


namespace System
{
    public static class SyntaxNodeExtensions
    {
        public static TNode Indent<TNode>(this TNode node,
            SyntaxTriviaList indentation)
            where TNode : SyntaxNode
        {
            return Instances.SyntaxIndentationOperator.Indent(
                node,
                indentation);
        }
    }
}
