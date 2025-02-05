using Microsoft.AspNetCore.Mvc;
using user_register.Data;
using user_register.Models;

namespace user_register.Controllers
{
    public class HomeController : Controller
    {

        private readonly DataAccess _dataAccess;

        public HomeController(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            try
            {
                var usuarios = _dataAccess.ListarUsuarios();
                return View(usuarios);
            }
            catch (Exception err)
            {
                var res = err.Message;
                TempData["Mensagemerro"] = "Ocorreu um erro na cria��o do usu�rio!";
                return View(res);
            }

        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var result = _dataAccess.Cadastrar(usuario);
                if (result)
                {
                    TempData["MensagemSucesso"] = "Usu�rio criado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensagemSucesso"] = "Erro ao criar usu�rio!";
                    return View(usuario);
                }
            }
            else
            {
                return View(usuario);
            }


        }

        public IActionResult Editar(int id)
        {
            try
            {
                var usuarios = _dataAccess.BuscarUsuarioPorId(id);
                return View(usuarios);
            }
            catch (Exception)
            {
                TempData["Mensagemerro"] = "Ocorreu um erro ao buscar usu�rio";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var result = _dataAccess.EditarUsuario(usuario);

                if (result)
                {
                    TempData["MensagemSucesso"] = "Usu�rio editado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensagemSucesso"] = "Erro ao editar usu�rio!";
                    return View(usuario);
                }

            }
            else
            {
                return View(usuario);
            };
        }



        public IActionResult Detalhes(int id)
        {
            try
            {
                var usuarios = _dataAccess.BuscarUsuarioPorId(id);
                return View(usuarios);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Remover(int id)
        {

            var removeIt = _dataAccess.RemoverUsuario(id);

            if (removeIt)
            {
                TempData["MensagemSucesso"] = "Usu�rio removido com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Algo deu errado na remo��o do usu�rio!";
                
            }
            return RedirectToAction("Index");
        }


    }
}
