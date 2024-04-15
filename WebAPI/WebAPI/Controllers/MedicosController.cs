using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Domains;
using WebAPI.Interfaces;
using WebAPI.Repositories;
using WebAPI.Utils.BlobStorage;
using WebAPI.Utils.Mail;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private IMedicoRepository _medicoRepository;

        private readonly EmailSendingService _emailSendingService;

        public MedicosController(EmailSendingService emailSendingService)
        {
            _medicoRepository = new MedicoRepository();
            _emailSendingService = emailSendingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_medicoRepository.ListarTodos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorId")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                return Ok(_medicoRepository.BuscarPorId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpPost]
        //public IActionResult Post(MedicoViewModel medicoModel)
        //{
        //    Usuario user = new Usuario();
        //    user.Nome = medicoModel.Nome;
        //    user.Email = medicoModel.Email;
        //    user.TipoUsuarioId = medicoModel.IdTipoUsuario;
        //    user.Foto = medicoModel.Foto;
        //    user.Senha = medicoModel.Senha;

        //    user.Medico = new Medico();
        //    user.Medico.Crm = medicoModel.Crm;
        //    user.Medico.EspecialidadeId = medicoModel.EspecialidadeId;


        //    user.Medico.Endereco = new Endereco();
        //    user.Medico.Endereco.Logradouro = medicoModel.Logradouro;
        //    user.Medico.Endereco.Numero = medicoModel.Numero;
        //    user.Medico.Endereco.Cep = medicoModel.Cep;

        //    _medicoRepository.Cadastrar(user);

        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MedicoViewModel medicoModel)
        {
            try
            {
                // Objeto a ser cadastrado
                Usuario user = new Usuario();

                // Recebe os valores e preenche as propriedades do objeto
                user.Nome = medicoModel.Nome;
                user.Email = medicoModel.Email;
                user.TipoUsuarioId = medicoModel.IdTipoUsuario;
                user.Foto = medicoModel.Foto;
                user.Senha = medicoModel.Senha;

                user.Medico = new Medico();
                user.Medico.Crm = medicoModel.Crm;
                user.Medico.EspecialidadeId = medicoModel.EspecialidadeId;

                // Define o nome do container do blob
                var containerName = "containervitalg10";

                // Define a string de conexão
                var conectionString = "DefaultEndpointsProtocol=https;AccountName=vitalmikaelg10;AccountKey=LZcV9x7wPD6ZFOJfJFXkO3EZ3Gp+UnMFwzk5X2hJgoo3kg4JhCUZ53jSMjd2M0ZHV+z4/PLuTZAd+ASt19O3Iw==;EndpointSuffix=core.windows.net";

                // Aqui vamos chamar o metodo para upload da imagem 
                user.Foto = await AzureBlobStorageHelper.UploadImageBlobAsync(medicoModel.arquivo!, conectionString, containerName);

                user.Medico.Endereco = new Endereco();
                user.Medico.Endereco.Logradouro = medicoModel.Logradouro;
                user.Medico.Endereco.Numero = medicoModel.Numero;
                user.Medico.Endereco.Cep = medicoModel.Cep;
                _medicoRepository.Cadastrar(user);

                await _emailSendingService.SendWelcomeEmail(user.Email!, user.Nome!);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }




        [HttpGet("BuscarPorIdClinica")]
        public IActionResult GetByIdClinica(Guid id)
        {
            try
            {
                return Ok(_medicoRepository.ListarPorClinica(id)); ;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorData")]
        public IActionResult GetByDate(DateTime data, Guid id)
        {
            try
            {
                return Ok(_medicoRepository.BuscarPorData(data, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateProfile(MedicoViewModel medico)
        {
            try
            {
                Guid idUsuario = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_medicoRepository.AtualizarPerfil(idUsuario, medico));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}