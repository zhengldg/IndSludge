using AutoMapper;
using GFAPP.EntityFramework;
using GFAPP.Framework;
using GFAPP.Model.Authentication.Users;
using GFAPP.Model.EleDuplicate;
using GFAPP.Model.EleDuplicate.Dtos;
using GFAPP.Model.IndSludge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using GFAPP.Model.Signature;
using AutoMapper.QueryableExtensions;

namespace GFAPP.Web.Controllers
{
    /// <summary>
    /// 电子联单
    /// </summary>
    [Route("api/eleDuplicate")]
    [Authorize]
    public class EleDuplicateController : BaseController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<UserInfo> userManager;
        public IAppSession Session { get; }

        public EleDuplicateController(ApplicationDbContext context, UserManager<UserInfo> userManager, IAppSession Session)
        {
            this.context = context;
            this.userManager = userManager;
            this.Session = Session;
        }

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name =  "GetById")]
        public async Task<ActionResult> GetById(int id)
        {
            var item = await context.EleDuplicates.FindAsync(id);
            if(item  == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        /// <summary>
        /// 删除联单
        /// </summary>
        [HttpGet("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await context.EleDuplicates.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            if(item.State== Model.Enums.EleDuplicateState.Finished)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "已完成的联单无法删除"  );
            }
            context.EleDuplicates.Remove(item);
            await context.SaveChangesAsync();

            return new NoContentResult();
        }

        /// <summary>
        /// 联单创建
        /// </summary>
        [HttpPost("newELe")]
        public async Task<ActionResult> newELe([FromBody]EleDuplicateCreateDto input)
        {
            var entity = Mapper.Map<EleDuplicateInfo>(input);
            entity.Code = $"L{DateTime.Now.ToString("yyMMddhhmmss")}{new Random().Next(100, 999)}";
            var user = await userManager.FindByNameAsync(Session.Username);
            var company = await context.Companys.FindAsync(user.CompanyInfoId);
            entity.GeneratedCompany = company;
            entity.TimeOfGeneratedSubmit = DateTime.Now;
            entity.State = Model.Enums.EleDuplicateState.Carring;
            context.EleDuplicates.Add(entity);

            var sign = new SignatureInfo()
            {
                ImgBase64 = input.CeneratedOperatorSignBase64
            };
            entity.CeneratedOperatorSign = sign;

            // 保存提交记录
            await context.Records.AddAsync(new Model.Record.RecordInfo()
            {
                Event  = EleDuplicateRecordTypes.GeneratedCompanySubmited.ToString(),
                OccurredTime = DateTime.Now,
                RelationshipId = entity.Id,
                Operator = Session.Username
            });

            await context.SaveChangesAsync();

            return Ok(entity.Id);
        }

        [HttpGet("new/{id}")]
        public async Task<ActionResult> GetCreate(int id)
        {
            var entity = await context.EleDuplicates.FindAsync(id);
            if(entity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Mapper.Map<EleDuplicateCreateDto>(entity));
            }
        }

        /// <summary>
        /// 经营单位退回
        /// </summary>
        [HttpPost("back")]
        public async Task<ActionResult> ProcessedBack(int id)
        {
            var entity = await context.EleDuplicates.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.ProcessedCompanyId != Session.CompanyId)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "无效操作");
            }

            entity.State = Model.Enums.EleDuplicateState.Backed;
            context.EleDuplicates.Update(entity);

            // 保存提交记录
            await context.Records.AddAsync(new Model.Record.RecordInfo()
            {
                Event = EleDuplicateRecordTypes.ProcessedCompanyBacked.ToString(),
                OccurredTime = DateTime.Now,
                RelationshipId = entity.Id,
                Operator = Session.Username
            });

            await context.SaveChangesAsync();

            return CreatedAtRoute("GetById", new { id = entity.Id }, entity);
        }

        /// <summary>
        /// 联单接收
        /// </summary>
        [HttpPost("accept")]
        public async Task<ActionResult> ProcessedSubmit(EleDuplicateAcceptInput input)
        {
            var entity = await context.EleDuplicates.FindAsync(input.Id);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.ProcessedCompanyId != Session.CompanyId)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "无效操作");
            }
            Mapper.Map(input, entity, typeof(EleDuplicateAcceptInput), typeof(EleDuplicateInfo));
            entity.State = Model.Enums.EleDuplicateState.Finished;
            entity.TimeOfProcessedSubmit = DateTime.Now;
            // 保存提交记录
            await context.Records.AddAsync(new Model.Record.RecordInfo()
            {
                Event = EleDuplicateRecordTypes.ProcessedCompanySubmited.ToString(),
                OccurredTime = DateTime.Now,
                RelationshipId = entity.Id,
                Operator = Session.Username
            });
            context.EleDuplicates.Update(entity);
            await context.SaveChangesAsync();

            return CreatedAtRoute("GetById", new { id = entity.Id }, entity);
        }

        /// <summary>
        /// 获取首页待办任务
        /// </summary>
        [HttpPost("homeMissions")]
        public async Task<ActionResult> GetHomeMissions()
        {
            //var companyId = Session.CompanyId;
            var missions = await context.EleDuplicates
                //.Where(x => x.GeneratedCompanyId == companyId && (x.State == Model.Enums.EleDuplicateState.Created || x.State == Model.Enums.EleDuplicateState.Backed)
                //|| x.ProcessedCompanyId == companyId && (x.State == Model.Enums.EleDuplicateState.Carring)
                //)
                .Include(x => x.GeneratedCompany).Include(x => x.ProcessedCompany)
                .OrderByDescending(x => x.Code).Take(5).AsNoTracking().ToListAsync();
            var items = Mapper.Map<List<EleDuplicateMissionDto>>(missions);
            return new ObjectResult(items);
        }

        /// <summary>
        /// 获取待办任务列表
        /// </summary>
        [HttpGet("pagedMission")]
        public async Task<ActionResult> GetPagedMission([FromQuery]GetPagedMissionInput input)
        {
            int skipCount = (input.PageIndex - 1) * input.PageSize;

            var companyId = Session.CompanyId;
            var query = context.EleDuplicates
                .Where(x => x.GeneratedCompanyId == companyId && (x.State == Model.Enums.EleDuplicateState.Created || x.State == Model.Enums.EleDuplicateState.Backed)
                || x.ProcessedCompanyId == companyId && (x.State == Model.Enums.EleDuplicateState.Carring)
                ).Include(x => x.GeneratedCompany).Include(x => x.ProcessedCompany).AsQueryable();
            if(string.IsNullOrWhiteSpace(input.SortField))
            {
                query = query.OrderByDescending(x => x.Code);
            }

            query = query.Skip(skipCount).Take(10);
            var items = Mapper.Map<List<EleDuplicateMissionDto>>( await query.ToListAsync());
            return new ObjectResult(items);
        }

        /// <summary>
        /// 获取已办任务列表
        /// </summary>
        [HttpGet("pagedFinished")]
        public async Task<ActionResult> GetPagedFinished([FromQuery]GetPagedMissionInput input)
        {
            int skipCount = (input.PageIndex - 1) * input.PageSize;

            var companyId = Session.CompanyId;
            var query = context.EleDuplicates
                .Where(x => x.GeneratedCompanyId == companyId && (x.State > Model.Enums.EleDuplicateState.Created )
                || x.ProcessedCompanyId == companyId && (x.State == Model.Enums.EleDuplicateState.Finished || x.State == Model.Enums.EleDuplicateState.Backed)
                ).Include(x => x.GeneratedCompany).Include(x => x.ProcessedCompany).AsQueryable();
            if (string.IsNullOrWhiteSpace(input.SortField))
            {
                query = query.OrderByDescending(x => x.Code);
            }

            query = query.Skip(skipCount).Take(10);
            var items = Mapper.Map<List<EleDuplicateMissionDto>>(await query.ToListAsync());
            return new ObjectResult(items);
        }
    }
}