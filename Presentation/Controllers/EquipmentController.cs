using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [Route("api/equipments")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EquipmentController(IServiceManager service) => _service = service;
      

        [HttpGet]
        public async Task <ActionResult> GetEquipments()
        { 
            var equipments = await _service.EquipmentService.GetAllEquipmentsAsync(false);
                return Ok(equipments);
           
        }

        [HttpGet("{id:guid}")]
        public async Task <  ActionResult> GetEquipment(Guid id)
        {
            var equipment = await _service.EquipmentService.GetEquipmentAsync(id,false);
            if (equipment is null)
                throw new EquipmentNotFoundException(id);
            return Ok(equipment);
        }

        //[HttpPost]
        //public async Task<ActionResult<EquipmentDto>> CreateEquipment([FromBody] CreateEquipmentDto createEquipmentDto)
        //{
        //    var equipment = await _equipmentService.CreateEquipment(createEquipmentDto);
        //    return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, equipment);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<EquipmentDto>> UpdateEquipment(int id, [FromBody] UpdateEquipmentDto updateEquipmentDto)
        //{
        //    var equipment = await _equipmentService.UpdateEquipment(id, updateEquipmentDto);
        //    return Ok(equipment);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteEquipment(int id)
        //{
        //    await _equipmentService.DeleteEquipment(id);
        //    return NoContent();
        //}
    }
}
