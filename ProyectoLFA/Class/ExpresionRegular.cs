using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProyectoLFA.Class
{
    class ExpresionRegular
    {
        //EXPRESION PARA EVALUAR EL SET
        private static string SETS = @"^(\s*([A-Z])+\s*=\s*((((\'([A-Z]|[a-z]|[0-9]|_)\'\.\.\'([A-Z]|[a-z]|[0-9]|_)\')\+)*(\'([A-z]|[a-z]|[0-9]|_)\'\.+\'([A-z]|[a-z]|[0-9]|_)\')*(\'([A-z]|[a-z]|[0-9]|_)\')+)|(CHR\(+([0-9])+\)+\.\.CHR\(+([0-9])+\)+)+)\s*)";

        //EXPRESION PARA EVALUAR EL TOKEN
        private static string TOKENS = @"^(\s*TOKEN\s*[0-9]+\s*=\s*(([A-Z]+)|((\'*)([a-z]|[A-Z]|[1-9]|(\<|\>|\=|\+|\-|\*|\(|\)|\{|\}|\[|\]|\.|\,|\:|\;))(\'))+|((\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9]|\')*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*([A-Z]|[a-z]|[0-9])*\s*\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*\{*\s*([A-Z]|[a-z]|[0-9])*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*(\||\'|\*|\?|\[|\]|\{|\}|\(|\)|\\)*\s*)+)+)";

        //EXPERSIONES REGULARES PARA FUNCIONES Y ERRORES    
        private static string ACTIONSANDERRORS
           = @"^((\s*RESERVADAS\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*)+|}+\s*|(\s*([A-Z]|[a-z]|[0-9])\s*\(\s*\)\s*)+|{+\s*|(\s*[0-9]+\s*=\s*'([A-Z]|[a-z]|[0-9])+'\s*|}+\s)*(\s*ERROR\s*=\s*[0-9]+\s*))$";
        public static string Archivo(string data, ref int line)
        {
            

            string mensaje = "";

            bool first = true;
            bool setExists = false;
            bool tokenExists = false;
            bool actionExists = false;

            int actionCount = 0;
            int actionsError = 0;
            int tokenCount = 0;
            int setCount = 0;

            string[] lines = data.Split('\n');
            int count = 0;

            foreach (var item in lines)
            {
                count++;
                if (!string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item))
                {
                    if (first)
                    {
                        first = false;
                        if (item.Contains("SETS"))
                        {
                            setExists = true;
                            mensaje = "Formato Correcto";
                        }
                        else if (item.Contains("TOKENS"))
                        {
                            tokenExists = true;
                            mensaje = "Formato Correcto";
                        }
                        else
                        {
                            line = 1;
                            return "Error en linea 1: Se esperaba SETS o TOKENS";
                        }
                    }
                    else if (setExists)
                    {
                        Match setMatch = Regex.Match(item, SETS);
                        if (item.Contains("TOKENS"))
                        {
                            if (setCount < 1)
                            {
                                line = count;
                                return "Error: Se esperaba almenos un SET";
                            }
                            setExists = false;
                            tokenExists = true;
                        }
                        else
                        {
                            if (!setMatch.Success)
                            {
                                return $"Error en linea: {count}";
                            }
                            tokenCount++;
                        }

                        setCount++;
                    }
                    else if (tokenExists)
                    {
                        Match m = Regex.Match(item, TOKENS);
                        if (item.Contains("ACTIONS"))
                        {
                            if (tokenCount < 1)
                            {
                                line = count;
                                return "Error: Se esperaba almenos un TOKEN";
                            }
                            actionCount++;
                            tokenExists = false;
                            actionExists = true;
                        }
                        else
                        {
                            if (!m.Success)
                            {
                                return $"Error en linea: {count}";
                            }
                            tokenCount++;
                        }
                    }
                    else if (actionExists)
                    {
                        if (item.Contains("ERROR"))
                        {
                            actionsError++;
                        }
                        Match actMatch = Regex.Match(item, ACTIONSANDERRORS);
                        if (!actMatch.Success)
                        {
                            return $"Error en linea: {count}";
                        }
                    }
                }
            }
            if (actionCount < 1)
            {
                return $"Error: Se esperaba la sección de ACTIONS";
            }
            if (actionsError < 1)
            {
                return $"Error: Se esperaba una la sección de ERROR";
            }
            line = count;
            return mensaje;
        }
    }
}
