using System.Text.RegularExpressions;
public class Function : Utiles2
{
    public static string DeclararFunction(string instruction)
    {


        Match captura = Regex.Match(instruction, Expresiones.function());
        GroupCollection groups = captura.Groups;

        if (!Es_un_nombre_valido(groups["nombre_funcion"].ToString().Trim()))
            return "\"Se esperaba un identificador, " + groups["nombre_funcion"].ToString() + " identificador inválido\"";
        if (Es_palabra_reservada(groups["nombre_funcion"].ToString().Trim()))
            return "\"No se pueden utilizar palabras reservadas como identificadores, " + groups["nombre_funcion"].ToString() + "\"";

        cuerpo_funcion.Add(groups["cuerpo_funcion"].ToString());
        nombre_funciones.Add(groups["nombre_funcion"].ToString().Trim());
        string[] parametros_introducidos = groups["parametros"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < parametros_introducidos.Length; i++)
            if (!Es_un_nombre_valido(parametros_introducidos[i]))
                return "\"Se esperaba un identificador, " + parametros_introducidos[i] + " identificador inválido\"";
            else if (Es_palabra_reservada(groups["nombre_funcion"].ToString().Trim()))
                return "\"No se pueden utilizar palabras reservadas como identificadores, " + parametros_introducidos[i] + "\"";


        parametros.Add(parametros_introducidos);

        return "";
    }


    public static string Call_me(string instruction, int i)
    {
        if (i >= nombre_funciones.Count) { return instruction; }

        int inicio_del_llamado = 0;
        int inicio_de_palametros = 0;
        int fin_llamada = 0;

        string patron = Regex.Escape(nombre_funciones[i]) + @"\((.*)\)";
        Match match = Regex.Match(instruction, patron);

        if (!match.Success) { return Call_me(instruction, i + 1); }

        for (int j = instruction.Length - 1; j >= 0; j--)
        {
            if (instruction[j] == nombre_funciones[i][nombre_funciones[i].Length - 1])
            {
                for (int k = 0; k < nombre_funciones[i].Length; k++)
                {
                    if (!(instruction[j - k] == nombre_funciones[i][nombre_funciones[i].Length - 1 - k]))
                        break;

                    if (k == nombre_funciones[i].Length - 1)
                    {

                        inicio_del_llamado = j - k;
                        inicio_de_palametros = instruction.IndexOf('(', inicio_del_llamado);//+1


                        fin_llamada = Parentesis_que_cierra(inicio_de_palametros, instruction);

                        if (fin_llamada == 0)
                            return "\"Se esperaba )\"";

                        string respuesta_al_llamado = Return_call(instruction.Substring(inicio_de_palametros + 1, fin_llamada - inicio_de_palametros - 1), i);

                        instruction = instruction.Remove(inicio_del_llamado, fin_llamada - inicio_del_llamado + 1);
                        instruction = instruction.Insert(inicio_del_llamado, respuesta_al_llamado);

                        match = Regex.Match(instruction, patron);
                        if (!match.Success)
                            goto endLoop;
                        else
                            return Call_me(instruction, i);

                    }
                }
            }
        }
    endLoop:


        return Call_me(instruction, i + 1);
    }



    public static string Return_call(string llamado, int i)
    {
        llamado = Call_me(llamado, i + 1);
        int izq = 0;
        int derecha = 0;
        List<string> parametros_introducidos = new List<string>();
        bool let = Expresiones.IsValid(llamado, Expresiones.let_in()).Success;
        bool let_in = true;

        for (int k = 0; k < llamado.Length; k++)
        {
            if (llamado[k] == '"')
                k = Be_Ignorant(llamado, k);

            if (!let)
            {
                if (llamado[k] == ',')
                {
                    derecha = k;
                    string parametros = llamado.Substring(izq, derecha - izq);
                    parametros_introducidos.Add(parametros);
                    izq = derecha + 1;
                }
            }
            else
            {
                if (llamado[k] == 'l' && llamado[k + 1] == 'e' && llamado[k + 2] == 't')
                    let_in = false;

                if (llamado[k] == 'i' && llamado[k + 1] == 'n')
                    let_in = true;

                if (llamado[k] == ',' && let_in)
                {
                    derecha = k - 1;
                    string parametros = llamado.Substring(izq, derecha - izq);
                    parametros_introducidos.Add(parametros);

                    izq = derecha + 1;
                }
            }
            if (k == llamado.Length - 1)
            {
                string parametros = llamado.Substring(izq);
                parametros_introducidos.Add(parametros);
            }

        }


        if (parametros[i].Length == parametros_introducidos.Count)
        {
            llamado = cuerpo_funcion[i];
            for (int j = 0; j < parametros_introducidos.Count; j++)
                llamado = Variable_X_Valor(llamado, parametros[i][j], parametros_introducidos[j]);
        }
        else
        {
            if (parametros[i].Length > parametros_introducidos.Count)
            {
                string Concatenar(string[] parametros, int j)
                {
                    if (j >= parametros.Length) return " ";
                    return parametros[j] + Concatenar(parametros, j + 1);
                }
                return "\"" + "No se ha dado ningún argumento que corresponda al parámetro requerido" + Concatenar(parametros[i], parametros_introducidos.Count) + "de " + nombre_funciones[i] + "\"";

            }
            else
            {
                return "\"" + "Ninguna sobrecarga para el método " + nombre_funciones[i] + " toma " + parametros_introducidos.Count.ToString() + " argumentos" + "\"";
            }
        }



        return HULK.Identifier(llamado);


    }


    public static List<string> nombre_funciones = new List<string>();
    public static List<string[]> parametros = new List<string[]>();
    public static List<string> cuerpo_funcion = new List<string>();

}

