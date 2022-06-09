using System;

using Microsoft.CodeAnalysis;

using Instances = R5T.F0022.Instances;


namespace System
{
    public static class SyntaxTokenExtensions
    {
        /// <summary>
        /// Adds indentation before the token.
        /// Note: there is no IndentBlock() for a token, since a token can only exist on a single line.
        /// If there is a new line in the leading trivia of the token, indentation is added after the last new line. Else it is added to the start of the leading trivia.
        /// </summary>
        ///// <inheritdoc cref="Glossary.ForTrivia.StartLine" path="/definition"/>
        public static SyntaxToken Indent(this SyntaxToken token,
            SyntaxTriviaList indentation)
        {
            return Instances.SyntaxIndentationOperator.Indent(
                token,
                indentation);
        }
    }
}
