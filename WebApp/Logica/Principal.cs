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
        }
        
        public List<Hijo> Hijos { get; set; }
        public List<Directora> Directoras { get; set; }
        public List<Padre> Padres { get; set; }
        public List<Docente> Docentes { get; set; }
        public List<Nota> Notas { get; set; }


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
                                    usuario.RolSeleccionado = Roles.Directora;
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
                                        usuario.RolSeleccionado = Roles.Directora;
                                    }
                                }
                            }
                            else
                            {
                                List<Hijo> hijos = ObtenerHijos();
                                foreach (var hijo in hijos)
                                {
                                    if (hijo.Email == email)
                                    {
                                        usuario.Email = email;
                                        usuario.Roles = item.Roles;
                                        usuario.Nombre = hijo.Nombre;
                                        usuario.Apellido = hijo.Apellido;
                                        usuario.RolSeleccionado = Roles.Directora;
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
            string path = @"C:\Datos\ArchivoClaves.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(claves);
                archivo.Write(output);
            }
        }

        public void EscribirHijos(List<Hijo> hijos)
        {
            string path = @"C:\Datos\ArchivoHijos.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(hijos);
                archivo.Write(output);
            }
        }

        public void EscribirDocentes(List<Docente> docentes)
        {
            string path = @"C:\Datos\ArchivoClaves.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(docentes);
                archivo.Write(output);
            }
        }

        public void EscribirPadres(List<Padre> padres)
        {
            string path = @"C:\Datos\ArchivoPadres.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(padres);
                archivo.Write(output);
            }
        }

        public void EscribirDirectoras(List<Directora> directoras)
        {
            string path = @"C:\Datos\ArchivoDirectoras.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(directoras);
                archivo.Write(output);
            }
        }


        public Resultado AAlumno(Hijo hijo, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, hijo, usuarioLogueado))
            {
                Hijos = ObtenerHijos();
                List<Clave> claves = ObtenerClaves();
                hijo.Id = Hijos.Count() + 1;
                Hijos.Add(hijo);
                EscribirHijos(Hijos);
                Clave clave = new Clave();
                Random random = new Random();
                clave.Contrasena = random.Next(10000000, 99999999).ToString();
                clave.Email = hijo.Email;
                clave.Roles = usuarioLogueado.Roles;
                claves.Add(clave);
                EscribirClaves(claves);

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
                        List<Clave> claves = ObtenerClaves();
                        foreach (var clave in claves)
                        {
                            if (clave.Email == usuarioLogueado.Email)
                            {
                                clave.Email = hijo.Email;
                                EscribirClaves(claves);
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
            
                List<Clave> claves = ObtenerClaves();
                bool band = false;
                int x = -1;
                int x2 = -1;
                foreach (var item in claves)
                {
                    x++;
                    if (item.Email == usuarioLogueado.Email)
                    {
                        foreach (var item2 in Hijos)
                        {
                            x2++;
                            if (item2.Id == id)
                            {
                                band = true;
                                break;
                            }

                        }
                        break;
                    }

                }
                if (band == true)
                {
                    claves.RemoveAt(x);
                    EscribirClaves(claves);
                    Hijos.RemoveAt(x2);
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
                            if (clave.Email == usuarioLogueado.Email)
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
                List<Clave> claves = ObtenerClaves();
                bool band = false;
                int x = -1;
                int x2 = -1;
                foreach (var item in claves)
                {
                    x++;
                    if (item.Email == usuarioLogueado.Email)
                    {
                        foreach (var item2 in Directoras)
                        {
                            x2++;
                            if (item2.Id == id)
                            {
                                band = true;
                                break;
                            }

                        }
                        break;
                    }

                }
                if (band == true)
                {
                    claves.RemoveAt(x);
                    EscribirClaves(claves);
                    Directoras.RemoveAt(x2);
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
                Directoras = ObtenerDirectoras();
                List<Clave> claves = ObtenerClaves();
                docente.Id = Directoras.Count() + 1;
                Docentes.Add(docente);
                EscribirDirectoras(Directoras);
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
            if (rol != usuarioLogueado.RolSeleccionado)
                resultado.Errores.Add("El rol seleccionado no es el de Diocente.");
            else
            {
                List<Clave> claves = ObtenerClaves();
                bool band = false;
                int x = -1;
                int x2 = -1;
                foreach (var item in claves)
                {
                    x++;
                    if (item.Email == usuarioLogueado.Email)
                    {
                        foreach (var item2 in Docentes)
                        {
                            x2++;
                            if (item2.Id == id)
                            {
                                band = true;
                                break;
                            }

                        }
                        break;
                    }

                }
                if (band == true)
                {
                    claves.RemoveAt(x);
                    EscribirClaves(claves);
                    Docentes.RemoveAt(x2);
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
                            if (doc.Email == usuarioLogueado.Email)
                            {
                                clave.Email = docente.Email;
                                EscribirClaves(claves);
                                doc.Apellido = docente.Apellido;
                                doc.Email = docente.Email;
                                doc.Nombre = docente.Nombre;
                                doc.Salas = docente.Salas;
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
            if (Roles.Docente == usuarioLogueado.RolSeleccionado)
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
                Directoras = ObtenerDirectoras();
                List<Clave> claves = ObtenerClaves();
                padre.Id = Padres.Count() + 1;
                Padres.Add(padre);
                EscribirDirectoras(Directoras);
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
                            if (clave.Email == usuarioLogueado.Email)
                            {
                                clave.Email = padre.Email;
                                EscribirClaves(claves);
                                pa.Apellido = padre.Apellido;
                                pa.Nombre = padre.Nombre;
                                pa.Email = padre.Email;
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
            if (rol != usuarioLogueado.RolSeleccionado)
                resultado.Errores.Add("El rol seleccionado no es el de Padre.");
            else
            {
                List<Clave> claves = ObtenerClaves();
                bool band = false;
                int x = -1;
                int x2 = -1;
                foreach (var item in claves)
                {
                    x++;
                    if (item.Email == usuarioLogueado.Email)
                    {
                        foreach (var item2 in Padres)
                        {
                            x2++;
                            if (item2.Id == id)
                            {
                                band = true;
                                break;
                            }

                        }
                        break;
                    }

                }
                if (band == true)
                {
                    claves.RemoveAt(x);
                    EscribirClaves(claves);
                    Padres.RemoveAt(x2);
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
            if (Roles.Padre == usuarioLogueado.RolSeleccionado)
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
            if (nota.Titulo != null && nota.Descripcion != null)
            {
                if (usuarioLogueado.RolSeleccionado == Roles.Directora)
                {
                    if (salas != null)
                    {
                        foreach (var sala in salas)
                        {
                            if (hijos == null)
                            {
                                foreach (var alumno in ObtenerHijos())
                                {
                                    if (alumno.Sala == sala)
                                    {
                                        int indice = alumno.Notas.Count()-1;
                                        alumno.Notas[indice] = nota;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var hijo in hijos)
                                {
                                    foreach (var alumno in ObtenerHijos())
                                    {
                                        if (alumno.Id == hijo.Id & alumno.Sala == sala)
                                        {
                                            int indice = alumno.Notas.Count() - 1;
                                            alumno.Notas[indice] = nota;
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
                            var bandera = true;
                            Docente docente = ObtenerDocentes().Where(x => x.Email == usuarioLogueado.Email).FirstOrDefault();
                            foreach (var sala in docente.Salas)
                            {
                                if (salas.Contains(sala) == false)
                                {
                                    bandera = false;
                                }
                            }
                            if (bandera == true)
                            {
                                foreach (var sala in salas)
                                {
                                    if (hijos == null)
                                    {
                                        foreach (var alumno in ObtenerHijos())
                                        {
                                            if (alumno.Sala == sala)
                                            {
                                                int indice = alumno.Notas.Count()-1;
                                                alumno.Notas[indice] = nota;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var hijo in hijos)
                                        {
                                            foreach (var alumno in ObtenerHijos())
                                            {
                                                if (alumno.Id == hijo.Id & alumno.Sala == sala)
                                                {
                                                    int indice = alumno.Notas.Count()-1;
                                                    alumno.Notas[indice] = nota;
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
                            if (hijos != null)
                            {
                                foreach (var hijo in hijos)
                                {
                                    foreach (var alumno in ObtenerHijos())
                                    {
                                        foreach (var hijodelpadre in padre.Hijos)
                                        {
                                            if (hijodelpadre == alumno && alumno.Id == hijo.Id)
                                            {
                                                int indice = alumno.Notas.Count()-1;
                                                alumno.Notas[indice] = nota;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                resultado.Errores.Add("No se ha seleccionado ningun alumno.");
                            }
                        }
                    }
                }
            }
            else
            {
                resultado.Errores.Add("Tiene que rellenar todos los campos.");
            }
            return resultado;
        }

        public Resultado MarcarNotaComoLeida(Nota nota, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (usuarioLogueado.RolSeleccionado == Roles.Padre)
            {
                Padre padre = ObtenerPadres().Where(x => x.Email == usuarioLogueado.Email && x.Apellido == usuarioLogueado.Apellido && x.Nombre == usuarioLogueado.Nombre).FirstOrDefault();
                List<Hijo> hijos = padre.Hijos.ToList();
                var bandera = false;
                foreach (var hijo in hijos)
                {
                    foreach (var notita in hijo.Notas)
                    {
                        if (notita.Id == nota.Id)
                        {
                            nota.Leida = true;
                            bandera = true;
                        }
                    }
                }
                if (bandera == false)
                {
                    resultado.Errores.Add("La nota se ha seleccionado erroneamente.");
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

            return resultado;
        }

        public Resultado DesasignarDocenteSala(Docente docente, Sala sala, UsuarioLogueado usuarioLogueado)
        {
            var salasDocente = docente.Salas != null ? docente.Salas.ToList() : new List<Sala>();

            if (salasDocente.Any(x => x.Id == sala.Id) == true)
                salasDocente.Remove(sala);

            docente.Salas = salasDocente.ToArray();

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
            Roles rol = Roles.Docente;
            if (rol != usuarioLog.RolSeleccionado)
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
                        else
                        {
                            if (docente.Salas.Count() == 0)
                                resul.Errores.Add("El docente tiene que tener salas asignadas.");
                        }
                    }
                }
            }
            return resul.EsValido;
        }

        public bool VerificarCampos(Resultado resul, Padre padre, UsuarioLogueado usuarioLog)
        {
            Roles rol = Roles.Padre;
            if (rol != usuarioLog.RolSeleccionado)
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
                        else
                        {
                            if (padre.Hijos.Count() == 0)
                                resul.Errores.Add("El padre tiene que tener hijos.");
                        }
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
                        if (hijo.Institucion == null)
                            resul.Errores.Add("El alumno debe asistir a una institucion.");
                        else
                        {
                            if (hijo.FechaNacimiento == null)
                            {
                                resul.Errores.Add("La fecha de nacimiento es un campo obligatorio.");
                            }
                        }
                    }
                }
            }
            return resul.EsValido;
        }


        public List<Clave> ObtenerClaves()
        {
            string path = @"C:\Datos\ArchivoClaves.txt";
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
            string path = @"C:\Datos\ArchivoDirectoras.txt";
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
            string path = @"C:\Datos\ArchivoDocentes.txt";
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
            string path = @"C:\Datos\ArchivoPadres.txt";
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
            string path = @"C:\Datos\ArchivoHijos.txt";
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


        public Resultado AsignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Padre> padres = ObtenerPadres();
            bool flag = false;
            foreach (var pad in padres)
            {
                if (pad.Id == padre.Id)
                {
                    int indice = padre.Hijos.Count();
                    padre.Hijos[indice] = hijo;
                    EscribirPadres(padres);
                    flag = true;
                    break;
                }
            }
            if (flag == false)
                resultado.Errores.Add("El padre no se encuentra registrado.");
            return resultado;
        }

        public Resultado DesasignarHijoPadre(Hijo hijo, Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Hijo> hijos = new List<Hijo>();
            List<Padre> padres = ObtenerPadres();
            bool flag = false;
            if (usuarioLogueado.RolSeleccionado == Roles.Padre)
            {
                foreach (var pad in padres)
                {
                    if (pad.Id == padre.Id)
                    {
                        hijos = pad.Hijos.ToList();
                        foreach (var hij in hijos)
                        {
                            if (hij == hijo)
                            {
                                hijos.Remove(hij);
                                flag = true;
                                break;
                            }
                        }
                        pad.Hijos = hijos.ToArray();
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
            return ObtenerHijos().Where(x => x.Id == idPersona).FirstOrDefault().Notas;
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
                        hijos = padre.Hijos.ToList();
                        break;
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
                            if (alumno.Sala == sala & hijos.Any(x => x == alumno) == false)
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
            foreach (var docente in ObtenerDocentes())
            {
                foreach (var sala in docente.Salas)
                {
                    Sala sa = salas.Where(x => x.Id == sala.Id).FirstOrDefault();
                    if (sa == null)
                        salas.Add(sala);
                }
            }
            return salas.ToArray();
        }

        public Resultado ResponderNota(Nota nota, Comentario nuevoComentario, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            List<Nota> notas = new List<Nota>();
            bool flag = false;
            if (usuarioLogueado.RolSeleccionado == Roles.Padre | usuarioLogueado.RolSeleccionado == Roles.Docente | usuarioLogueado.RolSeleccionado == Roles.Directora)
            {
                foreach (var hijo in ObtenerHijos())
                {
                    foreach (var not in hijo.Notas)
                    {
                        if (not == nota)
                        {
                            int indice = nota.Comentarios.Count();
                            nota.Comentarios[indice] = nuevoComentario;
                            flag = true;
                            break;
                        }
                    }
                }
            }
            else
                resultado.Errores.Add("No es posible agregar comentario con el rol seleccionado.");
            if (flag == false)
                resultado.Errores.Add("No se ha encontrado la nota correspondiente.");
            
            throw new NotImplementedException();
        }
    }
}
