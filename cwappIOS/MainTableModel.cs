using System.Collections.Generic;

namespace cwappIOS
{

    //Main model for displaying table data, login features etc...
    public class MainTableModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public int totalRows { get; set; }
        public List<ModelFields> apiData { get; set; }
    }

    public class ModelFields
    {
        public int entryid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string created_by { get; set; }
        public string created_at { get; set; }
        public string updated_by { get; set; }
        public string updated_at { get; set; }
        public string sessionid { get; set; }
        public int? is_approved { get; set; }
        public int totalRows { get; set; }
    }


    //Clean model for sending data to server for edit purposes
    public class SendFields
    {
        public string question { get; set; }
        public string answer { get; set; }
    }

    //Model when deserializing returnd JSON from edit route
    public class UpdatedDataModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public UpdatedFields apiData { get; set; }
    }

    public class UpdatedFields
    {
        public int entryid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string created_by { get; set; }
        public string created_at { get; set; }
        public string updated_by { get; set; }
        public string updated_at { get; set; }
        public string sessionid { get; set; }
        public object is_approved { get; set; }
    }



}
