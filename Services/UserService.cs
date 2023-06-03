
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using AxeAssessmentToolWebAPI.FrontendModel;
using AxeAssessmentToolWebAPI.Response_Models;
using Microsoft.Data.SqlClient;
using System.Data;
using AxeAssessmentToolWebAPI.Migrations;
using AxeAssessmentToolWebAPI.Models;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AxeAssessmentToolWebAPI.Services
{
    public class UserService : IUserService
    {

        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        // salt value
        private readonly string salt;

        public UserService(DataContext context,IMapper mapper, IConfiguration configuration,IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostEnvironment)
        {
            this._dataContext = context;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this._hostEnvironment = hostEnvironment;
            this._mapper = mapper;
            this.salt = _configuration.GetValue<string>("Salt");
        }

        public async Task<IQueryable<User>> GetAllUser(int curentPage)
        {
            IQueryable<User> response;

            Pagination page = new Pagination();
            page.currentPage = curentPage;
            page.pageCount = (int) Math.Ceiling(_dataContext.Users.Count() / page.pageSize);

            response = _dataContext.Users
                .Where(u => u.isAdmin == false)
                .Skip((page.currentPage - 1) * (int)page.pageSize)
                .Take((int) page.pageSize);

            return response;
        }

        public async Task<int> GetUserPageCount()
        {
            Pagination page = new Pagination();
            return (int)Math.Ceiling(_dataContext.Users.Count() / page.pageSize);
        }



        public async Task<UserData> GetUserEmailAndId()
        {
            string id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData);
            string email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            User? user = await _dataContext.Users.Where(u => u.UserId.ToString() == id).FirstOrDefaultAsync();

            UserData response = new UserData();
            response.UserId = Int16.Parse(id);
            response.Email = email;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;

            return response;
        }

        public async Task<string> UserSubmitTest(TestSubmit answers)
        {
            User user = await _dataContext.Users.Where(ut => ut.UserId == answers.UserId).FirstOrDefaultAsync();
            UserTest? userTest = user.UserTest;

            // delete the previous test questions
            _dataContext.User_QuestionAttempted.RemoveRange(_dataContext.User_QuestionAttempted
                .Where(u_qa => u_qa.UserId == answers.UserId));
            await _dataContext.SaveChangesAsync();

            if (answers.CandidatesQuestions.Count == 0)
            {
                // update the user test parameters
                user.violation = answers.Violation;
                userTest.Attempted = answers.TotalQuestions;
                userTest.EndTest = true;
                userTest.TestPeriod = answers.TestPeriod;
                userTest.TotalQuestions = answers.TotalQuestions;
                await _dataContext.SaveChangesAsync();
                return "success";
            }
            // get all the questions
            List<Question> questions = await _dataContext.Questions.ToListAsync();

            answers.CandidatesQuestions.ForEach(async answer =>
            {
                // get the appropriate question
                Question? question = questions.Where(q => q.QuestionId == answer.QuestionId).FirstOrDefault(); 
                List<Option> questionOptions = question.Options;

                answer.SelectedOption.ForEach(async (option) =>
                {
                    User_QuestionAttempted u_qa = new User_QuestionAttempted();
                    u_qa.QuestionId = question.QuestionId;
                    u_qa.UserId = answers.UserId;
                    u_qa.OptionIndex = option;
                    await _dataContext.User_QuestionAttempted.AddAsync(u_qa);

                    // find the option of the optionIndex
                    Option op = questionOptions[option];

                    // update the score
                    userTest.Score += op.Score;
                });
            });
            user.violation = answers.Violation;
            userTest.Attempted = answers.TotalQuestions;
            userTest.EndTest = true;
            userTest.TestPeriod = answers.TestPeriod;
            userTest.TotalQuestions = answers.TotalQuestions;
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<MessageAndCode> RegisterUser(User user)
        {
            MessageAndCode response = new MessageAndCode();
            if (user == null)
            {
                response.Message = "Please provide credentials";
                response.error = true;
                return response;
            }
            // valid email
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                // checking if the user already registered
                var check = await _dataContext.Users.AnyAsync(u => u.Email == user.Email && u.Phone == user.Phone);
                if(!check) // if check returns false we are good to go forward and register
                {
                    // hashing the user password
                    // user.Password = hashPassword($"{user.Password}{salt}");
                    // save the user token to db
                    user.UserToken = await generateUserTestToken("",false);
                    await _dataContext.Users.AddAsync(user);
                    await _dataContext.SaveChangesAsync();
                    response.Message = "Successfully registered";
                    response.error = false;
                    return response;
                }
                response.Message = "Email has already been registered";
                response.error = true;
                return response;
            }
            response.Message = "Please check email";
            response.error = true;
            return response;
        }

        public async Task<string> UpdateQuestionAttempted(int userId, int questionId, List<int> optionsIndex)
        {
            // get the user
            User? user = await _dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                return "";
            }
            else
            {
                // update the field
                optionsIndex.ForEach(async (option) =>
                {
                    User_QuestionAttempted u_qa = new User_QuestionAttempted();
                    u_qa.QuestionId = questionId;
                    u_qa.UserId = userId;
                    u_qa.OptionIndex = option;
                    await _dataContext.User_QuestionAttempted.AddAsync(u_qa);
                });
                return "success";
            }
        }

        public async Task<User> GetUser(int id)
        {
            var result = await _dataContext.Users.FindAsync(id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<string> UploadResume(IFormFile resume)
        {
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");
            if (resume != null)
            {
                var localFileName = resume.FileName;

                var filePath = Path.Combine(app_data_path, "CandidatesResume",localFileName);
                var stream = new FileStream(filePath.Trim('"'), FileMode.Create);
                resume.CopyTo(stream);
                return "success";
            }
            return "";
        }

        public async Task<string> GetUserResume(int userId)
        {
            string hostURL = "https://localhost:7143";
            string? localName = _dataContext.Users.FindAsync(userId).Result.UserResumeFileName;
            if (localName == null)
            {
                return "";
            }
            else
            {
                return hostURL + "/CandidatesResume/" + localName;
            }
        }

        public async Task<string> GetUserProfileImage(int userId)
        {
            string hostURL = "https://localhost:7143";
            string? localName = _dataContext.Users.FindAsync(userId).Result.UserProfileImage;
            if (localName == null)
            {
                return "";
            }
            else
            {
                return hostURL + "/CandidatesProfile/" + localName;
            }
        }

        public async Task<string> UploadUserProfile(IFormFile image)
        {
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");
            if (image != null)
            {
                var localFileName = image.FileName;

                var filePath = Path.Combine(app_data_path, "CandidatesProfile", localFileName);
                var stream = new FileStream(filePath.Trim('"'), FileMode.Create);
                image.CopyTo(stream);
                return "success";
            }
            return "";
        }

        public async Task<Token> LoginUser(Login creds)
        {
            Token response = new Token();
            if (creds.email == "" || creds.password == "")
            {
                return null;
            }
            User? user = await _dataContext.Users.Where(u => u.Email == creds.email).FirstOrDefaultAsync();
            
            if (user != null)
            {
                          
                if (user.Password == hashPassword($"{creds.password}{salt}"))
                {
                    if ((bool)user.isAdmin && user.isAdmin != null)
                    {
                        response.token = createToken(user);
                        response.isAdmin = true;
                        return response;
                    }
                    else
                    {
                        response.token = createToken(user);
                        response.isAdmin = false;
                        return response;
                    }
                }
                else
                {
                    response.error = "Incorrect Password";
                    response.isAdmin = false;
                    return response;
                }
            }
            else
            {
                response.error = "No such account is registered";
                response.isAdmin = false;
                return response;
            }
        }

        public async Task<string> LoginWithUserToken(string token)
        {
            // CHECK IF THE TOKEN EXISTS IN DATABASE
            User? user = await _dataContext.Users.Where(u => u.UserToken == token).FirstOrDefaultAsync();
            if (user == null)
            {
                return "";
            }
            // return a new jwt token
            return createToken(user);
        }

        public async Task<string> ForgotPassword(string email, string newPassword)
        {
            // check if the email is valid
            // valid email
            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                // checking if the user exists
                var user = await _dataContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
                if (user != null)
                {
                    // update the password
                    user.Password = hashPassword($"{newPassword}{salt}");
                    _dataContext.SaveChanges();
                    return "Password updated successfully";
                }
                return "No suchuser profile exists";
            }
            return "Invalid Email";
        }

        public async Task<string> UpdateSqlScore(int userId)
        {
            // find the user
            User? user = await _dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            user.UserTest.Score += 10;
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<UserTest> GetUserTest(string email)
        {
            // check if the user has a test data
            var check = await _dataContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            User updatedUser;
            if (check != null)
            {
                if (check.UserTest == null)
                {
                    // add a UserTest Record to the corresponding user
                    updatedUser = await addUserTestRecord(check);
                }
                else
                {
                    return check.UserTest;
                }
            }
            else
            {
                return null;
            }
            return updatedUser.UserTest;
        }

        private async Task<User> addUserTestRecord(User user)
        {
            var userTest = new UserTest
            {
                EndTest = false,
                Score = 0,
                Attempted = 0,
                UserId = user.UserId,
            };
            _dataContext.UserTests.Add(userTest);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<TestData>> GetTestData(int userId)
        {
            List<TestData> testData = new List<TestData>();
            // get the user testId
            int? testId = _dataContext.UserTests.Where(ut => ut.UserId == userId).FirstOrDefaultAsync().Result.TestId;
            if (testId == null)
            {
                return null;
            }
            // early check
            Test test = await _dataContext.Test.Where(t => t.TestId == testId).FirstOrDefaultAsync();
            if(test.MCQ_Count == 0)
            {
                return testData;
            }
            return _getAptitudeQuestions(testId);
        }

        public async Task<IEnumerable<SQL_TestData>> SqlTestData(int userId)
        {
            List<SQL_TestData> testData = new List<SQL_TestData>();
            // get the user testId
            int? testId = _dataContext.UserTests.Where(ut => ut.UserId == userId).FirstOrDefaultAsync().Result.TestId;
            if (testId == null)
            {
                return null;
            }
            Test test = await _dataContext.Test.Where(t => t.TestId == testId).FirstOrDefaultAsync();
            if (test.SQL_Count == 0)
            {
                return testData;
            }
            return _getSqlQuestions(testId);
        }

        private List<TestData> _getAptitudeQuestions(int? testId)
        {
            List<TestData> testData = new List<TestData>();
            // get all the Questions that are test specific
            IQueryable<TestQuestions> testsQuestions = _dataContext.TestQuestions.Where(t => t.TestId == testId);

            // join the TestQuestion and Question to get all the aptitude questions
            IQueryable<Question> questions = (from tq in testsQuestions
                                              join q in _dataContext.Questions
                                              on tq.QuestionId equals q.QuestionId
                                              where (q.IsSQL == 0 || q.IsSQL == null)
                                              select q);

            questions.ToList().ForEach(async (q) =>
            {
                testData.Add(
                    new TestData
                    {
                        QuestionId = q.QuestionId,
                        Question = q.QuestionContent,
                        QuestionImage = await getQuestionImage(q.QuestionId),
                        QuestionType = q.QuestionTypeId != null ? getQuestionType(q.QuestionTypeId) : null,
                        Options = getOptions(q.QuestionId),
                    }
                );
            });
            return testData;
        }

        private IQueryable<SQL_TestData> _getSqlQuestions(int? testId)
        {
            // get all the Questions that test specific
            IQueryable<TestQuestions> testsQuestions = _dataContext.TestQuestions.Where(t => t.TestId == testId);

            // join the test questions and questions to get the test specific questions
            IQueryable<Question> questions = (from tq in testsQuestions
                                              join q in _dataContext.Questions
                                              on tq.QuestionId equals q.QuestionId
                                              where (q.IsSQL != 0 || q.IsSQL != null)
                                              select q);

            // join the sql questions and questions, and get the sql questions
            IQueryable<SQL_Question> sqlQuestions = (from que in questions
                                                     join sql in _dataContext.SQL_Question
                                                     on que.IsSQL equals sql.QuestionId
                                                     select sql);
            // map the SQL_Question to SQL_TestData
            return sqlQuestions.Select(q => _mapper.Map<SQL_TestData>(q));
        }

        public async Task<List<CandidateRules>> GetTestRules(int userId)
        {
            // get the user testId
            int? testId = _dataContext.UserTests.Where(ut => ut.UserId == userId).FirstOrDefaultAsync().Result.TestId;
            if (testId == null)
            {
                return null;
            }
            // get the test rules
            List<TestRules> testsQuestions = await _dataContext.TestRules.Where(tr => tr.TestId == testId).ToListAsync();
            List<CandidateRules> rules = new List<CandidateRules>();
            testsQuestions.ForEach((tr) =>
            {
                Rules r = _dataContext.Rules.Where(r => r.RuleId == tr.Rule).FirstOrDefault();
                if (r != null)
                {
                    rules.Add(new CandidateRules
                    {
                        RuleId = r.RuleId,
                        RuleDisplay = r.RuleDisplay,
                        short_name = r.short_name
                    });
                }
            });
            return rules;
        }

        public async Task<List<TestQuestions>> GetTestQuestions2(int userId)
        {
            // get the user testId
            int? testId = _dataContext.UserTests.Where(ut => ut.UserId == userId).FirstOrDefaultAsync().Result.TestId;
            if (testId == null)
            {
                return null;
            }
            Test test = await _dataContext.Test.Where(t => t.TestId == testId).FirstOrDefaultAsync();
            // get all the questions of the corresponding testId
            List<TestQuestions> testQuestions = await _dataContext.TestQuestions.Where(t => t.TestId == testId).ToListAsync();
            return testQuestions;
        }

        public async Task<CandidateTest_QuestionsData> GetTestQuestions(int userId)
        {
            CandidateTest_QuestionsData result = new CandidateTest_QuestionsData();
            IQueryable<SQL_TestData> sqlQuestions;
            List<TestData> aptitudeQuestions = new List<TestData>();

            // get the user testId
            int? testId = _dataContext.UserTests.Where(ut => ut.UserId == userId).FirstOrDefaultAsync().Result.TestId;
            if (testId == null)
            {
                return null;
            }
            Test test = await _dataContext.Test.Where(t => t.TestId == testId).FirstOrDefaultAsync();
            if(test.MCQ_Count == 0)
            {
                result.AptitudeQuestions = aptitudeQuestions;
                // get the sql questions and return
                result.SqlQuestions = _getSqlQuestions(testId);
                return result;
            }
            if (test.SQL_Count == 0)
            {
                result.SqlQuestions = Enumerable.Empty<SQL_TestData>().AsQueryable();
                // get the mcq questions and return
                result.AptitudeQuestions = _getAptitudeQuestions(testId);
                return result;
            }
            else
            {
                // get the sql quetions and mcq questions;
                result.SqlQuestions = _getSqlQuestions(testId);
                result.AptitudeQuestions = _getAptitudeQuestions(testId);
                if (result != null)
                {   
                    return result;
                }
                return null;
            }   
        }

        public async Task<TestInfo> GetTestInformation(int userId)
        {
            // get the user testId
            int? testId = _dataContext.UserTests.Where(ut => ut.UserId == userId).FirstOrDefaultAsync().Result.TestId;
            if (testId == null)
            {
                return null;
            }
            Test test = await _dataContext.Test.Where(t => t.TestId == testId).FirstOrDefaultAsync();
            TestInfo response = new TestInfo
            {
                TestName = test.TestName,
                McqCount = test.MCQ_Count,
                SqlCount = test.SQL_Count
            };
            return response;
        }

        private async Task<string> getQuestionImage(int questionId)
        {
            string hostURL = "https://localhost:7143";
            string? localName = _dataContext.Questions.FindAsync(questionId).Result.QuestionImage;
            if (localName == null)
            {
                return "";
            }
            else
            {
                return hostURL + "/QuestionImages/" + localName;
            }
        }

        public async Task<bool> IsUserTerminated(int userId)
        {
            User? user = await _dataContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if(user == null)
            {
                return false;
            }
            return user.violation >= 3 ? true : false;
        }

        public async Task<int> GetUserViolation(int userId)
        {
            User? user = await _dataContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return -1;
            }
            return user.violation;
        }

        public async Task<bool> UpdateUserViolation(int userId)
        {
            User? user = await _dataContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            user.violation++;
            _dataContext.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateScore(int userId, int questionId, List<int> options)
        {
            User? user = await _dataContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            // getthe question
            Question? question = await _dataContext.Questions.FindAsync(questionId);
            if (question != null)
            {
                // update the score of the selected option
                foreach (var item in options)
                {
                    int score = question.Options[item].Score;
                    //update the score
                    UserTest userTest = user.UserTest;
                    userTest.Score += score;
                    _dataContext.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateEndTest(int userId)
        {
            User? user = await _dataContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if(user == null)
            {
                return false;
            }
            UserTest userTest = user.UserTest;
            userTest.EndTest = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        private string hashPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            var passwordBytes = Encoding.Default.GetBytes(password);
            var hashedPassword = hash.ComputeHash(passwordBytes);
            return Convert.ToHexString(hashedPassword);
        }

        private List<string> getOptions(int questionId)
        {
            List<string> options = new List<string>();
            List<Option> query = _dataContext.Options.Where(op => op.QuestionId == questionId).ToList();
            foreach (var item in query)
            {
                options.Add(item.Answer);
            }
            return options;
        }

        private string getQuestionType(int? qt)
        {
            QuestionType result = _dataContext.QuestionType.Find(qt);
            string type = result.Type;
            return result != null ? type : "";
        }

        private async Task<string> generateUserTestToken(string token,bool foundToken)
        {
            if (foundToken)
            {
                token = token.Substring(0, 3) + "-" + token.Substring(4, 4) + "-" + token.Substring(8, 4);
                return token;
            }
            char[] letters = "abcdefghijklmnopqrstuvwxyz@!?$ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            //CREATING THE TOKEN
            token = helperFunction_generateUserTestToken(letters);
            // checkig if the token exists in the database
            var result = await _dataContext.Users.Where(u => u.UserToken == token).FirstOrDefaultAsync();
            if (result == null)
            {
                // ADDING HE HYPHEN AFTER 3 CHARS and returning
                return await generateUserTestToken(token, true);
            }
            else
            {
                return await generateUserTestToken(token, false);
            }
        }

        private string helperFunction_generateUserTestToken(char[]letters)
        {
            Random random = new Random();
            string token = string.Empty;
            for (int i = 0; i < 12; i++)
            {
                token += letters[random.Next(0, letters.Length)].ToString();
            }
            return token;
        }

        private string createToken(User user)
        {
            if (user == null)
            {
                return "";
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData,user.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email), 
            };
            if ((bool)user.isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Token")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials :creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        private async Task<string> getIdealQuery(int questionId)
        {
            // get the sql question
            SQL_Question question = await _dataContext.SQL_Question.FindAsync(questionId);
            if (question == null)
            {
                return "";
            }
            return question.SQL_Answer.Query;
        }

        public async Task<SQL_Response> RunSqlQuery(UserQuery query)
        {
            SQL_Response response = new SQL_Response();

            DataTable userDataTable = new DataTable();
            DataTable idealDataTable = new DataTable();

            string queryString = query.query;
            string idealQueryString = await getIdealQuery(query.questionId);

            string connectionString = _configuration.GetSection("ConnectionStrings").GetSection("connectionString2").Value;
            connectionString = $"{connectionString}";

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // run the user query
                SqlCommand u_cmd = new SqlCommand(queryString, connection);
                // create data adapter
                SqlDataAdapter u_da = new SqlDataAdapter(u_cmd);
                // this will query your database and return the result to your datatable
                u_da.Fill(userDataTable);

                // run the ideal query
                SqlCommand i_cmd = new SqlCommand(idealQueryString, connection);
                // create data adapter
                SqlDataAdapter i_da = new SqlDataAdapter(i_cmd);
                // this will query your database and return the result to your datatable
                i_da.Fill(idealDataTable);


                // intersection of two datatables for comparing the datatables
                try
                {
                    DataTable result = userDataTable.AsEnumerable()
                    .Intersect(idealDataTable.AsEnumerable(), DataRowComparer.Default)
                    .CopyToDataTable();

                    if (userDataTable.Rows.Count != result.Rows.Count)
                    {
                        MessageAndCode msg = new MessageAndCode()
                        {
                            Message = "error"
                        };
                        response.Message = msg;
                    }
                    else
                    {
                        MessageAndCode msg = new MessageAndCode()
                        {
                            Message = "success"
                        };
                        response.Message = msg;
                    }
                }
                catch(Exception exp)
                {
                    MessageAndCode msg = new MessageAndCode()
                    {
                        Message = "error"
                    };
                    response.Message = msg;
                }

                List<List<string>> output = new List<List<string>>();
                foreach (DataRow row in userDataTable.Rows)
                {
                    List<string> inner = new List<string>();
                    foreach (DataColumn column in userDataTable.Columns)
                    {
                        inner.Add(row[column].ToString());
                    }
                    output.Add(inner);
                }   
                response.Result = output;
                await connection.CloseAsync();
                u_da.Dispose();
                i_da.Dispose();
                
                return response;
            }
            catch (Exception exp)
            {
                MessageAndCode msg = new MessageAndCode()
                {
                    Message = exp.Message
                };
                response.Message = msg;
                await connection.CloseAsync();
                return response;
            }
            finally
            {
                // Always call Close when done reading.
                await connection.CloseAsync();
            }
        }

        public async Task<string> SubmitQuery(List<SubmitSql> queries)
        {
            // find the user
            User? user = await _dataContext.Users.FindAsync(queries[0].UserId);
            queries.ForEach(async query =>
            {
                // add the user Query to questionsAttempted
                User_QuestionAttempted u_qa = new User_QuestionAttempted
                {
                    QuestionId = query.QuestionId,
                    UserId = query.UserId,
                    SqlResult = query.Result,
                    SqlQuery = query.Query,
                };
                await _dataContext.User_QuestionAttempted.AddAsync(u_qa);

                // add score if query is correct
                if (query.Result)
                {
                    if (user != null)
                    {
                        user.UserTest.Score += 10;
                    }
                }
                user.UserTest.Attempted += 1;
                user.UserTest.TotalQuestions += 1;
            });
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<string> UserTestName(int userId)
        {
            // get the test id
            int testId = (await _dataContext.Users.FindAsync(userId)).UserTest.TestId;
            Test test = await _dataContext.Test.FindAsync(testId);
            if (test == null)
            {
                return "";
            }
            string testName = test.TestName;
            return testName;
        }
    }
}
