using ComicsLibrary.Common;
using ComicsLibrary.Common.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ComicsLibrary.SqlData
{
    public class Library : ILibrary
    {
        private readonly IConfiguration _config;

        public Library(IConfiguration config)
        {
            _config = config;
        }

        public List<NextComicInSeries> GetAllNextIssues()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            return connection.Query<NextComicInSeries>(
                "ComicsLibrary.GetHomeBooks",
                commandType: CommandType.StoredProcedure).ToList();
        }

        public NextComicInSeries GetNextUnread(int seriesId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            return connection.QuerySingle<NextComicInSeries>(
                "ComicsLibrary.GetHomeBooks", new 
                { 
                    SeriesId = seriesId 
                },
                commandType: CommandType.StoredProcedure);
        }

        public List<LibrarySeries> GetSeries()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            return connection.Query<LibrarySeries>(
                "ComicsLibrary.GetSeries", 
                commandType: CommandType.StoredProcedure).ToList();
        }

        public LibrarySeries GetSeries(int seriesId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            return connection.QuerySingle<LibrarySeries>(
                "ComicsLibrary.GetSeries", new 
                { 
                    SeriesId = seriesId 
                }, 
                commandType: CommandType.StoredProcedure);
        }

        public void UpdateHomeBookType(HomeBookType homeBookType)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Library"));
            connection.Execute(
                "ComicsLibrary.UpdateHomeBookType", 
                new 
                { 
                    SeriesId = homeBookType.SeriesId, 
                    BookTypeId = homeBookType.BookTypeId, 
                    Enabled = homeBookType.Enabled 
                }, 
                commandType: CommandType.StoredProcedure);
        }
    }
}
