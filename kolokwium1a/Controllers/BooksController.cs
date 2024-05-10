namespace kolokwium1a.Controllers;

using System.Transactions;
using kolokwium1a.Models;
using kolokwium1a.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }
    
    [HttpGet("{id}/genres")]
    public async Task<IActionResult> GetGenre(int id)
    {
        if (!await _booksRepository.bookExists(id))
            return NotFound($"The book with given ID - {id} does not exist");

        var animal = await _booksRepository.getBook(id);
        
        return Ok(animal);
    }
    
    /*// Version with implicit transaction
    [HttpPost]
    public async Task<IActionResult> AddAnimal(NewAnimalWithProcedures newAnimalWithProcedures)
    {
        if (!await _animalsRepository.DoesOwnerExist(newAnimalWithProcedures.OwnerId))
            return NotFound($"Owner with given ID - {newAnimalWithProcedures.OwnerId} doesn't exist");

        foreach (var procedure in newAnimalWithProcedures.Procedures)
        {
            if (!await _animalsRepository.DoesProcedureExist(procedure.ProcedureId))
                return NotFound($"Procedure with given ID - {procedure.ProcedureId} doesn't exist");
        }

        await _animalsRepository.AddNewAnimalWithProcedures(newAnimalWithProcedures);

        return Created(Request.Path.Value ?? "api/animals", newAnimalWithProcedures);
    }
    
    // Version with transaction scope
    [HttpPost]
    [Route("with-scope")]
    public async Task<IActionResult> AddAnimalV2(NewAnimalWithProcedures newAnimalWithProcedures)
    {

        if (!await _animalsRepository.DoesOwnerExist(newAnimalWithProcedures.OwnerId))
            return NotFound($"Owner with given ID - {newAnimalWithProcedures.OwnerId} doesn't exist");

        foreach (var procedure in newAnimalWithProcedures.Procedures)
        {
            if (!await _animalsRepository.DoesProcedureExist(procedure.ProcedureId))
                return NotFound($"Procedure with given ID - {procedure.ProcedureId} doesn't exist");
        }

        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var id = await _animalsRepository.AddAnimal(new NewAnimalDTO()
            {
                Name = newAnimalWithProcedures.Name,
                Type = newAnimalWithProcedures.Type,
                AdmissionDate = newAnimalWithProcedures.AdmissionDate,
                OwnerId = newAnimalWithProcedures.OwnerId
            });

            foreach (var procedure in newAnimalWithProcedures.Procedures)
            {
                await _animalsRepository.AddProcedureAnimal(id, procedure);
            }

            scope.Complete();
        }

        return Created(Request.Path.Value ?? "api/animals", newAnimalWithProcedures);
    }*/
}