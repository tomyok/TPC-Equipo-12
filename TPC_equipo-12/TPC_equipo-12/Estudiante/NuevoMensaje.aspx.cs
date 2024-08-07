﻿using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_12
{
    public partial class NuevoMensaje1 : System.Web.UI.Page
    {
        public List<Usuario> usuarios = new List<Usuario>();
        public UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        public MensajeUsuarioNegocio mensajeNegocio = new MensajeUsuarioNegocio();
        public NotificacionNegocio notificacionNegocio = new NotificacionNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["estudiante"] == null)
            {
                Session["MensajeError"] = "No puede acceder a esa pestaña sin ser estudiante.";
                Response.Redirect("../LogIn.aspx");
            }
            if (!IsPostBack)
            {
                EstudianteMasterPage master = (EstudianteMasterPage)Page.Master;
                master.VerificarMensaje();

                Estudiante estudiante = (Estudiante)Session["estudiante"];
                usuarios = usuarioNegocio.ListarUsuarios();
                foreach (Usuario usuario in usuarios)
                {
                    if (usuario.IDUsuario == estudiante.IDUsuario)
                    {
                        usuarios.Remove(usuario);
                        break;
                    }
                }
                Session.Add("usuarios", usuarios);
                if (usuarios.Count == 0)
                {
                    Session["MensajeError"] = "No hay usuarios disponibles para enviar mensajes.";
                    Response.Redirect("EstudianteMensajes.aspx");
                }
                else
                {

                    ddlDestinatario.DataSource = usuarios;
                    ddlDestinatario.DataTextField = "NombreCompleto";
                    ddlDestinatario.DataValueField = "IDUsuario";
                    ddlDestinatario.DataBind();
                }
                
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            MensajeUsuario mensaje = new MensajeUsuario();
            Estudiante estudiante = (Estudiante)Session["estudiante"];
            mensaje.UsuarioEmisor = estudiante;
            mensaje.UsuarioReceptor = usuarioNegocio.buscarUsuario(Convert.ToInt32(ddlDestinatario.SelectedValue));
            mensaje.Asunto = txtAsunto.Text;
            mensaje.Mensaje = txtMensaje.Text;
            mensaje.FechaHora = DateTime.Now;
            mensajeNegocio.EnviarMensaje(mensaje);
            int id = mensajeNegocio.UltimoIDMensaje();
            mensaje.IDMensaje = id;
            notificacionNegocio.AgregarNotificacionXMensaje(mensaje);
            Session["MensajeExito"] = "Mensaje enviado con éxito.";
            Response.Redirect("EstudianteMensajes.aspx");


        }
    }
}