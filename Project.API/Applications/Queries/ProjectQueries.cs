using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Project.Infrastructure;

namespace Project.API.Applications.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly ProjectContext _context;
        private string _connStr;

        public ProjectQueries(string connStr, ProjectContext context)
        {
            _connStr = connStr;
            _context = context;
        }

        public async Task<dynamic> GetProjectDetail(int projectId)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                conn.Open();

                string sql = @"SELECT
                    Projects.Id
                    ,Projects.Company
                    ,Projects.Avatar
                    ,Projects.ProvinceId
                    ,Projects.FinStage
                    ,Projects.FinMoney
                    ,Projects.Valuation
                    ,Projects.FinPercentage
                    ,Projects.Introduction
                    ,Projects.UserId
                    ,Projects.Income
                    ,Projects.Revenue
                    ,Projects.UserName
                    ,Projects.BrokerageOption
                    ,ProjectVisibleRules.Tags
                    ,ProjectVisibleRules.Visible
                    FROM 
                    Projects INNER JOIN ProjectVisibleRules 
                    ON Projects.Id = ProjectVisibleRules.ProjectId
                    WHERE Projects.id = @projectId";
                return await conn.QueryAsync<dynamic>(sql, new { projectId });
            }
        }

        public async Task<dynamic> GetProjectsByUserId(int userId)
        {
            using (var conn = _context.Database.GetDbConnection())
            {
                conn.Open();

                string sql = @"SELECT
                Projects.Id
                ,Projects.Company
                ,Projects.Avatar
                ,Projects.FinStage
                ,Projects.Introduction
                ,Projects.Tags
                ,Projects.ShowSecurityInfo
                ,Projects.CreateTime
                FROM 
                Projects WHERE Projects.UserId =@userId";
                return await conn.QueryAsync<dynamic>(sql, new { userId });
            }
        }
    }
}
