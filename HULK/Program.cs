using System.Text.RegularExpressions;
using System.Text;
class HULK : Expresiones
{
    static void Main(string[] args)
    {
        while (true)
        {
            string instruction = Console.ReadLine();

            if (instruction.IndexOf(">") == 0 && instruction.LastIndexOf(";") == instruction.Length - 1)
            {
                instruction = instruction.Remove(0, 1);
                instruction = instruction.Remove(instruction.Length - 1);

                string imprimir = Identifier(instruction);
                if (imprimir.IndexOf('"') == 0 && imprimir.LastIndexOf('"') == imprimir.Length - 1)
                {
                    imprimir = imprimir.Remove(0, 1);
                    imprimir = imprimir.Remove(imprimir.Length - 1);
                }
                Console.WriteLine(imprimir);

            }
            else
            {
                if (instruction.IndexOf(">") != 0 && instruction.LastIndexOf(";") != instruction.Length - 1)
                    Console.WriteLine("Se esperaba \'>\'\nSe esperaba \';\'");
                else if (instruction.LastIndexOf(";") == instruction.Length - 1)
                    Console.WriteLine("Se esperaba \'>\'");
                else Console.WriteLine("Se esperaba \';\'");
            }
        }

    }
    public static string Identifier(string linea)
    {
        string instruction = linea.Trim();

        //Se trata de definir la linea introducida en un tipo específico. Si es reconocida por algún tipo, se manda a la clase determinada para ese tipo
        //Si es Function
        if (IsValid(instruction, function()).Success)
        {
            return Function.DeclararFunction(instruction);
        }//Si es let-in
        else if (IsValid(instruction, let_in()).Success)
        {
            return Identifier(Let_in.Return_Let_in(instruction));
        }
        //Si es Print
        else if (IsValid(instruction, print()).Success)
        {
            return Identifier(Print.TokenPrint(instruction));
        }
        //Si es if-else
        else if (IsValid(instruction, if_else()).Success)
        {
            return Identifier(If_else.ReturnIf_else(instruction));
        }
        //Comprobar si se está llamando a alguna de las funciones declaradas
        else if (Function.nombre_funciones.Count != 0)
        {
            instruction = Function.Call_me(instruction, 0);
        }
        //Si es string
        if (IsValid(instruction, text()).Success)
        {
            return Operar.OperationString(instruction);
        }
        //si es booleano
        else if (IsValid(instruction, BoolType1()).Success)
            return Operar.OperationBoolType1(instruction).ToString();

        //si es booleano
        else if (IsValid(Regex.Replace(instruction, @"\s*", ""), BoolType2()).Success)
        {
            return Operar.OperationBoolType2(Regex.Replace(instruction, @"\s*", "")).ToString();
        }
        //si es numero
        else if (IsValid(Regex.Replace(instruction, @"\s*", ""), Number()).Success)
        {
            return Operar.OperationNumber(Regex.Replace(instruction, @"\s*", ""));

        }
        //Sino se reconoció la linea. Se manda a un metodo que se ocupa de definir que error sintáctico se cometió
        else
        {
            if (IsValid(instruction, llamado_inexistente()).Success)
            {
                GroupCollection groups = IsValid(instruction, llamado_inexistente()).Groups;
                return "No existe ningún método declarado con el nombre " + groups["nombre_funcion"].Value.ToString();
            }

            return "Error sintactico: Expresión inválida";
        }


    }


}