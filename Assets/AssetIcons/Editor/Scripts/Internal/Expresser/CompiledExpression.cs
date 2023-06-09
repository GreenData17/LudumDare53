﻿//  
// Copyright (c) 2017 Anthony Marmont. All rights reserved.
// Licensed for use under the Unity Asset Store EULA. See https://unity3d.com/legal/as_terms for full license information.  
// 

#if UNITY_EDITOR
#pragma warning disable

using AssetIcons.Editors.Internal.Expresser.Input;
using AssetIcons.Editors.Internal.Expresser.Processing;
using System.Text;

namespace AssetIcons.Editors.Internal.Expresser
{
	/// <summary>
	/// <para>A mathematical expression in a compiled format.</para>
	/// </summary>
	/// <example>
	/// <para>Below is an example of constructing a CompiledExpression with a string and evaluating it.</para>
	/// <code>
	/// using System;
	/// using AssetIcons.Editors.Internal.Expresser;
	/// 
	/// internal class Program
	/// {
	/// 	public static void Main (string[] args)
	/// 	{
	/// 		var context = new MathContextBuilder()
	/// 			.WithTerm ("Width", new StaticValueProvider (10))
	/// 			.Build();
	/// 
	/// 		var expression = new CompiledExpression ("0.1*Width", context);
	/// 
	/// 		var result = expression.Evaluate();
	/// 
	/// 		Console.WriteLine (expression);        // 0.1 * Width
	/// 		Console.WriteLine (result.ValueClass); // ValueClassifier.Float
	/// 		Console.WriteLine (result.FloatValue); // 1
	/// 	}
	/// }
	/// </code>
	/// </example>
	internal class CompiledExpression
	{
		private readonly MathValue[] calculationBuffer;

		/// <summary>
		/// <para></para>
		/// </summary>
		public IMathContext Context { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public IntermediateExpression Intermediate { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public ExpressionSyntax Syntax { get; private set; }

		private CompiledExpression(ExpressionSyntax syntax, IMathContext context = null)
		{
			Syntax = syntax;
			Context = context;

			Intermediate = IntermediateExpression.Compile(syntax, context);
			calculationBuffer = new MathValue[Intermediate.DistSize];
		}

		public static CompiledExpression Compile(ExpressionSyntax syntax, IMathContext context = null)
		{
			return new CompiledExpression(syntax, context);
		}

		public static CompiledExpression Compile(string expression, IMathContext context = null)
		{
			return new CompiledExpression(new ExpressionSyntax(expression), context);
		}

		public MathValue Evaluate()
		{
			return Intermediate.Evaluate(calculationBuffer);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			var lastToken = Syntax.Tokens[0];
			for (int i = 1; i < Syntax.Tokens.Length; i++)
			{
				var token = Syntax.Tokens[i];
				sb.Append(lastToken);
				if (lastToken.Operation != SyntaxTokenKind.OpenParentheses
					&& token.Operation != SyntaxTokenKind.CloseParentheses
					&& token.Operation != SyntaxTokenKind.Percentage
					&& token.Operation != SyntaxTokenKind.Comma
					&& lastToken.Operation != SyntaxTokenKind.Not)
				{
					sb.Append(' ');
				}
				lastToken = token;
			}
			sb.Append(Syntax.Tokens[Syntax.Tokens.Length - 1]);
			return sb.ToString();
		}
	}
}

#pragma warning restore
#endif
