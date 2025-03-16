using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using TaskManager.Dtos;
using TaskManager.Models;

namespace TaskManager.Data;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        
        //TODO : TaskTime
        CreateMap<TaskForEdit, TaskTime>();
        CreateMap<TaskTime, TaskForEdit>();

        CreateMap<TaskDto, TaskTime>();
        CreateMap<TaskTime, TaskDto>();

        CreateMap<CreateTaskDto, TaskTime>();
        CreateMap<TaskTime, CreateTaskDto>();

        //TODO : Category
        CreateMap<CategoryForEditDto, Category>();
        CreateMap<Category, CategoryForEditDto>();

        //TODO : User
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();

        CreateMap<UserForRegistration, User>();
        CreateMap<User, UserForRegistration>();

        CreateMap<UserForEdit, User>();
        CreateMap<User, UserForEdit>();



    }
}