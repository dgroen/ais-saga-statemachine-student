using AutoMapper;
using AutoMapper.Internal;
using Events.StudentEvents;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentService.DTO;
using StudentService.Models;
using StudentService.Services;

namespace StudentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public StudentController(IStudentServices studentServices, IMapper mapper, IBus bus)
        {
            _studentServices = studentServices;
            _mapper = mapper;
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddStudentDTO addStudentDTO)
        {
            var mapModel = _mapper.Map<Student>(addStudentDTO);

            var res = await _studentServices.AddStudent(mapModel);

            if (res is not null)
            {
                // map model to the DTO and pass the DTO object to the bus queue
                var mapResult = _mapper.Map<ResponseStudentDTO>(res);
                // Send to the Bus
                var endPoint = await _bus.GetSendEndpoint(new Uri("queue:" + MessageBrokers.RabbitMQQueues.SagaBusQueue));
                await endPoint.Send<IGETValueEvent>(new
                {
                    StudentId = Guid.Parse(mapResult.StudentId),
                    Title = mapResult.Title,
                    Email = mapResult.Email,
                    RequireDate = mapResult.RequireDate,
                    Age = mapResult.Age,
                    Location = mapResult.Location
                });
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest();
        }
    }
}
