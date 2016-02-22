using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [Language("SpeechExpression")]
    public class SpeechExpression : Grammar
    {
        // text $var @Alias some more text @AliasWithArgs(arg1 $var; @InnerAlias $var)

        public const string AliasSymbol = "@";
        public const string VarSymbol = "$";
        public const string OpenBracketSymbol = "(";
        public const string CloseBracketSymbol = ")";
        public const string ArgSeparatorSymbol = ";";

        public const string RootExpressionName = "Root";
        public const string TextLiteralName = "Text";
        public const string NameLiteralName = "Name";
        public const string AliasLiteralName = "Alias";
        public const string ArgumentListLiteralName = "ArgumentList";
        public const string ExpressionLiteralName = "Expression";
        public const string VariableLiteralName = "Variable";

        public SpeechExpression()
        {
            FreeTextLiteral text = new FreeTextLiteral(TextLiteralName, FreeTextOptions.AllowEof, AliasSymbol, VarSymbol, OpenBracketSymbol, ArgSeparatorSymbol, CloseBracketSymbol);
            IdentifierTerminal name = TerminalFactory.CreateCSharpIdentifier(NameLiteralName);

            NonTerminal root = new NonTerminal(RootExpressionName);
            NonTerminal expression = new NonTerminal(ExpressionLiteralName);
            NonTerminal argumentList = new NonTerminal(ArgumentListLiteralName);
            NonTerminal alias = new NonTerminal(AliasLiteralName);
            NonTerminal variable = new NonTerminal(VariableLiteralName);

            argumentList.Rule = MakePlusRule(argumentList, ToTerm(ArgSeparatorSymbol), root);
            alias.Rule = AliasSymbol + name + OpenBracketSymbol + argumentList + CloseBracketSymbol | AliasSymbol + name;
            variable.Rule = VarSymbol + name;

            expression.Rule = text | variable | alias;
            root.Rule = MakeStarRule(root, expression);

            Root = root;

            text.Escapes.Add("\\" + AliasSymbol, AliasSymbol);
            text.Escapes.Add("\\" + VarSymbol, VarSymbol);
            text.Escapes.Add("\\" + OpenBracketSymbol, OpenBracketSymbol);
            text.Escapes.Add("\\" + CloseBracketSymbol, CloseBracketSymbol);
            text.Escapes.Add("\\" + ArgSeparatorSymbol, ArgSeparatorSymbol);

            MarkTransient(root, expression);
            RegisterBracePair(OpenBracketSymbol, CloseBracketSymbol);
        }
    }
}
