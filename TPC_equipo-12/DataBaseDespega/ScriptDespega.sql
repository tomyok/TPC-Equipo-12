Create Database DB_DESPEGA
GO
Use DB_DESPEGA
GO
Create Table Imagenes(
	IDImagenes int not null primary key identity(1, 1),
	URLIMG varchar(200) not null
)
GO
Create Table Usuarios(
	IDUsuario int not null primary key identity(1, 1),
	Nombre varchar(100) not null,
	Apellido varchar(100) not null,
	DNI int not null unique,
	Genero char null check(Genero='M' or Genero='F' or Genero='X'),
	Email varchar(100) not null unique,
	EsProfesor bit not null default 0,
	IDImagen int not null Foreign Key References Imagenes(IDImagenes),
	Contrasenia varchar(100) not null
)
GO
Create Table Categorias(
	IDCategoria int not null Primary Key Identity(1,1),
	Nombre varchar(100) not null unique
)
GO
Create Table Cursos(
	IDCurso int not null Primary Key Identity(1,1),
	Nombre varchar(200) not null,
	Descripcion varchar(500) not null,
	IDImagen int not null Foreign Key References Imagenes(IDImagenes),
	Estreno DateTime not null,
	Duracion int not null,
)
GO
Create Table Inscripciones(
	IDInscripcion int not null unique Identity(1, 1),
	IDusuario int not null Foreign Key References Usuarios(IDUsuario),
	IDCurso int not null Foreign Key References Cursos(IDCurso),
	Estado bit not null Default 0,
	Primary Key (IDUsuario, IDCurso)
)
GO
Create Table Profesor(
	IDProfesor int not null primary key Foreign Key References Usuarios(IDUsuario),
)
GO
Create Table ProfesorXCursos(
	IDProfesor int not null Foreign Key References Profesor(IDProfesor),
	IDCurso int not null Foreign Key References Cursos(IDCurso)
	Primary Key (IDProfesor, IDCurso)
)
GO
Create Table Estudiantes(
	IDEstudiante int not null Primary Key Foreign Key References Usuarios(IDUsuario),
	Estado bit not null 
)
GO
Create Table EstudiantesXCursos(
	IDEstudiante int not null Foreign Key References Estudiantes(IDEstudiante),
	IDCurso int not null Foreign Key References Cursos(IDCurso),
	Completado bit not null Default 0,
	Primary Key (IDEstudiante, IDCurso)
)
GO
Create Table CategoriasXCurso(
	IDCurso int not null Foreign Key References Cursos(IDCurso),
	IDCategoria int not null Foreign Key References Categorias(IDCategoria)
	Primary Key (IDCurso, IDCategoria)
)
GO
Create Table Materiales(
	IDMaterial int not null Primary Key Identity(1,1),
	Nombre varchar(100) not null,
	TipoMaterial varchar(100) not null,
	URLMaterial varchar(200)
)
GO
Create Table Lecciones(
	IDLeccion int not null Primary Key Identity(1,1),
	NroLeccion int not null unique,
	Nombre varchar(100) not null,
	Descripcion varchar(500) not null,
)
GO
Create Table LeccionesXEstudiantes(
	IDEstudiante int not null Foreign Key References Estudiantes(IDEstudiante),
	IDLeccion int not null Foreign Key References Lecciones(IDLeccion),
	Completado bit not null Default 0,
	Primary Key (IDEstudiante, IDLeccion)
)
GO
Create Table MaterialesXLecciones(
	IDMaterial int not null Foreign Key References Materiales(IDMaterial),
	IDLeccion int not null Foreign Key References Lecciones(IDLeccion),
	Primary Key (IDMaterial, IDLeccion)
)
GO
Create Table Unidades(
	IDUnidad int not null Primary Key Identity(1, 1),
	NroUnidad int not null unique,
	Nombre varchar(100) not null,
	Descripcion varchar(100) not null,
)
GO
Create Table LeccionesXUnidades(
	IDUnidad int not null Foreign Key References Unidades(IDUnidad),
	IDLeccion int not null Foreign Key References Lecciones(IDLeccion),
	Primary Key (IDUnidad, IDLeccion)
)
GO
Create Table UnidadesXCurso(
	IDUnidad int not null Foreign Key References Unidades(IDUnidad),
	IDCurso int not null Foreign Key References Cursos(IDCurso),
	Primary Key (IDUnidad, IDCurso)
)
GO
Create Table Mensajes(
	IDMensaje int not null Primary Key identity(1, 1),
	Mensaje varchar(500) not null,
	IDEmisor int not null Foreign Key References Usuarios(IDusuario),
	IDReceptor int not null Foreign Key References Usuarios(IDusuario)
)
GO
Create Table MensajesXUsuario(
	IDMensaje int not null Foreign Key References Mensajes(IDMensaje),
	IDUsuario int not null Foreign Key References Usuarios(IDUsuario),
	Primary Key (IDMensaje, IDUsuario)
)
GO
Create Table Notificaciones(
	IDNotificacion int not null Primary Key Identity(1, 1),
	Mensaje varchar(200) not null,
	Tipo varchar(100) not null check(Tipo='INSCRIPCION' or Tipo='MENSAJE'),
	Fecha datetime not null default(getdate()),
	Leido bit not null default 0,
	IDInscripcion int Foreign Key References Inscripciones(IDInscripcion),
	IDMensaje int Foreign Key References Mensajes(IDMensaje)
)
GO
Create Table Resenias(
	IDResenia int not null Primary Key Identity(1, 1),
	IDEstudiante int not null Foreign Key References Estudiantes(IDEstudiante),
	Resenia varchar(200) not null,
	Calificacion int not null check(Calificacion between 1 and 10),
	Fecha datetime not null default(getdate())
)
GO
Create Table ReseniasXCurso(
	IDCurso int not null Foreign Key References Cursos(IDCurso),
	IDResenia int not null Foreign Key References Resenias(IDResenia),
	Primary Key (IDCurso, IDResenia)
)
GO
Create Table NotificacionesXUsuario(
		IDNotificacion int not null Foreign Key References Notificaciones(IDNotificacion),
		IDUsuario int not null Foreign Key References Usuarios(IDUsuario)
		Primary Key (IDNotificacion, IDUsuario)
)
GO