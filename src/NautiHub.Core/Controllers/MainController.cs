using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Abstractions;
using NautiHub.Core.Communication;
using System.Net;
using NautiHub.Core.Mediator;
using NautiHub.Core.Resources;

namespace NautiHub.Core.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    protected ICollection<string> Erros = new List<string>();
    
    protected IMediatorHandler _mediator;
    protected readonly MessagesService _messagesService;
    
    public MainController(IMediatorHandler mediator = null, MessagesService messagesService = null)
    {
        _mediator = mediator;
        _messagesService = messagesService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
    }

    protected ProblemDetails ResponseError(
        string title = "",
        string message = "",
        int statusCode = StatusCodes.Status400BadRequest
        )
    {
        return new ProblemDetails()
        {
            Status = statusCode,
            Title = string.IsNullOrEmpty(title) ? (_messagesService?.Error_Problem_Request ?? "Problema na Requisição") : title,
            Detail = string.IsNullOrEmpty(message) ? "" : message,
        };
    }

protected ActionResult Unauthorized(
        string mensagem = "Você não está autorizado a acessar este recurso",
        string titulo = "Não Autorizado"
    )
    {
        return new ObjectResult(
            ResponseError(titulo, mensagem, StatusCodes.Status401Unauthorized)
        );
    }

protected ActionResult NotAllowed(
        string mensagem = "Você não tem permissão para realizar esta operação",
        string titulo = "Operação Proibida"
    ) => new ObjectResult(ResponseError(titulo, mensagem, StatusCodes.Status403Forbidden));

protected ActionResult NotFound(
        string mensagem = null,
        string titulo = null
    )
    {
        var defaultMensagem = _messagesService?.Error_Not_Found_Message ?? "Não foi possível encontrar o registro";
        var defaultTitulo = _messagesService?.Error_Not_Found_Title ?? "Registro não encontrado";
        
        return NotFound(new ResponseResult
        {
            Title = titulo ?? defaultTitulo,
            Status = StatusCodes.Status404NotFound,
            Errors = new ResponseErrorMessages(_messagesService)
            {
                Mensages = [mensagem ?? defaultMensagem]
            }
        });
    }

protected ActionResult BadRequest(
        string mensagem = null,
        string titulo = null
    )
    {
        var defaultMensagem = _messagesService?.Error_Bad_Request_Message ?? "Não foi possível salvar a alteração.";
        var defaultTitulo = _messagesService?.Error_Bad_Request_Title ?? "Não foi possível salvar";
        
        return BadRequest(ResponseError(titulo ?? defaultTitulo, mensagem ?? defaultMensagem, StatusCodes.Status400BadRequest));
    }

protected ActionResult Conflict(
        string mensagem = null,
        string titulo = null
    )
    {
        var defaultMensagem = _messagesService?.Error_Conflict_Message ?? "Registro já existe na base";
        var defaultTitulo = _messagesService?.Error_Conflict_Title ?? "Registro já existe";
        
        return Conflict(ResponseError(titulo ?? defaultTitulo, mensagem ?? defaultMensagem, StatusCodes.Status409Conflict));
    }

    protected ActionResult CustomResponseCreated()
    {
        if (Created())
        {
            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        var modelErro = new ModelStateDictionary();
        foreach (var erro in Erros)
        {
            modelErro.AddModelError("Mensagens", erro);
        }

        return BadRequest(new ValidationProblemDetails(modelErro));
    }

    protected ActionResult CustomResponse(object? result = null)
    {
        if (Created())
            return Ok(result);

        var modelErro = new ModelStateDictionary();
        foreach (var erro in Erros)
        {
            modelErro.AddModelError("Mensagens", erro);
        }

        return BadRequest(new ValidationProblemDetails(modelErro));
    }

    protected ActionResult CustomResponse(ValidationResult result)
    {
        Dictionary<string, string[]> listaErrosValidacao = [];

        var errosValidacao = result.Errors.Select(c => c.ErrorMessage).ToArray();

        var modelErro = new ModelStateDictionary();

        foreach (var erro in errosValidacao)
        {
            modelErro.AddModelError("Mensagens", erro);
        }

        foreach (var erro in Erros)
        {
            modelErro.AddModelError("Mensagens", erro);
        }

        return BadRequest(new ValidationProblemDetails(modelErro));
    }

    protected ActionResult CustomResponse(ValidationResult result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Dictionary<string, string[]> listaErrosValidacao = [];

        var modelErro = new ModelStateDictionary();

        foreach (var erro in Erros)
        {
            modelErro.AddModelError("Mensagens", erro);
        }

        var problemDetails = new ValidationProblemDetails(modelErro)
        {
            Status = modelErro.Count == 0 ? (int)statusCode : StatusCodes.Status400BadRequest
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = modelErro.Count == 0 ? (int)statusCode : StatusCodes.Status400BadRequest
        };
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        IEnumerable<ModelError> erros = modelState.Values.SelectMany(e => e.Errors);

        foreach (ModelError erro in erros)
        {
            AddErrorMessage(erro.ErrorMessage);
        }

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ResponseResult resposta)
    {
        ResponseHasError(resposta);

        return CustomResponse();
    }

    protected bool ResponseHasError(ResponseResult resposta)
    {
        if (resposta == null || resposta.Errors.Mensages.Count == 0)
            return false;

        foreach (var mensagem in resposta.Errors.Mensages)
        {
            AddErrorMessage(mensagem);
        }

        return true;
    }

    protected bool Created() => Erros.Count == 0;

    protected void AddErrorMessage(string erro) => Erros.Add(erro);

    protected void CleanErrorsMessages() => Erros.Clear();
}
