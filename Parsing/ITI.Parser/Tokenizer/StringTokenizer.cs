using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class StringTokenizer : ITokenizer
    {
        private string _toParse;
        private int _pos;
        private int _maxPos;
        private TokenType _curToken;
        private double _doubleValue;
        private string _stringVal;

        public StringTokenizer(string s)
            : this(s, 0, s.Length)
        {
        }

        public StringTokenizer(string s, int startIndex)
            : this(s, startIndex, s.Length)
        {
        }

        public StringTokenizer(string s, int startIndex, int count)
        {
            _curToken = TokenType.None;
            _toParse = s;
            _pos = startIndex;
            _maxPos = startIndex + count;
        }

        #region Input reader

        private char Peek()
        {
            Debug.Assert(!IsEnd);
            return _toParse[_pos];
        }

        private char Read()
        {
            Debug.Assert(!IsEnd);
            return _toParse[_pos++];
        }

        private void Forward()
        {
            Debug.Assert(!IsEnd);
            ++_pos;
        }

        private bool IsEnd => _pos >= _maxPos;

        #endregion Input reader

        public TokenType CurrentToken => _curToken;

        public bool Match(TokenType t)
        {
            if (_curToken == t)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchDouble(out double value)
        {
            value = _doubleValue;
            if (_curToken == TokenType.Number)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger(int expectedValue)
        {
            if (_curToken == TokenType.Number
                && _doubleValue < int.MaxValue
                && (int)_doubleValue == expectedValue)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger(out int value)
        {
            if (_curToken == TokenType.Number
                && _doubleValue < int.MaxValue)
            {
                value = (int)_doubleValue;
                GetNextToken();
                return true;
            }
            value = 0;
            return false;
        }

        public bool MatchString(string expectedValue)
        {
            if (_curToken == TokenType.Variable
                && _stringVal == expectedValue)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchString(out string value)
        {
            if (_curToken == TokenType.Variable)
            {
                value = _stringVal;
                GetNextToken();
                return true;
            }
            value = string.Empty;
            return false;
        }

        public TokenType GetNextToken()
        {
            if (IsEnd) return _curToken = TokenType.EndOfInput;
            char c = Read();
            while (char.IsWhiteSpace(c))
            {
                if (IsEnd) return _curToken = TokenType.EndOfInput;
                c = Read();
            }
            switch (c)
            {
                case '+': _curToken = TokenType.Plus; break;
                case '-': _curToken = TokenType.Minus; break;
                case '?': _curToken = TokenType.QuestionMark; break;
                case ':': _curToken = TokenType.Colon; break;
                case '*': _curToken = TokenType.Mult; break;
                case '/': _curToken = TokenType.Div; break;
                case '(': _curToken = TokenType.OpenPar; break;
                case ')': _curToken = TokenType.ClosePar; break;
                default:
                    {
                        if (char.IsDigit(c))
                        {
                            _curToken = TokenType.Number;
                            double val = (int)(c - '0');
                            while (!IsEnd && char.IsDigit(c = Peek()))
                            {
                                val = val * 10 + (int)(c - '0');
                                Forward();
                            }
                            _doubleValue = val;
                        }
                        else if (char.IsLetter(c))
                        {
                            _curToken = TokenType.Variable;
                            string val = c + string.Empty;
                            while (!IsEnd && char.IsLetter(c = Peek()))
                            {
                                val += c;
                                Forward();
                            }
                            _stringVal = val;
                        }
                        else _curToken = TokenType.Error;
                        break;
                    }
            }
            return _curToken;
        }
    }
}