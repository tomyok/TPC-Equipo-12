﻿using AccesoDB;
using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Negocio
{
    public class MensajeUsuarioNegocio
    {
        private Datos datos;
        private UsuarioNegocio UsuarioNegocio = new UsuarioNegocio();
        public MensajeUsuarioNegocio()
        {
            datos = new Datos();
            UsuarioNegocio = new UsuarioNegocio();
        }

        public List<MensajeUsuario> listarMensajes(string consulta, int IDUsuario)
        {
            List<MensajeUsuario> lista = new List<MensajeUsuario>();
            try
            {
                if (consulta == "recibidos")
                {
                    datos.SetearConsulta("select m.IDMensaje, m.Mensaje, m.FechaHora, m.IDEmisor, m.IDReceptor, m.Asunto, m.Leido from Mensajes m WHERE m.IDReceptor = @IDUsuario order by m.FechaHora desc");
                    datos.SetearParametro("@IDUsuario", IDUsuario);
                }
                else if (consulta == "enviados")
                {
                    datos.SetearConsulta("select m.IDMensaje, m.Mensaje, m.FechaHora, m.IDEmisor, m.IDReceptor, m.Asunto, m.Leido from Mensajes m WHERE m.IDEmisor = @IDUsuario order by m.FechaHora desc");
                    datos.SetearParametro("@IDUsuario", IDUsuario);
                }
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    MensajeUsuario aux = new MensajeUsuario();
                    aux.IDMensaje = (int)datos.Lector["IDMensaje"];
                    aux.Mensaje = (string)datos.Lector["Mensaje"];
                    if (datos.Lector["Asunto"] != DBNull.Value)
                    {
                        aux.Asunto = (string)datos.Lector["Asunto"];
                    }
                    else
                    {
                        aux.Asunto = "Sin Asunto";
                    }
                    aux.FechaHora = (DateTime)datos.Lector["FechaHora"];
                    aux.Leido = (bool)datos.Lector["Leido"];
                    aux.UsuarioEmisor = new Usuario();
                    aux.UsuarioEmisor.IDUsuario = (int)datos.Lector["IDEmisor"];
                    aux.UsuarioReceptor = new Usuario();
                    aux.UsuarioReceptor.IDUsuario = (int)datos.Lector["IDReceptor"];
                    lista.Add(aux);
                }
                foreach (MensajeUsuario mensaje in lista)
                {
                    int idEmisor = mensaje.UsuarioEmisor.IDUsuario;
                    int idReceptor = mensaje.UsuarioReceptor.IDUsuario;
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                    mensaje.UsuarioEmisor = usuarioNegocio.buscarUsuario(idEmisor);
                    mensaje.UsuarioReceptor = usuarioNegocio.buscarUsuario(idReceptor);
                }
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }

        public void EnviarMensaje(MensajeUsuario mensaje)
        {
            try
            {
                datos.SetearConsulta("insert into Mensajes (Mensaje, FechaHora, IDEmisor, IDReceptor, Asunto) values (@Mensaje, @FechaHora, @IDEmisor, @IDReceptor, @Asunto)");
                datos.SetearParametro("@Mensaje", mensaje.Mensaje);
                datos.SetearParametro("@FechaHora", mensaje.FechaHora);
                datos.SetearParametro("@IDEmisor", mensaje.UsuarioEmisor.IDUsuario);
                datos.SetearParametro("@IDReceptor", mensaje.UsuarioReceptor.IDUsuario);
                if (mensaje.Asunto == null || mensaje.Asunto == "")
                {
                    mensaje.Asunto = null;
                }
                datos.SetearParametro("@Asunto", mensaje.Asunto);
                datos.EjecutarAccion();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public MensajeUsuario BuscarMensaje(int idMensaje)
        {
            MensajeUsuario mensaje = new MensajeUsuario();
            try
            {
                datos.SetearConsulta("select m.IDMensaje, m.Mensaje, m.FechaHora, m.IDEmisor, m.IDReceptor, m.Asunto from Mensajes m WHERE m.IDMensaje = @IDMensaje");
                datos.SetearParametro("@IDMensaje", idMensaje);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    mensaje.IDMensaje = (int)datos.Lector["IDMensaje"];
                    mensaje.Mensaje = (string)datos.Lector["Mensaje"];
                    if (datos.Lector["Asunto"] != DBNull.Value)
                    {
                        mensaje.Asunto = (string)datos.Lector["Asunto"];
                    }
                    else
                    {
                        mensaje.Asunto = "Sin Asunto";
                    }
                    mensaje.FechaHora = (DateTime)datos.Lector["FechaHora"];
                    mensaje.UsuarioEmisor = new Usuario();
                    mensaje.UsuarioEmisor.IDUsuario = (int)datos.Lector["IDEmisor"];
                    mensaje.UsuarioReceptor = new Usuario();
                    mensaje.UsuarioReceptor.IDUsuario = (int)datos.Lector["IDReceptor"];
                }
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                mensaje.UsuarioReceptor = usuarioNegocio.buscarUsuario(mensaje.UsuarioReceptor.IDUsuario);
                mensaje.UsuarioEmisor = usuarioNegocio.buscarUsuario(mensaje.UsuarioEmisor.IDUsuario);
                return mensaje;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public List<MensajeRespuesta> ObtenerRespuestas(int idMensaje)
        {
            List<MensajeRespuesta> respuestas = new List<MensajeRespuesta>();

            try
            {
                datos.SetearConsulta("SELECT r.IDRespuesta, r.Respuesta, r.FechaHora, r.IDEmisor FROM Respuestas r INNER JOIN Mensajes m ON r.IDMensaje = m.IDMensaje WHERE r.IDMensaje = @IDMensaje");
                datos.SetearParametro("@IDMensaje", idMensaje);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    MensajeRespuesta respuesta = new MensajeRespuesta();
                    respuesta.IDRespuesta = (int)datos.Lector["IDRespuesta"];
                    respuesta.Texto = (string)datos.Lector["Respuesta"];
                    respuesta.FechaHora = (DateTime)datos.Lector["FechaHora"];
                    respuesta.UsuarioEmisor = new Usuario();
                    respuesta.UsuarioEmisor.IDUsuario = (int)datos.Lector["IDEmisor"];


                    respuestas.Add(respuesta);
                }
                foreach (MensajeRespuesta mensaje in respuestas)
                {
                    int idEmisor = mensaje.UsuarioEmisor.IDUsuario;
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                    mensaje.UsuarioEmisor = usuarioNegocio.buscarUsuario(idEmisor);
                }
                return respuestas;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener las respuestas: " + ex.Message);
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }


        }
        public void GuardarRespuesta(MensajeRespuesta respuesta)
        {
            try
            {
                MensajeRespuesta mensaje = respuesta;
                datos.SetearConsulta("INSERT INTO Respuestas (IDMensaje, Respuesta, FechaHora, IDEmisor) VALUES (@IDMensaje, @Respuesta, @FechaHora, @IDEmisor)");
                datos.SetearParametro("@IDMensaje", mensaje.IDMensajeOriginal);
                datos.SetearParametro("@Respuesta", mensaje.Texto);
                datos.SetearParametro("@FechaHora", DateTime.Now);
                datos.SetearParametro("@IDEmisor", respuesta.UsuarioEmisor.IDUsuario);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw new Exception("Error al guardar la respuesta: " + ex.Message);
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public bool BorrarMensaje(int idMensaje)
        {
            try
            {
                datos.SetearConsulta("DELETE FROM Mensajes WHERE IDMensaje = @IDMensaje");
                datos.SetearParametro("@IDMensaje", idMensaje);
                datos.EjecutarAccion();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al borrar el mensaje: " + ex.Message);
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public int UltimoIDMensaje()
        {
            try
            {
                datos.SetearConsulta("select top(1)IDMensaje from Mensajes order by IDMensaje desc");
                datos.EjecutarLectura();
                int id = 0;
                if (datos.Lector.Read())
                {
                    id = (int)datos.Lector["IDMensaje"];
                }
                return id;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public int UltimoIDRespuesta()
        {
            try
            {
                datos.SetearConsulta("select top(1)IDRespuesta from Respuestas order by IDRespuesta desc");
                datos.EjecutarLectura();
                int id = 0;
                if (datos.Lector.Read())
                {
                    id = (int)datos.Lector["IDRespuesta"];
                }
                return id;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public MensajeRespuesta buscarRespuesta(int idRespuesta)
        {
            if (idRespuesta == 0)
            {
                return null;
            }
            try
            {
                MensajeRespuesta aux = new MensajeRespuesta();
                datos.SetearConsulta("select r.IDRespuesta, r.IDMensaje, r.Respuesta, r.FechaHora, r.IDEmisor from Respuestas r WHERE r.IDRespuesta = @IDRespuesta");
                datos.SetearParametro("@IDRespuesta", idRespuesta);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    aux.IDRespuesta = (int)datos.Lector["IDRespuesta"];
                    aux.IDMensajeOriginal = (int)datos.Lector["IDMensaje"];
                    aux.Texto = (string)datos.Lector["Respuesta"];
                    aux.FechaHora = (DateTime)datos.Lector["FechaHora"];
                    aux.UsuarioEmisor = new Usuario();
                    aux.UsuarioEmisor= UsuarioNegocio.buscarUsuario((int)datos.Lector["IDEmisor"]);
                }
                return aux;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
        public void MarcarComoLeido(int idMensaje)
        {
            try
            {
                datos.SetearConsulta("update Mensajes set Leido = 1 where IDMensaje = @IDMensaje");
                datos.SetearParametro("@IDMensaje", idMensaje);
                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.LimpiarParametros();
                datos.CerrarConexion();
            }
        }
    }
}