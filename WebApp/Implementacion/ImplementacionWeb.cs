using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contratos;
using Logica;

namespace Implementacion
{
    public class ImplementacionWeb : IServicioWeb
    {
        private Principal Principal { get; set; }
        public ImplementacionWeb()
        {
            Principal = new Principal();
            string claves = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoClaves.txt");
            if (!File.Exists(claves))
                File.Create(claves);
            string directoras = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoDirectoras.txt");
            if (!File.Exists(directoras))
                File.Create(directoras);
            string notas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoNotas.txt");
            if (!File.Exists(notas))
                File.Create(notas);
            string padres = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoPadres.txt");
            if (!File.Exists(padres))
                File.Create(padres);
            string docentes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoDocentes.txt");
            if (!File.Exists(docentes))
                File.Create(docentes);
            string hijos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoHijos.txt");
            if (!File.Exists(hijos))
                File.Create(hijos);
            string salas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoSalas.txt");
            if (!File.Exists(salas))
                File.Create(salas);
        }
        
        public Resultado AltaAlumno(Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            return Principal.AAlumno(hijo, usuarioLogueado);
        }

        public Resultado AltaDirectora(Directora directora, UsuarioLogueado usuarioLogueado)
        {
            return Principal.ADirectora(directora, usuarioLogueado);
        }

        public Resultado AltaDocente(Docente docente, UsuarioLogueado usuarioLogueado)
        {
           return Principal.ADocente(docente, usuarioLogueado);
        }

        public Resultado AltaNota(Nota nota, Sala[] salas, Hijo[] hijos, UsuarioLogueado usuarioLogueado)
        {
            return Principal.AltaNota(nota, salas, hijos, usuarioLogueado);
        }

        public Resultado AltaPadreMadre(Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.APadre(padre, usuarioLogueado);
        }

        public Resultado AsignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            return Principal.AsignarDocenteSala(docente, sala, usuarioLogueado);
        }

        public Resultado AsignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.AsignarHijoPadre(hijo, padre, usuarioLogueado);
        }

        public Resultado DesasignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            return Principal.DesasignarDocenteSala(docente, sala, usuarioLogueado);
        }

        public Resultado DesasignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.DesasignarHijoPadre(hijo, padre, usuarioLogueado);
        }

        public Resultado EditarAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            return Principal.MAlumno(id, hijo, usuarioLogueado);
        }

        public Resultado EditarDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            return Principal.MDirectora(id, directora, usuarioLogueado);
        }

        public Resultado EditarDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            return Principal.MDocente(id, docente, usuarioLogueado);
        }

        public Resultado EditarPadreMadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.MPadre(id, padre, usuarioLogueado);
        }

        public Resultado EliminarAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            return Principal.BAlumno(id, hijo, usuarioLogueado);
        }

        public Resultado EliminarDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            return Principal.BDirectora(id, directora, usuarioLogueado);
        }

        public Resultado EliminarDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            return Principal.BDocente(id, docente, usuarioLogueado);
        }

        public Resultado EliminarPadreMadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            return Principal.BPadre(id, padre, usuarioLogueado);
        }

        public Resultado MarcarNotaComoLeida(Nota nota, UsuarioLogueado usuarioLogueado)
        {
            return Principal.MarcarNotaComoLeida(nota, usuarioLogueado);
        }

        public Hijo ObtenerAlumnoPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            return Principal.ObtenerAlumnoPorId(usuarioLogueado, id);
        }

        public Grilla<Hijo> ObtenerAlumnos(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            return Principal.ObtenerAlumnos(usuarioLogueado, paginaActual, totalPorPagina, busquedaGlobal);
        }

        public Nota[] ObtenerCuadernoComunicaciones(int idPersona, UsuarioLogueado usuarioLogueado)
        {
            return Principal.ObtenerCuadernoComunicaciones(idPersona, usuarioLogueado);
        }

        public Directora ObtenerDirectoraPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            return Principal.ObtenerDirectoraPorId(usuarioLogueado, id);
        }

        public Grilla<Directora> ObtenerDirectoras(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            return Principal.ObtenerDirectoras(usuarioLogueado, paginaActual, totalPorPagina, busquedaGlobal);
        }

        public Docente ObtenerDocentePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            return Principal.ObtenerDocentePorId(usuarioLogueado, id);
        }

        public Grilla<Docente> ObtenerDocentes(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            return Principal.ObtenerDocentes(usuarioLogueado, paginaActual, totalPorPagina, busquedaGlobal);
        }

        public Institucion[] ObtenerInstituciones()
        {
            throw new NotImplementedException();
        }

        public string ObtenerNombreGrupo()
        {
            return $"Ramirez-Tabin";
        }

        public Padre ObtenerPadrePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            return Principal.ObtenerPadrePorId(usuarioLogueado, id);
        }

        public Grilla<Padre> ObtenerPadres(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            return Principal.ObtenerPadres(usuarioLogueado, paginaActual, totalPorPagina, busquedaGlobal);
        }

        public Hijo[] ObtenerPersonas(UsuarioLogueado usuarioLogueado)
        {
            return Principal.ObtenerPersonas(usuarioLogueado);
        }

        public Sala[] ObtenerSalasPorInstitucion(UsuarioLogueado usuarioLogueado)
        {
            return Principal.ObtenerSalasPorInstitucion(usuarioLogueado);
        }

        public UsuarioLogueado ObtenerUsuario(string email, string clave)
        {
            return Principal.Loguear(email,clave);
        }

        public Resultado ResponderNota(Nota nota, Comentario nuevoComentario, UsuarioLogueado usuarioLogueado)
        {
            return Principal.ResponderNota(nota, nuevoComentario, usuarioLogueado);
        }
    }
}
