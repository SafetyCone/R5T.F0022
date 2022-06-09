using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.T0132;


namespace R5T.F0022
{
	[FunctionalityMarker]
	public interface ISyntaxIndentationOperator : IFunctionalityMarker
	{
        public TNode Indent<TNode>(
            TNode node,
            SyntaxTriviaList indentation)
            where TNode : SyntaxNode
        {
            // Get end-of-line trivias.
            var endOfLineTrivias = node.GetEndOfLineTrivias();

            // Foreach end-of-line trivia, get its next token (the first token on the next line), and unless its token type is None(), accumulate the token.
            var firstTokensOfLines = endOfLineTrivias
                .Select(xTrivia =>
                {
                    var isInLeadingTrivia = xTrivia.IsInLeadingTrivia();
                    if (isInLeadingTrivia)
                    {
                        return xTrivia.Token;
                    }
                    else
                    {
                        var output = xTrivia.Token.GetNextToken(includeDirectives: true);
                        return output;
                    }
                })
                .Where(xToken => xToken.IsNotNone())
                // Add the first token to the list, since we will want to indent that too.
                .Append(node.GetFirstToken_HandleDocumentationComments())
                // There will be duplicates if there are multiple lines between tokens (which is always).
                .Distinct()
                .Now();

            // Annotate tokens.
            node = node.AnnotateTokens_Untyped(
                firstTokensOfLines,
                out var annotationsByFirstTokensOfLines);

            // Now indent each token by annotation.
            foreach (var annotation in annotationsByFirstTokensOfLines.Values)
            {
                node = annotation.ModifyToken(
                    node,
                    xToken => xToken.Indent(indentation));
            }

            return node;
        }

        /// <summary>
        /// Adds indentation before the token.
        /// Note: there is no IndentBlock() for a token, since a token can only exist on a single line.
        /// If there is a new line in the leading trivia of the token, indentation is added after the last new line. Else it is added to the start of the leading trivia.
        /// </summary>
        ///// <inheritdoc cref="Glossary.ForTrivia.StartLine" path="/definition"/>
        public SyntaxToken Indent(
            SyntaxToken token,
            SyntaxTriviaList indentation)
        {
            // A token always has leading trivia, is just might be empty.
            var leadingTrivia = token.LeadingTrivia;

            var indexOfLastNewLineOrStructure = leadingTrivia.LastIndexWhere(
                // Structured trivia might have new lines within the structure, and we will want to add new lines after the structure.
                x => x.IsNewLine() || x.HasStructure);

            if (IndexHelper.IsFound(indexOfLastNewLineOrStructure))
            {
                var newLeadingTrivia = leadingTrivia
                    // Include the new line.
                    .Take(indexOfLastNewLineOrStructure + 1)
                    // Insert the indentation.
                    .Concat(indentation)
                    // Take the rest.
                    .Concat(
                        leadingTrivia.Skip(indexOfLastNewLineOrStructure + 1))
                    .ToSyntaxTriviaList();

                var output = token.WithLeadingTrivia(newLeadingTrivia);
                return output;
            }
            else
            {
                var output = token.AddLeadingLeadingTrivia(indentation.ToArray());
                return output;
            }
        }
    }
}