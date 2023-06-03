using AutoMapper;
using AxeAssessmentToolWebAPI.Response_Models;

namespace AxeAssessmentToolWebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Question, TestData>();
            CreateMap<SQL_Question, SQL_TestData>();
            CreateMap<Test, TestDetails>();
        }

    }
}
