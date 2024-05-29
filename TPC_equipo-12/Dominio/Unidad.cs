﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Unidad
    {
        public int NroUnidad { get; set; }
        public int IDCurso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Leccion> Lecciones {  get; set; }
        public bool Completada { get; set; }

        public Leccion Leccion
        {
            get => default;
            set
            {
            }
        }
    }
}
