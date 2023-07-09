using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilator
{
    public enum TokenType
    {
        Class,
        Expression,
        Method,
        Options,
        EndOptions,
        String,
        Number,
        NumberZero,
        EndOfMethod,
        ErrorClass,
        ErrorMethod,
        ErrorString,
        Undefined,
        EOF
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }

        public Token(TokenType type, string value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public Token() { }
    }

    public class Lexer
    {
        private readonly string _text;
        private int _position;
        private char[] SymbolArr = { '!', ';', '?', '-', '_', '/', '#', '№', '%', '^', '*', '(', ')', '+', '=', '<', '>', ' ' };

        private char CurrentChar => _position < _text.Length ? _text[_position] : '\0';

        public Lexer(string text)
        {
            _text = text;
        }

        public int GetPosition()
        {
            return _position;
        }

        private void SetNextPosition()
        {
            _position++;
        }

        private void SetPrevPosition()
        {
            _position--;
        }

        private void SkipWhiteSpace()
        {
            while (char.IsWhiteSpace(CurrentChar))
            {
                SetNextPosition();
            }
        }

        private bool GrammerLetter(char letter)
        {
            if ((letter >= 'a' && letter <= 'z') || (letter >= 'A' && letter <= 'Z'))
                return true;
            else
                return false;
        }

        private bool GrammerSymbol(char symbol)
        {
            foreach (char i in SymbolArr)
            {
                if (symbol == i)
                    return true;
            }
            return false;
        }

        private (string, bool) CheckClass()
        {
            int state = 0;
            string result = "C";
            while (state != 6 && state != -1)
            {
                switch (state)
                {
                    case 0:
                        {
                            if (CurrentChar == 'o') { state = 1; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (CurrentChar == 'n') { state = 2; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (CurrentChar == 's') { state = 3; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (CurrentChar == 'o') { state = 4; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (CurrentChar == 'l') { state = 5; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (CurrentChar == 'e') { state = 6; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    default: break;
                }
            }
            if (state == -1)
            {
                while (GrammerLetter(CurrentChar))
                {
                    result += CurrentChar;
                    SetNextPosition();
                }
                SetPrevPosition();
                return (result, false);
            }
            return (result, true);
        }

        private (string, bool) CheckMethod()
        {
            int state = 0;
            string result = "W";
            while (state != 8 && state != -1)
            {
                switch (state)
                {
                    case 0:
                        {
                            if (CurrentChar == 'r') { state = 1; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (CurrentChar == 'i') { state = 2; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (CurrentChar == 't') { state = 3; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (CurrentChar == 'e') { state = 4; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (CurrentChar == 'L') { state = 5; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (CurrentChar == 'i') { state = 6; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 6:
                        {
                            if (CurrentChar == 'n') { state = 7; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (CurrentChar == 'e') { state = 8; result += CurrentChar; SetNextPosition(); }
                            else
                            {
                                state = -1;
                            }
                            break;
                        }
                    default: break;
                }
            }
            if (state == -1)
            {
                while (GrammerLetter(CurrentChar))
                {
                    result += CurrentChar;
                    SetNextPosition();
                }
                SetPrevPosition();
                return (result, false);
            }
            return (result, true);
        }

        private string CheckDigit()
        {
            string result = "";
            while (char.IsDigit(CurrentChar))
            {
                result += CurrentChar;
                SetNextPosition();
            }
            return result;
        }

        private (string, bool) CheckString()
        {
            string result = "\"";
            while (CurrentChar != '"')
            {
                if (CurrentChar == '\0')
                {
                    return (result, false);
                }
                if (GrammerSymbol(CurrentChar) || GrammerLetter(CurrentChar) || char.IsDigit(CurrentChar))
                {
                    result += CurrentChar;
                    SetNextPosition();
                }
                else
                {
                    return (result, false);
                }
            }
            SetNextPosition();
            return (result += "\"", true);
        }

        public Token GetNextToken()
        {
            int position = GetPosition();
            while (CurrentChar != '\0')
            {
                if (CurrentChar == 'C')
                {
                    SetNextPosition();
                    var res = CheckClass();
                    string lx = res.Item1;
                    if (res.Item2)
                    {
                        return new Token(TokenType.Class, lx, position);
                    }
                    else
                    {
                        SetNextPosition();
                        return new Token(TokenType.ErrorClass, lx, position);
                    }
                }
                else if (CurrentChar == 'W')
                {
                    SetNextPosition();
                    var res = CheckMethod();
                    string lx = res.Item1;
                    if (res.Item2)
                    {
                        return new Token(TokenType.Method, lx, position);
                    }
                    else
                    {
                        SetNextPosition();
                        return new Token(TokenType.ErrorMethod, lx, position);
                    }
                }
                else if (CurrentChar == '.')
                {
                    SetNextPosition();
                    return new Token(TokenType.Expression, ".", position);
                }
                else if (CurrentChar == '(')
                {
                    SetNextPosition();
                    return new Token(TokenType.Options, "(", position);
                }
                else if (CurrentChar == ')')
                {
                    SetNextPosition();
                    return new Token(TokenType.EndOptions, ")", position);
                }
                else if (CurrentChar == '0')
                {
                    SetNextPosition();
                    return new Token(TokenType.NumberZero, "0", position);
                }
                else if (char.IsDigit(CurrentChar) && CurrentChar != '0')
                {
                    string lx = CheckDigit();
                    return new Token(TokenType.Number, lx, position);
                }
                else if (CurrentChar == '"')
                {
                    SetNextPosition();
                    var res = CheckString();
                    string lx = res.Item1;
                    if (res.Item2)
                    {
                        return new Token(TokenType.String, lx, position);
                    }
                    else if (!res.Item2 && lx == "\"")
                    {
                        return new Token(TokenType.Undefined, lx, position);
                    }
                    else
                    {
                        return new Token(TokenType.ErrorString, lx, position);
                    }
                }
                else if (CurrentChar == ';')
                {
                    SetNextPosition();
                    return new Token(TokenType.EndOfMethod, ";", position);
                }
                else
                {
                    string curChar = CurrentChar.ToString();
                    SetNextPosition();
                    return new Token(TokenType.Undefined, curChar, position);
                }
            }
            return new Token(TokenType.EOF, "", position);
        }

        public Token[] Tokenize()
        {
            var tokens = new List<Token>();
            Token token;
            do
            {
                SkipWhiteSpace();
                token = GetNextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.EOF /*tokens.Count < 8*/);
            return tokens.ToArray();
        }
    }
}
