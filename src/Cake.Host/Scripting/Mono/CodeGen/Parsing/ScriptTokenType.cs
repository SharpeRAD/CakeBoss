namespace Cake.Host.Scripting.Mono.CodeGen.Parsing
{
    public enum ScriptTokenType
    {
        Word = 0,
        If = 1,
        Else = 2,
        While = 3,
        Switch = 4,
        String = 5,
        Semicolon = 6,
        LeftBrace = 7,
        RightBrace = 8,
        LeftParenthesis = 9,
        RightParenthesis = 10,
        Character = 11
    }
}