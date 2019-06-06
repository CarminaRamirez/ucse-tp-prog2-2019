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
        public List<Usuario> Usuarios { get; set; }
        public List<Hijo> Hijos { get; set; }

        public Principal()
        {
            Usuarios = new List<Usuario>();
            Hijos = new List<Hijo>();
        }
        

        public UsuarioLogueado Loguear (string email, string clave)
        {
            UsuarioLogueado usuario = null;
            List<Clave> claves = ObtenerClaves();
            foreach (var item in claves)
            {
                if (item.Email == email & item.Contraseña == clave)
                {
                    usuario.Email = email;
                    usuario.Roles = item.Roles;
                   
                    List<Usuario> usuarios = ObtenerUsuarios();
                    foreach (var usu in usuarios)
                    {
                        if (usu.Email == email)
                        { 
                            usuario.Nombre = usu.Nombre;
                            usuario.Apellido = usu.Apellido;
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

        public Resultado ADirectora(Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado, directora, usuarioLogueado))
            {
                Usuarios.Add(directora);
                EscribirUsuarios(Usuarios);
                List<Clave> claves = ObtenerClaves();
                Clave clave = new Clave();
                Random random = new Random();
                clave.Contraseña = random.Next(10000000, 99999999).ToString();
                clave.Email = directora.Email;
                clave.Roles = usuarioLogueado.Roles;
                claves.Add(clave);
                EscribirClaves(claves);
            }
                            
            return resultado;
        }

        public Resultado MDirectora (int id, Directora directora, UsuarioLogueado usuarioLogueado)
        {
            Resultado resultado = new Resultado();
            if (VerificarCampos(resultado,directora,usuarioLogueado))
            {
                List<Usuario> usuarios = ObtenerUsuarios();
                bool band = false;
                foreach (var usuario in usuarios)
                {
                    if (usuario.Id == id & usuario is Directora)
                    {
                        List<Clave> claves = ObtenerClaves();
                        foreach (var clave in claves)
                        {
                            if (usuario.Email == usuarioLogueado.Email)
                            {
                                clave.Email = directora.Email;
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
                    Usuario direc = Usuarios.Where(x => x.Id == id && x is Directora).FirstOrDefault();
                    Usuarios.Remove(direc);
                    Usuarios.Add(directora);
                    EscribirUsuarios(Usuarios);
                }
            }
              
            
            return resultado;
        }

        public Resultado BDirectora (int id, Directora directora, UsuarioLogueado usuarioLogueado)
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
                        foreach (var item2 in Usuarios)
                        {
                            x2++;
                            if (item2.Id == id & item2 is Directora)
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
                clave.Contraseña = random.Next(10000000, 99999999).ToString();
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
                clave.Contraseña = random.Next(10000000, 99999999).ToString();
                clave.Email = padre.Email;
                clave.Roles = usuarioLogueado.Roles;
                claves.Add(clave);
                EscribirClaves(claves);
            }

            return resultado;
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
                                else
                                {
                                    if (directora.Institucion == null)
                                        resul.Errores.Add("La institucion es un campo obligatorio.");
                                }
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
