using System;
using System.Collections.Generic;
using System.Text;

namespace _2SemesterOpgave.Models
{
    public class Accesibility // Klasse til at repræsentere tilgængeligheden af en artikel, som indeholder en 2D-array af datoer, der angiver perioder, hvor artiklen er tilgængelig
    {
        public DateOnly[,] Periods { get; set; } = new DateOnly[12, 31]; // 2D-array til at gemme perioder, hvor artiklen er tilgængelig. Arrayet har en fast størrelse på 10x10, hvilket betyder, at det kan indeholde op til 100 perioder.

    }
}
