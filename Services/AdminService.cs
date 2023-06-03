
using AutoMapper;
using AxeAssessmentToolWebAPI.Migrations;
using AxeAssessmentToolWebAPI.Models;
using AxeAssessmentToolWebAPI.Response_Models;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace AxeAssessmentToolWebAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly DataContext _dataContext;
        private readonly DataContext2 _dataContext2;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public AdminService(DataContext context,DataContext2 dataContext2,IMapper mapper, IHostingEnvironment hostEnvironment)
        {
            _dataContext = context;
            this._hostEnvironment = hostEnvironment;
            this._mapper = mapper;
            _dataContext2 = dataContext2;
        }
        
        public async Task<bool> AddAdminProfile(int userId)
        {
            // find the user
            User? user = await _dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            // make admin
            user.isAdmin = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCandidateSelction(int userId,bool status)
        {
            User? user = await _dataContext.Users.FindAsync(userId);
            if(user == null)
            {
                return false;
            }
            user.SelectionStatus = status ? 1 : -1;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpateSelectionState(int userId, int state)
        {
            // get the user
            User? user = await _dataContext.Users.FindAsync(userId);
            // update the state
            user.SelectionStatus = state;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddBulkQuestionsData(IFormFile file)
        {
            // file copied to QuestionBulk folder
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");
            var filePath = "";
            if (file != null)
            {
                var localFileName = file.FileName;
                filePath = Path.Combine(app_data_path, "QuestionBulk", localFileName);
                var stream = new FileStream(filePath.Trim('"'), FileMode.Create);
                file.CopyTo(stream);
            }

            // read the file from folder

            // level-1 transformation

            // append to dt

            // sql bulk copy

            // biforcate to questionas and options


            return false;
        }

        public async Task<List<Rules>> GetAllTestRules()
        {
            return await _dataContext.Rules.ToListAsync();
        }

        public async Task<string> GetTestCreator(int creatorId)
        {
            User creator = await _dataContext.Users.Where(u => u.UserId == creatorId).FirstOrDefaultAsync();
            if (creator == null)
            {
                return null;
            }
            return creator.FirstName +" " + creator.LastName;
        }

        public async Task<string> CreateTest(Test test)
        {
            if (test == null)
            {
                return null;
            }
            await _dataContext.Test.AddAsync(test);
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<string> DeleteTest(int testId, int deleteId)
        {
            try
            {
                // get the test
                Test test = await _dataContext.Test.FindAsync(testId);
                // soft delete
                test.D_Date = DateTime.Now;
                test.D_Id = deleteId;
                await _dataContext.SaveChangesAsync();
                return "success";
            }
            catch(Exception exp)
            {
                return exp.Message;
            }
        }

        public async Task<List<TestPaper>> GetTestPaperQuestions(int testId)
        {
            List<TestPaper> response = await (from tq in _dataContext.TestQuestions join
                                         q in _dataContext.Questions on tq.QuestionId equals q.QuestionId
                                              from qt in _dataContext.QuestionType.Where(qt => q.QuestionTypeId == qt.QuestionTypeId).DefaultIfEmpty()
                                              where(tq.TestId == testId)
                                              select new TestPaper
                                              {
                                                  Id = q.QuestionId,
                                                  Content = q.QuestionContent,
                                                  Type = qt.Type == null ? "SQL" : qt.Type,
                                                  TestId = q.TestTypeId,
                                                  short_name = qt.short_name == null ? "sql" : qt.short_name
                                              }
                                  ).ToListAsync();
            return response;
        } 

        public async Task<string> UpdateTest(int testId,Test updated)
        {
            Test? test = await _dataContext.Test.FindAsync(testId);
            // delete the testquestions and TestRules
            _dataContext.TestQuestions.RemoveRange(_dataContext.TestQuestions.Where(tq => tq.TestId == testId));
            _dataContext.TestRules.RemoveRange(_dataContext.TestRules.Where(tq => tq.TestId == testId));
            await _dataContext.SaveChangesAsync();

            // update the test
            if ( test != null )
            {
                test.TestName = updated.TestName;
                test.TestCreator = updated.TestCreator;
                test.U_Date = DateTime.Now;
                test.U_Id = updated.TestCreator;
                test.MCQ_Count = updated.MCQ_Count;
                test.SQL_Count = updated.SQL_Count;
                test.TestQuestions = updated.TestQuestions;
                test.TestRules = updated.TestRules;
                await _dataContext.SaveChangesAsync();
                return "success";
            }
            return "";
        }

        public async Task<IQueryable<TestDetails>> GetAllTest()
        {
            return _dataContext.Test.Where(t => t.D_Date == null).Select(t => _mapper.Map<TestDetails>(t));
        }

        public async Task<string> UpdateUserTestId(int testId, int userId)
        {
            // Find the user
            User? user = await _dataContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.UserTest.TestId = testId;
                await _dataContext.SaveChangesAsync();
                return "success";
            }
            return null;
        }

        public async Task<IQueryable<Rules>> GetTestRules(int userId)
        {
            // get the test assigned to the user
            int testId = (await _dataContext.Users.FindAsync(userId)).UserTest.TestId;
            if (testId == 0) return null;
            IQueryable<Rules> testRules = from tr in _dataContext.TestRules
                                   join r in _dataContext.Rules on tr.Rule equals r.RuleId
                                   where (tr.TestId == testId)
                                   select r;
            return testRules;
        }

        public async Task<string> DownloadTestFormat()
        {
            HttpResponseMessage response;
            HttpRequestMessage request = new HttpRequestMessage(); 
            // get the image localname
            string localName = "profile.jpg";
            // set the directry path
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");

            var filePath = Path.Combine(app_data_path, "CandidatesProfile", localName);

            var contents = File.ReadAllBytes(filePath); 

            response = request.CreateResponse(HttpStatusCode.OK);
            // Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(contents);
            return base64String;
            /*response.Content = new StreamContent(ms);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = localName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            return response;*/
        }

        public async Task<string> GetCandidateProfile(int userId)
        {
            // get the user iamge filename
            string localName = (await _dataContext.Users.FindAsync(userId)).UserProfileImage;
            if (localName == null)
            {
                return null;
            }
            // set the directry path
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");
            var filePath = Path.Combine(app_data_path, "CandidatesProfile", localName);
            var contents = File.ReadAllBytes(filePath);
            // Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(contents);
            return base64String;
        }

        public async Task<List<TableDefinition>> GetTableSchemas()
        {
            List<TableDefinition> tables = new List<TableDefinition>();
            string query = @"select ICS.TABLE_NAME,ICS.COLUMN_NAME,ICS.DATA_TYPE from INFORMATION_SCHEMA.COLUMNS ics inner join sys.tables t on ics.TABLE_NAME = t.name where TABLE_NAME NOT IN ('__EFMigrationsHistory','__EFMigrationsHistory')";
            DbCommand command = _dataContext2.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            _dataContext2.Database.OpenConnection();
            DbDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            // get the list of datatable from datatable
            List<DataTable> dataTables = await _splitDataTable(dt);

            // create the TableDefintion object
            foreach (DataTable dataTable in dataTables)
            {
                List<string> columns = new List<string>();
                List<string> types = new List<string>();
                // add the types and columns
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataTable.Rows[i];
                    columns.Add(dr.ItemArray[1].ToString());
                    types.Add(dr.ItemArray[2].ToString());
                }
                tables.Add(new TableDefinition
                {
                    TableName = dataTable.Rows[0].ItemArray[0].ToString(),
                    DataType = types,
                    Columns = columns
                });
            }

            return tables;
        }

        public async Task<IQueryable<Rules>> GetTestRules_Edit(int testId)
        {
            IQueryable<Rules> testRules = from tr in _dataContext.TestRules
                                          join r in _dataContext.Rules on tr.Rule equals r.RuleId
                                          where (tr.TestId == testId)
                                          select r;
            return testRules;
        }

        private async Task<List<DataTable>> _splitDataTable(DataTable dt)
        {
            List<DataTable> dataTables = new List<DataTable>();
            int start = 0;
            int end = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow ptr1 = dt.Rows[i];
                if (i == dt.Rows.Count - 1)
                {
                    end = i;
                    DataTable result = await _dt(dt, start, end);
                    dataTables.Add(result);
                    break;
                }
                DataRow ptr2 = dt.Rows[i + 1];
                if (ptr1.ItemArray[0].ToString() != ptr2.ItemArray[0].ToString())
                {
                    end = i;
                    DataTable result = await _dt(dt, start, end);
                    dataTables.Add(result);
                    start = i + 1;
                }
            }
            return dataTables;
        }

        private async Task<DataTable> _dt(DataTable dt,int start,int end)
        {
            DataTable result = dt.Clone();
            for (int i = start; i <= end; i++)
            {
                DataRow row = dt.Rows[i];
                result.Rows.Add(row.ItemArray);  
            }
            return result;
        }
    }
}
