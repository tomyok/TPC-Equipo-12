﻿using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class UsuarioNegocio
    {
        private Datos Datos;
        private CursoNegocio Cursos;

        public UsuarioNegocio()
        {
            Datos = new Datos();
            Cursos = new CursoNegocio();

        }
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                Datos.SetearConsulta("select u.IDUsuario, u.Nombre, u.Apellido, u.Email, u.Contrasenia, u.DNI, u.Genero, u.EsProfesor,u.ContraseniaHash,u.ContraseniaSalt, i.IDImagenes, i.URLIMG from Usuarios u left join Imagenes i on u.IDImagen = i.IDImagenes");
                Datos.EjecutarLectura();
                while (Datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.IDUsuario = Datos.Lector.GetInt32(0);
                    aux.Nombre = (string)Datos.Lector["Nombre"];
                    aux.Apellido = (string)Datos.Lector["Apellido"];
                    aux.Email = (string)Datos.Lector["Email"];
                    aux.ContraseniaHash = (string)Datos.Lector["ContraseniaHash"];
                    aux.ContraseniaSalt = (string)Datos.Lector["ContraseniaSalt"];
                    aux.DNI = (int)Datos.Lector["DNI"];
                    aux.Genero = (string)Datos.Lector["Genero"];
                    aux.EsProfesor = (bool)Datos.Lector["EsProfesor"];
                    if (Datos.Lector["IDImagenes"] != DBNull.Value)
                    {
                        aux.ImagenPerfil = new Imagen();
                        aux.ImagenPerfil.IDImagen = (int)Datos.Lector["IDImagenes"];
                        aux.ImagenPerfil.URL = (string)Datos.Lector["URLIMG"];
                    }
                    else
                    {
                        aux.ImagenPerfil = new Imagen();
                        aux.ImagenPerfil.IDImagen = 0;
                        aux.ImagenPerfil.URL = "https://www.abc.com.py/resizer/1J9J9Q1";
                    }


                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Datos.CerrarConexion();
            }

        }
        public int UltimoIdUsuario()
        {
            try
            {
                Datos.SetearConsulta("select top 1 IDUsuario from Usuarios order by IDUsuario desc");
                Datos.EjecutarLectura();
                Datos.Lector.Read();
                return Datos.Lector.GetInt32(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UltimoIdImagen()
        {
            try
            {
                Datos.SetearConsulta("select top 1 IDImagenes from Imagenes order by IDImagenes desc");
                Datos.EjecutarLectura();
                Datos.Lector.Read();
                return Datos.Lector.GetInt32(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AgregarUsuario(Usuario usuario, string contrasenia)
        {
            //EmailService envioMensaje = new EmailService();
            try
            {
                //Datos.SetearConsulta("insert into Imagenes (URLIMG) values(@URLIMG)");
                //Datos.SetearParametro("@URLIMG", usuario.ImagenPerfil.URL);
                //Datos.EjecutarAccion();
                //int aux = UltimoIdImagen();
                //Datos.SetearConsulta("insert into Usuarios (Nombre, Apellido, Email, Clave, DNI, Genero, EsProfesor, IDImagen) values (@Nombre, @Apellido, @Email, @Clave, @DNI, @Genero, @EsProfesor, @IDImagen)");
                if (ExisteUsuario(usuario))
                {
                    throw new Exception("El usuario ya existe");
                }
                else
                {
                    string salt = GenerateSalt();
                    string hash = HashPassword(contrasenia, salt);
                    usuario.ContraseniaHash = hash;
                    usuario.ContraseniaSalt = salt;
                    Datos.SetearConsulta("insert into Usuarios (Nombre, Apellido, Email, ContraseniaHash,ContraseniaSalt, DNI, Genero, EsProfesor) values (@Nombre, @Apellido, @Email, @ContraseniaHash,@ContraseniaSalt, @DNI, @Genero, @EsProfesor)");
                    Datos.SetearParametro("@Nombre", usuario.Nombre);
                    Datos.SetearParametro("@Apellido", usuario.Apellido);
                    Datos.SetearParametro("@Email", usuario.Email);
                    Datos.SetearParametro("@ContraseniaHash", usuario.ContraseniaHash);
                    Datos.SetearParametro("@ContraseniaSalt", usuario.ContraseniaSalt);
                    Datos.SetearParametro("@DNI", usuario.DNI);
                    Datos.SetearParametro("@Genero", (object)usuario.Genero ?? DBNull.Value);
                    Datos.SetearParametro("@EsProfesor", usuario.EsProfesor);
                    //Datos.SetearParametro("@IDImagen", (object)usuario.ImagenPerfil.IDImagen?? DBNull.Value);
                    Datos.EjecutarAccion();
                    //envioMensaje.EnviarEmailRegistroExitoso(usuario.Email);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ModificarUsuario(Usuario usuario, string nuevaContrasenia = null)
        {
            try
            {
                string consulta = "UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, DNI = @DNI, Genero = @Genero, EsProfesor = @EsProfesor";
                if (nuevaContrasenia != null)
                {

                    string nuevoSalt = GenerateSalt();
                    string nuevoHash = HashPassword(nuevaContrasenia, nuevoSalt);

                    consulta += ", ContraseniaHash = @ContraseniaHash, ContraseniaSalt = @ContraseniaSalt";
                    Datos.SetearParametro("@ContraseniaHash", nuevoHash);
                    Datos.SetearParametro("@ContraseniaSalt", nuevoSalt);
                }
                consulta += " WHERE IDUsuario = @IDUsuario";

                Datos.SetearConsulta(consulta);

                Datos.SetearParametro("@Nombre", usuario.Nombre);
                Datos.SetearParametro("@Apellido", usuario.Apellido);
                Datos.SetearParametro("@Email", usuario.Email);
                Datos.SetearParametro("@DNI", usuario.DNI);
                Datos.SetearParametro("@Genero", usuario.Genero);
                Datos.SetearParametro("@EsProfesor", usuario.EsProfesor);
                Datos.SetearParametro("@IDUsuario", usuario.IDUsuario);

                Datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EliminarUsuario(Usuario usuario)
        {
            try
            {
                Datos.SetearConsulta("delete from Imagenes where IDImagenes = @IDImagenes");
                Datos.SetearParametro("@IDImagenes", usuario.ImagenPerfil.IDImagen);
                Datos.EjecutarAccion();
                if (usuario.EsProfesor)
                {
                    Datos.SetearConsulta("delete from Profesores where IDProfesor = @IDProfesor");
                    Datos.SetearParametro("@IDProfesor", usuario.IDUsuario);
                    Datos.EjecutarAccion();
                }
                else
                {
                    Datos.SetearConsulta("delete from Estudiantes where IDEstudiante = @IDEstudiante");
                    Datos.SetearParametro("@IDEstudiante", usuario.IDUsuario);
                    Datos.EjecutarAccion();
                }

                Datos.SetearConsulta("delete from Usuarios where IDUsuario = @IDUsuario");
                Datos.SetearParametro("@IDUsuario", usuario.IDUsuario);
                Datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Logueo(Usuario usuario, string contrasenia)
        {
            Datos Datos = new Datos();
            try
            {

                Datos.SetearConsulta("SELECT ContraseniaHash, ContraseniaSalt FROM Usuarios WHERE Email = @Email");
                Datos.SetearParametro("@Email", usuario.Email);
                Datos.EjecutarLectura();

                if (Datos.Lector.Read())
                {
                    string storedHash = Datos.Lector["ContraseniaHash"].ToString();
                    string storedSalt = Datos.Lector["ContraseniaSalt"].ToString();


                    if (BCrypt.Net.BCrypt.Verify(contrasenia + storedSalt, storedHash))
                    {

                        return TraerEmail(usuario);

                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Datos.LimpiarParametros();
                Datos.CerrarConexion();
            }
        }
        public bool TraerEmail(Usuario usuario)
        {
            Datos Datos = new Datos();
            try
            {
                Datos.SetearConsulta("SELECT IDUsuario, EsProfesor FROM Usuarios WHERE Email = @Email");
                Datos.SetearParametro("@Email", usuario.Email);
                Datos.EjecutarLectura();

                if (Datos.Lector.Read())
                {
                    usuario.IDUsuario = Datos.Lector.GetInt32(0);
                    usuario.EsProfesor = (bool)Datos.Lector["EsProfesor"];
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Datos.LimpiarParametros();
                Datos.CerrarConexion();
            }
        }
        public Profesor SetearProfesor(int idUsuario)
        {
            Datos Datos = new Datos();
            Profesor profesor = new Profesor();
            try
            {
                Datos.SetearConsulta("SELECT u.IDUsuario, u.Nombre, u.Apellido, u.DNI, u.Genero, u.Email, u.ContraseniaHash,u.ContraseniaSalt, u.EsProfesor, u.IDImagen, i.URLIMG " +
                     "FROM Usuarios u " +
                     "LEFT JOIN Imagenes i ON u.IDImagen = i.IDImagenes " +
                     "WHERE u.IDUsuario = @IDProfesor");
                Datos.SetearParametro("@IDProfesor", idUsuario);
                Datos.EjecutarLectura();
                while (Datos.Lector.Read())
                {
                    profesor.IDUsuario = Datos.Lector.GetInt32(0);
                    profesor.Nombre = (string)Datos.Lector["Nombre"];
                    profesor.Apellido = (string)Datos.Lector["Apellido"];
                    profesor.DNI = (int)Datos.Lector["DNI"];
                    //profesor.Genero = (string)Datos.Lector["Genero"];
                    if (Datos.Lector["Genero"] != DBNull.Value)
                    {
                        profesor.Genero = (string)Datos.Lector["Genero"];
                    }
                    else
                    {
                        profesor.Genero = "No contesta";
                    }
                    profesor.Email = (string)Datos.Lector["Email"];
                    profesor.ContraseniaHash = (string)Datos.Lector["ContraseniaHash"];
                    profesor.ContraseniaSalt = (string)Datos.Lector["ContraseniaSalt"];
                    profesor.EsProfesor = (bool)Datos.Lector["EsProfesor"];
                    profesor.ImagenPerfil = new Imagen();
                    if (Datos.Lector["IDImagen"] != DBNull.Value)
                    {
                        profesor.ImagenPerfil.IDImagen = (int)Datos.Lector["IDImagen"];
                        profesor.ImagenPerfil.URL = (string)Datos.Lector["URLIMG"];
                    }
                    else
                    {
                        profesor.ImagenPerfil.IDImagen = 0;
                    }
                }
                return profesor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Datos.CerrarConexion();
            }
        }

        public Estudiante SetearEstudiante(int idUsuario)
        {
            Datos Datos = new Datos();
            Estudiante estudiante = new Estudiante();
            try
            {
                Datos.SetearConsulta("SELECT u.IDUsuario, u.Nombre, u.Apellido, u.DNI, u.Genero, u.Email, u.ContraseniaHash,u.ContraseniaSalt, u.EsProfesor, u.IDImagen, i.URLIMG " +
                     "FROM Usuarios u " +
                     "LEFT JOIN Imagenes i ON u.IDImagen = i.IDImagenes " +
                     "WHERE u.IDUsuario = @IDEstudiante");
                Datos.SetearParametro("@IDEstudiante", idUsuario);
                Datos.EjecutarLectura();
                while (Datos.Lector.Read())
                {
                    estudiante.IDUsuario = Datos.Lector.GetInt32(0);
                    estudiante.Nombre = (string)Datos.Lector["Nombre"];
                    estudiante.Apellido = (string)Datos.Lector["Apellido"];
                    estudiante.DNI = (int)Datos.Lector["DNI"];
                    //estudiante.Genero = (string)Datos.Lector["Genero"];
                    if (Datos.Lector["Genero"] != DBNull.Value)
                    {
                        estudiante.Genero = (string)Datos.Lector["Genero"];
                    }
                    else
                    {
                        estudiante.Genero = "No contesta";
                    }
                    estudiante.Email = (string)Datos.Lector["Email"];
                    estudiante.ContraseniaHash = (string)Datos.Lector["ContraseniaHash"];
                    estudiante.ContraseniaSalt = (string)Datos.Lector["ContraseniaSalt"];
                    estudiante.EsProfesor = (bool)Datos.Lector["EsProfesor"];
                    estudiante.ImagenPerfil = new Imagen();
                    if (Datos.Lector["IDImagen"] != DBNull.Value)
                    {
                        estudiante.ImagenPerfil.IDImagen = (int)Datos.Lector["IDImagen"];
                        estudiante.ImagenPerfil.URL = (string)Datos.Lector["URLIMG"];
                    }
                    else
                    {
                        estudiante.ImagenPerfil.IDImagen = 0;
                    }
                }
                return estudiante;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Datos.CerrarConexion();
            }

        }

        public Usuario buscarUsuario(int idUsuario)
        {
            Usuario usuario = new Usuario();
            try
            {
                Datos.SetearConsulta("select IDUsuario, Nombre, Apellido, DNI, Genero, Email, ContraseniaHash, ContraseniaSalt, EsProfesor, IDImagen from Usuarios where IDUsuario = @IDUsuario");
                Datos.SetearParametro("@IDUsuario", idUsuario);
                Datos.EjecutarLectura();
                while (Datos.Lector.Read())
                {
                    usuario.IDUsuario = Datos.Lector.GetInt32(0);
                    usuario.Nombre = (string)Datos.Lector["Nombre"];
                    usuario.Apellido = (string)Datos.Lector["Apellido"];
                    usuario.DNI = (int)Datos.Lector["DNI"];
                    usuario.Genero = (string)Datos.Lector["Genero"];
                    usuario.Email = (string)Datos.Lector["Email"];
                    usuario.ContraseniaHash = (string)Datos.Lector["ContraseniaHash"];
                    usuario.ContraseniaSalt = (string)Datos.Lector["ContraseniaSalt"];
                    usuario.EsProfesor = (bool)Datos.Lector["EsProfesor"];
                    usuario.ImagenPerfil = new Imagen();
                    if (Datos.Lector["IDImagen"] != DBNull.Value)
                    {
                        usuario.ImagenPerfil.IDImagen = (int)Datos.Lector["IDImagen"];
                    }
                    else
                    {
                        usuario.ImagenPerfil.IDImagen = 0;
                    }


                }
                Datos.LimpiarParametros();
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Datos.CerrarConexion();
            }
        }
        public bool ExisteUsuario(Usuario usuario)
        {
            try
            {
                Datos.SetearConsulta("select IDUsuario from Usuarios where Email = @Email");
                Datos.SetearParametro("@Email", usuario.Email);
                Datos.EjecutarLectura();
                if (Datos.Lector.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Datos.LimpiarParametros();
                Datos.CerrarConexion();
            }
        }

        public void publicarComentario(Usuario emisor, Leccion leccion, string comentario)
        {
            Datos datos = new Datos();
            try
            {
                Comentario primerComentario = new Comentario(comentario, leccion, emisor);
                datos.SetearConsulta("INSERT INTO Comentarios(IDleccion, CuerpoComentario, IDUsuarioEmisor, FechaCreacion, IDImagen, Estado) VALUES (@idLeccion, @cuerpoComentario, @idEmisor, @fechaCreacion, @idImagen, @estado)");
                datos.SetearParametro("@idLeccion", primerComentario.Leccion.IDLeccion);
                datos.SetearParametro("@cuerpoComentario", primerComentario.CuerpoComentario);
                datos.SetearParametro("@idEmisor", primerComentario.UsuarioEmisor.IDUsuario);
                datos.SetearParametro("@fechaCreacion", primerComentario.FechaCreacion);
                datos.SetearParametro("@idImagen", emisor.ImagenPerfil == null ? (object)DBNull.Value : emisor.ImagenPerfil.IDImagen);
                datos.SetearParametro("@estado", primerComentario.Estado);
                datos.EjecutarAccion();
                datos.LimpiarParametros();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }

        private string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}
