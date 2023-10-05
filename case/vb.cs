using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace work
{
    public class generation
    {
        public static string[] Split(string code)
        {
            code = code.Replace('\t', ' ');
            //code = code.Replace('\n', ' ');

            string pattern = @"(";
            int i = 0;
            foreach (KeyValuePair<char, TokenType> entry in Token.SpecialSymbols) 
            {
                pattern += "\\";
                pattern += entry.Key;
                if (Token.SpecialSymbols.Count > i + 1) pattern += "|";
                else pattern += @")";
                i++;
            }
            string[] myArray = code.Split(' ').SelectMany(
                    word => Regex.Split(
                        word, pattern
                        )
                    ).ToArray();
            return myArray;
        }
        public static Token[] Tokens(string[] keys)
        {
            List<Token> tokens = new List<Token>();
            for(int i = 0; i < keys.Length; i++)
                if (!String.IsNullOrEmpty(keys[i]) && keys[i].Length != 0 && keys[i][0] != ' ')
                    tokens.Add(new Token(keys[i]));
            return tokens.ToArray<Token>();
        }
    }

    
    public enum TokenType
    {
        INTEGER, DOUBLE, STRING,
        DIM, AS,

        CASE, SELECT, TO, ELSE, END,

        PLUS, MINUS, MULTIPLY, DIVIDE,
        OR, AND,
        DEGREE,
        EQUAL, MORE, LESS,
        COMMA, DOT, COLON, SEMICOLON,
        LPAR, RPAR,
        UNDERSCORE,
        IDENTIFIER, LITERAL,
        TOKEN_ERROR,
        ENDLINE
    }
    public class Token
    {
        static TokenType[] Delimiters = new TokenType[]
{
            TokenType.PLUS, TokenType.MINUS, TokenType.MULTIPLY,
            TokenType.DIVIDE, TokenType.OR, TokenType.AND, TokenType.DEGREE,
            TokenType.EQUAL, TokenType.MORE, TokenType.LESS,
            TokenType.COMMA, TokenType.DOT, TokenType.COLON,
            TokenType.SEMICOLON, TokenType.LPAR, TokenType.RPAR,
            TokenType.UNDERSCORE
};
        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>() {
            {"integer", TokenType.INTEGER},
            {"double", TokenType.DOUBLE},
            {"string", TokenType.STRING},
            {"dim", TokenType.DIM},
            {"as", TokenType.AS},
            {"or", TokenType.OR},
            {"and", TokenType.AND},
            {"case", TokenType.CASE},
            {"to", TokenType.TO},
            {"select", TokenType.SELECT},
            {"else", TokenType.ELSE},
            {"end", TokenType.END},

            {"Integer", TokenType.INTEGER},
            {"Double", TokenType.DOUBLE},
            {"String", TokenType.STRING},
            {"Dim", TokenType.DIM},
            {"As", TokenType.AS},
            {"Or", TokenType.OR},
            {"And", TokenType.AND},
            {"Case", TokenType.CASE},
            {"To", TokenType.TO},
            {"Select", TokenType.SELECT},
            {"Else", TokenType.ELSE},
            {"End", TokenType.END}
        };
        public static Dictionary<char, TokenType> SpecialSymbols = new Dictionary<char, TokenType>()
        {
            {'+', TokenType.PLUS},
            {'-', TokenType.MINUS},
            {'*', TokenType.MULTIPLY},
            {'/', TokenType.DIVIDE},
            {'|', TokenType.OR},
            {'^', TokenType.DEGREE},
            {'=', TokenType.EQUAL},
            {'>', TokenType.MORE},
            {'<', TokenType.LESS},
            {',', TokenType.COMMA},
            {'.', TokenType.DOT},
            {':', TokenType.COLON},
            {';', TokenType.SEMICOLON},
            {'(', TokenType.LPAR},
            {')', TokenType.RPAR},
            {'\n', TokenType.ENDLINE},
            //{'_', TokenType.UNDERSCORE}
        };
        
        
        public TokenType Type;
        public string Value;
        public Token(TokenType token)
        {
            Type = token;
        }
        public Token(string word)
        {

            if (int.TryParse(word, out int a))
            {
                Value = word;
                Type = TokenType.LITERAL;
            }
            else if (word.Length == 1 && IsSpecialSymbol(word[0])) Type = SpecialSymbols[word[0]];
            else if (IsSpecialWord(word)) Type = SpecialWords[word];
            else if (isIdentifier(word))
            {
                Type = TokenType.IDENTIFIER;
                Value = word;
            }
            else
            {
                Type = TokenType.TOKEN_ERROR;
                Value = word;
            }
        }
        public static bool isIdentifier(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            if (
                word[0] >= 65   && word[0] <= 90   || //A-Z
                word[0] >= 97   && word[0] <= 122  || //a-z
                word[0] >= 1040 && word[0] <= 1103 || //А-Я а-я
                word[0] == 'Ё'  || word[0] == 'ё'  || word[0] == '_'
                )
            {
                for(int i = 1; i < word.Length; i++)
                {
                    if (!(
                        word[0] >= 65   && word[0] <= 90   || //A-Z
                        word[0] >= 97   && word[0] <= 122  || //a-z
                        word[0] >= 1040 && word[0] <= 1103 || //А-Я а-я
                        word[0] == 'Ё'  || word[0] == 'ё'  || word[0] == '_' ||
                        word[0] >= 48   && word[0] <= 57   )) //0-9
                        return false;
                }
            }
            else
                return false;
            return true;
        }
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            return SpecialWords.ContainsKey(word);
        }
        public static bool IsDelimiter(Token token) => Delimiters.Contains(token.Type);
        public static bool IsSpecialSymbol(char ch) => SpecialSymbols.ContainsKey(ch);
        public override string ToString()
        {
            if(Type == TokenType.TOKEN_ERROR)
                return Value + " - is an identifier with an error";
            if(Value != null)
                return Type + " - " + Value;
            return Type.ToString();
        }
    }
}