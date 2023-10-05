using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work
{
    public class LR
    {
        List<Token> tokens;

        Token _Curent()
        {
            Error1();
            return Curent();
        }
        Token _CurentUp()
        {
            i++;
            Error1();
            return Curent();
        }


        Token? Curent() => i < tokens.Count ? tokens[i] : null;
        Token? Previe() => i > 0 ? tokens[i - 1] : null;
        Token? Next() => i < tokens.Count - 1 ? tokens[i + 1] : null;
        
        public LR(Token[] Tokens)
        {
            tokens = new List<Token>();
            foreach (Token token in Tokens)
            {
                if (token.Type != TokenType.TOKEN_ERROR)
                {
                    tokens.Add(token);
                }
                else
                {
                    throw new Exception(token.ToString());
                }
            }
        }
        int i;
        public void Check()
        {
            i = 0;
            while (i != -1)
                _1();
        }

        void _1()
        {
            if(i != -1)
            {
                if(Curent() == null)
                {
                    i = -1;
                }
                else
                    switch(Curent().Type)
                    {
                        case TokenType.DIM:
                            _16();
                            break;
                        case TokenType.IDENTIFIER:
                            _4();
                            break;
                        case TokenType.SELECT:
                            _25();
                            break;
                        case TokenType.ENDLINE:
                            i++;
                            break;
                        default:
                            Error2(TokenType.DIM, TokenType.IDENTIFIER, TokenType.SELECT);
                            break;
                    }
            }
        }

        void _4()
        {
            _CurentUp();
            if(Curent().Type == TokenType.EQUAL)
                _5();
            else if (Curent().Type == TokenType.PLUS     ||
                     Curent().Type == TokenType.MINUS    ||
                     Curent().Type == TokenType.MULTIPLY ||
                     Curent().Type == TokenType.DIVIDE)
            {
                _CurentUp();
                if (Curent().Type == TokenType.EQUAL)
                    _5();
                else Error2(TokenType.EQUAL);
            } else Error2(TokenType.EQUAL);
        }
        Bauer_Zamelzon b;
        public List<Troyka> Troyka = new List<Troyka>();
        void _5()
        {
            List<Token> stack = new List<Token>();
            i++;
            while (Curent() != null && Curent().Type != TokenType.ENDLINE)
            {
                stack.Add(Curent());
                i++;
            }
            if(stack.Count == 0)
            {
                throw new Exception("Ожидалась операция");
            }
            if(b == null)
                b = new Bauer_Zamelzon(stack);
            else
            {
                int ii = b.index;
                b = new Bauer_Zamelzon(stack);
                b.index = ii;
            }
            b.listTroyka = Troyka;
            b.Start();
        }

        void _16()
        {
            _CurentUp();
            if(Curent().Type == TokenType.IDENTIFIER)
            {
                _CurentUp();
                if (Curent().Type == TokenType.COMMA)
                    _16();
                else if (Curent().Type == TokenType.AS)
                    _18();
                else Error2(TokenType.COMMA, TokenType.AS);
            } else Error2(TokenType.IDENTIFIER);
        }
        void _18()
        {
            _CurentUp();
            if (Curent().Type == TokenType.INTEGER ||
               Curent().Type == TokenType.DOUBLE ||
               Curent().Type == TokenType.STRING)
                i++;
            else Error2(TokenType.INTEGER, TokenType.DOUBLE, TokenType.STRING);
        }
        void _25()
        {
            if(_CurentUp().Type == TokenType.CASE)
            {
                if (_CurentUp().Type == TokenType.IDENTIFIER)
                {
                    if (_CurentUp().Type == TokenType.ENDLINE)
                    {
                        while (Curent() != null && Curent().Type != TokenType.END)
                            _28();
                        _Curent();
                        if (_CurentUp().Type != TokenType.SELECT) Error2(TokenType.SELECT);
                        i++;

                    } else Error2(TokenType.ENDLINE);
                } else Error2(TokenType.IDENTIFIER);
            } else Error2(TokenType.CASE);
        }
        void _28()
        {
            if(_CurentUp().Type == TokenType.CASE)
            {
                _CurentUp();
                if (Curent().Type == TokenType.LITERAL)
                {
                    if(_CurentUp().Type == TokenType.ENDLINE)
                    {
                        _28x(false);
                    }
                    else if(Curent().Type == TokenType.TO)
                    {
                        if (_CurentUp().Type == TokenType.LITERAL)
                        {
                            if (_CurentUp().Type == TokenType.ENDLINE)
                            {
                                _28x(false);
                            }
                            else Error2(TokenType.ENDLINE);
                        } else Error2(TokenType.LITERAL);
                    } else Error2(TokenType.ENDLINE, TokenType.TO);
                }
                else if(Curent().Type == TokenType.ELSE && _CurentUp().Type == TokenType.ENDLINE)
                {
                    _28x(true);
                } else Error2(TokenType.LITERAL, TokenType.ELSE);
            } else Error2(TokenType.CASE);
        }
        void _28x(bool end)
        {
            while (Curent() != null && Curent().Type != TokenType.CASE && Curent().Type != TokenType.END)
                _1();
            if(_Curent().Type == TokenType.CASE)
            {
                if (end) Error2(TokenType.END);
                i--;
            }
            else if(Curent().Type != TokenType.END) Error2(TokenType.END);
        }



        #region Ошибки
        private void Error1()
        {
            if (Curent() == null)
                throw new Exception(GetLine(i) + " Строка, Ожидалось наличие " + (1 + i) + "-ого элемента");
        }
        private void Error2() => Error2(i, false, null);
        private void Error2(params TokenType[] TokenTypes) => Error2(i, false, TokenTypes);
        private void Error2(int I) => Error2(I, false, null);
        private void Error2(int I, bool next) => Error2(I, next, null);
        private void Error2(int I, params TokenType[] TokenTypes) => Error2(I, false, TokenTypes);
        private void Error2(int I, bool next, params TokenType[] TokenTypes)
        {
            Error1();
            if (TokenTypes != null && TokenTypes.Length != 0)
            {
                string err = "(";
                for (int i = 0; i < TokenTypes.Length; i++)
                {
                    err += TokenTypes[i].ToString();
                    if (i + 1 < TokenTypes.Length)
                        err += ", ";
                }
                err += ")";
                if (next)
                {
                    Error1();
                    throw new Exception(GetLine(I) + " Строка, " + (1 + I) + " и " + (2 + I) + " элементов (" + Curent() + " и " + Curent() + "), ожидалось " + err);
                }
                throw new Exception(GetLine(I) + " Строка, " + "Вместо " + (1 + I) + "-ого элемента (" + Curent() + "), ожидалось " + err);
            }
            if (next)
            {
                Error1();
                throw new Exception(GetLine(I) + " Строка, " + (1 + I) + " и " + (2 + I) + " элемент (" + Curent() + " и " + Curent() + "), не ожидался");
            }
            throw new Exception(GetLine(I) + " Строка, " + (1 + I) + "-ый элемент (" + Curent() + "), не ожидался");
        }
        int GetLine(int I)
        {
            int L = 1;
            for (int Li = 0; Li < I && Li < tokens.Count; Li++)
            {
                if (tokens[Li].Type == TokenType.ENDLINE)
                    L++;
            }
            return L;
        }
        #endregion
    }
}
