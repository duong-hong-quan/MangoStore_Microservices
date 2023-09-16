using AutoMapper;
using Azure;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly ResponeDto _respone;
        private IMapper _mapper;
        public CouponAPIController(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _respone = new ResponeDto();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponeDto Get()
        {
            try
            {
                IEnumerable<Coupon> obj = _db.Coupons.ToList();
                _respone.Result = _mapper.Map<IEnumerable<CouponDto>>(obj);
            }
            catch (Exception ex)
            {
                _respone.IsSuccess = false;
                _respone.Message = ex.Message;

            }
            return _respone;
        }


        [HttpGet]
        [Route("{id:int}")]
        public ResponeDto Get(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CounponId == id);

                _respone.Result = _mapper.Map<CouponDto>(obj); ;
            }
            catch (Exception ex)
            {

                _respone.IsSuccess = false;
                _respone.Message = ex.Message;
            }
            return _respone;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponeDto GetByCode(string code)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());

                _respone.Result = _mapper.Map<CouponDto>(obj); ;
            }
            catch (Exception ex)
            {

                _respone.IsSuccess = false;
                _respone.Message = ex.Message;
            }
            return _respone;
        }

        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponeDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(obj);
                _db.SaveChanges();
              

                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long)(couponDto.DiscountAmount * 100),
                    Name=couponDto.CouponCode,
                    Currency="usd",
                    Id = couponDto.CouponCode
                  
                };
                var service = new Stripe.CouponService();
                service.Create(options);
                _respone.Result = _mapper.Map<CouponDto>(obj); ;
            }
            catch (Exception ex)
            {

                _respone.IsSuccess = false;
                _respone.Message = ex.Message;
            }
            return _respone;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]

        public ResponeDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(obj);
                _db.SaveChanges();
                _respone.Result = _mapper.Map<CouponDto>(obj); ;
            }
            catch (Exception ex)
            {

                _respone.IsSuccess = false;
                _respone.Message = ex.Message;
            }
            return _respone;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]

        public ResponeDto Delete(int id)
        {
            try
            {
                Coupon obj = _db.Coupons.First(u=> u.CounponId == id);
                _db.Coupons.Remove(obj);
                _db.SaveChanges();
            
                var service = new Stripe.CouponService();
                service.Delete(obj.CouponCode);
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


