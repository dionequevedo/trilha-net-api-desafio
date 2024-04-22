using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Entities;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Alterações implementadas
            var tarefas = _context.Tarefas.Find(id);
            if (tarefas == null)
            {
                return NotFound($"Não há tarefas com o Id: \'{id}\'!");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Alteração implementada
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains("")).ToList();
            if (tarefas.Count == 0)
            {
                return NotFound("Não há tarefas cadastradas!");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Alterações implementadas
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();
            if (tarefas.Count == 0)
            {
                return NotFound($"Não há tarefas com o título: \'{titulo}\'!");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
            if (tarefas.Count == 0)
            {
                return NotFound($"Não há tarefas para a data de \'{data.ToString("yyyy-MM-dd")}\'!");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Alteração implementada
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            if (tarefa == null)
            {
                return NotFound();
            }
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
            if (tarefa.Titulo == "")
            {
                return BadRequest(new { Erro = "O título não pode ser vazio" });
            }
            if (tarefa.Descricao == "")
            {
                return BadRequest(new { Erro = "A descrição deve ser preenchido" });
            }               

            // Alteração implementada
            _context.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound($"{id} não encontrado!");
            }

            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
            if (tarefa.Titulo == "")
            {
                return BadRequest(new { Erro = "O título deve ser preenchido" });
            }
            if (tarefa.Descricao == "")
            {
                return BadRequest(new { Erro = "A descrição deve ser preenchido" });
            }

            // Alterações implementadas
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Update(tarefaBanco);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound($"{id} não encontrado!");

            // Alteração implementada
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return Ok($"Registro {id} removido com sucesso!");
        }
    }
}
