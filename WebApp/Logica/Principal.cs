using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contratos;
using Newtonsoft.Json;

namespace Logica
{
    public class Principal
    {
        public Principal()
        {            
            Hijos = new List<Hijo>();
            Directoras = new List<Directora>();
            Padres = new List<Padre>();
            Docentes = new List<Docente>();
            Notas = new List<Nota>();
            Salas = ObtenerSalas();
        }
        
        public List<Hijo> Hijos { get; set; }
        public List<Directora> Directoras { get; set; }
        public List<Padre> Padres { get; set; }
        public List<Docente> Docentes { get; set; }
        public List<Nota> Notas { get; set; }
        public List<Sala> Salas { get; set; }
        

        public UsuarioLogueado Loguear(string email, string clave)
        {
            UsuarioLogueado usuario = new UsuarioLogueado();
            List<Clave> claves = ObtenerClaves();
            foreach (var item in claves)
            {
                if (item.Email == email & item.Contrasena == clave)
                {
                    Roles rol = item.Roles[0];
                    if (rol == Roles.Directora)
                    {
                        List<Directora> directoras = ObtenerDirectoras();
                        foreach (var dire in directoras)
                        {
                            if (dire.Email == email)
                            {
                                usuario.Email = email;
                                usuario.Roles = item.Roles;
                                usuario.Nombre = dire.Nombre;
                                usuario.Apellido = dire.Apellido;
                                usuario.RolSeleccionado = Roles.Directora;
                            }
                        }
                    }
                    else
                    {
                        if (rol == Roles.Docente)
                        {
                            List<Docente> docentes = ObtenerDocentes();
                            foreach (var docente in docentes)
                            {
                                if (docente.Email == email)
                                {
                                    usuario.Email = email;
                                    usuario.Roles = item.Roles;
                                    usuario.Nombre = docente.Nombre;
                                    usuario.Apellido = docente.Apellido;
                                    usuario.RolSeleccionado = Roles.Docente;
                                }
                            }
                        }
                        else
                        {
                            if (rol == Roles.Padre)
                            {
                                List<Padre> padres = ObtenerPadres();
                                foreach (var padre in padres)
                                {
                                    if (padre.Email == email)
                                    {
                                        usuario.Email = email;
                                        usuario.Roles = item.Roles;
                                        usuario.Nombre = padre.Nombre;
                                        usuario.Apellido = padre.Apellido;
                                        usuario.RolSeleccionado = Roles.Padre;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return usuario;
        }

        public void EscribirClaves(List<Clave> claves)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoClaves.txt");
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(claves);
                archivo.Write(output);
            }
        }

        public void EscribirHijos(List<Hijo> hijos)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoHijos.txt");
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(hijos);
                archivo.Write(output);
            }
        }

        public void EscribirDocentes(List<Docente> docentes)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoDocentes.txt");
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(docentes);
                archivo.Write(output);
            }
        }

        public void EscribirPadres(List<Padre> padres)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoPadres.txt");
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(padres);
                archivo.Write(output);
            }
        }

