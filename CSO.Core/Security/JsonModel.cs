using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CSO.Core.Security
{
    public enum JsonType
    {
        Error = 0,
        Success = 1,
        Info = 2,
        Warning = 3,
        Question = 4
    }


    public enum TasksStatus
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3,
        Start = 4,
        Stop = 5,
    }

    public class JsonModel
    {
        public JsonModel()
        {
        }

        public JsonModel(JsonType jsonType, string message, string title = null, object result = null)
        {
            JsonType = jsonType;
            Message = message;
            Title = title;
            Result = result;
        }

        public string AlertType => JsonType.ToString();

        /*[ScriptIgnore]*/
        public JsonType JsonType { get; set; }

        public string Message { get; set; }
        public object Result { get; set; }
        public string Title { get; set; }

        public static JsonModel DeleteSuccess(string name)
        {
            return new JsonModel(JsonType.Success, string.Empty, name + " " + GlobalConstant.Deleted);
        }
        public static JsonModel DeleteFailed(string name)
        {
            return new JsonModel(JsonType.Error, string.Empty, name + " " + GlobalConstant.DeleteFailed);
        }
        public static JsonModel UpdateSuccess(string name)
        {
            return new JsonModel(JsonType.Success, string.Empty, name + " " + GlobalConstant.Updated);
        }
        public static JsonModel UpdateFailed(string name)
        {
            return new JsonModel(JsonType.Error, string.Empty, name + " " + GlobalConstant.UpdateFailed);
        }
        public static JsonModel CreateSuccess(string name)
        {
            return new JsonModel(JsonType.Success, string.Empty, name + " " + GlobalConstant.Created);
        }
        public static JsonModel CreateFailed(string name)
        {
            return new JsonModel(JsonType.Error, string.Empty, name + " " + GlobalConstant.CreateFailed);
        }

        public static JsonModel ModelStateError(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            string errors = JsonConvert.SerializeObject(modelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => error.ErrorMessage));
            return new JsonModel(JsonType.Error, errors);
        }
    }
}
