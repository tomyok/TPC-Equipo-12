﻿using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace TPC_equipo_12
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master master = (Master)Page.Master;
                master.VerificarMensaje();

                dropGenero.Items.Add("Masculino");
                dropGenero.Items.Add("Femenino");
                dropGenero.Items.Add("No binario");
                dropGenero.Items.Add("No contesta");
            }
        }
        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario();
            UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
            try
            {
                if (!ValidarFormulario())
                {
                    return;
                }
                else
                {
                    usuario.Nombre = InputNombres.Text;
                    usuario.Apellido = InputApellidos.Text;
                    usuario.DNI = Convert.ToInt32(InputDNI.Text);
                    usuario.Email = InputEmail.Text;
                    string Contrasenia = InputPassword.Text;
                    usuario.EsProfesor = false;
                    if (dropGenero.SelectedValue == "Masculino")
                    {
                        usuario.Genero = "M";
                    }
                    else if (dropGenero.SelectedValue == "Femenino")
                    {
                        usuario.Genero = "F";
                    }
                    else if (dropGenero.SelectedValue == "No binario")
                    {
                        usuario.Genero = "x";
                    }
                    usuarioNegocio.AgregarUsuario(usuario, Contrasenia);
                    Session["usuario"] = usuario;
                    Session["MensajeExito"] = "Usuario registrado con éxito!";

                    Response.Redirect("LogIn.aspx", false);
                }


            }
            catch (Exception ex)
            {
                Session["MensajeError"] = "El Email ingresado ya esta asociado a una cuenta!";
                Response.Redirect("SignUp.aspx", false);
            }
        }
        protected bool ValidarFormulario()
        {
            if (InputNombres.Text == "" || InputApellidos.Text == "" || InputDNI.Text == "" || InputEmail.Text == "" || InputPassword.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "info", "<script>showMessage('Faltan campos por completar!', 'info');</script>", false);
                return false;
            }

            return true;
        }
        
    }
}