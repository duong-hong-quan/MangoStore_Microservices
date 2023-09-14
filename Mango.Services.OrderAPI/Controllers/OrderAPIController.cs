using AutoMapper;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;
using Mango.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private ResponeDto _respone;
        private IMapper _mapper;
        private readonly AppDBContext _db;
        private IProductService _productService;

        public OrderAPIController(IMapper mapper, AppDBContext db, IProductService productService)
        {
            _respone = new ResponeDto();
            _mapper = mapper;
            _db = db;
            _productService = productService;
        }
        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponeDto> CreateOrder(CartDto cartDto)
        {
            try
            {

                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreate = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreate.OrderHeaderId;
                _respone.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;
            }

            return _respone;
        }
    }
}
