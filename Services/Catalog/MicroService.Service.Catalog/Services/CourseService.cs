﻿using AutoMapper;
using FreeCourse.Shared.Dtos;
using MicroService.Service.Catalog.Dtos;
using MicroService.Service.Catalog.Models;
using MicroService.Service.Catalog.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Service.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(categori => true).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return Response<CourseDto>.Fail("Course Not found", 404);
            }
            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string id)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == id).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);

        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newcourse = _mapper.Map<Course>(courseCreateDto);
            await _courseCollection.InsertOneAsync(newcourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newcourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var newcourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, newcourse);
            if (result == null) return Response<NoContent>.Fail("Course Nofound", 404);
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0) return Response<NoContent>.Success(204);
            return Response<NoContent>.Fail("Course Nofound", 404);
        }
    }
}
