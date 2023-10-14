using System;
using System.Reflection;
using System.Text.RegularExpressions;

public class Expresiones
{
    public static Match IsValid(string instruction, string patronER)
    {
        Regex regex = new Regex(patronER);
        Match match = regex.Match(instruction);

        return match;
    }

   public static string function()
    {
        return @"^function\s+(?<nombre_funcion>(.*))\((?<parametros>(.*))\)\s*=>(?<cuerpo_funcion>(.*))$";
    }
    public static string if_else()
    {
        return @"if\s*\((?<expresion_boleana>.*)\)\s*(?<condicion_if>.*)else\s*(?<condicion_else>.*)$";
    }
    public static string text()
    {
        return "^\\\".*?\\\"(@+(\\\".*?\\\"))*?$";
    }
    public static string BoolType2()
    {
        return @"^([\(\-+)])*\d+(\.\d+)?([)])*(([\(\-\+\*\/\^\)])*\d+(\.\d+)?([)])*)*([^<>!=]*([<>!]=?|==)[^<>!=]*)([\(\-+)])*\d+(\.\d+)?([)])*(([\(\-\+\*\/\^\)])*\d+(\.\d+)?([)])*)*(\s*(&|\|)\s*([\(\-+)])*\d+(\.\d+)?([)])*(([\(\-\+\*\/\^\)])*\d+(\.\d+)?([)])*)*([^<>!=]*([<>!]=?|==)[^<>!=]*)([\(\-+)])*\d+(\.\d+)?([)])*(([\(\-\+\*\/\^\)])*\d+(\.\d+)?([)])*)*)*?$";
    }
    public static string BoolType1()
    {
        return @"^(!*(true|True|false|False)\s*((&|\|)\s*!*(true|True|false|False))*?)$";
    }
    public static string Number()
    {
        return @"^([\(\-+)])*\d+(\.\d+)?([)])*(([\(\-\+\*\/\^\%\)])*\d+(\.\d+)?([)])*)*$";
    }
    public static string let_in()
    {
        return @"let\s+(.*)=(.*)";
    }
    public static string print()
    {
        return @"print\((?<cuerpo_print>.*)\)";
    }
    public static string llamado_inexistente()
    {
        return @"(?<nombre_funcion>[a-zA-Z_][a-zA-Z0-9_]*)\s*\((.*)\)";
    }

}