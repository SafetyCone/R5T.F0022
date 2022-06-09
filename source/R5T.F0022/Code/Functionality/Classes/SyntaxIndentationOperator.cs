using System;


namespace R5T.F0022
{
	public class SyntaxIndentationOperator : ISyntaxIndentationOperator
	{
    #region Infrastructure

	    public static SyntaxIndentationOperator Instance { get; } = new();

	    private SyntaxIndentationOperator()
	    {
	    }

	    #endregion
}
}