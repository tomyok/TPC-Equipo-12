﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace TPC_equipo_12
{
    public partial class ProfesorMensajes : System.Web.UI.Page
    {
        public List<MensajeUsuario> mensajes = new List<MensajeUsuario>();
        public MensajeUsuarioNegocio mensajeUsuarioNegocio = new MensajeUsuarioNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["profesor"] == null)
            {
                Session["MensajeError"] = "No puede acceder a esa pestaña sin ser profesor.";
                Response.Redirect("../LogIn.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["MensajeExito"] != null)
                {
                    string msj = Session["MensajeExito"].ToString();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", $@"showMessage('{msj}', 'success');", true);
                    Session["MensajeExito"] = null;
                }
                if (Session["MensajeError"] != null)
                {
                    string msj = Session["MensajeError"].ToString();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", $@"showMessage('{msj}', 'error');", true);
                    Session["MensajeError"] = null;
                }
                if (Session["MensajeInfo"] != null)
                {
                    string msj = Session["MensajeInfo"].ToString();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Info", $@"showMessage('{msj}', 'info');", true);
                    Session["MensajeInfo"] = null;
                }
                Profesor profesor = (Profesor)Session["profesor"];
                mensajes = mensajeUsuarioNegocio.listarMensajes(profesor.IDUsuario);
                Session.Add("mensajes", mensajes);
                rptMensajes.DataSource = mensajes;
                rptMensajes.DataBind();
            }
        }



        protected void btnVerMensaje_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int idMensaje = Convert.ToInt32(btn.CommandArgument);
            MensajeUsuario mensaje = mensajeUsuarioNegocio.BuscarMensaje(idMensaje);
            //mensajeUsuarioNegocio.MarcarComoLeido(mensaje);
            Session.Add("mensaje", mensaje);

            Response.Redirect("VerMensaje.aspx");
        }

        protected void btnVerMensaje_Command(object sender, CommandEventArgs e)
        {
            Button btn = (Button)sender;
            int idMensaje = Convert.ToInt32(btn.CommandArgument);
            MensajeUsuario mensaje = mensajeUsuarioNegocio.BuscarMensaje(idMensaje);
            //mensajeUsuarioNegocio.MarcarComoLeido(mensaje);
            Session.Add("mensaje", mensaje);
        }
    }
}