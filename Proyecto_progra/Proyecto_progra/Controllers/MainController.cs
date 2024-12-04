using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_progra.Models;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace Proyecto_progra.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Mainpage()
        {
            return View();
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }

        public ActionResult CrearConcierto()
        {
            return View();
        }
        public ActionResult BorrarConcierto()
        {
            return View();
        }

        [HttpPost]

        public JsonResult Login(string CORREO, string CLAVE)
        {
            using (ConciertosEntities1 bd = new ConciertosEntities1())
            {
                try
                {
                    var resultado = bd.Database.SqlQuery<int?>(
                        "EXEC SP_VALIDARUSUARIO @CORREO, @CLAVE",
                        new System.Data.SqlClient.SqlParameter("@CORREO", CORREO),
                        new System.Data.SqlClient.SqlParameter("@CLAVE", CLAVE)
                    ).FirstOrDefault();

                    if (resultado != null && resultado > 0)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Login exitoso",
                            data = new
                            {
                                IDUSUARIO = resultado
                            }
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Correo o clave incorrectos"
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ocurrió un error durante el proceso de login",
                        error = ex.Message
                    });
                }
            }
        }

        [HttpPost]
        public JsonResult AgregarConcierto( string nombre_Banda,string genero_Musical,DateTime? fecha_Concierto,TimeSpan? hora_Concierto,string pais,string direccion_Concierto)
        {
            try
            {
                // Validar que los parámetros no sean nulos o vacíos
                if (string.IsNullOrWhiteSpace(nombre_Banda) || string.IsNullOrWhiteSpace(genero_Musical) ||
                    !fecha_Concierto.HasValue || !hora_Concierto.HasValue ||
                    string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(direccion_Concierto))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Todos los campos son obligatorios."
                    });
                }

                using (var bd = new ConciertosEntities1())
                {
                    // Crear parámetros de salida
                    var registrado = new ObjectParameter("REGISTRADO", typeof(bool));
                    var mensaje = new ObjectParameter("MENSAJE", typeof(string));

                    // Llamar al procedimiento almacenado
                    bd.SP_REGISTRARCONCIERTO(
                        nombre_Banda,
                        genero_Musical,
                        fecha_Concierto,
                        hora_Concierto,
                        pais,
                        direccion_Concierto,
                        registrado,
                        mensaje
                    );

                    // Obtener valores de salida
                    bool registradoValue = (bool)registrado.Value;
                    string mensajeValue = (string)mensaje.Value;

                    // Retornar respuesta según el resultado del procedimiento almacenado
                    return Json(new
                    {
                        success = registradoValue,
                        message = mensajeValue
                    });
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return Json(new
                {
                    success = false,
                    message = "Error al registrar el concierto.",
                    error = ex.Message
                });
            }
        }

        public JsonResult ObtenerConciertos()
        {
            using (ConciertosEntities1 bd = new ConciertosEntities1())
            {
                // Realizas la consulta sin formatear las fechas
                var conciertos = bd.Concierto.Select(c => new
                {
                    c.Nombre_Banda,
                    c.Genero_Musical,
                    c.Fecha_Concierto,
                    c.Hora_Concierto,
                    c.Pais,
                    c.Direccion_Concierto
                }).ToList();

                // Luego, formateas las fechas y horas después de la consulta
                var conciertosFormateados = conciertos.Select(c => new
                {
                    c.Nombre_Banda,
                    c.Genero_Musical,
                    Fecha_Concierto = c.Fecha_Concierto.ToString("yyyy-MM-dd"),
                    Hora_Concierto = c.Hora_Concierto.ToString(@"hh\:mm"),
                    c.Pais,
                    c.Direccion_Concierto
                }).ToList();

                // Ahora puedes pasar estos conciertos formateados a tu vista o hacer lo que necesites con ellos
                return Json(new
                {
                    success = true,
                    data = conciertosFormateados // Se devuelven los datos formateados
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EliminarConcierto(int codigo)
        {
            try
            {
                using (ConciertosEntities1 db = new ConciertosEntities1()) // Asegúrate de usar tu DbContext
                {
                    // Buscar el concierto por el código
                    var concierto = db.Concierto.FirstOrDefault(c => c.ID == codigo);

                    if (concierto == null)
                    {
                        return Json(new { success = false, message = "Concierto no encontrado." }, JsonRequestBehavior.AllowGet);
                    }

                    // Eliminar el concierto
                    db.Concierto.Remove(concierto);
                    db.SaveChanges();

                    return Json(new { success = true, message = "Concierto eliminado exitosamente." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al intentar eliminar el concierto: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult Modificar_Concierto()
        {
            return View();
        }




    }
}