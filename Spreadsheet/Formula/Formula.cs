// Skeleton written by Joe Zachary for CS 3500, January 2019

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Formulas.TokenType;
//Author:  Andrew Hare  u1033940

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {

        private List<Token> formulaTokens;
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(string formula): this(formula, x => x, x => true)
        {

        }
        public Formula(string formula, Normalizer normalizer, Validator validator)
        {
            if (formula == null || normalizer == null || validator == null)
            {
                throw new ArgumentNullException("Cannot construct a formula with a null paramter");
            }
            formulaTokens = new List<Token>(GetTokens(formula));
            if (formulaTokens == null)
            {
                throw new FormulaFormatException("Cannot construct a formula with no tokens");
            }
            int formulaLength = formula.Length;
            int currentPosition = 0;
            int numberOpenParenthesis = 0;
            int numberClosedParenthesis = 0;
            Token lastToken = new Token(null, Invalid);
            formulaTokens = new List<Token>();
            string tempText;
            foreach (Token token in GetTokens(formula))
            {
                if (token.type == Invalid)
                {
                    throw new FormulaFormatException("Cannot construct a forumula with an Invalid token");
                }
                if (token.type == RParen)
                {
                    numberClosedParenthesis++;
                    if (numberClosedParenthesis > numberOpenParenthesis)
                    {
                        throw new FormulaFormatException("Cannot construct a formula without having the proper amount of open parantheses before a closed parantheses");
                    }
                }
                if (token.type == LParen)
                {
                    numberOpenParenthesis++;
                }
                if (currentPosition == 0)
                {
                    if (token.type != Number && token.type != Var && token.type != LParen)
                    {
                        throw new FormulaFormatException("Cannot construct a forumula without beggining with a number, a variable, or a left parentheses");
                    }
                }
                if (currentPosition == formulaLength)
                {
                    if (token.type != Number && token.type != Var && token.type != RParen)
                    {
                        throw new FormulaFormatException("Cannot construct a forumula without ending with a number, a variable, or a right parentheses");
                    }
                }
                if (lastToken.type == LParen || lastToken.type == Oper)
                {
                    if (token.type != Number && token.type != Var && token.type != LParen && token.text != " ")
                    {
                        throw new FormulaFormatException("Cannot construct a forumula without having the proper sequence of tokens");
                    }
                }
                if (lastToken.type == Number || lastToken.type == Var || lastToken.type == RParen)
                {
                    if (token.type != Oper && token.type != RParen && token.text != " ")
                    {
                        throw new FormulaFormatException("Cannot construct a forumula without having the proper sequence of tokens");
                    }
                }
                if(token.type == Var)
                {
                    tempText = normalizer(token.text);
                    if(!validator(tempText))
                    {
                        throw new FormulaFormatException("Cannot construct a formula with a false validator");
                    }
                    formulaTokens.Add(new Token(tempText, token.type));
                }
                else
                {
                    formulaTokens.Add(new Token(token.text, token.type));
                }
                lastToken = token;
                currentPosition++;
            }
            if (numberOpenParenthesis != numberClosedParenthesis)
            {
                throw new FormulaFormatException("Cannot construct a formula with more open parantheses than closed parantheses");
            }
            if (lastToken.type == Invalid)
            {
                throw new FormulaFormatException("Cannot construct a formula with no tokens");
            }
            if (lastToken.type == Oper)
            {
                throw new FormulaFormatException("Cannot construct a formula with an operator as the last token");
            }
        }

        private struct Token
        {
            public string text;

            public TokenType type;
            
            public Token(string _text, TokenType _type)
            {
                text = _text;
                type = _type;
            }
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            Stack valueStack = new Stack();
            Stack operatorStack = new Stack();
            double result = 0;
            
            foreach (Token token in formulaTokens)
            {
                if(token.type == Number)
                {
                    if (operatorStack.Count != 0)
                    {
                        if ((string)operatorStack.Peek() == "*")
                        {
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = Convert.ToDouble(token.text) * tempValue;
                            valueStack.Push(tempResult);
                        }
                        else if ((string)operatorStack.Peek() == "/")
                        {
                            if(Convert.ToDouble(token.text) == 0)
                            {
                                throw new FormulaEvaluationException("Can't divide by 0");
                            }
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempValue / Convert.ToDouble(token.text);
                            valueStack.Push(tempResult);
                        }
                        else
                        {
                            valueStack.Push(token.text);
                        }
                    }
                    else
                    {
                        valueStack.Push(token.text);
                    }
                }
                else if(token.type == Var)
                {
                    double tempVar;
                    try
                    {
                        tempVar = lookup(token.text);
                    }
                    catch
                    {
                        throw new FormulaEvaluationException("Need a definition of a variable");
                    }
                    if (operatorStack.Count != 0)
                    {
                        if ((string)operatorStack.Peek() == "*")
                        {
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempVar * tempValue;
                            valueStack.Push(tempResult);
                        }
                        else if ((string)operatorStack.Peek() == "/")
                        {
                            if (tempVar == 0)
                            {
                                throw new FormulaEvaluationException("Can't divide by 0");
                            }
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempValue / tempVar;
                            valueStack.Push(tempResult);
                        }
                        else
                        {
                            valueStack.Push(tempVar);
                        }
                    }
                    else
                    {
                        valueStack.Push(tempVar);
                    }
                }
                else if(token.type == Oper)
                {
                    bool operatorWasPushed = false;
                    if (operatorStack.Count != 0)
                    {
                        if (token.text == "*")
                        {
                            operatorStack.Push(token.text);
                            operatorWasPushed = true;
                        }
                        else if (token.text == "/")
                        {
                            operatorStack.Push(token.text);
                            operatorWasPushed = true;
                        }
                        else if ((string)operatorStack.Peek() == "+" && operatorWasPushed == false)
                        {
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempValue2 = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempValue + tempValue2;
                            valueStack.Push(tempResult);
                            operatorStack.Push(token.text);
                        }
                        else if ((string)operatorStack.Peek() == "-" && operatorWasPushed == false)
                        {
                            double tempValue2 = Convert.ToDouble(valueStack.Pop());
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempValue - tempValue2;
                            valueStack.Push(tempResult);
                            operatorStack.Push(token.text);
                        }
                        else
                        {
                            operatorStack.Push(token.text);
                        }
                    }
                    else
                        {
                            operatorStack.Push(token.text);
                        }
                }
                else if(token.type == LParen)
                {
                    operatorStack.Push(token.text);
                }
                else if(token.type == RParen)
                {
                    if ((string)operatorStack.Peek() == "+")
                    {
                        double tempValue = Convert.ToDouble(valueStack.Pop());
                        double tempValue2 = Convert.ToDouble(valueStack.Pop());
                        double tempResult;
                        operatorStack.Pop();
                        tempResult = tempValue + tempValue2;
                        valueStack.Push(tempResult);
                    }
                    if ((string)operatorStack.Peek() == "-")
                    {
                        double tempValue = Convert.ToDouble(valueStack.Pop());
                        double tempValue2 = Convert.ToDouble(valueStack.Pop());
                        double tempResult;
                        operatorStack.Pop();
                        tempResult = tempValue2 - tempValue;
                        valueStack.Push(tempResult);
                    }
                    operatorStack.Pop();
                    if (operatorStack.Count != 0)
                    {
                        if ((string)operatorStack.Peek() == "*")
                        {
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            double tempValue2 = Convert.ToDouble(valueStack.Pop());
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempValue * tempValue2;
                            valueStack.Push(tempResult);
                        }
                        else if((string)operatorStack.Peek() == "/")
                        {
                            double tempValue2 = Convert.ToDouble(valueStack.Pop());
                            double tempValue = Convert.ToDouble(valueStack.Pop());
                            if (tempValue2 == 0)
                            {
                                throw new FormulaEvaluationException("Can't divide by 0");
                            }
                            double tempResult;
                            operatorStack.Pop();
                            tempResult = tempValue / tempValue2;
                            valueStack.Push(tempResult);
                        }
                    }
                }
            }
            if(operatorStack.Count == 0)
            {
                result = Convert.ToDouble(valueStack.Pop());
            }
            else
            {
                if((string)operatorStack.Peek() == "+")
                {
                    double tempValue = Convert.ToDouble(valueStack.Pop());
                    double tempValue2 = Convert.ToDouble(valueStack.Pop());
                    result = tempValue + tempValue2;
                }
                else if ((string)operatorStack.Peek() == "-")
                {
                    double tempValue2 = Convert.ToDouble(valueStack.Pop());
                    double tempValue = Convert.ToDouble(valueStack.Pop());
                    result = tempValue - tempValue2;
                }
            }

            return result;
        }

        public ISet<string> GetVariables()
        {
            HashSet<string> variablesToReturn = new HashSet<string>();

            foreach(Token token in formulaTokens)
            {
                if(token.type == Var)
                {
                    variablesToReturn.Add(token.text);
                }
            }
            return variablesToReturn;
        }

        public override string ToString()
        {
            string stringToReturn = null;
            foreach(Token token in formulaTokens)
            {
                stringToReturn += token.text;
            }
            return stringToReturn;
        }
        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Each token is described by a
        /// Tuple containing the token's text and TokenType.  There are no empty tokens, and no
        /// token contains white space.
        /// </summary>
        private static IEnumerable<Token> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall token pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String tokenPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5}) | (.)",
                                            spacePattern, lpPattern, rpPattern, opPattern, varPattern, doublePattern);

            // Create a Regex for matching tokens.  Notice the second parameter to Split says 
            // to ignore embedded white space in the pattern.
            Regex r = new Regex(tokenPattern, RegexOptions.IgnorePatternWhitespace);

            // Look for the first match
            Match match = r.Match(formula);

            // Start enumerating tokens
            while (match.Success)
            {
                // Ignore spaces
                if (!match.Groups[1].Success)
                {
                    // Holds the token's type
                    TokenType type;

                    if (match.Groups[2].Success)
                    {
                        type = LParen;
                    }
                    else if (match.Groups[3].Success)
                    {
                        type = RParen;
                    }
                    else if (match.Groups[4].Success)
                    {
                        type = Oper;
                    }
                    else if (match.Groups[5].Success)
                    {
                        type = Var;
                    }
                    else if (match.Groups[6].Success)
                    {
                        type = Number;
                    }
                    else if (match.Groups[7].Success)
                    {
                        type = Invalid;
                    }
                    else
                    {
                        // We shouldn't get here
                        throw new InvalidOperationException("Regular exception failed in GetTokens");
                    }

                    // Yield the token
                    yield return new Token(match.Value, type);
                }

                // Look for the next match
                match = match.NextMatch();
            }
        }
    }

    /// <summary>
    /// Identifies the type of a token.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Left parenthesis
        /// </summary>
        LParen,

        /// <summary>
        /// Right parenthesis
        /// </summary>
        RParen,

        /// <summary>
        /// Operator symbol
        /// </summary>
        Oper,

        /// <summary>
        /// Variable
        /// </summary>
        Var,

        /// <summary>
        /// Double literal
        /// </summary>
        Number,

        /// <summary>
        /// Invalid token
        /// </summary>
        Invalid
    };

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    public delegate string Normalizer(string s);

    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
