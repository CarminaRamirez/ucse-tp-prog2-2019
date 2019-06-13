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
            Usuarios = new List<Usuario>();
            Hijos = new List<Hijo>();
            Directoras = new List<Directora>();
        }
        public List<Usuario> Usuarios { get; set; }
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

        public List<Usuario> ObtenerUsuarios()
        {
            string path = @"C:\Datos\ArchivoUsuarios.txt";
            List<Usuario> lista = new List<Usuario>();
            using (StreamReader leer = new StreamReader(path))
            {
                string contenido = leer.ReadToEnd();
                lista = JsonConvert.DeserializeObject<List<Usuario>>(contenido);
            }
            if (lista == null)// ==> devolver 
                return new List<Usuario>();
            else
                return lista;
        }

        public void EscribirUsuarios(List<Usuario> usuarios)
        {
            string path = @"C:\Datos\ArchivoUsuarios.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(usuarios);
                archivo.Write(output);
            }
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


        public void EscribirDirectoras(List<Directora> directoras)
        {
            string path = @"C:\Datos\ArchivoDirectoras.txt";
            using (StreamWriter archivo = new System.IO.StreamWriter(path, false))
            {
                string output = JsonConvert.SerializeObject(directoras);
                archivo.Write(output);
            }
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
                Usuarios.Add(directora);
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
            //List<Clave> claves = ObtenerClaves();
            Directora dire = new Directora();
            //foreach (var item in claves)
            //{
                if (Roles.Directora == usuarioLogueado.RolSeleccionado)
                    dire = ObtenerDirectoras().Where(x => x.Id == id).FirstOrDefault();
            //}
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
                Usuarios.Add(docente);
                EscribirUsuarios(Usuarios);
                List<Clave> claves = ObtenerClaves();
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
                resultado.Errores.Add("El rol seleccionado no es el de Docente.");
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
                        foreach (var item2 in Usuarios)
                        {
                            x2++;
                            if (item2.Id == id & item2 is Docente)
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
                    Usuarios.RemoveAt(x2);
                    EscribirUsuarios(Usuarios);
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
                List<Usuario> usuarios = ObtenerUsuarios();
                bool band = false;
                foreach (var usuario in usuarios)
                {
                    if (usuario.Id == id & usuario is Docente)
                    {
                        List<Clave> claves = ObtenerClaves();
                        foreach (var clave in claves)
                        {
                            if (usuario.Email == usuarioLogueado.Email)
                            {
                                clave.Email = docente.Email;
                                EscribirClaves(claves);
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
                else
                {
                    Usuario direc = Usuarios.Where(x => x.Id == id && x is Docente).FirstOrDefault();
                    Usuarios.Remove(direc);
                    Usuarios.Add(docente);
                    EscribirUsuarios(Usuarios);
                }
            }
            return resultado;
        }



        public Resultado APadre(Padre padre, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, padre, usuarioLogueado))
            {
                Usuarios.Add(padre);
                EscribirUsuarios(Usuarios);
                List<Clave> claves = ObtenerClaves();
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
                                        int indice = alumno.Notas.Count();
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
                                            int indice = alumno.Notas.Count();
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
                                                int indice = alumno.Notas.Count();
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
                                                    int indice = alumno.Notas.Count();
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
                            if (hijos != null)
                            {
                                foreach (var hijo in hijos)
                                {
                                    foreach (var alumno in ObtenerHijos())
                                    {
                                        if (alumno.Id == hijo.Id)
                                        {
                                            int indice = alumno.Notas.Count();
                                            alumno.Notas[indice] = nota;
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
    }
}
