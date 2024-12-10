using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Practice.Service.EmployeeAPI.Data;
using Practice.Service.EmployeeAPI.Model;
using Practice.Service.EmployeeAPI.Model.Dto;
using Practice.Service.EmployeeAPI.Utility;

namespace Practice.Service.EmployeeAPI.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        public readonly EmployeeDbContext _db;
        public readonly IMapper _mapper;
        public readonly ResponseDto _response;
        public EmployeeAPIController(EmployeeDbContext db,
                                     IMapper mapper,
                                     ResponseDto response)
        {
            _db = db;
            _mapper = mapper;
            _response = response;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        IEnumerable<Employee> empList = _db.employees.ToList();
        //        _response.Message = "Get";
        //        _response.isSuccess = true;
        //        _response.Result = _mapper.Map<IEnumerable<EmployeeDto>>(empList);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.isSuccess = false;
        //        _response.Message = ex.Message;
        //    }
        //    return Ok(_response);
        //}

        //Done By Store Procedure
        [HttpGet]
        [ActionName("getEmployee")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var empList = await _db.employees.FromSqlRaw("spGetEmployeeList").ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<EmployeeDto>>(empList);
                _response.Message = "Get All";
                _response.isSuccess = true;
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }



        //[HttpGet]
        //[Route("getEmployee/{id:int}")]
        //public IActionResult Get(int id)
        //{
        //    try
        //    {
        //        Employee? employee = _db.employees.FirstOrDefault(u => u.Id == id);
        //        if (employee != null)
        //        {
        //            _response.Result = _mapper.Map<EmployeeDto>(employee);
        //            _response.Message = "Get Data";
        //            _response.isSuccess = true;

        //        }
        //        else
        //        {
        //            _response.Message = "Employee not Exist";
        //            _response.isSuccess = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _response.isSuccess = false;
        //        _response.Message = ex.Message;
        //    }
        //    return Ok(_response);
        //}


        //Done By Store Procedure
        [HttpGet]
        [Route("getEmployee/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var empId = new SqlParameter("@EmpId", id);
                var action = new SqlParameter("@Action", SD.httpType.GET);
                var empDetails = await _db.employees.FromSqlRaw(@"exec spGetEmployeeById @EmpId, @Action", empId, action).ToListAsync();
                if (empDetails != null)
                {
                    _response.Result = empDetails;
                    _response.Message = "Get Data";
                    _response.isSuccess = true;

                }
                else
                {
                    _response.Message = "Employee not Exist";
                    _response.isSuccess = false;
                }

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }


        //[HttpDelete]
        //[Route("{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        Employee? employee = _db.employees.FirstOrDefault(u => u.Id == id);
        //        if (employee != null)
        //        {
        //            var delEmp = _db.Remove(employee);
        //            _db.SaveChanges();
        //            _response.Result = delEmp;
        //            _response.Message = "Deleted";
        //            _response.isSuccess = true;
        //        }
        //        else
        //        {
        //            _response.Message = "employee not exist";
        //            _response.isSuccess = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _response.isSuccess = false;
        //        _response.Message = ex.Message;
        //    }
        //    return Ok(_response);
        //}

        //Done By Store Procedure
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var empId = new SqlParameter("@EmpId", id);
                var action = new SqlParameter("@Action", SD.httpType.GET);
                int deletedEmp = _db.Database.ExecuteSqlRaw(@"spDeleteEmployee @EMPID", empId);
                _response.Result = deletedEmp;
                _response.Message = "Deleted";
                _response.isSuccess = true;
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }

        //[HttpPost]
        //[Route("addEmp")]
        //public IActionResult Post([FromBody] EmployeeDto empDto)
        //{
        //    try
        //    {
        //        Employee empObj = _mapper.Map<Employee>(empDto);
        //        _db.employees.Add(empObj);
        //        _db.SaveChanges();
        //        _response.Result = _mapper.Map<EmployeeDto>(empObj);
        //        _response.Message = "Save"; 
        //        _response.isSuccess = true;


        //    }
        //    catch (Exception ex)
        //    {
        //        _response.isSuccess = false;
        //        _response.Message = ex.Message;
        //    }
        //    return Ok(_response);
        //}


        //Done By Store Procedure
        [HttpPost]
        [Route("addEmp")]
        public async Task<IActionResult> Post([FromBody] EmployeeDto empDto)
        {
            try
            {
                Employee empObj = _mapper.Map<Employee>(empDto);
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@FIRSTNAME", empObj.firstName));
                param.Add(new SqlParameter("@LASTNAME", empObj.lastName));
                param.Add(new SqlParameter("@EMAIL", empObj.email));
                param.Add(new SqlParameter("@PHONE", empObj.phone));
                param.Add(new SqlParameter("@CITY", empObj.city));
                param.Add(new SqlParameter("@STATE", empObj.state));
                param.Add(new SqlParameter("@ZIPCODE", empObj.zipcode));
                param.Add(new SqlParameter("@COUNTRY", empObj.country));
                var emp = await _db.Database.ExecuteSqlRawAsync(@"exec spInsertEmployee @FIRSTNAME,@LASTNAME,@EMAIL,@PHONE,@CITY,@STATE,@ZIPCODE,@COUNTRY", param.ToArray());
                //_db.employees.Add(empObj);
                //_db.SaveChanges();
                _response.Result = emp;
                _response.Message = "Save";
                _response.isSuccess = true;
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }



        [HttpPut]
        [Route("updateEmp")]
        public IActionResult Put([FromBody] EmployeeDto empDto)
        {
            try
            {
                Employee empObj = _mapper.Map<Employee>(empDto);
                _db.employees.Update(empObj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<EmployeeDto>(empObj);
                _response.Message = "Update";
                _response.isSuccess = true;
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }
    }
}
