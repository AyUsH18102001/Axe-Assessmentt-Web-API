using AxeAssessmentToolWebAPI.Data;
using AxeAssessmentToolWebAPI.Models;
using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace AxeAssessmentToolWebAPI.Services
{
    public class UserService : IUserService
    {

        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostEnvironment;
        // salt value
        private readonly string salt;

        public UserService(DataContext context, IConfiguration configuration,IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostEnvironment)
        {
            this._dataContext = context;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
            this._hostEnvironment = hostEnvironment;
            this.salt = _configuration.GetValue<string>("Salt");
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<string> GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData);
        public async Task<string> GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

        public async Task<bool> RegisterUser(User user)
        {
            if (user == null)
            {
                return false;
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
                    user.Password = hashPassword($"{user.Password}{salt}");
                    // save the user token to db
                    user.UserToken = await generateUserTestToken("",false);
                    await _dataContext.Users.AddAsync(user);
                    _dataContext.SaveChanges();
                    return true;
                }
            }
            return false;
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

        public async Task<string> UploadResume(int userId,IFormFile resume)
        {
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "App_Data");
            if (resume != null)
            {
                var localFileName = resume.FileName;
                // find the user and add the filename of its resume
                User? user = await _dataContext.Users.FindAsync(userId);
                if (user==null)
                {
                    return "";
                }
                user.UserResumeFileName = localFileName;
                await _dataContext.SaveChangesAsync();
                var filePath = Path.Combine(app_data_path, localFileName);
                var stream = new FileStream(filePath.Trim('"'), FileMode.Create);
                resume.CopyTo(stream);
                return "success";
            }
            return "";
        }

        public async Task<string> LoginUser(string email, string password)
        {
            if (email == "" || password == "")
            {
                return "";
            }
            User? user = await _dataContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            
            if (user != null)
            {
                          
                if (user.Password == hashPassword($"{password}{salt}"))
                {
                    return createToken(user);
                }
            }
            return "";
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

        public async Task<bool> ForgotPassword(string email, string newPassword)
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
                    return true;
                }
            }
            return false;
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

        public async Task<List<TestData>> GetTestData()
        {
            List<Question> questions = await _dataContext.Questions.ToListAsync();
            List<TestData> testData = new List<TestData>();
            foreach (var question in questions)
            {
                testData.Add(
                    new TestData
                    {
                        QuestionId = question.QuestionId,
                        Question = question.QuestionContent,
                        QuestionType = question.QuestionsType,
                        Options = getOptions(question.QuestionId)
                    }
               );
            }
            return testData;
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
            _dataContext.SaveChanges();
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

    }
}
