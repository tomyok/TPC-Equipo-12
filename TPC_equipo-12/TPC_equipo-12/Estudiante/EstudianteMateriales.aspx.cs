﻿using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace TPC_equipo_12
{
    public partial class EstudianteMateriales : System.Web.UI.Page
    {
        public List<MaterialLeccion> listaMateriales = new List<MaterialLeccion>();
        public MaterialNegocio materialNegocio = new MaterialNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["estudiante"] == null)
            {
                Session["MensajeError"] = "No puede acceder a esa pestaña sin ser un estudiante.";
                Response.Redirect("../LogIn.aspx");
            }
            if (!IsPostBack)
            {
                EstudianteMasterPage master = (EstudianteMasterPage)Page.Master;
                master.VerificarMensaje();

                listaMateriales = materialNegocio.ListarMateriales((int)Session["IDLeccion"]);
                Session.Add("ListaMateriales", listaMateriales);
                rptMateriales.DataSource = listaMateriales;
                rptMateriales.DataBind();


            }
        }
        private string ExtractVideoId(string youtubeLink)
        {

            var regex = new Regex(@"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/\s]{11})");

            var match = regex.Match(youtubeLink);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {

                return null;
            }
        }
        private string CargarIframe(MaterialLeccion material)
        {
            string youtubeLink = material.URL;

            string videoId = ExtractVideoId(youtubeLink);

            string iframeHtml = $@"
        <iframe 
            class='w-100'
            height='600'
            src='https://www.youtube.com/embed/{videoId}' 
            title='YouTube video player' 
            frameborder='0' 
            allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture' 
            allowfullscreen>
        </iframe>";
            return iframeHtml;
        }
        protected void rptMateriales_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MaterialLeccion material = e.Item.DataItem as MaterialLeccion;
                if (material.TipoMaterial == "Video")
                {
                    Literal ltlYoutubeVideo = (Literal)e.Item.FindControl("ltlYoutubeVideo");
                    ltlYoutubeVideo.Text = CargarIframe(material);
                }
                else if (material.TipoMaterial == "Documento")
                {
                    Literal ltlDocumento = (Literal)e.Item.FindControl("ltlDocumento");
                    ltlDocumento.Text = $@"
                    <a href='{material.URL}' target='_blank' class='border p-2 m-2 d-inline-block mb-4'>
                        {material.Nombre}
                    </a>";
                }
            }
        }

        protected void ButtonBackLecciones_Click(object sender, EventArgs e)
        {
            Response.Redirect("EstudianteLecciones.aspx", false);
        }
    }
}