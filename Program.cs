using System;
using System.Linq;
using System.Collections;

string strToDo = "Polímero";
string strTmp = "";
string strSilaba = "";
int lastPos = 0;
Silabica sbl = new Silabica();
Console.Write("'{0}' = ", strToDo);

try
{
    
    ArrayList silabas = sbl.PosicionSilabas(strToDo);

    foreach (int intPos in silabas)
    {
        if (intPos > 0)
        {
            strTmp = strToDo.Substring(lastPos, intPos - lastPos);
            strSilaba = strSilaba + strTmp + "-";
        }
        lastPos = intPos;
    }

    strTmp = strToDo.Substring(lastPos);
    strSilaba = strSilaba + strTmp;

    Console.WriteLine(strSilaba);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

class Silabica
{
    
    /// <summary>
    /// Thanks to the code published by Dimkir.
    /// https://github.com/dimkir/cyrillizer/blob/master/doc/SeparadorDeSilabas.cs
    /// </summary>
    
    const int MAX_SILABAS = 20;

    private int lonPal;
    // Longitud de la palabra
    private int numSil;
    // Número de silabas de la palabra
    private int tonica;
    // Posición de la silaba tónica (empieza en 1)
    private bool encTonica;
    // Indica si se ha encontrado la silaba tónica
    private int letraTildada;
    // Posición de la letra tildda, si la hay 
    private ArrayList posiciones;
    // Posiciones de inicio de las silabas
    private String ultPal;
    // Última palabra tratada, se guarda para
    // no repetir el proceso si se pide la misma
    // En la mayoría de las lenguas, las palabras pueden dividirse en sílabas
    // que constan de un núcleo silábico, un ataque que antecede al núcleo
    // silábico y una coda que sigue al núcleo silábico
    // (http://es.wikipedia.org/wiki/Sílaba)


    /// <summary>
    ///     ''' Constructor
    ///     ''' </summary>
    public Silabica()
    {
        ultPal = String.Empty;
        posiciones = new ArrayList();
    }

    /// <summary>
    ///     ''' Devuelve un array con las posiciones de inicio de las sílabas de palabra
    ///     ''' </summary>
    ///     ''' <param name="palabra"></param>
    ///     ''' <returns></returns>
    public ArrayList PosicionSilabas(String palabra)
    {
        Calcular(palabra);
        return posiciones;
    }

    /// <summary>
    ///     ''' Devuelve el número de silabas de palabra
    ///     ''' </summary>
    ///     ''' <param name="palabra"></param>
    ///     ''' <returns></returns>
    public int NumeroSilabas(String palabra)
    {
        Calcular(palabra);
        return numSil;
    }

    /// <summary>
    ///     ''' Devuelve la posición de la sílaba tónica de palabra
    ///     ''' </summary>
    ///     ''' <param name="palabra"></param>
    ///     ''' <returns></returns>
    public int SilabaTonica(String palabra)
    {
        Calcular(palabra);
        return tonica;
    }
    /// <summary>
    ///     ''' Determina si una palabra está correctamente tildada
    ///     ''' </summary>
    ///     ''' <param name="silabeo"></param>
    ///     ''' <param name="palabra"></param>
    ///     ''' <returns>
    ///     ''' 0 - bien tildada
    ///     ''' 7 - varias tildes en la palabra
    ///     ''' 8 - aguda mal tildada
    ///     ''' 9 - llana mal tildada
    ///     ''' </returns>
    public int BienTildada(ArrayList silabeo, string palabra)
    {
        int numSilabas = System.Convert.ToInt32(silabeo[0]);

        // Comprueba si hay má de una tilde en la palabra
        if (palabra.ToLower().Count(TieneTilde) > 1)
            return 7;
        int posTónica = System.Convert.ToInt32(silabeo[numSilabas + 1]);

        if (numSilabas - posTónica < 2)
        {
            // Si la palabra no es esdrújula
            char ultCar = palabra[palabra.Length - 1];
            int final = (posTónica < numSilabas ? System.Convert.ToInt32(silabeo[posTónica + 1]) : palabra.Length) - System.Convert.ToInt32(silabeo[posTónica]);
            string silaba = palabra.Substring(System.Convert.ToInt32(silabeo[posTónica]), final).ToLower();
            int i;

            // Se busca si hay tilde en la sílaba tónica
            for (i = 0; i <= silaba.Length - 1; i++)
            {
                if ("áéíóú".IndexOf(silaba[i]) > -1)
                    break;
            }

            if (i < silaba.Length)
            {
                // Hay tilde en la sílaba tónica
                // La palabra es aguda y no termina en n, s, vocal -> error
                if ((posTónica == numSilabas) && ("nsáéíióúu".IndexOf(ultCar) == -1))
                    return 8;

                // La palabra es llana y termina en n, s, vocal -> error
                if ((posTónica == numSilabas - 1) && ("nsaeiou".IndexOf(ultCar) != -1))
                    return 9;
            }
        }

        return 0;
    }

    // *******************************************************

    // *******************************************************

    // *             OPERACIONES PRIVADAS                    *

    // *******************************************************

    // *******************************************************


    /// <summary>
    ///     ''' Determina si un caracter está tildado.
    ///     ''' </summary>
    ///     ''' <param name="c"></param>
    ///     ''' <returns></returns>
    private bool TieneTilde(char c)
    {
        if ("áéíóú".IndexOf(c) != -1)
            return true;
        else
            return false;
    }

    /// <summary>
    ///     ''' Determina si hay que llamar a PosicionSilabas (si palabra
    ///     ''' es la misma que la última consultada, no hace falta)
    ///     ''' </summary>
    ///     ''' <param name="palabra"></param>
    public void Calcular(String palabra)
    {
        if (palabra != ultPal)
        {
            ultPal = palabra.ToLower();
            PosicionSilabas();
        }
    }

    /// <summary>
    ///     ''' Determina si c es una vocal fuerte o débil acentuada
    ///     ''' </summary>
    ///     ''' <param name="c"></param>
    ///     ''' <returns></returns>
    private bool VocalFuerte(char c)
    {
        switch (c)
        {
            case 'a':
            case 'á':
            case 'A':
            case 'Á':
            case 'à':
            case 'À':
            case 'e':
            case 'é':
            case 'E':
            case 'É':
            case 'è':
            case 'È':
            case 'í':
            case 'Í':
            case 'ì':
            case 'Ì':
            case 'o':
            case 'ó':
            case 'O':
            case 'Ó':
            case 'ò':
            case 'Ò':
            case 'ú':
            case 'Ú':
            case 'ù':
            case 'Ù':
                {
                    return true;
                }
        }
        return false;
    }

    /// <summary>
    ///     ''' Determina si c no es una vocal
    ///     ''' </summary>
    ///     ''' <param name="c"></param>
    ///     ''' <returns></returns>
    private bool esConsonante(char c)
    {
        if (!VocalFuerte(c))
        {
            switch (c)
            {
                case 'i':
                case 'I':
                case 'u':
                case 'U':
                case 'ü':
                case 'Ü':
                    {
                        return false;
                    }
            }

            return true;
        }

        return false;
    }

    /// <summary>
    ///     ''' Determina si se forma un hiato
    ///     ''' </summary>
    ///     ''' <returns></returns>
    private bool Hiato()
    {
        char tildado = ultPal[letraTildada];

        // Sólo es posible que haya hiato si hay tilde
        if ((letraTildada > 1) && (ultPal[letraTildada - 1] == 'u') && (ultPal[letraTildada - 2] == 'q'))
            return false;
        // La 'u' de "qu" no forma hiato
        // El caracter central de un hiato debe ser una vocal cerrada con tilde

        if ((tildado == 'í') || (tildado == 'ì') || (tildado == 'ú') || (tildado == 'ù'))
        {
            if ((letraTildada > 0) && VocalFuerte(ultPal[letraTildada - 1]))
                return true;

            if ((letraTildada < (lonPal - 1)) && VocalFuerte(ultPal[letraTildada + 1]))
                return true;
        }

        return false;
    }

    /// <summary>
    ///     ''' Determina el ataque de la silaba de pal que empieza
    ///     ''' en pos y avanza pos hasta la posición siguiente al
    ///     ''' final de dicho ataque
    ///     ''' </summary>
    ///     ''' <param name="pal"></param>
    ///     ''' <param name="pos"></param>
    private int Ataque(String pal, int pos)
    {
        // Se considera que todas las consonantes iniciales forman parte del ataque

        char ultimaConsonante = 'a';
        while ((pos < lonPal) && ((esConsonante(pal[pos])) && (pal[pos] != 'y')))
        {
            ultimaConsonante = pal[pos];
            pos += 1;
        }

        // (q | g) + u (ejemplo: queso, gueto)

        if (pos < lonPal - 1)
        {
            if (pal[pos] == 'u')
            {
                if (ultimaConsonante == 'q')
                    pos += 1;
                else if (ultimaConsonante == 'g')
                {
                    char letra = pal[pos + 1];
                    if ((letra == 'e') || (letra == 'é') || (letra == 'i') || (letra == 'í'))
                        pos += 1;
                }
            }
            else
// La u con diéresis se añade a la consonante
if ((System.Convert.ToChar(pal[pos]) == 'ü') || (System.Convert.ToChar(pal[pos]) == 'Ü'))
            {
                if (ultimaConsonante == 'g')
                    pos += 1;
            }
        }

        return pos;
    }

    /// <summary>
    ///     ''' Determina el núcleo de la silaba de pal cuyo ataque
    ///     ''' termina en pos - 1 y avanza pos hasta la posición
    ///     ''' siguiente al final de dicho núcleo
    ///     ''' </summary>
    ///     ''' <param name="pal"></param>
    ///     ''' <param name="pos"></param>
    private int Nucleo(String pal, int pos)
    {
        int anterior = 0;
        // Sirve para saber el tipo de vocal anterior cuando hay dos seguidas
        // 0 = fuerte
        // 1 = débil acentuada
        // 2 = débil
        if (pos >= lonPal)
            return pos;
        // ¡¿No tiene núcleo?!
        // Se salta una 'y' al principio del núcleo, considerándola consonante

        if (pal[pos] == 'y')
            pos += 1;

        // Primera vocal

        if (pos < lonPal)
        {
            char c = pal[pos];
            switch (c)
            {
                case 'á':
                case 'Á':
                case 'à':
                case 'À':
                case 'é':
                case 'É':
                case 'è':
                case 'È':
                case 'ó':
                case 'Ó':
                case 'ò':
                case 'Ò':
                    {
                        letraTildada = pos;
                        encTonica = true;
                        anterior = 0;
                        pos += 1;
                        break;
                        break;
                    }

                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'o':
                case 'O':
                    {
                        anterior = 0;
                        pos += 1;
                        break;
                        break;
                    }

                case 'í':
                case 'Í':
                case 'ì':
                case 'Ì':
                case 'ú':
                case 'Ú':
                case 'ù':
                case 'Ù':
                case 'ü':
                case 'Ü':
                    {
                        letraTildada = pos;
                        anterior = 1;
                        pos += 1;
                        encTonica = true;
                        return pos;
                    }

                case 'i':
                case 'I':
                case 'u':
                case 'U':
                    {
                        anterior = 2;
                        pos += 1;
                        break;
                        break;
                    }
            }
        }

        // 'h' intercalada en el núcleo, no condiciona diptongos o hiatos

        bool hache = false;
        if (pos < lonPal)
        {
            if (pal[pos] == 'h')
            {
                pos += 1;
                hache = true;
            }
        }

        // Segunda vocal

        if (pos < lonPal)
        {
            char c = pal[pos];
            switch (c)
            {
                case 'á':
                case 'Á':
                case 'à':
                case 'À':
                case 'é':
                case 'É':
                case 'è':
                case 'È':
                case 'ó':
                case 'Ó':
                case 'ò':
                case 'Ò':
                    {
                        letraTildada = pos;
                        if (anterior != 0)
                            encTonica = true;
                        if (anterior == 0)
                        {
                            // Dos vocales fuertes no forman silaba
                            if (hache)
                                pos -= 1;
                            return pos;
                        }
                        else
                            pos += 1;

                        break;
                        break;
                    }

                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'o':
                case 'O':
                    {
                        if (anterior == 0)
                        {
                            // Dos vocales fuertes no forman silaba
                            if (hache)
                                pos -= 1;
                            return pos;
                        }
                        else
                            pos += 1;

                        break;
                        break;
                    }

                case 'í':
                case 'Í':
                case 'ì':
                case 'Ì':
                case 'ú':
                case 'Ú':
                case 'ù':
                case 'Ù':
                    {
                        letraTildada = pos;

                        if (anterior != 0)
                        {
                            // Se forma diptongo
                            encTonica = true;
                            pos += 1;
                        }
                        else if (hache)
                            pos -= 1;

                        return pos;
                    }

                case 'i':
                case 'I':
                case 'u':
                case 'U':
                case 'ü':
                case 'Ü':
                    {
                        if (pos < lonPal - 1)
                        {
                            // ¿Hay tercera vocal?
                            char siguiente = pal[pos + 1];
                            if (!esConsonante(siguiente))
                            {
                                char letraAnterior = pal[pos - 1];
                                if (letraAnterior == 'h')
                                    pos -= 1;
                                return pos;
                            }
                        }

                        // dos vocales débiles iguales no forman diptongo
                        if (pal[pos] != pal[pos - 1])
                            pos += 1;

                        return pos;
                    }
            }
        }

        // ¿tercera vocal?

        if (pos < lonPal)
        {
            char c = pal[pos];
            if ((c == 'i') || (c == 'u'))
            {
                // Vocal débil
                pos += 1;
                // Es un triptongo	
                return pos;
            }
        }

        return pos;
    }

    /// <summary>
    ///     ''' Determina la coda de la silaba de pal cuyo núcleo
    ///     ''' termina en pos - 1 y avanza pos hasta la posición
    ///     ''' siguiente al final de dicha coda
    ///     ''' </summary>
    ///     ''' <param name="pal"></param>
    ///     ''' <param name="pos"></param>
    private int Coda(String pal, int pos)
    {
        if ((pos >= lonPal) || (!esConsonante(pal[pos])))
            return pos;
        else
        {
            // No hay coda
            if (pos == lonPal - 1)
            {
                // Final de palabra
                pos += 1;
                return pos;
            }

            // Si sólo hay una consonante entre vocales, pertenece a la siguiente silaba

            if (!esConsonante(pal[pos + 1]))
                return pos;

            char c1 = pal[pos];
            char c2 = pal[pos + 1];

            // ¿Existe posibilidad de una tercera consonante consecutina?

            if ((pos < lonPal - 2))
            {
                char c3 = pal[pos + 2];

                if (!esConsonante(c3))
                {
                    // No hay tercera consonante
                    // Los grupos ll, lh, ph, ch y rr comienzan silaba
                    if ((c1 == 'l') && (c2 == 'l'))
                        return pos;
                    if ((c1 == 'c') && (c2 == 'h'))
                        return pos;
                    if ((c1 == 'r') && (c2 == 'r'))
                        return pos;

                    /// ////// grupos nh, sh, rh, hl son ajenos al español(DPD)
                    if ((c1 != 's') && (c1 != 'r') && (c2 == 'h'))
                        return pos;

                    // Si la y está precedida por s, l, r, n o c (consonantes alveolares),
                    // una nueva silaba empieza en la consonante previa, si no, empieza en la y

                    if ((c2 == 'y'))
                    {
                        if ((c1 == 's') || (c1 == 'l') || (c1 == 'r') || (c1 == 'n') || (c1 == 'c'))
                            return pos;

                        pos += 1;
                        return pos;
                    }

                    // dante caso
                    if ((c2 == 'ã') || (c1 == 'õ'))
                    {
                        if (esConsonante(c1))
                            return pos;

                        pos += 1;
                        return pos;
                    }

                    // gkbvpft + l

                    if ((((c1 == 'b') || (c1 == 'v') || (c1 == 'c') || (c1 == 'k') || (c1 == 'f') || (c1 == 'g') || (c1 == 'p') || (c1 == 't')) && (c2 == 'l')))
                        return pos;

                    // gkdtbvpf + r

                    if ((((c1 == 'b') || (c1 == 'v') || (c1 == 'c') || (c1 == 'd') || (c1 == 'k') || (c1 == 'f') || (c1 == 'g') || (c1 == 'p') || (c1 == 't')) && (c2 == 'r')))
                        return pos;

                    pos += 1;
                    return pos;
                }
                else
                {
                    // Hay tercera consonante
                    if ((pos + 3) == lonPal)
                    {
                        // Tres consonantes al final ¿palabras extranjeras?
                        if ((c2 == 'y'))
                        {
                            // 'y' funciona como vocal
                            if ((c1 == 's') || (c1 == 'l') || (c1 == 'r') || (c1 == 'n') || (c1 == 'c'))
                                return pos;
                        }

                        if (c3 == 'y')
                            // 'y' final funciona como vocal con c2
                            pos += 1;
                        else
                            // Tres consonantes al final ¿palabras extranjeras?
                            pos += 3;
                        return pos;
                    }

                    if ((c2 == 'y'))
                    {
                        // 'y' funciona como vocal
                        if ((c1 == 's') || (c1 == 'l') || (c1 == 'r') || (c1 == 'n') || (c1 == 'c'))
                            return pos;

                        pos += 1;
                        return pos;
                    }

                    // Los grupos pt, ct, cn, ps, mn, gn, ft, pn, cz, tz, ts comienzan silaba (Bezos)

                    if ((c2 == 'p') && (c3 == 't') || (c2 == 'c') && (c3 == 't') || (c2 == 'c') && (c3 == 'n') || (c2 == 'p') && (c3 == 's') || (c2 == 'm') && (c3 == 'n') || (c2 == 'g') && (c3 == 'n') || (c2 == 'f') && (c3 == 't') || (c2 == 'p') && (c3 == 'n') || (c2 == 'c') && (c3 == 'z') || (c2 == 't') && (c3 == 's') || (c2 == 't') && (c3 == 's'))
                    {
                        pos += 1;
                        return pos;
                    }

                    // Los grupos consonánticos formados por una consonante
                    // seguida de 'l' o 'r' no pueden separarse y siempre inician
                    // sílaba 
                    // 'ch'
                    if ((c3 == 'l') || (c3 == 'r') || ((c2 == 'c') && (c3 == 'h')) || (c3 == 'y'))
                        // 'y' funciona como vocal
                        // Siguiente sílaba empieza en c2
                        pos += 1;
                    else
                        pos += 2;
                }
            }
            else
            {
                if ((c2 == 'y'))
                    return pos;

                // La palabra acaba con dos consonantes
                pos += 2;
            }
        }
        return pos;
    }

    /// <summary>
    ///     ''' Devuelve un array con las posiciones de inicio de las sílabas de ultPal
    ///     ''' </summary>
    private void PosicionSilabas()
    {
        posiciones.Clear();

        lonPal = ultPal.Length;
        encTonica = false;
        tonica = 0;
        numSil = 0;
        letraTildada = -1;

        // Se recorre la palabra buscando las sílabas

        int actPos = 0;
        while (actPos < lonPal)
        {
            numSil += 1;
            posiciones.Add(actPos);
            // Marca el principio de la silaba actual
            // Las sílabas constan de tres partes: ataque, núcleo y coda

            actPos = Ataque(ultPal, actPos);
            actPos = Nucleo(ultPal, actPos);
            actPos = Coda(ultPal, actPos);

            if ((encTonica) && (tonica == 0))
                tonica = numSil;
        }

        // Si no se ha encontrado la sílaba tónica (no hay tilde), se determina en base a
        // las reglas de acentuación

        if (!encTonica)
        {
            if (numSil < 2)
                tonica = numSil;
            else
            {
                // Monosílabos
                // Polisílabos
                char letraFinal = ultPal[lonPal - 1];
                char letraAnterior = ultPal[lonPal - 2];

                if ((!esConsonante(letraFinal) || (letraFinal == 'y')) || (((letraFinal == 'n') || (letraFinal == 's') && !esConsonante(letraAnterior))))
                    tonica = numSil - 1;
                else
                    // Palabra llana
                    tonica = numSil;
            }
        }
    }

}