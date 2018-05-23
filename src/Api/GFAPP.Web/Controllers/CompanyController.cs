using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GFAPP.EntityFramework;
using GFAPP.Model;
using GFAPP.Model.Company.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GFAPP.Web.Controllers
{
    /// <summary>
    /// 企业
    /// </summary>
    [Produces("application/json")]
    [Route("api/company")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CompanyController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 获取待办任务列表
        /// </summary>
        [HttpGet("pagedCompany")]
        public async Task<ActionResult> GetPagedCompany([FromQuery]GetPagedCompanyInput input)
        {
            int skipCount = (input.PageIndex - 1) * input.PageSize;

            var query = dbContext.Companys.AsQueryable();
            if(!string.IsNullOrWhiteSpace(input.Key))
            {
                query = query.Where(x => x.Name.Contains(input.Key) || x.LegalPerson.Contains(input.Key));
            }
            if (input.CompanyType.HasValue)
            {
                query = query.Where(x => x.CompanyType == input.CompanyType.Value);
            }
            int total = await query.CountAsync();
            if (string.IsNullOrWhiteSpace(input.SortField))
            {
                query = query.OrderBy(x => x.District);
            }

            query = query.Skip(skipCount).Take(input.PageSize);
            var items = Mapper.Map<List<CompanyDto>>(await query.ToListAsync());
            return new ObjectResult(new PageOutput<CompanyDto>(total, items));
        }
    }
}