        public void EscribirDirectoras(List<Directora> directoras)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoDirectoras.txt");
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(directoras);
                archivo.Write(output);
            }
        }

        public void EscribirNotas(List<Nota> notas)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoNotas.txt");
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(notas);
                archivo.Write(output);
            }
        }


        public Resultado AAlumno(Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, hijo, usuarioLogueado))
            {
                Hijos = ObtenerHijos();
                hijo.Id = Hijos.Count() + 1;
                Hijos.Add(hijo);
                EscribirHijos(Hijos);
            }
            return resultado;
        }

        public Resultado MAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, hijo, usuarioLogueado))
            {
                List<Hijo> hijos = ObtenerHijos();
                bool band = false;
                foreach (var hij in hijos)
                {
                    if (hij.Id == id)
                    {
                        hij.Apellido = hijo.Apellido;
                        hij.Email = hijo.Email;
                        hij.FechaNacimiento = hijo.FechaNacimiento;
                        hij.Institucion = hijo.Institucion;
                        hij.Nombre = hijo.Nombre;
                        hij.Notas = hijo.Notas;
                        hij.ResultadoUltimaEvaluacionAnual = hij.ResultadoUltimaEvaluacionAnual;
                        hij.Sala = hijo.Sala;
                        EscribirHijos(hijos);
                        Hijos = hijos;
                        band = true;
                        break;
                           
                    }
                }
                if (band == false)
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }

        public Resultado BAlumno(int id, Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();

            Hijos = ObtenerHijos();
            hijo = Hijos.Where(x => x.Id == hijo.Id).FirstOrDefault();
            if (hijo != null)
            {
                Hijos.Remove(hijo);
                EscribirHijos(Hijos);
            }
            else
            {
                resultado.Errores.Add("No se encontró el usuario.");
            }
            
            return resultado;
        }

        public Hijo ObtenerAlumnoPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            Hijo hijo = new Hijo();
            hijo = ObtenerHijos().Where(x => x.Id == id).FirstOrDefault();
            return hijo;
        }

        public Grilla<Hijo> ObtenerAlumnos(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            Grilla<Hijo> grilla = new Grilla<Hijo>();
            List<Hijo> lista = ObtenerHijos();
            grilla.Lista = lista.Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal) || x.Email.Contains(busquedaGlobal)).Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray();
            grilla.CantidadRegistros = lista.Count;
            return grilla;
        }


        public Resultado ADirectora(Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, directora, usuarioLogueado))
            {
                //Usuarios = ObtenerUsuarios();
                Directoras = ObtenerDirectoras();
                List<Clave> claves = ObtenerClaves();
                directora.Id = Directoras.Count() + 1;
                
                Directoras.Add(directora);
                EscribirDirectoras(Directoras);
                //EscribirUsuarios(Usuarios);
                Clave clave = new Clave();
                Random random = new Random();
                clave.Contrasena = random.Next(10000000, 99999999).ToString();
                clave.Email = directora.Email;
                clave.Roles = usuarioLogueado.Roles;
                claves.Add(clave);
                EscribirClaves(claves);
                
            }
            return resultado;
        }

        public Resultado MDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, directora, usuarioLogueado))
            {
                List<Directora> directoras = ObtenerDirectoras();
                bool band = false;
                foreach (var dire in directoras)
                {
                    if (dire.Id == id)
                    {
                        List<Clave> claves = ObtenerClaves();
                        foreach (var clave in claves)
                        {
                            if (clave.Email == dire.Email)
                            {
                                clave.Email = directora.Email;
                                EscribirClaves(claves);
                                dire.Apellido = directora.Apellido;
                                dire.Cargo = directora.Cargo;
                                dire.FechaIngreso = directora.FechaIngreso;
                                dire.Institucion = directora.Institucion;
                                dire.Nombre = directora.Nombre;
                                dire.Email = directora.Email;
                                EscribirDirectoras(directoras);
                                Directoras = directoras;
                                band = true;
                                break;
                            }
                        }
                    }
                }
                if (band == false)
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }

        public Resultado BDirectora(int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            Roles rol = Roles.Directora;
            if (rol != usuarioLogueado.RolSeleccionado)
                resultado.Errores.Add("El rol seleccionado no es el de Directora.");
            else
            {
                Directoras = ObtenerDirectoras();
                List<Clave> claves = ObtenerClaves();
                directora = Directoras.Where(x => x.Id == directora.Id).FirstOrDefault();
                Clave clave = claves.Where(x => x.Email == directora.Email).FirstOrDefault();
                if (directora != null & clave != null)
                {
                    claves.Remove(clave);
                    EscribirClaves(claves);
                    Directoras.Remove(directora);
                    EscribirDirectoras(Directoras);
                }
                else
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }

        public Directora ObtenerDirectoraPorId(UsuarioLogueado usuarioLogueado, int id)
        {
            Directora dire = new Directora();
            if (Roles.Directora == usuarioLogueado.RolSeleccionado)
                dire = ObtenerDirectoras().Where(x => x.Id == id).FirstOrDefault();
            return dire;
        }

        public Grilla<Directora> ObtenerDirectoras(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            Grilla<Directora> grilla = new Grilla<Directora>();
            List<Directora> lista = ObtenerDirectoras();
            grilla.Lista = lista.Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal) || x.Email.Contains(busquedaGlobal)).Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray();
            grilla.CantidadRegistros = lista.Count;
            return grilla;
        }


        public Resultado ADocente(Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, docente, usuarioLogueado))
            {
                Docentes = ObtenerDocentes();
                List<Clave> claves = ObtenerClaves();
                docente.Id = Docentes.Count() + 1;
                Docentes.Add(docente);
                EscribirDocentes(Docentes);
                Clave clave = new Clave();
                Random random = new Random();
                clave.Contrasena = random.Next(10000000, 99999999).ToString();
                clave.Email = docente.Email;
                clave.Roles = usuarioLogueado.Roles;
                claves.Add(clave);
                EscribirClaves(claves);
            }
            return resultado;
        }
        
        public Resultado BDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            Roles rol = Roles.Docente;
            if (rol != usuarioLogueado.RolSeleccionado & usuarioLogueado.RolSeleccionado != Roles.Directora)
                resultado.Errores.Add("El rol seleccionado no es el de Docente.");
            else
            {
                Docentes = ObtenerDocentes();
                List<Clave> claves = ObtenerClaves();
                docente = Docentes.Where(x => x.Id == docente.Id).FirstOrDefault();
                Clave clave = claves.Where(x => x.Email == docente.Email).FirstOrDefault();
                if (docente != null & clave != null)
                {
                    claves.Remove(clave);
                    EscribirClaves(claves);
                    Docentes.Remove(docente);
                    EscribirDocentes(Docentes);
                }
                else
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }
        
        public Resultado MDocente(int id, Docente docente, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, docente, usuarioLogueado))
            {
                List<Docente> docentes = ObtenerDocentes();
                bool band = false;
                foreach (var doc in docentes)
                {
                    if (doc.Id == id)
                    {
                        List<Clave> claves = ObtenerClaves();
                        foreach (var clave in claves)
                        {
                            if (doc.Email == clave.Email)
                            {
                                clave.Email = docente.Email;
                                EscribirClaves(claves);
                                doc.Apellido = docente.Apellido;
                                doc.Email = docente.Email;
                                doc.Nombre = docente.Nombre;
                                doc.Salas = docente.Salas;
                                EscribirDocentes(docentes);
                                Docentes = docentes;
                                band = true;
                                break;
                            }
                        }
                    }
                }
                if (band == false)
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }

        public Docente ObtenerDocentePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            Docente docente = new Docente();
            if (Roles.Docente == usuarioLogueado.RolSeleccionado | Roles.Directora == usuarioLogueado.RolSeleccionado)
                docente = ObtenerDocentes().Where(x => x.Id == id).FirstOrDefault();
            return docente;
        }

        public Grilla<Docente> ObtenerDocentes(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            Grilla<Docente> grilla = new Grilla<Docente>();
            List<Docente> lista = ObtenerDocentes();
            grilla.Lista = lista.Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal) || x.Email.Contains(busquedaGlobal)).Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray();
            grilla.CantidadRegistros = lista.Count;
            return grilla;
        }



        public Resultado APadre(Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, padre, usuarioLogueado))
            {
                Padres = ObtenerPadres();
                List<Clave> claves = ObtenerClaves();
                padre.Id = Padres.Count() + 1;
                Padres.Add(padre);
                EscribirPadres(Padres);
                Clave clave = new Clave();
                Random random = new Random();
                clave.Contrasena = random.Next(10000000, 99999999).ToString();
                clave.Email = padre.Email;
                clave.Roles = usuarioLogueado.Roles;
                claves.Add(clave);
                EscribirClaves(claves);
            }
            return resultado;
        }

        public Resultado MPadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, padre, usuarioLogueado))
            {
                List<Padre> padres = ObtenerPadres();
                bool band = false;
                foreach (var pa in padres)
                {
                    if (pa.Id == id)
                    {
                        List<Clave> claves = ObtenerClaves();
                        foreach (var clave in claves)
                        {
                            if (clave.Email == pa.Email)
                            {
                                clave.Email = padre.Email;
                                EscribirClaves(claves);
                                pa.Apellido = padre.Apellido;
                                pa.Nombre = padre.Nombre;
                                pa.Email = padre.Email;
                                pa.Hijos = padre.Hijos;
                                EscribirPadres(padres);
                                Padres = padres;
                                band = true;
                                break;
                            }
                        }
                    }
                }
                if (band == false)
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }

        public Resultado BPadre(int id, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            Roles rol = Roles.Padre;
            if (rol != usuarioLogueado.RolSeleccionado & usuarioLogueado.RolSeleccionado != Roles.Directora & usuarioLogueado.RolSeleccionado != Roles.Docente)
                resultado.Errores.Add("El rol seleccionado no es el de Padre.");
            else
            {
                Padres = ObtenerPadres();
                List<Clave> claves = ObtenerClaves();
                padre = Padres.Where(x => x.Id == padre.Id).FirstOrDefault();
                Clave clave = claves.Where(x => x.Email == padre.Email).FirstOrDefault();
                if (padre != null & clave != null)
                {
                    claves.Remove(clave);
                    EscribirClaves(claves);
                    Padres.Remove(padre);
                    EscribirPadres(Padres);
                }
                else
                {
                    resultado.Errores.Add("No se encontró el usuario.");
                }
            }
            return resultado;
        }

        public Padre ObtenerPadrePorId(UsuarioLogueado usuarioLogueado, int id)
        {
            Padre padre = new Padre();
            if (Roles.Padre == usuarioLogueado.RolSeleccionado | Roles.Docente == usuarioLogueado.RolSeleccionado | Roles.Directora == usuarioLogueado.RolSeleccionado)
                padre = ObtenerPadres().Where(x => x.Id == id).FirstOrDefault();
            return padre;
        }
        
        public Grilla<Padre> ObtenerPadres(UsuarioLogueado usuarioLogueado, int paginaActual, int totalPorPagina, string busquedaGlobal)
        {
            Grilla<Padre> grilla = new Grilla<Padre>();
            List<Padre> lista = ObtenerPadres();
            grilla.Lista = lista.Where(x => string.IsNullOrEmpty(busquedaGlobal) || x.Nombre.Contains(busquedaGlobal) || x.Apellido.Contains(busquedaGlobal) || x.Email.Contains(busquedaGlobal)).Skip(paginaActual * totalPorPagina).Take(totalPorPagina).ToArray();
            grilla.CantidadRegistros = lista.Count;
            return grilla;
        }
        

        //HAY QUE HACER UN ARCHIVO DE NOTAS Y OBTENER NOTAS PARA ESCRIBIRLAS
        public Resultado AltaNota(Nota nota, Sala[] salas, Hijo[] hijos, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            Notas = ObtenerNotas();
            nota.Id = Notas.Count() + 1;
            Hijos = ObtenerHijos();
            List<Padre> listapadres = ObtenerPadres();
            if (nota.Titulo != null && nota.Descripcion != null)
            {
                if (usuarioLogueado.RolSeleccionado == Roles.Directora)
                {
                    if (salas != null)
                    {
                        foreach (var sala in salas)
                        {
                            if (hijos.Count() == 0)
                            {
                                foreach (var alumno in Hijos)
                                {
                                    if (alumno.Sala.Id == sala.Id)
                                    {
                                        List<Nota> notashijo = new List<Nota>();
                                        if (alumno.Notas != null)
                                            notashijo = alumno.Notas.ToList();
                                        notashijo.Add(nota);
                                        alumno.Notas = notashijo.ToArray();                                        
                                        Padre padre = listapadres.Where(x => x.Hijos.Any(x2 => x2.Id == alumno.Id)).FirstOrDefault();
                                        List<Nota> notaspadre = new List<Nota>();
                                        Hijo hijopadre = padre.Hijos.Where(x => x.Id == alumno.Id).FirstOrDefault();
                                        if (hijopadre.Notas != null)
                                            notaspadre = hijopadre.Notas.ToList();
                                        notaspadre.Add(nota);
                                        foreach (var pad in listapadres)
                                        {
                                            if (pad.Id == padre.Id)
                                            {
                                                pad.Hijos.Where(x => x.Id == hijopadre.Id).FirstOrDefault().Notas = notaspadre.ToArray();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var hijo in hijos)
                                {
                                    foreach (var alumno in Hijos)
                                    {
                                        if (alumno.Id == hijo.Id & alumno.Sala.Id == sala.Id)
                                        {
                                            List<Nota> notashijo = new List<Nota>();
                                            if (alumno.Notas != null)
                                                notashijo = alumno.Notas.ToList();
                                            notashijo.Add(nota);
                                            alumno.Notas = notashijo.ToArray();
                                            Padre padre = listapadres.Where(x => x.Hijos.Any(x2 => x2.Id == alumno.Id)).FirstOrDefault();
                                            List<Nota> notaspadre = new List<Nota>();
                                            Hijo hijopadre = padre.Hijos.Where(x => x.Id == alumno.Id).FirstOrDefault();
                                            if (hijopadre.Notas != null)
                                                notaspadre = hijopadre.Notas.ToList();
                                            notaspadre.Add(nota);
                                            foreach (var pad in listapadres)
                                            {
                                                if (pad.Id == padre.Id)
                                                {
                                                    pad.Hijos.Where(x => x.Id == hijopadre.Id).FirstOrDefault().Notas = notaspadre.ToArray();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        resultado.Errores.Add("No se ha seleccionado ninguna sala ni alumno.");
                    }
                }
                else
                {
                    if (usuarioLogueado.RolSeleccionado == Roles.Docente)
                    {
                        if (salas != null)
                        {                            
                            Docente docente = ObtenerDocentes().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                            foreach (var salaparametro in salas)
                            {
                                var bandera = false;
                                Sala salaseleccionada = new Sala();
                                foreach (var sala in docente.Salas)
                                {
                                    if (sala.Id == salaparametro.Id)
                                    {
                                        bandera = true;
                                        salaseleccionada = sala;
                                    }
                                }
                                if (bandera == true)
                                {
                                        if (hijos.Count() == 0)
                                        {
                                            foreach (var alumno in Hijos)
                                            {
                                                if (alumno.Sala.Id == salaseleccionada.Id)
                                                {
                                                    List<Nota> notashijo = new List<Nota>();
                                                    if (alumno.Notas != null)
                                                        notashijo = alumno.Notas.ToList();
                                                    notashijo.Add(nota);
                                                    alumno.Notas = notashijo.ToArray();
                                                    Padre padre = listapadres.Where(x => x.Hijos.Any(x2 => x2.Id == alumno.Id)).FirstOrDefault();
                                                    List<Nota> notaspadre = new List<Nota>();
                                                    Hijo hijopadre = padre.Hijos.Where(x => x.Id == alumno.Id).FirstOrDefault();
                                                    if (hijopadre.Notas != null)
                                                        notaspadre = hijopadre.Notas.ToList();
                                                    notaspadre.Add(nota);
                                                    foreach (var pad in listapadres)
                                                    {
                                                        if (pad.Id == padre.Id)
                                                        {
                                                            pad.Hijos.Where(x => x.Id == hijopadre.Id).FirstOrDefault().Notas = notaspadre.ToArray();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (var hijo in hijos)
                                            {
                                                foreach (var alumno in Hijos)
                                                {
                                                    if (alumno.Id == hijo.Id & alumno.Sala.Id == salaseleccionada.Id)
                                                    {
                                                        List<Nota> notashijo = new List<Nota>();
                                                        if (alumno.Notas != null)
                                                            notashijo = alumno.Notas.ToList();
                                                        notashijo.Add(nota);
                                                        alumno.Notas = notashijo.ToArray();
                                                        Padre padre = listapadres.Where(x => x.Hijos.Any(x2 => x2.Id == alumno.Id)).FirstOrDefault();
                                                        List<Nota> notaspadre = new List<Nota>();
                                                        Hijo hijopadre = padre.Hijos.Where(x => x.Id == alumno.Id).FirstOrDefault();
                                                        if (hijopadre.Notas != null)
                                                            notaspadre = hijopadre.Notas.ToList();
                                                        notaspadre.Add(nota);
                                                        foreach (var pad in listapadres)
                                                        {
                                                            if (pad.Id == padre.Id)
                                                            {
                                                                pad.Hijos.Where(x => x.Id == hijopadre.Id).FirstOrDefault().Notas = notaspadre.ToArray();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                }
                                else
                                {
                                    resultado.Errores.Add("Se ha seleccionado una sala incorrecta.");
                                }
                            }  
                        }
                        else
                        {
                            resultado.Errores.Add("No se ha seleccionado ninguna sala ni alumno.");
                        }
                    }
                    else
                    {
                        if (usuarioLogueado.RolSeleccionado == Roles.Padre)
                        {
                            Padre padre = ObtenerPadres().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                            if (hijos.Count() != 0)
                            {
                                foreach (var hijo in hijos)
                                {
                                    foreach (var alumno in Hijos)
                                    {
                                        foreach (var hijodelpadre in padre.Hijos)
                                        {
                                            if (hijodelpadre.Id == alumno.Id && alumno.Id == hijo.Id)
                                            {
                                                List<Nota> notashijo = new List<Nota>();
                                                if (alumno.Notas != null)
                                                    notashijo = alumno.Notas.ToList();
                                                notashijo.Add(nota);
                                                alumno.Notas = notashijo.ToArray();
                                                List<Nota> notaspadre = new List<Nota>();
                                                Hijo hijopadre = padre.Hijos.Where(x => x.Id == alumno.Id).FirstOrDefault();
                                                if (hijopadre.Notas != null)
                                                    notaspadre = hijopadre.Notas.ToList();
                                                notaspadre.Add(nota);
                                                foreach (var pad in listapadres)
                                                {
                                                    if (pad.Id == padre.Id)
                                                    {
                                                        pad.Hijos.Where(x => x.Id == hijopadre.Id).FirstOrDefault().Notas = notaspadre.ToArray();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var hijj in Hijos)
                                {
                                    foreach (var hij in padre.Hijos)
                                    {
                                        if (hijj.Id == hij.Id)
                                        {
                                            List<Nota> notashijo = new List<Nota>();
                                            if (hijj.Notas != null)
                                                notashijo = hijj.Notas.ToList();
                                            notashijo.Add(nota);
                                            hijj.Notas = notashijo.ToArray();
                                            List<Nota> notaspadre = new List<Nota>();
                                            Hijo hijopadre = padre.Hijos.Where(x => x.Id == hij.Id).FirstOrDefault();
                                            if (hijopadre.Notas != null)
                                                notaspadre = hijopadre.Notas.ToList();
                                            notaspadre.Add(nota);
                                            foreach (var pad in listapadres)
                                            {
                                                if (pad.Id == padre.Id)
                                                {
                                                    pad.Hijos.Where(x => x.Id == hijopadre.Id).FirstOrDefault().Notas = notaspadre.ToArray();
                                                }
                                            }
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
            else
            {
                resultado.Errores.Add("Tiene que rellenar todos los campos.");
            }
            if (resultado.Errores.Count == 0)
            {
                Notas.Add(nota);
                EscribirNotas(Notas);
                EscribirHijos(Hijos);
                EscribirPadres(listapadres);
            }           
            return resultado;
        }

        //EDITAR ARCHIVOS DE PADRES E HIJOS, PREGUNTAR SOLAMENTE POR ROL PADRE
        public Resultado MarcarNotaComoLeida(Nota nota, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (usuarioLogueado.RolSeleccionado == Roles.Padre)
            {
                Padres = ObtenerPadres();
                Hijos = ObtenerHijos();
                Padre padre = Padres.Where(x => x.Email == usuarioLogueado.Email && x.Apellido == usuarioLogueado.Apellido && x.Nombre == usuarioLogueado.Nombre).FirstOrDefault();
                List<Hijo> hijos = padre.Hijos.ToList();
                var bandera = false;
                foreach (var hijo in hijos)
                {
                    foreach (var notita in hijo.Notas)
                    {
                        if (notita.Id == nota.Id)
                        {
                            foreach (var pad in Padres)
                            {
                                if (pad.Hijos != null)
                                {
                                    pad.Hijos.Where(x => x.Id == hijo.Id).FirstOrDefault().Notas.Where(x => x.Id == notita.Id).FirstOrDefault().Leida = true;
                                    Hijos.Where(x => x.Id == hijo.Id).FirstOrDefault().Notas.Where(x => x.Id == notita.Id).FirstOrDefault().Leida = true;
                                    bandera = true;
                                }
                                
                            }
                        }
                    }
                }
                if (bandera == false)
                {
                    resultado.Errores.Add("La nota se ha seleccionado erroneamente.");
                }
                else
                {
                    EscribirHijos(Hijos);
                    EscribirPadres(Padres);
                }
            }
            else
            {
                resultado.Errores.Add("El usuario no tiene el rol necesario para marcar la nota como leida.");
            }
            if (nota == null)
                resultado.Errores.Add("No se ha seleccionado ninguna nota.");
            
            return resultado;
        }


        public Resultado AsignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Sala> salasDocente = new List<Sala>();
            if (docente.Salas != null)
                salasDocente = docente.Salas.ToList();

            if (salasDocente.Any(x => x.Id == sala.Id) == false)
                salasDocente.Add(sala);
            else
                resultado.Errores.Add("La sala ya se encuentra asignada.");

            docente.Salas = salasDocente.ToArray();
            List<Docente> docentes = ObtenerDocentes();
            foreach (var item in docentes)
            {
                if (docente.Id == item.Id)
                {
                    item.Salas = docente.Salas;
                }
            }
            EscribirDocentes(docentes);

            return resultado;
        }

        public Resultado DesasignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            var salasDocente = docente.Salas != null ? docente.Salas.ToList() : new List<Sala>();

            if (salasDocente.Any(x => x.Id == sala.Id) == true)
                salasDocente.Remove(sala);

            docente.Salas = salasDocente.ToArray();
            List<Docente> docentes = ObtenerDocentes();
            foreach (var item in docentes)
            {
                if (docente.Id == item.Id)
                {
                    item.Salas = docente.Salas;
                }
            }
            EscribirDocentes(docentes);

            return new Resultado();
        }


        public bool VerificarCampos(Resultado resul, Directora directora, UsuarioLogueado usuarioLog)
        {
            Roles rol = Roles.Directora;
            if (rol != usuarioLog.RolSeleccionado)
                resul.Errores.Add("El rol seleccionado no es el de Directora.");
            else
            {
                if (directora.Nombre == null)
                    resul.Errores.Add("El nombre es un campo obligatorio.");
                else
                {
                    if (directora.Apellido == null)
                        resul.Errores.Add("El apellido es un campo obligatorio.");
                    else
                    {
                        if (directora.Email == null)
                            resul.Errores.Add("El email no es válido o está vacío.");
                        else
                        {
                            if (directora.Cargo == null)
                                resul.Errores.Add("El cargo es un campo obligatorio.");
                            else
                            {
                                if (directora.FechaIngreso == null)
                                    resul.Errores.Add("La fecha de ingreso es un campo obligatorio.");
                            }
                        }
                    }
                }
            }

            return resul.EsValido;
        }

        public bool VerificarCampos(Resultado resul, Docente docente, UsuarioLogueado usuarioLog)
        {
            
            if (Roles.Docente != usuarioLog.RolSeleccionado & Roles.Directora != usuarioLog.RolSeleccionado)
                resul.Errores.Add("El rol seleccionado no es el de Docente.");
            else
            {
                if (docente.Apellido == null)
                    resul.Errores.Add("El apellido es un campo obligatorio.");
                else
                {
                    if (docente.Email == null)
                        resul.Errores.Add("El email no es válido o está vacío.");
                    else
                    {
                        if (docente.Nombre == null)
                            resul.Errores.Add("El nombre es un campo obligatorio.");
                    }
                }
            }
            return resul.EsValido;
        }

        public bool VerificarCampos(Resultado resul, Padre padre, UsuarioLogueado usuarioLog)
        {
           
            if (Roles.Docente != usuarioLog.RolSeleccionado & Roles.Directora != usuarioLog.RolSeleccionado & Roles.Padre != usuarioLog.RolSeleccionado)
                resul.Errores.Add("El rol seleccionado no es el de Docente.");
            else
            {
                if (padre.Apellido == null)
                    resul.Errores.Add("El apellido es un campo obligatorio.");
                else
                {
                    if (padre.Email == null)
                        resul.Errores.Add("El email no es válido o está vacío.");
                    else
                    {
                        if (padre.Nombre == null)
                            resul.Errores.Add("El nombre es un campo obligatorio.");
                    }
                }
            }
            return resul.EsValido;
        }

        public bool VerificarCampos(Resultado resul, Hijo hijo, UsuarioLogueado usuarioLog)
        {

            if (hijo.Apellido == null)
                resul.Errores.Add("El apellido es un campo obligatorio.");
            else
            {
                if (hijo.Email == null)
                    resul.Errores.Add("El email no es válido o está vacío.");
                else
                {
                    if (hijo.Nombre == null)
                        resul.Errores.Add("El nombre es un campo obligatorio.");
                    else
                    {
                        if (hijo.FechaNacimiento == null)
                        {
                            resul.Errores.Add("La fecha de nacimiento es un campo obligatorio.");
                        }
                      
                    }
                }
            }
            return resul.EsValido;
        }


        public List<Clave> ObtenerClaves()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoClaves.txt");
            List<Clave> lista = new List<Clave>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Clave>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Clave>();
            else
                return lista;
        }

        public List<Directora> ObtenerDirectoras()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoDirectoras.txt");
            List<Directora> lista = new List<Directora>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Directora>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Directora>();
            else
                return lista;
        }

        public List<Docente> ObtenerDocentes()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoDocentes.txt");
            List<Docente> lista = new List<Docente>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Docente>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Docente>();
            else
                return lista;
        }

        public List<Padre> ObtenerPadres()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoPadres.txt");
            List<Padre> lista = new List<Padre>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Padre>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Padre>();
            else
                return lista;
        }

        public List<Hijo> ObtenerHijos()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoHijos.txt");
            List<Hijo> lista = new List<Hijo>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Hijo>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Hijo>();
            else
                return lista;
        }
        
        public List<Sala> ObtenerSalas()
        {
            List<Sala> lista = new List<Sala>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoSalas.txt");
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Sala>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Sala>();
            else
                return lista;
        }

        public List<Nota> ObtenerNotas()
        {
            List<Nota> lista = new List<Nota>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivoNotas.txt");
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Nota>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Nota>();
            else
                return lista;
        }


        public Resultado AsignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Hijo> hijosPadre = new List<Hijo>();
            if (padre.Hijos != null)
                hijosPadre = padre.Hijos.ToList();

            if (hijosPadre.Any(x => x.Id == hijo.Id) == false)
                hijosPadre.Add(hijo);
            else
                resultado.Errores.Add("La sala ya se encuentra asignada.");

            padre.Hijos = hijosPadre.ToArray();
            List<Padre> padres = ObtenerPadres();
            foreach (var item in padres)
            {
                if (padre.Id == item.Id)
                {
                    item.Hijos = padre.Hijos;
                }
            }
            EscribirPadres(padres);
            return resultado;
        }

        public Resultado DesasignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Hijo> hijos = new List<Hijo>();
            List<Padre> padres = ObtenerPadres();
            bool flag = false;
            if (usuarioLogueado.RolSeleccionado == Roles.Padre | usuarioLogueado.RolSeleccionado == Roles.Directora | usuarioLogueado.RolSeleccionado == Roles.Docente)
            {
                foreach (var pad in padres)
                {
                    if (pad.Id == padre.Id)
                    {
                        hijos = pad.Hijos.ToList();
                        foreach (var hij in hijos)
                        {
                            if (hij.Id == hijo.Id)
                            {
                                hijos.Remove(hij);
                                flag = true;
                                break;
                            }
                        }
                        pad.Hijos = hijos.ToArray();
                        EscribirPadres(padres);
                        break;
                    }
                    if (hijos == null)
                        resultado.Errores.Add("Padre no encontrado.");
                    else
                    {
                        if (flag == false)
                            resultado.Errores.Add("Hijo no encontrado.");
                    }
                }
            }
            else
                resultado.Errores.Add("Rol no corresponde a padre/madre.");
            return resultado;
        }

        public Nota[] ObtenerCuadernoComunicaciones(int idPersona, UsuarioLogueado usuarioLogueado)
        {
            if (ObtenerHijos().Where(x => x.Id == idPersona).FirstOrDefault().Notas != null)
                return ObtenerHijos().Where(x => x.Id == idPersona).FirstOrDefault().Notas;
            else
                return new List<Nota>().ToArray();
        }

        public Hijo[] ObtenerPersonas(UsuarioLogueado usuarioLogueado)
        {
            List<Hijo> hijos = new List<Hijo>();
            if (usuarioLogueado.RolSeleccionado == Roles.Padre)
            {
                foreach (var padre in ObtenerPadres())
                {
                    if (padre.Email == usuarioLogueado.Email)
                    {
                        if (padre.Hijos != null)
                        {
                            hijos = padre.Hijos.ToList();
                            break;
                        }
                    }
                }
            }
            else
            {
                if (usuarioLogueado.RolSeleccionado == Roles.Docente)
                {
                    Docente docente = ObtenerDocentes().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                    foreach (var sala in docente.Salas)
                    {
                        foreach (var alumno in ObtenerHijos())
                        {
                            if (alumno.Sala.Id == sala.Id & hijos.Any(x => x == alumno) == false)
                                hijos.Add(alumno);
                        }
                    }
                }
                else
                {
                    if (usuarioLogueado.RolSeleccionado == Roles.Directora)
                        hijos.AddRange(ObtenerHijos());
                }
            }
            return hijos.ToArray();
        }

        public Sala[] ObtenerSalasPorInstitucion(UsuarioLogueado usuarioLogueado)
        {
            List<Sala> salas = new List<Sala>();
            if (usuarioLogueado.RolSeleccionado == Roles.Docente | usuarioLogueado.RolSeleccionado == Roles.Directora)
                salas = ObtenerSalas();
            return salas.ToArray();
        }

        public Resultado ResponderNota(Nota nota, Comentario nuevoComentario, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Nota> notas = new List<Nota>();
            Hijos = ObtenerHijos();
            Padres = ObtenerPadres();
            Notas = ObtenerNotas();
            if (usuarioLogueado.RolSeleccionado == Roles.Padre | usuarioLogueado.RolSeleccionado == Roles.Docente | usuarioLogueado.RolSeleccionado == Roles.Directora)
            {
                foreach (var hijo in Hijos)
                {
                    if (hijo.Notas != null)
                    {
                        foreach (var not in hijo.Notas)
                        {
                            if (not.Id == nota.Id)
                            {
                                List<Comentario> comentarios = new List<Comentario>();
                                comentarios = not.Comentarios.ToList();
                                comentarios.Add(nuevoComentario);
                                not.Comentarios = comentarios.ToArray();
                            }
                        }
                    }
                }
                foreach (var padre in Padres)
                {
                    if (padre.Hijos != null)
                    {
                        foreach (var hijo in padre.Hijos)
                        {
                            if (hijo.Notas != null)
                            {
                                foreach (var not in hijo.Notas)
                                {
                                    if (not.Id == nota.Id)
                                    {
                                        List<Comentario> comentarios = new List<Comentario>();
                                        comentarios = not.Comentarios.ToList();
                                        comentarios.Add(nuevoComentario);
                                        not.Comentarios = comentarios.ToArray();
                                    }
                                }
                            }
                        }
                    }
                    
                }
                foreach (var not in Notas)
                {
                    if (not.Id == nota.Id)
                    {
                        List<Comentario> comentarios = new List<Comentario>();
                        comentarios = not.Comentarios.ToList();
                        comentarios.Add(nuevoComentario);
                        not.Comentarios = comentarios.ToArray();
                    }
                }
                EscribirNotas(Notas);
                EscribirPadres(Padres);
                EscribirHijos(Hijos);
            }
            else
                resultado.Errores.Add("No es posible agregar comentario con el rol seleccionado.");
            
            return resultado;
        }
    }
}
