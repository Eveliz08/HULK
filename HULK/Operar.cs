public class Operar
{
    public static string OperarNumeros(string equation)
    {
        int parentesis = 0;
        bool hay_numeros = false;
        List<char> operadores = new List<char>();
        List<string> op_prefija = new List<string>();
        int derecha = 0;
        int izquierda = 0;

         //En este método se implenta un algoritmo para solucionar ecuaciones aritméticas. Este consiste en parsear la ecuación y ordenarla según la jerarquía de las operadores, incluyendo paréntesis.
         //Es decir cuando termine este ciclo for va a quedar la equación ordenada, sin paréntisis, en una lista.
         //Ejemplo: (5*4+3*2)-1 quedaría: 5 4 * 3 2 * + 1 - 
        
        //Un caso como: -(-2*1) se sale del algoritmo implementado en este método para solucionar ecuaciones aritméticas.
       // Casos como: -2 o (-2*1) son solucionados por el algoritmo porwue se toma el "-" como parte del número.
       
        //Por lo tanto el caso de -(-2*1) se analiza en específico en las siguientes líneas de código. en la que se cambia "-" por "-1*"
        if(equation[0]=='-' && equation[1]=='(' )
        {
            equation = equation.Remove(0,1);
            equation = equation.Insert(0, "-1*");
        }

        for (int i = 0; i < equation.Length; i++)
        {
            AnalizarCaracteres(i);

            void AnalizarCaracteres(int i)
            {
                if (equation[i] == '(')
                {
                    AnalizarNumeros();
                    operadores.Add(equation[i]);
                    parentesis++;
                }
                else if (equation[i] == '^')
                {
                    AnalizarNumeros();
                    if (operadores.Count > 0 && operadores[operadores.Count - 1] == '^')
                        op_prefija.Add(equation[i].ToString());
                    else
                        operadores.Add(equation[i]);
                }
                else if (equation[i] == '*' || equation[i] == '/' || equation[i] == '%')
                {
                    AnalizarNumeros();

                    if (operadores.Count > 0 && (operadores[operadores.Count - 1] == '^' || operadores[operadores.Count - 1] == '*' || operadores[operadores.Count - 1] == '/' || operadores[operadores.Count - 1] == '%'))
                    {
                        op_prefija.Add(operadores[operadores.Count - 1].ToString());
                        operadores.RemoveAt(operadores.Count - 1);
                        AnalizarCaracteres(i);
                    }
                    else
                    {
                        operadores.Add(equation[i]);
                    }
                }
                else if (equation[i] == '-' && (i == 0 || equation[i - 1] == '('))
                {
                    hay_numeros = true;

                    derecha = i;
                    izquierda++;
                }
                else if (equation[i] == '+' || equation[i] == '-')
                {
                    AnalizarNumeros();
                    if (operadores.Count == 0 || operadores[operadores.Count - 1] == '(')
                    {
                        operadores.Add(equation[i]);
                    }
                    else
                    {
                        op_prefija.Add(operadores[operadores.Count - 1].ToString());
                        operadores.RemoveAt(operadores.Count - 1);
                        AnalizarCaracteres(i);
                    }
                }
                else if (equation[i] == ')' && parentesis > 0)
                {
                    AnalizarNumeros();
                    parentesis--;
                    int recorrerLista = operadores.Count - 1;
                    while (operadores[recorrerLista] != '(')
                    {
                        op_prefija.Add(operadores[recorrerLista].ToString());
                        operadores.RemoveAt(recorrerLista);
                        recorrerLista--;
                    }

                    operadores.RemoveAt(recorrerLista);

                }
                else if (equation[i] == ')')
                {
                    op_prefija.Insert(0, "Se esperaba (");
                    i = equation.Length - 1;
                    parentesis--;
                }
                else if (equation[i] == '0' || equation[i] == '1' || equation[i] == '2' || equation[i] == '3' || equation[i] == '4' || equation[i] == '5' || equation[i] == '6' || equation[i] == '7' || equation[i] == '8' || equation[i] == '9')
                {
                    if (!hay_numeros)
                    {
                        hay_numeros = true;

                        derecha = i;
                        izquierda++;
                    }
                    else
                    {
                        izquierda++;
                    }
                }
                else if (equation[i] == '.' && hay_numeros)
                {
                    izquierda++;
                }

            }
        }


        void AnalizarNumeros()
        {
            if (hay_numeros)
            {
                op_prefija.Add(equation.Substring(derecha, izquierda));
                hay_numeros = false;

                izquierda = 0;
            }

        }

        AnalizarNumeros();

        if (parentesis == 0 && operadores.Count > 0)
            for (int j = operadores.Count - 1; j >= 0; j--)
                op_prefija.Add(operadores[j].ToString());
        else if (parentesis > 0)
            return "Se esperaba )";
        else if (parentesis < 0)
            return "Se esperaba (";


        Calculadora(0);
        void Calculadora(int i)
        {
            try
            {
                if (i != 0 && (op_prefija[i] == "^" || op_prefija[i] == "*" || op_prefija[i] == "/" || op_prefija[i] == "+" || op_prefija[i] == "-" || op_prefija[i] == "%"))
                {
                    double x = Convert.ToDouble(op_prefija[i - 2]);
                    double y = Convert.ToDouble(op_prefija[i - 1]);
                    double resultado = 0.0;

                    switch (op_prefija[i])
                    {
                        case "+":
                            resultado = x + y;
                            break;

                        case "-":
                            resultado = x - y;
                            break;

                        case "*":
                            resultado = x * y;
                            break;

                        case "/":
                            resultado = x / y;

                            break;

                        case "^":
                            resultado = Math.Pow(x, y);
                            break;

                        case "%":
                            resultado = x % y;
                            break;
                    }

                    op_prefija.Insert(i + 1, resultado.ToString());
                    op_prefija.RemoveAt(i - 2);
                    op_prefija.RemoveAt(i - 2);
                    op_prefija.RemoveAt(i - 2);

                    Calculadora(0);
                }

                if (op_prefija.Count > 1)
                    Calculadora(i + 1);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                op_prefija.Insert(0, "Se esperaba un token numérico");
            }
        }

        if(op_prefija[0] == "-")
        {
            double x = Convert.ToDouble(op_prefija[1]);
            x = -x;
            return x.ToString();
        }
        

        return op_prefija[0];
    }

      //Los boleanos de tipo 1 son las estructuras boleanas con true y false
    public static bool OperarBoleanosTipo1(string instruction)
    {
        char[] separadores = new char[] { '&', '|' };

        if (instruction.Contains(separadores[0]) || instruction.Contains(separadores[1]))
        {
            string[] Tbool = instruction.Split(separadores);
            bool uno = Contador(Tbool[0]);
            bool dos = Contador(Tbool[1]);

            if (instruction.Contains(separadores[0])) return uno && dos;
            else return uno || dos;
        }
        else return Contador(instruction);

        bool Contador(string instruction)
        {
            int contador = 0;
            int i = 0;
            bool resultado = false;

            if (instruction.Contains("true") || instruction.Contains("True"))
                resultado = true;

            while (instruction[i] == '!')
            {
                contador++;
                i++;
            }

            if (contador % 2 == 0) return resultado;
            else return !resultado;
        }
    }


    //Los boleanos tipo 2 son las expresiones aritméticas implementadas con los operadores: "==", "!=", "<", ">", "<=", ">=".
    public static bool OperarBoleanosTipo2(string instruction)
    {
        int parentesis = 0;
        int derecha = 0;
        int izquierda = 0;
        bool hay_boleano = false;
        List<string> operaciones_boleanas = new List<string>();
        List<char> operadores = new List<char>();

     //Se utiliza el mismo algoritmo que se implenta para solucionar ecuaciones aritméticas
        for (int i = 0; i < instruction.Length; i++)
        {
            AnalizarCaracteres();
            void AnalizarCaracteres()
            {
                if (instruction[i] == '(')
                {
                    if (instruction.IndexOf('|', i, Utiles1.Parentesis_que_cierra(i, instruction) - i) != -1 || instruction.IndexOf('&', i, Utiles1.Parentesis_que_cierra(i, instruction) - i) != -1)
                    {
                        AnalizarBoleano();
                        operadores.Add(instruction[i]);
                        parentesis++;
                    }
                    else
                    {
                        if (!hay_boleano)
                        {
                            hay_boleano = true;

                            derecha = i;
                            izquierda = Utiles1.Parentesis_que_cierra(i, instruction) - i;
                        }
                        else
                        {
                            izquierda += Utiles1.Parentesis_que_cierra(i, instruction) - i+1;
                        }
                        i = Utiles1.Parentesis_que_cierra(i, instruction)+1;
                    }
                }
                else if (instruction[i] == ')')
                {
                    AnalizarBoleano();
                    operaciones_boleanas.Add(operadores[operadores.Count - 1].ToString());
                    operadores.RemoveAt(operadores.Count - 1);
                    operadores.RemoveAt(operadores.Count - 1);

                    parentesis--;

                }
                else if (instruction[i] == ')')
                {
                    operaciones_boleanas.Insert(0, "Se esperaba (");
                    i = instruction.Length - 1;
                    parentesis--;
                }
                else if (instruction[i] == '|' || instruction[i] == '&')
                {
                    AnalizarBoleano();
                    if (operadores.Count > 0 && operadores[operadores.Count - 1] != '(')
                    {
                        operaciones_boleanas.Add(operadores[operadores.Count - 1].ToString());
                        operadores.RemoveAt(operadores.Count - 1);
                        operadores.Add(instruction[i]);
                    }
                    else
                        operadores.Add(instruction[i]);
                }
                else
                {
                    if (!hay_boleano)
                    {
                        hay_boleano = true;

                        derecha = i;
                        izquierda++;
                    }
                    else
                    {
                        izquierda++;
                    }
                }

            }


        }
        void AnalizarBoleano()
        {
            if (hay_boleano)
            {
                operaciones_boleanas.Add(CalculadoraBoleana(instruction.Substring(derecha, izquierda)).ToString());
                hay_boleano = false;

                izquierda = 0;
            }

        }

        AnalizarBoleano();

        if (parentesis == 0 && operadores.Count > 0)
            for (int j = operadores.Count - 1; j >= 0; j--)
                operaciones_boleanas.Add(operadores[j].ToString());

        RecorrerLista(0);
        void RecorrerLista(int i)
        {
            if (i < operaciones_boleanas.Count)
            {
                if (operaciones_boleanas[i] == "|" || operaciones_boleanas[i] == "&")
                {
                    string resultado = operaciones_boleanas[i - 2] + operaciones_boleanas[i] + operaciones_boleanas[i - 1];
                    resultado = OperarBoleanosTipo1(resultado.Trim()).ToString();
                    operaciones_boleanas[i] = resultado;
                    operaciones_boleanas.RemoveAt(i - 1);
                    operaciones_boleanas.RemoveAt(i - 2);
                    RecorrerLista(0);

                }
                RecorrerLista(i + 1);
            }

        }

        return OperarBoleanosTipo1(operaciones_boleanas[0]);

        bool CalculadoraBoleana(string linea)
        {

            string[] separadores = new string[] { " <= ", ">=", "==", "!=", "<", ">" };

            string[] Tbool = linea.Split(separadores, 2, StringSplitOptions.RemoveEmptyEntries);
            
            double uno = Convert.ToDouble(OperarNumeros(Tbool[0]));
            double dos = Convert.ToDouble(OperarNumeros(Tbool[1]));

            

            if (linea.Contains("<="))
                return uno <= dos;
            else if (linea.Contains(">="))
                return uno >= dos;
            else if (linea.Contains("=="))
                return uno == dos;
            else if (linea.Contains("!="))
                return uno != dos;
            else if (linea.Contains("<"))
                return uno < dos;
            else
                return uno > dos;

        }
    }

//En este método no se quitan las comillas de principio y fin de un string, esto se debe a recursividad de los métodos.
//En la declaración de una variable por ejemplo puede haber otras estrcturas, por lo tanto se manda a analizar como una instrucción. 
//Por tanto si se quitan las comillas en este punto las va a quitar siempre aunque sea un string de una declaración de variable, por lo tanto esto va a dar un error posteriormente en el código.
    public static string OperarString(string instruction)
    {
        string[] Tstring = instruction.Split(new char[] { '@' });
        if (Tstring.Length > 1)
        {
            for (int i = 0; i < Tstring.Length; i++)
            {
                Tstring[i] = Tstring[i].Trim();
                if (i == 0) Tstring[i] = Tstring[i].Remove(Tstring[i].Length - 1);
                else if (i == Tstring.Length - 1) Tstring[i] = Tstring[i].Remove(0, 1);
                else
                {
                    Tstring[i] = Tstring[i].Remove(Tstring[i].Length - 1);
                    Tstring[i] = Tstring[i].Remove(0, 1);
                }
            }

        }
        return string.Concat(Tstring);
    }

}