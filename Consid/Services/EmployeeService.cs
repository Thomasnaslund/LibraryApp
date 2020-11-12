using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consid.Models;
using Consid.Services;
using Consid.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Consid.Services
{
    public class EmployeeService : IEmployeeService
    {

        private static readonly decimal _ceoSalary = 2.725m;
        private static readonly decimal _managerSalary = 1.725m;
        private static readonly decimal _defaultSalary = 1.125m;

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> TryAddAsync(EmployeeViewModel dto)
        {


            if (dto.IsCEO && await _context.Employees.AnyAsync(e => e.IsCEO == true))
            {
                return "CEO already exists";
            }

            if (!dto.IsManager && !dto.IsCEO && dto.ManagerId == null)
            {
                return "Manager is required for default employee";
            }

            dto.Salary = CalcSalary(dto);

            var employee = _mapper.Map<Employee>(dto);

            //Manual mapping, could maybe be done in automapper
            employee.Manager = await _context.Employees.FindAsync(dto.ManagerId);
            employee.ManagerId = dto.ManagerId;

            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();
            return null;

        }

        private decimal CalcSalary(EmployeeViewModel dto)
        {
            if (dto.IsCEO)
            {
                return _ceoSalary * dto.Rank;
            }
            else if (dto.IsManager)
            {
                return _managerSalary * dto.Rank;
            }
            else
            {
                return dto.Salary = _defaultSalary * dto.Rank;
            }
        }

        public async Task<bool> TryDeleteAsync(int id)
        {
            if (await IsManagingAsync(id))
            {
                return false;
            }

            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync()
        {

            List<EmployeeViewModel> employeesDto = new List<EmployeeViewModel>();
            employeesDto.AddRange(await GetCEOAsync());
            employeesDto.AddRange(await GetManagersAsync());
            employeesDto.AddRange(await GetRegularEmployees());

            return employeesDto;

        }


        public async Task<EmployeeViewModel> GetEmployeeAsync(int id)
        {
            var employeeDto = _mapper.Map<EmployeeViewModel>(await _context.Employees.FindAsync(id));
            employeeDto.IsManaging = await IsManagingAsync(id);
            employeeDto.Managers = await GetAvailableManagersAsync(employeeDto);

            return employeeDto;
            
        }


        public async Task<IEnumerable<EmployeeViewModel>> GetManagersAsync()
        {

            var query = _context.Employees.Include(e => e.Manager).Where(e => e.IsCEO == false && e.IsManager == true);

            List<Employee> employees = await query.AsNoTracking().ToListAsync();
            List<EmployeeViewModel> employeesDto = _mapper.Map<List<Employee>, List<EmployeeViewModel>>(employees);
            employeesDto.ForEach(e => e.Type = "Manager");

            return employeesDto;

        }


        public async Task<IEnumerable<EmployeeViewModel>> GetCEOAsync()
        {

            var query = _context.Employees.Include(e => e.Manager).Where(e => e.IsCEO == true);
            List<Employee> employees = await query.AsNoTracking().ToListAsync();
            List<EmployeeViewModel> employeesDto = _mapper.Map<List<Employee>, List<EmployeeViewModel>>(employees);
            employeesDto.ForEach(e => e.Type = "CEO");

            return employeesDto;

        }

        public async Task<IEnumerable<EmployeeViewModel>> GetRegularEmployees()
        {

            var query = _context.Employees.Include(e => e.Manager).Where(e => e.IsCEO == false && e.IsManager == false);
            List<Employee> employees = await query.AsNoTracking().ToListAsync();
            List<EmployeeViewModel> employeesDto = _mapper.Map<List<Employee>, List<EmployeeViewModel>>(employees);
            employeesDto.ForEach(e => e.Type = "Regular Employee");

            return employeesDto;

        }

        public async Task<bool> IsManagingAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.ManagerId == id);
        }

        public async Task<SelectList> GetAvailableManagersAsync(EmployeeViewModel dto)
        {
            if (dto.IsCEO)
            {
                return new SelectList(new List<SelectListItem>());
            }

            List<SelectListItem> managers = await _context.Employees.AsNoTracking()
                .Where(y => dto.IsManager ? y.IsCEO == true || y.IsManager == true : y.IsCEO == false && y.IsManager == true)
                .OrderBy(n => n.Manager.FirstName)
                   .Select(n =>
                   new SelectListItem
                   {
                       Value = n.Id.ToString(),
                       Text = n.FirstName + " " + n.LastName
                   }).ToListAsync();

            return new SelectList(managers, "Value", "Text", 1);
        }


        public async Task<string> TryUpdateAsync(EmployeeViewModel dto)
        {
            //Check if CEO already assigned
            var ceo = await _context.Employees.AsNoTracking().Where(e => e.IsCEO).FirstOrDefaultAsync();
            if (ceo != null && dto.IsCEO && ceo.Id != dto.Id)
            { 
                return "CEO already exists";
            }

            //For default employee check that manager is not null
            if (!dto.IsManager && !dto.IsCEO && dto.ManagerId == null) {

                return "Manager is required for default employee";
            }

            dto.Salary = CalcSalary(dto);
            var employee = _mapper.Map<Employee>(dto);

            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Id))
                {
                    { return "ConcurrencyError"; }
                }
                else
                {
                    { throw; }
                }
            }
            return null;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }


    }
}
