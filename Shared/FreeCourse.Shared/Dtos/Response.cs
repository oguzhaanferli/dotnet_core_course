using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool isSuccessful { get; private set; }
        public List<string> Errors { get; set; }


        //Static Factory Method
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, isSuccessful = true, StatusCode = statusCode };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), isSuccessful = true, StatusCode = statusCode };
        }
        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T> { Data = default(T), isSuccessful = false, StatusCode = statusCode, Errors = errors };
        }
        public static Response<T> Fail(string errors, int statusCode)
        {
            return new Response<T> { Data = default(T), isSuccessful = false, StatusCode = statusCode, Errors = new List<string>() { errors } };
        }
    }
}
