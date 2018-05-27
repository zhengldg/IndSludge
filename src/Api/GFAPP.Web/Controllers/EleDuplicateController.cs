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
using GFAPP.Model;
using GFAPP.Application.CodeGenerator;
using GFAPP.Model.CodeGenerator;

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
        private readonly ICodeGeneratorService codeGeneratorService;

        public IAppSession Session { get; }

        public EleDuplicateController(ApplicationDbContext context, UserManager<UserInfo> userManager, IAppSession Session, ICodeGeneratorService  codeGeneratorService)
        {
            this.context = context;
            this.userManager = userManager;
            this.Session = Session;
            this.codeGeneratorService = codeGeneratorService;
        }

        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult> GetById(int id)
        {
            var item = await context.EleDuplicates.FindAsync(id);
            if (item == null)
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
            if (item.State == Model.Enums.EleDuplicateState.Finished)
            {
                return Ok(new ApiResult("已完成的联单无法删除"));
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

            // 保存提交记录
            if (!input.Id.HasValue) // 发起联单
            {
                var user = await userManager.FindByNameAsync(Session.Username);
                var company = await context.Companys.Include(x=>x.City).FirstAsync(x=>x.Id == user.CompanyInfoId);
                entity.Code = await codeGeneratorService.Generate(CodeGeneratorType.EleDuplicate, company.City.Code);
                entity.GeneratedCompany = company;
                context.EleDuplicates.Add(entity);
                entity.TimeOfGeneratedSubmit = DateTime.Now;
                entity.State = Model.Enums.EleDuplicateState.Carring;
                entity.CeneratedOperatorSign = new SignatureInfo()
                {
                    ImgBase64 = input.CeneratedOperatorSignBase64
                };
                await context.SaveChangesAsync();
            }
            else
            {
                var exist = await context.EleDuplicates.FindAsync(input.Id);
                if (exist == null)
                {
                    return NotFound();
                }
                context.Entry(exist).CurrentValues.SetValues(input);
                exist.TimeOfGeneratedSubmit = DateTime.Now;
                exist.State = Model.Enums.EleDuplicateState.Carring;
                if (exist.CeneratedOperatorSignId.HasValue)
                {
                    var sign = await context.Signatures.FindAsync(exist.CeneratedOperatorSignId);
                    context.Signatures.Remove(sign);
                }
                exist.CeneratedOperatorSign = new SignatureInfo()
                {
                    ImgBase64 = input.CeneratedOperatorSignBase64
                };
            }

            await context.Records.AddAsync(new Model.Record.RecordInfo()
            {
                Event = EleDuplicateRecordTypes.GeneratedCompanySubmited.ToString(),
                OccurredTime = DateTime.Now,
                RelationshipId = entity.Id,
                Operator = Session.Username
            });
            await context.SaveChangesAsync();
            return Ok(entity.Id);
        }

        /// <summary>
        /// 返回创建联单信息
        /// </summary>
        [HttpGet("new/{id}")]
        public async Task<ActionResult> GetCreate(int id)
        {
            var entity = await context.EleDuplicates
                .Include(x => x.CeneratedOperatorSign)
                .Include(x => x.GeneratedCompany)
                .Include(x => x.CarryingCompany)
                .Include(x => x.ProcessedCompany)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
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
        public async Task<ActionResult> ProcessedBack([FromBody]BackInput input)
        {
            var entity = await context.EleDuplicates.FindAsync(input.Id);

            if (entity == null)
            {
                return NotFound();
            }

            //if (entity.ProcessedCompanyId != Session.CompanyId)
            //{
            //    return Ok(new ApiResult("无效操作"));
            //}

            entity.State = Model.Enums.EleDuplicateState.Backed;
            entity.BackReason = input.Reason;
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

            return Ok(input.Id);
        }

        /// <summary>
        /// 返回接收联单信息
        /// </summary>
        [HttpGet("process/{id}")]
        public async Task<ActionResult> GetProcess(int id)
        {
            var entity = await context.EleDuplicates.Include(x => x.GeneratedCompany).FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Mapper.Map<EleDuplicateAcceptDto>(entity));
            }
        }

        /// <summary>
        /// 返回接收联单信息
        /// </summary>
        [HttpGet("detail/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var entity = await context.EleDuplicates
                .Include(x => x.GeneratedCompany)
                .Include(x => x.CarryingCompany)
                .Include(x => x.ProcessedCompany)
                .Include(x => x.CeneratedOperatorSign)
                .Include(x => x.ProcessedOperatorSign)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Mapper.Map<EleDuplicateDetail>(entity));
            }
        }

        /// <summary>
        /// 联单接收
        /// </summary>
        [HttpPost("accept")]
        public async Task<ActionResult> ProcessedSubmit([FromBody]EleDuplicateAcceptDto input)
        {
            var entity = await context.EleDuplicates.FindAsync(input.Id);
            if (entity == null)
            {
                return NotFound();
            }
            //if (entity.ProcessedCompanyId != Session.CompanyId)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, "无效操作");
            //}
            Mapper.Map(input, entity, typeof(EleDuplicateAcceptDto), typeof(EleDuplicateInfo));
            entity.State = Model.Enums.EleDuplicateState.Finished;
            entity.TimeOfProcessedSubmit = DateTime.Now;
            entity.ProcessedOperatorSign = new SignatureInfo() { ImgBase64 = input.ProcessedOperatorSignBase64 };
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

            return Ok(input.Id);
        }

        /// <summary>
        /// 获取首页待办任务
        /// </summary>
        [HttpPost("homeMissions")]
        public async Task<ActionResult> GetHomeMissions()
        {
            var companyId = Session.CompanyId;
            var missions = await context.EleDuplicates
                .Where(x => x.State != Model.Enums.EleDuplicateState.Finished)
                .Where(x => x.GeneratedCompanyId == companyId
                || x.CarryingCompanyId == companyId
                || x.ProcessedCompanyId == companyId
                )
                .Include(x => x.GeneratedCompany).Include(x => x.ProcessedCompany)
                .OrderByDescending(x => x.Code).Take(3).AsNoTracking().ToListAsync();
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
                .Where(x => x.State != Model.Enums.EleDuplicateState.Finished)
                .Where(x => x.GeneratedCompanyId == companyId
                || x.CarryingCompanyId == companyId
                || x.ProcessedCompanyId == companyId
                );
            if(!string.IsNullOrWhiteSpace(input.Key))
            {
                query = query.Where(x => x.Code.Contains(input.Key)
                || x.GeneratedCompany.Name.Contains(input.Key)
                || x.CarryingCompany.Name.Contains(input.Key)
                || x.ProcessedCompany.Name.Contains(input.Key)
                );
            }
            int total = query.Count();

            if (string.IsNullOrWhiteSpace(input.SortField))
            {
                query = query.OrderByDescending(x => x.Code);
            }

            query = query.Skip(skipCount).Take(10);
            var items = Mapper.Map<List<EleDuplicateMissionDto>>( await query.ToListAsync());
            var apiResult = new ApiResult(items, total);
            return Ok(apiResult);
        }

        /// <summary>
        /// 获取已办任务列表
        /// </summary>
        [HttpGet("pagedFinished")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPagedFinished([FromQuery]GetPagedMissionInput input)
        {
            int skipCount = (input.PageIndex - 1) * input.PageSize;

            var companyId = Session.CompanyId;
            var query = context.EleDuplicates
                .Where(x => x.State == Model.Enums.EleDuplicateState.Finished)
                .Where(x => x.GeneratedCompanyId == companyId
                || x.CarryingCompanyId == companyId
                || x.ProcessedCompanyId == companyId
                );
            if (!string.IsNullOrWhiteSpace(input.Key))
            {
                query = query.Where(x => x.Code.Contains(input.Key)
                || x.GeneratedCompany.Name.Contains(input.Key)
                || x.CarryingCompany.Name.Contains(input.Key)
                || x.ProcessedCompany.Name.Contains(input.Key)
                );
            }
            int total = query.Count();
            if (string.IsNullOrWhiteSpace(input.SortField))
            {
                query = query.OrderByDescending(x => x.Code);
            }

            query = query.Skip(skipCount).Take(10);
            var items = Mapper.Map<List<EleDuplicateMissionDto>>(await query.ToListAsync());
            var apiResult = new ApiResult(items, total);
            return Ok(apiResult);
        }
    }
}