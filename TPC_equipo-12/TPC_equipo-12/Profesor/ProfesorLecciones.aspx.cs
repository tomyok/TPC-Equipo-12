﻿using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_12
{
    public partial class ProfesorLecciones : System.Web.UI.Page
    {
        public List<Leccion> listaLecciones = new List<Leccion>();
        public LeccionNegocio leccionNegocio = new LeccionNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["profesor"] == null)
            {
                Session["MensajeError"] = "No puede acceder a esa pestaña sin ser profesor.";
                Response.Redirect("../LogIn.aspx");
            }
            if (!IsPostBack)
            {
                ProfesorMasterPage master = (ProfesorMasterPage)Page.Master;
                master.VerificarMensaje();

                listaLecciones = leccionNegocio.ListarLecciones((int)Session["IDUnidadProfesor"]);
                Session.Add("ListaLeccionesProfesor", listaLecciones);
                rptLeccionesProf.DataSource = listaLecciones;
                rptLeccionesProf.DataBind();
            }
        }

        protected void ButtonBackUnidadProf_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProfesorUnidades.aspx");
        }

        protected void ButtonVerMaterialesProf_Command(object sender, CommandEventArgs e)
        {
            int IdLeccion = Convert.ToInt32(e.CommandArgument);
            Session.Add("IDLeccionProfesor", IdLeccion);
            Response.Redirect("ProfesorMateriales.aspx");
        }

        protected void ButtonCrearLeccionProf_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgregarLecciones.aspx");
        }

        protected void ButtonEliminarLeccionProf_Click(object sender, EventArgs e)
        {
            Response.Redirect("EliminarLecciones.aspx");
        }

        protected void ButtonModificarLeccionProf_Command(object sender, CommandEventArgs e)
        {
            int IdLeccion = Convert.ToInt32(e.CommandArgument);
            Session.Add("IDLeccionProfesor", IdLeccion);
            Response.Redirect("AgregarLecciones.aspx?IdLeccion=" + IdLeccion);
        }
    }
}